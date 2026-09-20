using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace WebRegistration.Data
{
    public class WebDbContext : DbContext
    {
        public WebDbContext(DbContextOptions<WebDbContext> 
            options) : base( options) 
        {}
        public DbSet<Employee> Employees { get; set; }

        public DbSet<User> Users { get; set; }
    }

}
