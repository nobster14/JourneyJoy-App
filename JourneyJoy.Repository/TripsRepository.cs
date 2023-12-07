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
    public class TripsRepository : RepositoryBase<Trip>, ITripsRepository
    {
        #region Constructors
        public TripsRepository(DatabaseContext context) : base(context)
        {
        }
        #endregion

        #region Interface methods
        public IEnumerable<Trip>? GetByIds(IEnumerable<Guid> id)
        {
            return DatabaseContext.Trips.Include(it => it.Attractions).Include(it => it.User).Where(it => id.Contains(it.Id));
        }
        #endregion

        #region Override methods
        public override Trip? GetById(Guid id)
        {
            return DatabaseContext.Trips.Include(it => it.Attractions).Include(it => it.User).Where(it => id == it.Id).FirstOrDefault();
        }
        #endregion
    }
}
