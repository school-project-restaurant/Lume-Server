using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lume.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedReservationsIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string[]>(
                name: "ReservationsId",
                table: "Tables",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(List<int>),
                oldType: "integer[]",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<int>>(
                name: "ReservationsId",
                table: "Tables",
                type: "integer[]",
                nullable: true,
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true);
        }
    }
}
