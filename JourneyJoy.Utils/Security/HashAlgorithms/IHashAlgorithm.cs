using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Utils.Security.HashAlgorithms
{
    public interface IHashAlgorithm
    {
        public bool Verify(string text, string hash);
        public string Hash(string text);
    }
}
