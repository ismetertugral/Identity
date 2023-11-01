using System.ComponentModel.DataAnnotations;

namespace ismetertugral.Identity.Models
{
    public class UserCreateAdminModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string Username { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
    }
}
