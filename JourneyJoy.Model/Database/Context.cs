using Microsoft.EntityFrameworkCore;
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

        }
        #endregion
    }
}
