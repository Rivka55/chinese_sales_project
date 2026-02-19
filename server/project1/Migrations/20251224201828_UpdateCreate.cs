using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project1.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Password",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "Identity",
                table: "Donors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Carts",
                newName: "IsPurchased");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "user");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Gifts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "Donors",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Carts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_CategoryId1",
                table: "Gifts",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Donors_IdentityNumber",
                table: "Donors",
                column: "IdentityNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Categories_CategoryId1",
                table: "Gifts",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Categories_CategoryId1",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_CategoryId1",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_Donors_IdentityNumber",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Donors",
                newName: "Identity");

            migrationBuilder.RenameColumn(
                name: "IsPurchased",
                table: "Carts",
                newName: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Password",
                table: "Users",
                column: "Password",
                unique: true);
        }
    }
}
