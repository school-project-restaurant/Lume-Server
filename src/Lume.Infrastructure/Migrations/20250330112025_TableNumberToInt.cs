using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lume.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TableNumberToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create a temporary column with the new type
            migrationBuilder.AddColumn<int>(
                name: "TableNumber_New",
                table: "Reservations",
                type: "integer",
                nullable: true);
            
            // Extract first element from array (or use default if empty)
            migrationBuilder.Sql(@"
            UPDATE ""Reservations"" 
            SET ""TableNumber_New"" = 
                CASE 
                    WHEN array_length(""TableNumber"", 1) > 0 THEN ""TableNumber""[1]
                    ELSE 0
                END");
            
            // Drop the old column
            migrationBuilder.DropColumn(
                name: "TableNumber",
                table: "Reservations");
            
            // Rename the new column to the original name
            migrationBuilder.RenameColumn(
                name: "TableNumber_New",
                table: "Reservations",
                newName: "TableNumber");
            
            // Set the column as non-nullable
            migrationBuilder.AlterColumn<int>(
                name: "TableNumber",
                table: "Reservations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Convert back to an array type
            migrationBuilder.AlterColumn<int[]>(
                name: "TableNumber",
                table: "Reservations",
                type: "integer[]",
                nullable: false);
            
            // Convert single values back to arrays
            migrationBuilder.Sql(@"
            UPDATE ""Reservations"" 
            SET ""TableNumber"" = ARRAY[""TableNumber""]");
        }
    }
}
