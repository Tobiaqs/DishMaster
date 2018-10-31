using Microsoft.EntityFrameworkCore.Migrations;

namespace wiedoetdeafwas.Migrations
{
    public partial class ScoreToDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Score",
                table: "GroupMembers",
                nullable: false,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Score",
                table: "GroupMembers",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
