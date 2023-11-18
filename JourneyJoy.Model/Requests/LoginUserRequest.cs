using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Requests
{
    public record LoginUserRequest
    {
        #region Properties
        public string Email
        {
            get; set;
        }
        public string Password
        {
            get;
            set;
        }
        #endregion
    }
}
