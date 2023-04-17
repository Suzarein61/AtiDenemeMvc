using Entities.Concrete;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WebApplication4.Models;
using static System.Net.WebRequestMethods;

namespace WebApplication4.Controllers
{
    [Authorize(Roles = "admin")]
    public class NorthwindProducts : Controller
    {

        public async Task<IActionResult> IndexAsync()
        {
            HttpClient httpClient = new HttpClient();
            DiscoveryDocumentResponse discovery = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7084");
            ClientCredentialsTokenRequest tokenRequest = new ClientCredentialsTokenRequest();
            tokenRequest.ClientId = "61";
            tokenRequest.ClientSecret = "ati";
            tokenRequest.Address = discovery.TokenEndpoint;
            TokenResponse tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(tokenRequest);
            httpClient.SetBearerToken(tokenResponse.AccessToken);

            HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7128/api/Products/getall");
           
            var content = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<DenemeModel>>(content);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> add()
        {
            return View();
                
        }
            

        [HttpPost]
        public async Task<IActionResult> add(DenemeModel model)
        {
            HttpClient httpClient = new HttpClient();
            DiscoveryDocumentResponse discovery = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7084");
            ClientCredentialsTokenRequest tokenRequest = new ClientCredentialsTokenRequest();
            tokenRequest.ClientId = "61";
            tokenRequest.ClientSecret = "ati";
            tokenRequest.Address = discovery.TokenEndpoint;
            TokenResponse tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(tokenRequest);
            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var product = model;
            var json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await httpClient.PostAsync("https://localhost:7128/api/Products/add", content);


            if (response.IsSuccessStatusCode)
            {
                return View("başarılı"); 
            }
            else
            {
                return View("hatalı");
            }
        }


    }
}
