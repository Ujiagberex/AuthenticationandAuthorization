using AuthApp2.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthApp2.Data
{
	public class DataContext : IdentityDbContext<ApplicationUser>
	{
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
    }
}
