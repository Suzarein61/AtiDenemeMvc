using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class UserSignUpViewModel
    {
        [Display(Name ="Ad Soyad")]
        [Required(ErrorMessage="Lütfen ad soyad giriniz")]
        public string NameSurname { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Lütfen Kullanıcı Adı giriniz")]
        public string UserName { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Lütfen şifre giriniz")]
        public string Password { get; set; }

        [Display(Name = "Şifre Tekrar")]
        [Compare("Password",ErrorMessage ="şifreler uyuşmuyor")]
        public string RePassword { get; set; }

        [Display(Name = "Mail")]
        [Required(ErrorMessage = "Lütfen mail giriniz")]
        public string Mail { get; set; }

   
    }
}
