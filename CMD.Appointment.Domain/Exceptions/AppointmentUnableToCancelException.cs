using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Appointment.Domain.Exceptions
{
    public class AppointmentUnableToCancelException:ApplicationException
    {
        public AppointmentUnableToCancelException() { }

        public AppointmentUnableToCancelException(string message) : base(message) { }

        public AppointmentUnableToCancelException(string message, Exception ex) : base(message, ex) { }
    }
}
