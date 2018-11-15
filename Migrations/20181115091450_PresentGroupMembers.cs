using Microsoft.EntityFrameworkCore.Migrations;

namespace wiedoetdeafwas.Migrations
{
    public partial class PresentGroupMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_TaskGroupRecords_TaskGroupRecordId",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_TaskGroupRecordId",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "TaskGroupRecordId",
                table: "GroupMembers");

            migrationBuilder.CreateTable(
                name: "PresentGroupMembers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    GroupMemberId = table.Column<string>(nullable: true),
                    TaskGroupRecordId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentGroupMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresentGroupMembers_GroupMembers_GroupMemberId",
                        column: x => x.GroupMemberId,
                        principalTable: "GroupMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PresentGroupMembers_TaskGroupRecords_TaskGroupRecordId",
                        column: x => x.TaskGroupRecordId,
                        principalTable: "TaskGroupRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PresentGroupMembers_GroupMemberId",
                table: "PresentGroupMembers",
                column: "GroupMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentGroupMembers_TaskGroupRecordId",
                table: "PresentGroupMembers",
                column: "TaskGroupRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PresentGroupMembers");

            migrationBuilder.AddColumn<string>(
                name: "TaskGroupRecordId",
                table: "GroupMembers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_TaskGroupRecordId",
                table: "GroupMembers",
                column: "TaskGroupRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_TaskGroupRecords_TaskGroupRecordId",
                table: "GroupMembers",
                column: "TaskGroupRecordId",
                principalTable: "TaskGroupRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
