using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Appointment.Domain.Services
{
    public class DateValidator : IDateValidator
    {
        public bool ValidateDate(DateOnly date)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            DateOnly maxDate = today.AddDays(30);

            if (date < today)
            {
                // Date is in the past
                return false;
            }
            else if (date > maxDate)
            {
                // Date is more than 30 days from now
                return false;
            }
            // Date is valid
            return true;
        }
    }
}
