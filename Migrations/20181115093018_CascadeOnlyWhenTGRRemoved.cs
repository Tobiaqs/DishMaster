using Microsoft.EntityFrameworkCore.Migrations;

namespace wiedoetdeafwas.Migrations
{
    public partial class CascadeOnlyWhenTGRRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PresentGroupMembers_GroupMembers_GroupMemberId",
                table: "PresentGroupMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_PresentGroupMembers_GroupMembers_GroupMemberId",
                table: "PresentGroupMembers",
                column: "GroupMemberId",
                principalTable: "GroupMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PresentGroupMembers_GroupMembers_GroupMemberId",
                table: "PresentGroupMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_PresentGroupMembers_GroupMembers_GroupMemberId",
                table: "PresentGroupMembers",
                column: "GroupMemberId",
                principalTable: "GroupMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
