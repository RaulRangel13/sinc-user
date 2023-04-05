using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class SintecProjectContext : DbContext
    {
        public SintecProjectContext(DbContextOptions<SintecProjectContext> options) : base(options) { }

        public DbSet<Customer> Customer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .Property(s => s.Id)
                .UseIdentityColumn()
                .IsUnicode(true);

            modelBuilder.Entity<Customer>()
                .Property(s => s.Email)
                .HasMaxLength(35)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(s => s.Password)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(s => s.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(s => s.AlteratedAt)
                .IsRequired(false);
        }
    }
}
