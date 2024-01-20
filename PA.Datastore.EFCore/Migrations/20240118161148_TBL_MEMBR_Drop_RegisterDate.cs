using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PA.Datastore.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class TBL_MEMBR_Drop_RegisterDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisteredDate",
                table: "Members");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredDate",
                table: "Members",
                type: "datetime2",
                nullable: true);
        }
    }
}
