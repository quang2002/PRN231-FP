using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FP_FAP.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportTicket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IssuerId = table.Column<int>(type: "int", nullable: false),
                    AssigneeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    File = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportTicket_User_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupportTicket_User_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SupportTicketMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SupportTicketId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicketMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportTicketMessage_SupportTicket_SupportTicketId",
                        column: x => x.SupportTicketId,
                        principalTable: "SupportTicket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupportTicketMessage_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_AssigneeId",
                table: "SupportTicket",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_IssuerId",
                table: "SupportTicket",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicketMessage_SenderId",
                table: "SupportTicketMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicketMessage_SupportTicketId",
                table: "SupportTicketMessage",
                column: "SupportTicketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportTicketMessage");

            migrationBuilder.DropTable(
                name: "SupportTicket");
        }
    }
}
