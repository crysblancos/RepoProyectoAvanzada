using Proyecto_Grupo02.Data;
using Proyecto_Grupo02.EF;
using Proyecto_Grupo02.Models;
using System;
using System.Threading.Tasks;

namespace Proyecto_Grupo02.Services
{
    public class ContactoService : IContactoService
    {
        private readonly CatalogoDbContext _context;

        public ContactoService()
        {
            _context = new CatalogoDbContext();
        }

        public async Task<bool> RegistrarAsync(ContactoModel model)
        {
            _context.Contactos.Add(new tbContacto
            {
                Nombre = model.Nombre,
                Correo = model.Correo,
                Asunto = model.Asunto,
                Mensaje = model.Mensaje,
                Fecha = DateTime.Now,
                IdEstado = EstadosConsts.Activo
            });

            return await _context.SaveChangesAsync() > 0;
        }
    }
}