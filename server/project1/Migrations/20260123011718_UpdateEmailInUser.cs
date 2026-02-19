using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project1.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmailInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Categories_CategoryId",
                table: "Gifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Categories_CategoryId1",
                table: "Gifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Donors_DonorId",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_CategoryId1",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Gifts");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Categories_CategoryId",
                table: "Gifts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Donors_DonorId",
                table: "Gifts",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Categories_CategoryId",
                table: "Gifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Donors_DonorId",
                table: "Gifts");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Gifts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_CategoryId1",
                table: "Gifts",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Categories_CategoryId",
                table: "Gifts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Categories_CategoryId1",
                table: "Gifts",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Donors_DonorId",
                table: "Gifts",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
