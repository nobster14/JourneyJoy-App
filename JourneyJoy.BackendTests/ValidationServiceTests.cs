using FluentAssertions;
using JourneyJoy.Utils.Validation;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.UnitTests
{
    public class ValidationServiceTests
    {
        private IValidationService validationService;

        [SetUp]
        public void Setup()
        {
            validationService = new ValidationService();
        }

        [Test]
        public void EmailValidationTest()
        {
            string[] validEmails = new string[] {
                "firstname.lastname@example.com",
                "email@123.123.123.123",
                "email@example-one.com",
                "email@example.co.jp",
                "_______@example.com",
                "\"email\"@example.com"
                };

            string[] invalidEmails = new string[] {
                "plainaddress",
                "#@%^%#$@#$@#.com",
                ".email@example.com",
                "@example.com"
                };

            validEmails.All(it => validationService.EmailValidator.Validate(it, out _)).Should().BeTrue();
            invalidEmails.All(it => !validationService.EmailValidator.Validate(it, out _)).Should().BeTrue();
        }

        [Test]
        public void PasswordValidationTest()
        {
            string validPassword = "dfkldfG7^";
            validationService.PasswordValidator.Validate(validPassword, out _).Should().BeTrue();

            string noLowerCaseLetterPassword = "DJFKFKF7&";
            validationService.PasswordValidator.Validate(noLowerCaseLetterPassword, out string error1).Should().BeFalse();
            error1.Should().Be("Password should contain At least one lower case letter");

            string noUpperCaseLetterPassword = "dfkldfd7&";
            validationService.PasswordValidator.Validate(noUpperCaseLetterPassword, out string error2).Should().BeFalse();
            error2.Should().Be("Password should contain At least one upper case letter");

            string tooShortPassword = "Ddfd7&";
            validationService.PasswordValidator.Validate(tooShortPassword, out string error3).Should().BeFalse();
            error3.Should().Be("Password should not be less than 8 or greater than 25 characters");

            string noNumericValuePassword = "dfkldfdD&";
            validationService.PasswordValidator.Validate(noNumericValuePassword, out string error4).Should().BeFalse();
            error4.Should().Be("Password should contain At least one numeric value");

            string noSpecialCharacterPassword = "dfkldfd7D";
            validationService.PasswordValidator.Validate(noSpecialCharacterPassword, out string error5).Should().BeFalse();
            error5.Should().Be("Password should contain At least one special case characters");
        }
    }
}
