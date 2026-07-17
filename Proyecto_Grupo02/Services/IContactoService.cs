using Proyecto_Grupo02.Models;
using System.Threading.Tasks;

namespace Proyecto_Grupo02.Services
{
    public interface IContactoService
    {
        Task<bool> RegistrarAsync(ContactoModel model);
    }
}