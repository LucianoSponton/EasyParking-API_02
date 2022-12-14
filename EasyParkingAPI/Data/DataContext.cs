using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model;
using Model.Enums;
using System;

namespace EasyParkingAPI.Data
{
    public class DataContext : DbContext
    {
        private readonly SqlConnection _SqlConnection;
        private readonly string _connectionString;

        public DataContext()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();
            _connectionString = configuration.GetValue<string>("ConnectionString");
            _SqlConnection = new SqlConnection(_connectionString);
        }

        public DbSet<Estacionamiento> Estacionamientos { get; set; }
        public DbSet<Jornada> Jornadas { get; set; }
        public DbSet<DataVehiculoAlojado> DataVehiculoAlojados { get; set; }
        public DbSet<RangoH> RangoHs { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_SqlConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //***************************************************************************************************
            // ESTACIONAMIENTO
            //***************************************************************************************************
            modelBuilder.Entity<Estacionamiento>()
                .HasKey(c => c.Id);

            //***************************************************************************************************
            // FAVORITO
            //***************************************************************************************************
            modelBuilder.Entity<Favorito>()
                .HasKey(c => c.Id);

            //***************************************************************************************************
            // VEHICULO
            //***************************************************************************************************

            modelBuilder.Entity<Vehiculo>()
                .HasKey(c => c.Id);
             
            modelBuilder.Entity<Vehiculo>()
                .HasIndex(c => c.Patente).IsUnique();

            //***************************************************************************************************
            // RESERVA
            //***************************************************************************************************

            modelBuilder.Entity<Reserva>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Reserva>()
               .Property(e => e.Estado)
               .HasConversion(
                   v => v.ToString(),
                   v => (EstadoReserva)Enum.Parse(typeof(EstadoReserva), v));

            //***************************************************************************************************
            // JORNADA
            //***************************************************************************************************
            modelBuilder.Entity<Jornada>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Jornada>()
               .HasOne(i => i.Estacionamiento)
               .WithMany(c => c.Jornadas);

            modelBuilder.Entity<Jornada>()
               .Property(e => e.DiaDeLaSemana)
               .HasConversion(
                   v => v.ToString(),
                   v => (Dia)Enum.Parse(typeof(Dia), v));

            //***************************************************************************************************
            // DATA VEHICULO ALOJADO
            //***************************************************************************************************
            modelBuilder.Entity<DataVehiculoAlojado>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<DataVehiculoAlojado>()
               .HasOne(i => i.Estacionamiento)
               .WithMany(c => c.TiposDeVehiculosAdmitidos);

            //***************************************************************************************************
            // RANGO H
            //***************************************************************************************************
            modelBuilder.Entity<RangoH>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<RangoH>()
               .HasOne(i => i.Jornada)
               .WithMany(c => c.Horarios);
        }
    }
}
