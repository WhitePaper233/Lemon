using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lemon.Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NickName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    Salt = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "NickName", "PasswordHash", "PhoneNumber", "Salt", "UserName" },
                values: new object[,]
                {
                    { new Guid("ce7d725b-09be-400a-9f51-0185dd98af74"), "Robert@mail.com", "Robert St", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, "12345678902", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, "Robert" },
                    { new Guid("e22e0709-bf31-44ec-ba27-f4158d796539"), null, "John Ave", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, "12345678901", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
