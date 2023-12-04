using JourneyJoy.Model.Database;
using JourneyJoy.Model.Database.Tables;
using JourneyJoy.Utils.Security.HashAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.IntegrationTests.ControllersTests
{
    [TestFixture]
    public class UserControllerTests : BaseControllerTests
    {
        #region Private fields

        private Guid userId;
        private string userEmail = null!;
        private string userPassword = null!;
        #endregion

        #region Override methods
        protected override void AddInitialData(DatabaseContext databaseContext)
        {
            userId = Guid.NewGuid();
            userEmail = "testuser@mail.com";
            userPassword = "tesT@ssword123";

            var user = new User
            {
                Id = userId,
                Nickname = "testNickname",
                Email = userEmail,
                Password = new BCryptAlgorithm().Hash(userPassword),
            };
            databaseContext.Add(user);

            databaseContext.SaveChanges();
        }

        #endregion

        #region Tests

        [Test]
        public async Task LoginUser_AuthotizarionHeaderAfterLoginShouldNotBeNull()
        {
            await SignIn(userEmail, userPassword, loginEndpoint);

            HttpClient.DefaultRequestHeaders.Authorization.Should().NotBeNull();
        }

        #endregion
    }
}
