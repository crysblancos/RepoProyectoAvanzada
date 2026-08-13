using Proyecto_Grupo02.Data;
using Proyecto_Grupo02.EF;
using Proyecto_Grupo02.Models;
using System;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;


namespace Proyecto_Grupo02.Services
{
    public class PersonalShopperService : IPersonalShopperService
    {
        private readonly CatalogoDbContext _context;

        public PersonalShopperService()
        {
            _context = new CatalogoDbContext();
        }

        public async Task<bool> RegistrarSolicitudAsync(
            int idUsuario,
            PersonalShopperModel model)
        {
            var idPendiente = await _context.Estados
                .Where(e => e.NombreEstado == "Pendiente")
                .Select(e => e.IdEstado)
                .FirstOrDefaultAsync();

            if (idPendiente == 0)
            {
                return false;
            }

            _context.SolicitudesShopper.Add(
                new tbSolicitudShopper
                {
                    IdUsuario = idUsuario,
                    EstiloBuscado = model.EstiloBuscado,
                    Talla = model.Talla,
                    Presupuesto = model.Presupuesto,
                    Necesidades = model.Necesidades,
                    Fecha = DateTime.Now,
                    IdEstado = idPendiente
                });

            return await _context.SaveChangesAsync() > 0;
        }
    }
}