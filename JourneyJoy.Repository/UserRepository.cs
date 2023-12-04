using JourneyJoy.Contracts;
using JourneyJoy.Model.Database;
using JourneyJoy.Model.Database.Tables;
using Microsoft.EntityFrameworkCore;
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

        #region Interface methods
        public User? FindUserByEmail(string email)
        {
            return DatabaseContext.Set<User>().FirstOrDefault(user => user.Email == email);
        }
        #endregion

        #region Override methods
        public override User? GetById(Guid id)
        {
            return DatabaseContext.Users.Include(it => it.UserTrips).Where(it => it.Id == id).FirstOrDefault();
        }
        #endregion
    }
}
