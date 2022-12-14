﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyParkingAPI.Migrations
{
    public partial class _0005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Reservas",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FotoDePerfil",
                table: "Reservas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Reservas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoDeVehiculo",
                table: "Reservas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "FotoDePerfil",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "TipoDeVehiculo",
                table: "Reservas");
        }
    }
}
