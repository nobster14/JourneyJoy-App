using JourneyJoy.Model.Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }
        #endregion

        #region Private methods
        private static void SetUpAttractionOwnershipOfLocation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attraction>()
                .OwnsOne(e => e.Location);
        }
        #endregion
    }
}
