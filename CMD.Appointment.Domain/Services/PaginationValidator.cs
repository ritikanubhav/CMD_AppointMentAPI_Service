using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Appointment.Domain.Services
{
    public static class PaginationValidator
    { 
        public static bool ValidatePagination(int pageNo,int PageSize)
        {
            if(pageNo > 0 && (PageSize > 0 && PageSize<=100) )
            {
                return true;
            }
            return false;
        }
    }
}
