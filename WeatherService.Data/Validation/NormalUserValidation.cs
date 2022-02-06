using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherService.Data.Validation
{
    public class NormalUserValidation : ParentUserValidation
    {
        private readonly string validPassword;
        public NormalUserValidation(string validPassword)
        {
            this.validPassword = validPassword;
        }

        public override (string, bool) ValidateUser(string userName, string enteredPassword)
        {
            if (enteredPassword == this.validPassword && userName.Trim().ToLowerInvariant() == "user")
            {
                return (userName, true);
            }

            return this.Parent.ValidateUser(userName, enteredPassword);
        }
    }
}
