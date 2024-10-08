﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CMD.Appointment.Data.Migrations
{
    /// <inheritdoc />
    public partial class Reset20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Date", "DoctorId", "Email", "LastModifiedBy", "LastModifiedDate", "Message", "PatientId", "Phone", "PurposeOfVisit", "Status", "Time" },
                values: new object[,]
                {
                    { 1, "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3968), new DateOnly(2024, 9, 20), 1, "john.doe@example.com", "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3969), "Please bring your medical reports.", 1, "919876543210", "Consultation", 0, new TimeOnly(9, 30, 0) },
                    { 2, "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3974), new DateOnly(2024, 9, 22), 2, "jane.smith@example.com", "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3975), "Appointment canceled by patient.", 2, "919234567890", "Follow Up", 1, new TimeOnly(11, 0, 0) },
                    { 3, "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3978), new DateOnly(2024, 9, 25), 3, "alice.jones@example.com", "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3979), "Dental checkup completed.", 3, "919345678901", "Treatment", 2, new TimeOnly(10, 0, 0) },
                    { 4, "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3981), new DateOnly(2024, 9, 26), 4, "mike.brown@example.com", "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3982), "Immediate attention required.", 4, "919456789012", "Emergency", 0, new TimeOnly(14, 0, 0) },
                    { 5, "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3985), new DateOnly(2024, 9, 28), 5, "chris.evans@example.com", "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3985), "Doctor is unavailable, appointment rescheduled.", 5, "919567890123", "General Checkup", 1, new TimeOnly(16, 30, 0) },
                    { 6, "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3988), new DateOnly(2024, 9, 30), 2, "emma.williams@example.com", "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3989), "Please bring your test reports.", 1, "919678901234", "Typhoid", 0, new TimeOnly(8, 0, 0) },
                    { 7, "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3991), new DateOnly(2024, 10, 1), 3, "olivia.johnson@example.com", "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3992), "Bring blood test results.", 2, "919789012345", "Malaria", 0, new TimeOnly(15, 0, 0) },
                    { 8, "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3995), new DateOnly(2024, 10, 3), 4, "ava.miller@example.com", "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3995), "Patient requested cancellation.", 3, "919890123456", "Cold", 1, new TimeOnly(13, 0, 0) },
                    { 9, "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3998), new DateOnly(2024, 10, 5), 5, "isabella.taylor@example.com", "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(3999), "Take medication before visit.", 4, "919901234567", "Fever", 0, new TimeOnly(11, 0, 0) },
                    { 10, "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(4002), new DateOnly(2024, 10, 7), 1, "mia.anderson@example.com", "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(4002), "No special instructions.", 5, "919012345678", "Consultation", 0, new TimeOnly(9, 30, 0) },
                    { 11, "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(4005), new DateOnly(2024, 10, 10), 3, "lucas.martin@example.com", "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(4005), "Doctor unavailable, please reschedule.", 1, "919123456789", "Follow Up", 1, new TimeOnly(14, 0, 0) },
                    { 12, "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(4008), new DateOnly(2024, 10, 12), 4, "sophia.lee@example.com", "Admin", new DateTime(2024, 9, 18, 19, 17, 40, 74, DateTimeKind.Local).AddTicks(4009), "Please bring your previous reports.", 2, "919234567890", "Treatment", 0, new TimeOnly(16, 30, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");
        }
    }
}
