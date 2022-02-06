using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherService.Data.Validation
{
    public class AdminUserValidation : ParentUserValidation
    {
        private readonly string validPassword;
        public AdminUserValidation(string validPassword)
        {
            this.validPassword = validPassword;
        }

        public override (string, bool) ValidateUser(string userName, string enteredPassword)
        {
            if (enteredPassword == this.validPassword && userName.Trim().ToLowerInvariant() == "admin")
            {
                return (userName, true);
            }

            return this.Parent.ValidateUser(userName, enteredPassword);
        }
    }
}
