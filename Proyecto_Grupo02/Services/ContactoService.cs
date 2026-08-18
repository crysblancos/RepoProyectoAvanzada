using Proyecto_Grupo02.Data;
using Proyecto_Grupo02.EF;
using Proyecto_Grupo02.Models;
using System;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;


namespace Proyecto_Grupo02.Services
{
    public class ContactoService : IContactoService
    {
        private readonly CatalogoDbContext _context;

        public ContactoService()
        {
            _context = new CatalogoDbContext();
        }

        public async Task<bool> RegistrarAsync(
            ContactoModel model)
        {
            var estadoPendiente = await _context.Estados
                .FirstOrDefaultAsync(e => e.NombreEstado == "Pendiente");

            if (estadoPendiente == null)
            {
                estadoPendiente = new tbEstado { NombreEstado = "Pendiente" };
                _context.Estados.Add(estadoPendiente);
                await _context.SaveChangesAsync();
            }

            _context.Contactos.Add(new tbContacto
            {
                Nombre = model.Nombre,
                Correo = model.Correo,
                Asunto = model.Asunto,
                Mensaje = model.Mensaje,
                Fecha = DateTime.Now,
                IdEstado = estadoPendiente.IdEstado
            });

            return await _context.SaveChangesAsync() > 0;
        }
    }
}