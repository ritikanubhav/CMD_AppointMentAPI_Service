using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Appointment.Domain.Exceptions
{  
    public class InvalidDoctorIdException : ApplicationException
    {
        // Constructor that accepts a custom message

        public InvalidDoctorIdException(string message)
            : base(message)
        {
        }

        // Constructor that accepts a custom message and inner exception
        public InvalidDoctorIdException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
