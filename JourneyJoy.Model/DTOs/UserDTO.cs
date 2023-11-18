using JourneyJoy.Model.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs
{
    public record UserDTO
    {
        public string? Id { get; set; }
        public string? Nickname { get; set; }

        public static UserDTO FromDatabaseUser(User user)
        {
            return new UserDTO()
            {
                Id = user.Id.ToString(),
                Nickname = user.Nickname,
            };
        }
    }
}
