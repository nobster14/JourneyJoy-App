using JourneyJoy.Utils.Security.Tokens;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Utils.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static string GetToken(this ControllerBase controller)
        {
            return controller.Request.Headers["Authorization"].First().Split(' ')[1];
        }

        public static Guid GetCallingUserId(this ControllerBase controller)
        {
            var token = controller.GetToken();
            return Guid.Parse(JwtTokenHelper.GetIdFromToken(token));
        }
    }
}
