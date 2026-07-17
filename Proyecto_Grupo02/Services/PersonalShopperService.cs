using Proyecto_Grupo02.Data;
using Proyecto_Grupo02.EF;
using Proyecto_Grupo02.Models;
using System;
using System.Threading.Tasks;

namespace Proyecto_Grupo02.Services
{
    public class PersonalShopperService : IPersonalShopperService
    {
        private readonly CatalogoDbContext _context;

        public PersonalShopperService()
        {
            _context = new CatalogoDbContext();
        }

        public async Task<bool> RegistrarSolicitudAsync(int idUsuario, PersonalShopperModel model)
        {
            _context.SolicitudesShopper.Add(new tbSolicitudShopper
            {
                IdUsuario = idUsuario,
                EstiloBuscado = model.EstiloBuscado,
                Talla = model.Talla,
                Presupuesto = model.Presupuesto,
                Necesidades = model.Necesidades,
                Fecha = DateTime.Now,
                IdEstado = EstadosConsts.Activo
            });

            return await _context.SaveChangesAsync() > 0;
        }
    }
}