using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Appointment.Domain.Exceptions
{
    public class InvalidPatientIdException:ApplicationException
    {
        // Constructor that accepts a custom message

        public InvalidPatientIdException(string message)
            : base(message)
        {
        }

        // Constructor that accepts a custom message and inner exception
        public InvalidPatientIdException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
