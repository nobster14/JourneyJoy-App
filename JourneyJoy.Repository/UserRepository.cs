using JourneyJoy.Contracts;
using JourneyJoy.Model.Database;
using JourneyJoy.Model.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        #region Constructors

        public UserRepository(DatabaseContext context) : base(context)
        {
        }

        #endregion
    }
}
