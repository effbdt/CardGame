using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class CardGameDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }

        public CardGameDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=CardGameDb;Integrated Security=True;MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(connStr);
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.CardId);
                entity.Property(e => e.CardId).ValueGeneratedOnAdd();
            });
            base.OnModelCreating(modelBuilder);
        }
    }

}
