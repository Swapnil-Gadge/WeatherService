using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherService.Data.Validation
{
    public class SuperAdminValidation : ParentUserValidation
    {
        private readonly string validPassword;
        public SuperAdminValidation(string validPassword)
        {
            this.validPassword = validPassword;
        }

        public override (string, bool) ValidateUser(string userName, string enteredPassword)
        {
            if (enteredPassword == this.validPassword && userName.Trim().ToLowerInvariant() == "superadmin")
            {
                return (userName, true);
            }

            return (string.Empty, false);
        }
    }
}
