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

        #endregion

        #region Interface methods
        public void Save()
        {
            databaseContext.SaveChanges();
        }

        #endregion
    }
}
