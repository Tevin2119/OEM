using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OEMRPS.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RockPaperScissorsGame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Player1 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Player2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    RoundsToWin = table.Column<int>(type: "int", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    RandomMode = table.Column<bool>(type: "bit", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockPaperScissorsGame", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RockPaperScissorsRounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Player1Choice = table.Column<int>(type: "int", nullable: false),
                    Player2Choice = table.Column<int>(type: "int", nullable: false),
                    Winner = table.Column<int>(type: "int", nullable: false),
                    RPSGameId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockPaperScissorsRounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RockPaperScissorsRounds_RockPaperScissorsGame_RPSGameId",
                        column: x => x.RPSGameId,
                        principalTable: "RockPaperScissorsGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RockPaperScissorsRounds_RPSGameId",
                table: "RockPaperScissorsRounds",
                column: "RPSGameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RockPaperScissorsRounds");

            migrationBuilder.DropTable(
                name: "RockPaperScissorsGame");
        }
    }
}
