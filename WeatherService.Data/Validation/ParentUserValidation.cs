using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherService.Data.Validation
{
    public abstract class ParentUserValidation
    {
        public ParentUserValidation Parent { get; set; }

        public abstract (string, bool) ValidateUser(string userName, string enteredPassword);
    }
}
