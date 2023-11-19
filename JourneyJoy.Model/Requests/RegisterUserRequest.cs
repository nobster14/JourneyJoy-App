﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Requests
{
    public record RegisterUserRequest
    {
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
