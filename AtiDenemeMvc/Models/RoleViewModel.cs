using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Rolü boş geçmeyiniz.")]
        [Display(Name = "Rol Adı")]
        public string RoleName { get; set; }
    }
}
