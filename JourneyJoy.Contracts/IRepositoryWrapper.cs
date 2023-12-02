using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; }
        ITripsRepository TripsRepository { get; }
        void Save();
    }
}
