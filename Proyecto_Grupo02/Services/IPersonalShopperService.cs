using Proyecto_Grupo02.Models;
using System.Threading.Tasks;

namespace Proyecto_Grupo02.Services
{
    public interface IPersonalShopperService
    {
        Task<bool> RegistrarSolicitudAsync(int idUsuario, PersonalShopperModel model);
    }
}