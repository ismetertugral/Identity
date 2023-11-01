using Microsoft.AspNetCore.Identity;

namespace ismetertugral.Identity.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public DateTime CreatedTime { get; set; }
    }
}
