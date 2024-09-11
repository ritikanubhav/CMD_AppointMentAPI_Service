using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CMD.Appointment.Data.Migrations
{
    /// <inheritdoc />
    public partial class Dataseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Date", "DoctorId", "Email", "LastModifiedBy", "LastModifiedDate", "Message", "PatientId", "Phone", "PurposeOfVisit", "Status", "Time" },
                values: new object[,]
                {
                    { 1, "Admin", new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5912), new DateOnly(2024, 9, 20), 202, "john.doe@example.com", "Admin", new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5924), "Please bring your medical reports.", 101, "123-456-7890", "General Checkup", 0, new TimeOnly(9, 30, 0) },
                    { 2, "Admin", new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5927), new DateOnly(2024, 9, 22), 203, "jane.smith@example.com", "Admin", new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5928), "Appointment canceled by patient.", 102, "987-654-3210", "Follow-up on blood test", 1, new TimeOnly(11, 0, 0) },
                    { 3, "Admin", new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5930), new DateOnly(2024, 9, 25), 204, "alice.jones@example.com", "Admin", new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5931), "Dental checkup completed.", 103, "555-123-4567", "Dental Checkup", 2, new TimeOnly(10, 0, 0) },
                    { 4, "Admin", new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5933), new DateOnly(2024, 9, 26), 205, "mike.brown@example.com", "Admin", new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5934), "Please bring your previous prescription.", 104, "555-987-6543", "Eye Examination", 0, new TimeOnly(14, 0, 0) },
                    { 5, "Admin", new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5936), new DateOnly(2024, 9, 28), 206, "chris.evans@example.com", "Admin", new DateTime(2024, 9, 12, 3, 37, 2, 495, DateTimeKind.Local).AddTicks(5937), "Doctor is unavailable, appointment rescheduled.", 105, "555-654-7891", "Orthopedic Consultation", 1, new TimeOnly(16, 30, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
