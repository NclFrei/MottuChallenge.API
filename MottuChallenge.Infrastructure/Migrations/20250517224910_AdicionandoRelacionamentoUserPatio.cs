﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottuChallenge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoRelacionamentoUserPatio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Patios",
                type: "RAW(16)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Patios_UserId",
                table: "Patios",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patios_User_UserId",
                table: "Patios",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patios_User_UserId",
                table: "Patios");

            migrationBuilder.DropIndex(
                name: "IX_Patios_UserId",
                table: "Patios");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Patios");
        }
    }
}
