using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormCraft.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class add_version_to_form : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "version",
                table: "form",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "version",
                table: "form");
        }
    }
}
