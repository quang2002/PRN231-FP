using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FP_FAP.Migrations
{
    public partial class Initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "SupportTicket");

            migrationBuilder.DropColumn(
                name: "File",
                table: "SupportTicket");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "SupportTicketMessage",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<byte[]>(
                name: "Attachment",
                table: "SupportTicketMessage",
                type: "longblob",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentName",
                table: "SupportTicketMessage",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "SupportTicket",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "SupportTicketMessage");

            migrationBuilder.DropColumn(
                name: "AttachmentName",
                table: "SupportTicketMessage");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "SupportTicketMessage",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "SupportTicket",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SupportTicket",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "SupportTicket",
                type: "longblob",
                nullable: false);
        }
    }
}
