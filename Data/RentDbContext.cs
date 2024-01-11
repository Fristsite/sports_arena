using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Play.Models;

namespace Play.Data
{
#nullable disable
    public class RentDbContext : DbContext
    {
        public RentDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Arena> Arenas { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingSportskit>BookingSportskits{get;set;}
        public DbSet<BookingCourt> BookingCourts { get; set; }
        public DbSet<BookingCartType> BookingCartTypes { get; set; }
        public DbSet<CourtDetails> CourtDetails { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SportsKit> SportsKits { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ratings> Ratings { get; set; }
        public DbSet<PersonToPlay> PersonToPlays { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingCartType>()
               .HasIndex(e => e.BookingCartTypeName)
               .IsUnique();

            modelBuilder.Entity<Game>()
            .HasIndex(e => e.GameName)
            .IsUnique();

            modelBuilder.Entity<User>()
          .HasIndex(e => new { e.UserName, e.PhoneNumber })
          .IsUnique();
            modelBuilder.Entity<Employee>()
            .HasIndex(e => new { e.UserName, e.PhoneNumber })
            .IsUnique();

            modelBuilder.Entity<Owner>()
            .HasIndex(e => new { e.UserName, e.PhoneNumber })
            .IsUnique();

           
        }

    }
}