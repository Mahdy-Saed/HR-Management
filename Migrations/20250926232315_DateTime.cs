using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Carrer.Migrations
{
    /// <inheritdoc />
    public partial class DateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // for solve the problem of converting from date to timestamp with time zone
             migrationBuilder.Sql("ALTER TABLE \"Requests\" ALTER COLUMN \"Type\" TYPE integer USING trim(\"Type\")::integer;");
            migrationBuilder.Sql("ALTER TABLE \"Requests\" ALTER COLUMN \"Request_Date\" TYPE timestamp with time zone USING \"Request_Date\"::timestamp with time zone;");
            migrationBuilder.Sql("ALTER TABLE \"Requests\" ALTER COLUMN \"Approved_Date\" TYPE timestamp with time zone USING \"Approved_Date\"::timestamp with time zone;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // إعادة الأنواع كما كانت سابقًا
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Requests",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Request_Date",
                table: "Requests",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Approved_Date",
                table: "Requests",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
