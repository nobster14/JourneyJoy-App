using JourneyJoy.Utils.Security.HashAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.UnitTests
{
    public class HashAlgorithmTests
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int length = 20;
        private Random random;

        [SetUp]
        public void Setup()
        {
            random = new Random();
        }

        [Test]
        public void BCryptAlgorithmTest()
        {
            var algorithm = new BCryptAlgorithm();
            var randomText = Enumerable.Repeat(chars, length)
                .Select(i => i[random.Next(i.Length)])
                .ToString();

            var hashedText = algorithm.Hash(randomText);

            var areMatching = algorithm.Verify(randomText, hashedText);
            areMatching.Should().BeTrue();
        }
    }
}
