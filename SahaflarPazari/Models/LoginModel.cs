using System.ComponentModel.DataAnnotations;

namespace SahaflarPazari.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Kullanıcı Zorunludur")]
        [Display(Name = "Kullanıcı Adı")]
        public string userName { get; set; }
        [Required(ErrorMessage ="Şifre Zorunludur")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}