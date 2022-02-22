using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsPlus.Data.Migrations
{
    public partial class UpdateNullabaleCategoryDesc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "SysAppRoles",
                keyColumn: "Id",
                keyValue: new Guid("1b60fd43-a1b5-4214-9ccc-d239f0f4c97c"),
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2022, 2, 16, 16, 43, 55, 649, DateTimeKind.Local).AddTicks(907), new DateTime(2022, 2, 16, 16, 43, 55, 649, DateTimeKind.Local).AddTicks(924) });

            migrationBuilder.UpdateData(
                table: "SysAppUsers",
                keyColumn: "Id",
                keyValue: new Guid("1b60fd43-a1b5-4214-9ccc-d239f0f4c97b"),
                columns: new[] { "CreatedDate", "JoinedDate" },
                values: new object[] { new DateTime(2022, 2, 16, 16, 43, 55, 649, DateTimeKind.Local).AddTicks(1243), new DateTimeOffset(new DateTime(2022, 2, 16, 16, 43, 55, 649, DateTimeKind.Unspecified).AddTicks(1168), new TimeSpan(0, 7, 0, 0, 0)) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "SysAppRoles",
                keyColumn: "Id",
                keyValue: new Guid("1b60fd43-a1b5-4214-9ccc-d239f0f4c97c"),
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2022, 2, 16, 16, 18, 35, 90, DateTimeKind.Local).AddTicks(3593), new DateTime(2022, 2, 16, 16, 18, 35, 90, DateTimeKind.Local).AddTicks(3606) });

            migrationBuilder.UpdateData(
                table: "SysAppUsers",
                keyColumn: "Id",
                keyValue: new Guid("1b60fd43-a1b5-4214-9ccc-d239f0f4c97b"),
                columns: new[] { "CreatedDate", "JoinedDate" },
                values: new object[] { new DateTime(2022, 2, 16, 16, 18, 35, 90, DateTimeKind.Local).AddTicks(3850), new DateTimeOffset(new DateTime(2022, 2, 16, 16, 18, 35, 90, DateTimeKind.Unspecified).AddTicks(3788), new TimeSpan(0, 7, 0, 0, 0)) });
        }
    }
}
