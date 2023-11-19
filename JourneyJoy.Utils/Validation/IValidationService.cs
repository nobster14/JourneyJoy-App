using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Utils.Validation
{
    public interface IValidationService
    {
        public PasswordValidator PasswordValidator { get; }
        public EmailValidator EmailValidator { get; }
    }
}
