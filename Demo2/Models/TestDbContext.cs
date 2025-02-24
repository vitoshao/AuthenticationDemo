using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Demo2.Models
{
    public partial class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
    }


}
