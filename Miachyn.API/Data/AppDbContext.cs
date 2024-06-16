using Miachyn.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Miachyn.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Furniture> Furnitures { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
