using Microsoft.EntityFrameworkCore;
using StockData.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockData.Objects
{
    public class StockDbContext : DbContext
    {
        public StockDbContext()
        {
        }
        public StockDbContext(DbContextOptions<StockDbContext> contextOptions) : base(contextOptions)
        {
        }
        public DbSet<StockHistory> StockHistory { get; set; }

        private readonly string _connectionString;

        public StockDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StockHistory>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<StockHistory>().HasIndex(x => new { x.StockId, x.DateTime }).IsUnique();
            modelBuilder.Entity<StockHistory>().Property(x => x.StockId).IsRequired(true).HasMaxLength(50);
            modelBuilder.Entity<StockHistory>().Property(x => x.OpenPrice).HasPrecision(18, 8);
            modelBuilder.Entity<StockHistory>().Property(x => x.ClosePrice).HasPrecision(18, 8);
        }
    }
}
