using ismetertugral.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ismetertugral.Identity.Context
{
    public class IEContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public IEContext(DbContextOptions options) : base(options)
        {

        }
    }
}
