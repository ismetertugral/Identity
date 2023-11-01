using System.ComponentModel.DataAnnotations;

namespace ismetertugral.Identity.Models
{
    public class RoleCreateModel
    {
        [Required]
        public string Name { get; set; }
    }
}
