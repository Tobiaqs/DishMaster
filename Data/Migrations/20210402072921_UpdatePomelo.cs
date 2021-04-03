using Microsoft.EntityFrameworkCore.Migrations;

namespace DishMaster.Data.Migrations
{
    public partial class UpdatePomelo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TaskGroupId",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "TaskGroups",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TaskGroups",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TaskGroupId",
                table: "TaskGroupRecords",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TaskGroupRecords",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TaskId",
                table: "TaskGroupMemberLinks",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaskGroupRecordId",
                table: "TaskGroupMemberLinks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "GroupMemberId",
                table: "TaskGroupMemberLinks",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TaskGroupMemberLinks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TaskGroupRecordId",
                table: "PresentGroupMembers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GroupMemberId",
                table: "PresentGroupMembers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "PresentGroupMembers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Groups",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PersonId",
                table: "GroupMembers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "GroupMembers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "GroupMembers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetUserRoles",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserRoles",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserClaims",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetRoles",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TaskGroupId",
                table: "Tasks",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Tasks",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "TaskGroups",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TaskGroups",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TaskGroupId",
                table: "TaskGroupRecords",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TaskGroupRecords",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TaskId",
                table: "TaskGroupMemberLinks",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaskGroupRecordId",
                table: "TaskGroupMemberLinks",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "GroupMemberId",
                table: "TaskGroupMemberLinks",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TaskGroupMemberLinks",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TaskGroupRecordId",
                table: "PresentGroupMembers",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GroupMemberId",
                table: "PresentGroupMembers",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "PresentGroupMembers",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Groups",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "PersonId",
                table: "GroupMembers",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "GroupMembers",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "GroupMembers",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserTokens",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetUserRoles",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserRoles",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserLogins",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserClaims",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetRoles",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
