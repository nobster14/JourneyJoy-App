﻿using JourneyJoy.Model.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Contracts
{
    public interface ITripsRepository : IRepositoryBase<Trip>
    {
        public IEnumerable<Trip>? GetByIds(IEnumerable<Guid> id);
    }
}
