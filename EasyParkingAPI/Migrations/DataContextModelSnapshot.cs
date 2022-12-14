﻿// <auto-generated />
using System;
using EasyParkingAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EasyParkingAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Model.DataVehiculoAlojado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CapacidadDeAlojamiento")
                        .HasColumnType("int");

                    b.Property<int?>("EstacionamientoId")
                        .HasColumnType("int");

                    b.Property<decimal>("Tarifa_Dia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Tarifa_Hora")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Tarifa_Mes")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Tarifa_Semana")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TipoDeVehiculo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EstacionamientoId");

                    b.ToTable("DataVehiculoAlojados");
                });

            modelBuilder.Entity("Model.Estacionamiento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ciudad")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Imagen")
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("Inactivo")
                        .HasColumnType("bit");

                    b.Property<double>("Latitud")
                        .HasColumnType("float");

                    b.Property<double>("Longitud")
                        .HasColumnType("float");

                    b.Property<decimal>("MontoReserva")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Observaciones")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PublicacionPausada")
                        .HasColumnType("bit");

                    b.Property<string>("TipoDeLugar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Estacionamientos");
                });

            modelBuilder.Entity("Model.Favorito", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EstacionamientoId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Favoritos");
                });

            modelBuilder.Entity("Model.Jornada", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DiaDeLaSemana")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EstacionamientoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EstacionamientoId");

                    b.ToTable("Jornadas");
                });

            modelBuilder.Entity("Model.RangoH", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DesdeHora")
                        .HasColumnType("int");

                    b.Property<int>("DesdeMinuto")
                        .HasColumnType("int");

                    b.Property<int>("HastaHora")
                        .HasColumnType("int");

                    b.Property<int>("HastaMinuto")
                        .HasColumnType("int");

                    b.Property<int?>("JornadaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("JornadaId");

                    b.ToTable("RangoHs");
                });

            modelBuilder.Entity("Model.Reserva", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CodigoDeValidacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EstacionamientoId")
                        .HasColumnType("int");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaDeCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaDeExpiracion")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Monto")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Patente")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VehiculoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Reservas");
                });

            modelBuilder.Entity("Model.Vehiculo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Patente")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TipoDeVehiculo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Patente")
                        .IsUnique()
                        .HasFilter("[Patente] IS NOT NULL");

                    b.ToTable("Vehiculos");
                });

            modelBuilder.Entity("Model.DataVehiculoAlojado", b =>
                {
                    b.HasOne("Model.Estacionamiento", "Estacionamiento")
                        .WithMany("TiposDeVehiculosAdmitidos")
                        .HasForeignKey("EstacionamientoId");
                });

            modelBuilder.Entity("Model.Jornada", b =>
                {
                    b.HasOne("Model.Estacionamiento", "Estacionamiento")
                        .WithMany("Jornadas")
                        .HasForeignKey("EstacionamientoId");
                });

            modelBuilder.Entity("Model.RangoH", b =>
                {
                    b.HasOne("Model.Jornada", "Jornada")
                        .WithMany("Horarios")
                        .HasForeignKey("JornadaId");
                });
#pragma warning restore 612, 618
        }
    }
}
