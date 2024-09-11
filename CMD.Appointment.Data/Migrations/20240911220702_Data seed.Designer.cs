﻿// <auto-generated />
using System;
using CMD.Appointment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CMD.Appointment.Data.Migrations
{
    [DbContext(typeof(AppointmentDbContext))]
    [Migration("20240911220702_Data seed")]
    partial class Dataseed
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CMD.Appointment.Domain.Entities.AppointmentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PurposeOfVisit")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<TimeOnly>("Time")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.ToTable("Appointments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedBy = "Admin",
                            CreatedDate = new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5912),
                            Date = new DateOnly(2024, 9, 20),
                            DoctorId = 202,
                            Email = "john.doe@example.com",
                            LastModifiedBy = "Admin",
                            LastModifiedDate = new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5924),
                            Message = "Please bring your medical reports.",
                            PatientId = 101,
                            Phone = "123-456-7890",
                            PurposeOfVisit = "General Checkup",
                            Status = 0,
                            Time = new TimeOnly(9, 30, 0)
                        },
                        new
                        {
                            Id = 2,
                            CreatedBy = "Admin",
                            CreatedDate = new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5927),
                            Date = new DateOnly(2024, 9, 22),
                            DoctorId = 203,
                            Email = "jane.smith@example.com",
                            LastModifiedBy = "Admin",
                            LastModifiedDate = new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5928),
                            Message = "Appointment canceled by patient.",
                            PatientId = 102,
                            Phone = "987-654-3210",
                            PurposeOfVisit = "Follow-up on blood test",
                            Status = 1,
                            Time = new TimeOnly(11, 0, 0)
                        },
                        new
                        {
                            Id = 3,
                            CreatedBy = "Admin",
                            CreatedDate = new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5930),
                            Date = new DateOnly(2024, 9, 25),
                            DoctorId = 204,
                            Email = "alice.jones@example.com",
                            LastModifiedBy = "Admin",
                            LastModifiedDate = new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5931),
                            Message = "Dental checkup completed.",
                            PatientId = 103,
                            Phone = "555-123-4567",
                            PurposeOfVisit = "Dental Checkup",
                            Status = 2,
                            Time = new TimeOnly(10, 0, 0)
                        },
                        new
                        {
                            Id = 4,
                            CreatedBy = "Admin",
                            CreatedDate = new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5933),
                            Date = new DateOnly(2024, 9, 26),
                            DoctorId = 205,
                            Email = "mike.brown@example.com",
                            LastModifiedBy = "Admin",
                            LastModifiedDate = new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5934),
                            Message = "Please bring your previous prescription.",
                            PatientId = 104,
                            Phone = "555-987-6543",
                            PurposeOfVisit = "Eye Examination",
                            Status = 0,
                            Time = new TimeOnly(14, 0, 0)
                        },
                        new
                        {
                            Id = 5,
                            CreatedBy = "Admin",
                            CreatedDate = new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5936),
                            Date = new DateOnly(2024, 9, 28),
                            DoctorId = 206,
                            Email = "chris.evans@example.com",
                            LastModifiedBy = "Admin",
                            LastModifiedDate = new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5937),
                            Message = "Doctor is unavailable, appointment rescheduled.",
                            PatientId = 105,
                            Phone = "555-654-7891",
                            PurposeOfVisit = "Orthopedic Consultation",
                            Status = 1,
                            Time = new TimeOnly(16, 30, 0)
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
