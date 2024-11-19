using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Application.Attributes
{
    public class TimeComparisonAttribute : ValidationAttribute
    {
        private readonly string _startTimeProperty;
        private readonly string _endTimeProperty;

        public TimeComparisonAttribute(string startTimeProperty, string endTimeProperty)
        {
            _startTimeProperty = startTimeProperty;
            _endTimeProperty = endTimeProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startTime = validationContext.ObjectType.GetProperty(_startTimeProperty)?.GetValue(validationContext.ObjectInstance, null);
            var endTime = validationContext.ObjectType.GetProperty(_endTimeProperty)?.GetValue(validationContext.ObjectInstance, null);

            if (startTime != null && endTime != null)
            {
                if (startTime is DateTime start && endTime is DateTime end)
                {
                    if (end <= start)
                    {
                        return new ValidationResult($"End time must be later than start time.", new[] { _endTimeProperty });
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
