using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartDorm.Migrations
{
    /// <inheritdoc />
    public partial class seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserAccounts",
                columns: new[] { "Id", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, true, "PrP+ZrMeO00Q+nC1ytSccRIpSvauTkdqHEBRVdRaoSE=", 2, "admin" },
                    { 2, true, "PrP+ZrMeO00Q+nC1ytSccRIpSvauTkdqHEBRVdRaoSE=", 1, "dormmanager" }
                });

            migrationBuilder.InsertData(
                table: "DormitoryManagers",
                columns: new[] { "Id", "FullName", "PhoneNumber", "UserId" },
                values: new object[] { 1, "mosayebi", "555-1234", 2 });

            migrationBuilder.InsertData(
                table: "Dormitories",
                columns: new[] { "DormitoryId", "Address", "CampusLocation", "GenderType", "ManagerId", "Name", "TotalCapacity" },
                values: new object[,]
                {
                    { 1, "Campus North Zone", "North", 0, 1, "Sadra", 300 },
                    { 2, "Campus West Zone", "West", 1, 1, "Sadaf", 250 }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "Capacity", "DormitoryId", "Floor", "GenderType", "HasPrivateBathroom", "IsActive", "RoomNumber", "WingOrBlock" },
                values: new object[,]
                {
                    { 1, 2, 1, 1, 0, false, true, 336, 3 },
                    { 2, 3, 1, 1, 0, true, true, 212, 1 },
                    { 3, 2, 2, 2, 1, true, true, 5, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Dormitories",
                keyColumn: "DormitoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Dormitories",
                keyColumn: "DormitoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DormitoryManagers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
