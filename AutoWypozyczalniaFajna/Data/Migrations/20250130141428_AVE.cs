using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoWypozyczalniaFajna.Data.Migrations
{
    /// <inheritdoc />
    public partial class AVE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        
           
            migrationBuilder.CreateTable(
                name: "Marka",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarkaNazwa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marka", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypPaliwa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Paliwo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypPaliwa", x => x.Id);
                });

       
  
          
            migrationBuilder.CreateTable(
                name: "Samochod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarkaId = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TypPaliwaId = table.Column<int>(type: "int", nullable: false),
                    NrRejestracyjny = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CenaZaDzien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samochod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Samochod_Marka_MarkaId",
                        column: x => x.MarkaId,
                        principalTable: "Marka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Samochod_TypPaliwa_TypPaliwaId",
                        column: x => x.TypPaliwaId,
                        principalTable: "TypPaliwa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wypozyczenie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SamochodId = table.Column<int>(type: "int", nullable: false),
                    DataWypozyczenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataZwrotu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CenaCalkowita = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wypozyczenie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wypozyczenie_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wypozyczenie_Samochod_SamochodId",
                        column: x => x.SamochodId,
                        principalTable: "Samochod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

          
         
          

            migrationBuilder.CreateIndex(
                name: "IX_Samochod_MarkaId",
                table: "Samochod",
                column: "MarkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Samochod_TypPaliwaId",
                table: "Samochod",
                column: "TypPaliwaId");

            migrationBuilder.CreateIndex(
                name: "IX_Wypozyczenie_SamochodId",
                table: "Wypozyczenie",
                column: "SamochodId");

            migrationBuilder.CreateIndex(
                name: "IX_Wypozyczenie_UserId",
                table: "Wypozyczenie",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Wypozyczenie");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Samochod");

            migrationBuilder.DropTable(
                name: "Marka");

            migrationBuilder.DropTable(
                name: "TypPaliwa");
        }
    }
}
