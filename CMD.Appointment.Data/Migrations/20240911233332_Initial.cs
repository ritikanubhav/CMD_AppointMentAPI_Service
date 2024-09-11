using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CMD.Appointment.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurposeOfVisit = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Date", "DoctorId", "Email", "LastModifiedBy", "LastModifiedDate", "Message", "PatientId", "Phone", "PurposeOfVisit", "Status", "Time" },
                values: new object[,]
                {
                    { 1, "Admin", new DateTime(2024, 9, 12, 5, 3, 32, 283, DateTimeKind.Local).AddTicks(3362), new DateOnly(2024, 9, 20), 202, "john.doe@example.com", "Admin", new DateTime(2024, 9, 12, 5, 3, 32, 283, DateTimeKind.Local).AddTicks(3377), "Please bring your medical reports.", 101, "123-456-7890", "General Checkup", 0, new TimeOnly(9, 30, 0) },
                    { 2, "Admin", new DateTime(2024, 9, 12, 5, 3, 32, 283, DateTimeKind.Local).AddTicks(3385), new DateOnly(2024, 9, 22), 203, "jane.smith@example.com", "Admin", new DateTime(2024, 9, 12, 5, 3, 32, 283, DateTimeKind.Local).AddTicks(3386), "Appointment canceled by patient.", 102, "987-654-3210", "Follow-up on blood test", 1, new TimeOnly(11, 0, 0) },
                    { 3, "Admin", new DateTime(2024, 9, 12, 5, 3, 32, 283, DateTimeKind.Local).AddTicks(3391), new DateOnly(2024, 9, 25), 204, "alice.jones@example.com", "Admin", new DateTime(2024, 9, 12, 5, 3, 32, 283, DateTimeKind.Local).AddTicks(3392), "Dental checkup completed.", 103, "555-123-4567", "Dental Checkup", 2, new TimeOnly(10, 0, 0) },
                    { 4, "Admin", new DateTime(2024, 9, 12, 5, 3, 32, 283, DateTimeKind.Local).AddTicks(3396), new DateOnly(2024, 9, 26), 205, "mike.brown@example.com", "Admin", new DateTime(2024, 9, 12, 5, 3, 32, 283, DateTimeKind.Local).AddTicks(3397), "Please bring your previous prescription.", 104, "555-987-6543", "Eye Examination", 0, new TimeOnly(14, 0, 0) },
                    { 5, "Admin", new DateTime(2024, 9, 12, 5, 3, 32, 283, DateTimeKind.Local).AddTicks(3400), new DateOnly(2024, 9, 28), 206, "chris.evans@example.com", "Admin", new DateTime(2024, 9, 12, 5, 3, 32, 283, DateTimeKind.Local).AddTicks(3401), "Doctor is unavailable, appointment rescheduled.", 105, "555-654-7891", "Orthopedic Consultation", 1, new TimeOnly(16, 30, 0) }
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
