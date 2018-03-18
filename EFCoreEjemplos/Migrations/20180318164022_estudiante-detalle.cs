using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EFCoreEjemplos.Migrations
{
    public partial class estudiantedetalle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Becado",
                table: "Estudiantes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Carrera",
                table: "Estudiantes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoriaDePago",
                table: "Estudiantes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Becado",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "Carrera",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "CategoriaDePago",
                table: "Estudiantes");
        }
    }
}
