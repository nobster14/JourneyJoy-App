using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Database.Tables
{
    [Index(nameof(Email), IsUnique = true)]
    public record User
    {
        public Guid Id { get; set; }

        [MaxLength(200)]
        public string Nickname { get; set; } = null!;

        /// <remarks>
        /// Length set to 256 according to discussion:
        /// <see href="https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address"/>
        /// </remarks>
        [MaxLength(256)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Hashed password
        /// </summary>
        [MaxLength(1000)]
        public string Password { get; set; } = null!;
        public ICollection<Trip> UserTrips { get; set; } = null!;
    }
}
