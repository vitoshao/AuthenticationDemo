using Microsoft.EntityFrameworkCore;

namespace Demo.DAL
{
    public partial class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
    }


}
