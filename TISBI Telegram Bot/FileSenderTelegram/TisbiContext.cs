using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserTISBINew
{
    internal class TisbiContext: DbContext
    {
        public DbSet<User> Users { get; set; }

        public TisbiContext() 
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=tisbidb;Trusted_Connection=True;");
        }
    }
}
