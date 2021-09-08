using AuthService.Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data.Context
{
    public class AuthServiceContext : DbContext
    {
        public AuthServiceContext(DbContextOptions<AuthServiceContext> options):base(options){}
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}
       

        public DbSet<User> Users { get; set; }
    }
}
