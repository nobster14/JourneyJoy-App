using JourneyJoy.Model.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
    }
}
