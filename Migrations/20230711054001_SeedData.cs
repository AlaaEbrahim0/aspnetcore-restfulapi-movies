using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MoviesApi.Helpers;

#nullable disable

namespace MoviesApi.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] {"Id", "Name", "NormalizedName", "ConcurrencyStamp"},
                values: new object[] {
                    Guid.NewGuid().ToString(), 
                    Roles.User.ToString(), 
                    Roles.User.ToString().ToUpper(),
				    Guid.NewGuid().ToString(), }
			);

			migrationBuilder.InsertData(
				table: "AspNetRoles",
				columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
				values: new object[] {
					Guid.NewGuid().ToString(),
					Roles.Admin.ToString(),
					Roles.Admin.ToString().ToUpper(),
					Guid.NewGuid().ToString(), }
			);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [AspNetRoles]");
        }
    }
}
