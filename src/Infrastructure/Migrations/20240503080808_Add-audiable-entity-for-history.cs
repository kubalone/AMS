using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AML.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addaudiableentityforhistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "TransactionLimits");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "TransactionLimits");

            migrationBuilder.RenameColumn(
                name: "ChangeDate",
                table: "LimitChangesHistory",
                newName: "LastModified");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                table: "LimitChangesHistory",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "LimitChangesHistory");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "LimitChangesHistory",
                newName: "ChangeDate");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                table: "TransactionLimits",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModified",
                table: "TransactionLimits",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
