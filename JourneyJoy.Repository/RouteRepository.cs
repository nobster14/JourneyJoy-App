using JourneyJoy.Contracts;
using JourneyJoy.Model.Database.Tables;
using JourneyJoy.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Repository
{
    public class RouteRepository : RepositoryBase<Route>, IRouteRepository
    {
        #region Constructors
        public RouteRepository(DatabaseContext context) : base(context)
        {
        }
        #endregion
    }
}
