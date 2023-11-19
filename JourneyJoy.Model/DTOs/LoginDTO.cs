using JourneyJoy.Model.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs
{
    public record LoginDTO
    {
        public string Token { get; set; } = null!;
        public DateTime? ExpiresAt { get; set; }
        public UserDTO User { get; set; } = null!;
    }
}
