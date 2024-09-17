using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMD.Appointment.Data.Migrations
{
    /// <inheritdoc />
    public partial class newdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7864), new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7865) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7869), new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7870) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7873), new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7873) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7876), new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7877) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7880), new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7880) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7883), new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7884) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7888), new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7888) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7891), new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7891) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7894), new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7895) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7897), new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7898) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7900), new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7901) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7903), new DateTime(2024, 9, 17, 7, 8, 1, 968, DateTimeKind.Local).AddTicks(7904) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7361), new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7362) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7367), new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7367) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7370), new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7371) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7374), new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7374) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7378), new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7378) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7381), new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7381) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7384), new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7384) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7388), new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7388) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7392), new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7392) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7395), new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7395) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7398), new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7399) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7401), new DateTime(2024, 9, 17, 7, 2, 38, 721, DateTimeKind.Local).AddTicks(7402) });
        }
    }
}
