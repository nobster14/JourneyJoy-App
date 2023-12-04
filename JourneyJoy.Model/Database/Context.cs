using JourneyJoy.Model.Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Database
{
    public class DatabaseContext : DbContext
    {
        #region Fields

        #region Public
        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        #endregion

        #endregion


        #region Constructors

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        #endregion

        #region Override methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetUpAttractionOwnershipOfLocation(modelBuilder);
            SetUpUserOwnershipOfTrip(modelBuilder);
            SetUpTripOwnershipOfAttraction(modelBuilder);
        }
        #endregion

        #region Private methods
        private static void SetUpAttractionOwnershipOfLocation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attraction>()
                .OwnsOne(e => e.Location);
        }

        private static void SetUpUserOwnershipOfTrip(ModelBuilder modelBuilder)
        {
            {
                modelBuilder.Entity<Trip>()
                    .HasOne(t => t.User)
                    .WithMany(t => t.UserTrips)
                    .OnDelete(DeleteBehavior.Cascade);

            }
        }

        private static void SetUpTripOwnershipOfAttraction(ModelBuilder modelBuilder)
        {
            {
                modelBuilder.Entity<Attraction>()
                    .HasOne(t => t.Trip)
                    .WithMany(t => t.Attractions)
                    .OnDelete(DeleteBehavior.Cascade);

            }
        }
        #endregion  
    }
}
