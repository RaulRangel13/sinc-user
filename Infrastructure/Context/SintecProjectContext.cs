using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class SintecProjectContext : DbContext
    {
        public SintecProjectContext(DbContextOptions<SintecProjectContext> options) : base(options) { }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<TwoFA> TwoFa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .Property(s => s.Id)
                .UseIdentityColumn()
                .IsUnicode(true);

            modelBuilder.Entity<Customer>()
                .Property(s => s.Name)
                .HasMaxLength(40)
                .IsRequired();

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
