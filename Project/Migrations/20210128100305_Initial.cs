using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ravid.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "char(64)", nullable: false),
                    Phone1 = table.Column<string>(type: "varchar(20)", nullable: false),
                    Phone2 = table.Column<string>(type: "varchar(20)", nullable: true),
                    DateRegistered = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Company = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Comment = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "TrasportRequests",
                columns: table => new
                {
                    TrasportRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ForDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "ntext", nullable: true),
                    NumberOfPlates = table.Column<int>(type: "int", nullable: false),
                    DeliveryFor = table.Column<string>(type: "nvarchar(400)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    TrasportRequestStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrasportRequests", x => x.TrasportRequestId);
                    table.ForeignKey(
                        name: "FK_TrasportRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInRoles", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserInRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[] { 1, "Administrator" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[] { 2, "Client" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Comment", "Company", "DateRegistered", "Email", "FirstName", "LastName", "Password", "Phone1", "Phone2" },
                values: new object[] { new Guid("482f8b0e-1fe7-4ea6-9c10-4726a859b627"), null, "ET Internet Services", new DateTime(2021, 1, 28, 12, 3, 4, 981, DateTimeKind.Local).AddTicks(9353), "eyal.ank@gmail.com", "Eyal", "Ankri", "744fd6f1e1f3bc2d2a023c27f4bcc1a12523767d55de7508c0b21a160ab1fdbf", "054-6680240", null });

            migrationBuilder.InsertData(
                table: "UserInRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, new Guid("482f8b0e-1fe7-4ea6-9c10-4726a859b627") });

            migrationBuilder.CreateIndex(
                name: "IX_TrasportRequests_UserId",
                table: "TrasportRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRoles_UserId",
                table: "UserInRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrasportRequests");

            migrationBuilder.DropTable(
                name: "UserInRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
