using Microsoft.EntityFrameworkCore;

namespace Demo1.Models
{
    public class TestDbInitialiser
    {
        private readonly TestDbContext _context;

        public TestDbInitialiser(TestDbContext context)
        {
            _context = context;
        }

        public void Run()
        {
            // TODO: Add initialisation logic.
            //_context.Database.EnsureDeleted();
            //_context.Database.EnsureCreated();
            _context.Database.Migrate();

            // TODO: Seed with sample data 
            // mark below code after run
            //var user1 = new Users { Account = "vito", Password = "123456" };
            //var user2 = new Users { Account = "shao", Password = "123456" };
            //_context.Users.Add(user1);
            //_context.Users.Add(user2);
            //_context.SaveChanges();
        }
    }
}
