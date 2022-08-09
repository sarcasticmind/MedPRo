
using keef2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Keefa1.Models
{
    public class DocContext : IdentityDbContext<AccountUser>
    {
        public DocContext(DbContextOptions options) : base(options)
        { }
        public  DbSet<Doctor> Doctors { get; set; }
        public DbSet<AccountUser> AccountUsers { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<Department> Departments { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=keefeta;Integrated Security=True");
        //    optionsBuilder.UseSqlServer("Server = SQL8001.site4now.net; Database = db_a855a7_keefta; User ID = db_a855a7_keefta_admin; Password = yeyeye123; MultipleActiveResultSets = true; Connection Timeout = 0");

        //}


    }
}
