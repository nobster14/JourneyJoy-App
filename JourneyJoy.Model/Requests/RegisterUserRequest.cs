using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Requests
{
    public record RegisterUserRequest
    {

        public string Nickname { get; set; }
        /// <summary>
        /// Password has to have beetwen 8 and 25 characters and minimum one of each (numeric value, special case character, lower case character, upper case character)
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Email in valid format
        /// </summary>
        public string Email { get; set; }
    }
}
