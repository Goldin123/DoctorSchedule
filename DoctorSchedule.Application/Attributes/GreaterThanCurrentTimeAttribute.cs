
using System.ComponentModel.DataAnnotations;

namespace DoctorSchedule.Application.Attributes
{
    public class GreaterThanCurrentTimeAttribute : ValidationAttribute
    {
        public GreaterThanCurrentTimeAttribute() : base("{0} must be in the future.")
        {
        }

        public override bool IsValid(object value)
        {
            if (value is DateTime dateTime)
            {
                return dateTime > DateTime.Now;
            }
            return false;
        }
    }
}