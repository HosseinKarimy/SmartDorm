using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartDorm.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DormitoryManagers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DormitoryManagers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DormitoryManagers_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    BirthdayYear = table.Column<int>(type: "INTEGER", nullable: true),
                    FieldOfStudy = table.Column<int>(type: "INTEGER", nullable: false),
                    EntryYear = table.Column<int>(type: "INTEGER", nullable: false),
                    DegreeLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dormitories",
                columns: table => new
                {
                    DormitoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    CampusLocation = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TotalCapacity = table.Column<int>(type: "INTEGER", nullable: false),
                    GenderType = table.Column<int>(type: "INTEGER", nullable: false),
                    ManagerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dormitories", x => x.DormitoryId);
                    table.ForeignKey(
                        name: "FK_Dormitories_DormitoryManagers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "DormitoryManagers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SpecialConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConditionType = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Severity = table.Column<int>(type: "INTEGER", nullable: false),
                    Needs = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialConditions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SleepHabit = table.Column<int>(type: "INTEGER", nullable: false),
                    NoiseToleranceLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    StudyStyle = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomAtmosphere = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomCapacity = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentPreferences_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DormitoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Floor = table.Column<int>(type: "INTEGER", nullable: false),
                    WingOrBlock = table.Column<int>(type: "INTEGER", nullable: false),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    HasPrivateBathroom = table.Column<bool>(type: "INTEGER", nullable: false),
                    GenderType = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_Rooms_Dormitories_DormitoryId",
                        column: x => x.DormitoryId,
                        principalTable: "Dormitories",
                        principalColumn: "DormitoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dormitories_ManagerId",
                table: "Dormitories",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_DormitoryManagers_UserId",
                table: "DormitoryManagers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DormitoryId",
                table: "Rooms",
                column: "DormitoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialConditions_StudentId",
                table: "SpecialConditions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPreferences_StudentId",
                table: "StudentPreferences",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "SpecialConditions");

            migrationBuilder.DropTable(
                name: "StudentPreferences");

            migrationBuilder.DropTable(
                name: "Dormitories");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "DormitoryManagers");

            migrationBuilder.DropTable(
                name: "UserAccounts");
        }
    }
}
