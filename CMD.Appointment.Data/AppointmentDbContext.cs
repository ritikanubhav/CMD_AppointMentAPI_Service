using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Appointment.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMD.Appointment.Data
{
    public class AppointmentDbContext:DbContext
    {
        // Mapping to database by configuring from program.cs
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
        { 

        }
        public AppointmentDbContext()
        {

        }
        //Mapping to Tables
        public DbSet<AppointmentModel> Appointments { get; set; }

    }
}
