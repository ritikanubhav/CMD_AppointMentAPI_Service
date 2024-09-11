using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Appointment.Domain.Services
{
    public interface IDateValidator
    {
        public bool ValidateDate(DateOnly date);
    }
}
