using EasyParkingAPI.Data;
using EasyParkingAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Model;
using Model.Enums;
using ServiceWebApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyParkingAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ReservaController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _UserId;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EasyParkingAuthContext _EasyParkingAuthContext;

        public ReservaController(IConfiguration configuration, EasyParkingAuthContext EasyParkingAuthContext,
                                            IHttpContextAccessor httpContextAccessor,
                                            UserManager<ApplicationUser> userManager)
        {
            try
            {
                _configuration = configuration;
                _userManager = userManager; 
                _EasyParkingAuthContext = EasyParkingAuthContext;

                HttpContext http = httpContextAccessor.HttpContext;
                var user = http.User;

                ApplicationUser appuser = _userManager.FindByNameAsync(user.Identity.Name).Result; // Obtengo los datos del usuario logeado

                _UserId = appuser.Id; // Obtengo el ID del usuario logeado

                if (String.IsNullOrEmpty(_UserId) | String.IsNullOrWhiteSpace(_UserId))
                {
                    throw new Exception("ERROR ... Usuario sin permisos necesarios.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Reserva>>> GetAllAsync()
        {
            try
            {
                DataContext dataContext = new DataContext();
                var lista = await dataContext.Reservas.AsNoTracking().ToListAsync();
                if (lista == null)
                {
                    return NotFound();
                }
                return lista;

            }
            catch (Exception e)
            {

                return BadRequest(Tools.Tools.ExceptionMessage(e));
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<ReservaDTO>>> GetMisReservasAsync()
        {
            try
            {
                DataContext dataContext = new DataContext();

                var reservas = dataContext.Reservas.Where(x => x.UserId == _UserId).AsNoTracking().ToList();

                List<ReservaDTO> listaDTO = new List<ReservaDTO>();

                foreach (var item in reservas)
                {
                    ServiceWebApi.DTO.ReservaDTO reservaDTO = new ServiceWebApi.DTO.ReservaDTO();
                    reservaDTO = Tools.Tools.PropertyCopier<Reserva, ServiceWebApi.DTO.ReservaDTO>.Copy(item, reservaDTO);
                    reservaDTO.Estacionamiento = await dataContext.Estacionamientos.Include("TiposDeVehiculosAdmitidos").AsNoTracking().Where(x=>x.Id == item.EstacionamientoId).FirstOrDefaultAsync();
                    listaDTO.Add(reservaDTO);
                }

                if (reservas == null)
                {
                    return NotFound();
                }

                return listaDTO;
            }
            catch (Exception e)
            {

                return BadRequest(Tools.Tools.ExceptionMessage(e));
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<ReservaDTO>>> GetReservasModalidadDueñoAsync()
        {
            try
            {
                DataContext dataContext = new DataContext();

                //List<ReservaDTO> lista = await dataContext.Reservas.Where(x => x.UserId == _UserId && x.Estado == EstadoReserva.ESPERANDO_ARRIBO)

                //    .Select(z => new ReservaDTO {  Nombre = z.Nombre, Apellido = z.Apellido, TipoDeVehiculo = z.TipoDeVehiculo, FechaDeCreacion = z.FechaDeCreacion,
                //                                    FechaDeExpiracion = z.FechaDeExpiracion,
                //                                    FotoDePerfil = z.FotoDePerfil, Patente = z.Patente, Estado = z.Estado,
                //                                    Id = z.Id}).ToListAsync();

               // var usuarios = await _EasyParkingAuthContext.Users.AsNoTracking().Select(z => z.Id).ToListAsync();

                //List<int> misEstacionamientosId = await dataContext.Estacionamientos.Where(x => x.UserId == _UserId).AsNoTracking().Select( x => x.Id).ToListAsync();
                //List<Reserva> reservas = (from estId in misEstacionamientosId
                //                        join res in dataContext.Reservas on estId equals res.Id
                //                        where res.Estado == EstadoReserva.ESPERANDO_ARRIBO
                //                        select res).ToList();

                //List<ReservaDTO> lista_reservas = await 
                //                            (from est in dataContext.Estacionamientos
                //                                  join res in dataContext.Reservas on est.Id equals res.EstacionamientoId 
                //                                  join us in _EasyParkingAuthContext.Users on res.UserId equals us.Id
                //                                  join v in dataContext.Vehiculos on us.Id equals v.UserId
                //                                    where (res.Estado == EstadoReserva.ESPERANDO_ARRIBO && est.UserId == _UserId) // el userId aca es del dueño
                //                                        select new ReservaDTO { Nombre = us.Nombre, Apellido = us.Apellido, FotoDePerfil = us.FotoDePerfil, TipoDeVehiculo = v.TipoDeVehiculo, Estado = res.Estado, FechaDeCreacion = res.FechaDeCreacion
                //                                        , FechaDeExpiracion = res.FechaDeExpiracion, Patente = v.Patente}
                //                                            ).ToListAsync();
                List<ReservaDTO> qwery1 = await
                            (from est in dataContext.Estacionamientos
                             join res in dataContext.Reservas on est.Id equals res.EstacionamientoId
                             where (res.Estado == EstadoReserva.ESPERANDO_ARRIBO && est.UserId == _UserId) // el userId aca es del dueño
                             select new ReservaDTO
                             {  
                                 UserId = res.UserId,
                                 Estado = res.Estado,
                                 FechaDeCreacion = res.FechaDeCreacion,
                                 FechaDeExpiracion = res.FechaDeExpiracion,
                                 Patente = res.Patente,
                                 VehiculoId = res.VehiculoId
                             }
                             ).AsNoTracking().ToListAsync();

                List<ReservaDTO> qwery2 = 
                            (from q in qwery1
                            join us in _EasyParkingAuthContext.Users.AsNoTracking() on q.UserId equals us.Id
                            select new ReservaDTO
                            {
                                UserId = us.Id,
                                Estado = q.Estado,
                                FechaDeCreacion = q.FechaDeCreacion,
                                FechaDeExpiracion = q.FechaDeExpiracion,
                                Patente = q.Patente,
                                VehiculoId = q.VehiculoId,
                                FotoDePerfil = us.FotoDePerfil,
                                Nombre = us.Nombre,
                                Apellido = us.Apellido

                            }
                            ).ToList();


                //foreach (var item in qwery2)
                //{
                //   var vehiculo = await dataContext.Vehiculos.Where(x => x.Patente == item.Patente).de();
                //   item.TipoDeVehiculo = vehiculo.TipoDeVehiculo;
                //}

                var xxx = await dataContext.Vehiculos.AsNoTracking().ToListAsync();

                List<ReservaDTO> qwery3 =
                          (from q in qwery2
                           join v in xxx on q.Patente equals v.Patente
                           select new ReservaDTO
                           {
                               UserId = q.UserId,
                               Estado = q.Estado,
                               FechaDeCreacion = q.FechaDeCreacion,
                               FechaDeExpiracion = q.FechaDeExpiracion,
                               FotoDePerfil = q.FotoDePerfil,
                               Nombre = q.Nombre,
                               Apellido = q.Apellido,
                               Patente = v.Patente,
                               TipoDeVehiculo = v.TipoDeVehiculo
                           }
                           ).ToList();

                //var x = dataContext.Vehiculos.AsNoTracking().ToListAsync();

                //var qwery3 = qwery2.Join(dataContext.Vehiculos.AsNoTracking(), q => q.Patente, v => v.Patente,
                //(q, v) => new ReservaDTO
                //{
                //    UserId = q.UserId,
                //    Estado = q.Estado,
                //    FechaDeCreacion = q.FechaDeCreacion,
                //    FechaDeExpiracion = q.FechaDeExpiracion,
                //    FotoDePerfil = q.FotoDePerfil,
                //    Nombre = q.Nombre,
                //    Apellido = q.Apellido,
                //    Patente = v.Patente,
                //    TipoDeVehiculo = v.TipoDeVehiculo
                //}).ToList();

                if (qwery3 == null)
                {
                    return NotFound();
                }

                return qwery3;
            }
            catch (Exception e)
            {

                return BadRequest(Tools.Tools.ExceptionMessage(e));
            }
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, AppUser")]
        public async Task<ActionResult> AddAsync([FromBody] Reserva reserva)
        {
            try
            {
                DataContext dataContext = new DataContext();
                reserva.UserId = _UserId;
                await dataContext.Reservas.AddAsync(reserva);
                await dataContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(Tools.Tools.ExceptionMessage(e));
            }
        }


        [HttpDelete("[action]/{estacionamientoId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, AppUser")]
        public async Task<ActionResult> DeleteAsync(int reservaId)
        {
            try
            {
                DataContext dataContext = new DataContext();
                var reserva = await dataContext.Reservas.FirstOrDefaultAsync(x => x.Id == reservaId && x.UserId == _UserId);
                dataContext.Reservas.Remove(reserva);
                await dataContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(Tools.Tools.ExceptionMessage(e));
            }
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, AppUser")]
        public async Task<ActionResult> SetReservaCanceladaAsync([FromBody] int reservaId)
        {
            try
            {
                DataContext dataContext = new DataContext();
                var reserva = dataContext.Reservas.Where(x => x.Id == reservaId).FirstOrDefault();
                reserva.Estado = EstadoReserva.CANCELADO;
                dataContext.Reservas.Update(reserva);
                await dataContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(Tools.Tools.ExceptionMessage(e));
            }
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, AppUser")]
        public async Task<ActionResult> SetReservaArriboExitosoAsync([FromBody] int reservaId)
        {
            try
            {
                DataContext dataContext = new DataContext();
                var reserva = dataContext.Reservas.Where(x => x.Id == reservaId).FirstOrDefault();
                reserva.Estado = EstadoReserva.ARRIBO_EXITOSO;
                dataContext.Reservas.Update(reserva);
                await dataContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(Tools.Tools.ExceptionMessage(e));
            }
        }

    }


}
