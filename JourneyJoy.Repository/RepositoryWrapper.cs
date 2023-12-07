using JourneyJoy.Contracts;
using JourneyJoy.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        #region Fields

        private DatabaseContext databaseContext;
        private IUserRepository? userRepository;
        private ITripsRepository? tripsRepository;
        private IAttractionRepository attractionRepository;
        #endregion

        #region Constructors

        public RepositoryWrapper(DatabaseContext databaseContext) => this.databaseContext = databaseContext;

        #endregion

        #region Properties

        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(databaseContext);

                return userRepository;
            }
        }

        public ITripsRepository TripsRepository
        {
            get
            {
                if (tripsRepository == null)
                    tripsRepository = new TripsRepository(databaseContext);

                return tripsRepository;
            }
        }

        public IAttractionRepository AttractionRepository
        {
            get
            {
                if (attractionRepository == null)
                    attractionRepository = new AttractionRepository(databaseContext);

                return attractionRepository;
            }
        }

        #endregion

        #region Interface methods
        public void Save()
        {
            databaseContext.SaveChanges();
        }

        #endregion
    }
}
