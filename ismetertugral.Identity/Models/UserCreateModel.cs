using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ismetertugral.Identity.Models
{
    public class UserCreateModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string Username { get; set; }
        [EmailAddress(ErrorMessage = "Email formatına uygun olması gereklidir.")]
        [Required(ErrorMessage = "Email adresi gereklidir.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre gereklidir.")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Cinsiyet gereklidir.")]
        public string Gender { get; set; }
    }
}
