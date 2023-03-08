using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    public partial class CreatePhoneTagTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Sliders",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AddColumn<int>(
                name: "PhoneTagId",
                table: "Phones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PhoneTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneTags", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Phones_PhoneTagId",
                table: "Phones",
                column: "PhoneTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Phones_PhoneTags_PhoneTagId",
                table: "Phones",
                column: "PhoneTagId",
                principalTable: "PhoneTags",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Phones_PhoneTags_PhoneTagId",
                table: "Phones");

            migrationBuilder.DropTable(
                name: "PhoneTags");

            migrationBuilder.DropIndex(
                name: "IX_Phones_PhoneTagId",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "PhoneTagId",
                table: "Phones");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Sliders",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80,
                oldNullable: true);
        }
    }
}
