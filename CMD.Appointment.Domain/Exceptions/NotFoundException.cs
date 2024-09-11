using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Appointment.Domain.Exceptions
{
    public class NotFoundException:ApplicationException
    {
        // Constructor that accepts a custom message
        
        public NotFoundException(string message)
            : base(message)
        {
        }

        // Constructor that accepts a custom message and inner exception
        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
