using System.Collections.Generic;
using System.Web.Mvc;

namespace Proyecto_Grupo02.Models
{
    public class AdministradorPrincipalViewModel
    {
        public int TotalProductos { get; set; }

        public int TotalClientes { get; set; }

        public int TotalPedidos { get; set; }

        public int TotalSucursales { get; set; }

        public int PromocionesActivas { get; set; }

        public int TotalVendedores { get; set; }
    }


    public class AdministradorProductoViewModel
    {
        public int IdProducto { get; set; }

        public string Nombre { get; set; }

        public string Categoria { get; set; }

        public decimal Precio { get; set; }

        public string Talla { get; set; }

        public string Color { get; set; }

        public string Imagen { get; set; }

        public string Estado { get; set; }
    }

    public class AdministradorCategoriaViewModel
    {
        public int IdCategoria { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string Estado { get; set; }
    }


    public class AdministradorCategoriaFormularioViewModel
    {
        public int IdCategoria { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int IdEstado { get; set; }

        public IEnumerable<SelectListItem> Estados { get; set; }
    }


    public class AdministradorProductoFormularioViewModel
    {
        public int IdProducto { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public decimal Precio { get; set; }

        public string Imagen { get; set; }

        public string Talla { get; set; }

        public string Color { get; set; }

        public bool Destacado { get; set; }

        public bool Novedad { get; set; }

        public int IdCategoria { get; set; }

        public int IdEstado { get; set; }


        public IEnumerable<SelectListItem> Categorias { get; set; }

        public IEnumerable<SelectListItem> Estados { get; set; }
    }

    public class AdministradorInventarioViewModel
    {
        public int IdInventario { get; set; }

        public string Producto { get; set; }

        public string Sucursal { get; set; }

        public string Talla { get; set; }

        public string Color { get; set; }

        public int Existencias { get; set; }

        public System.DateTime FechaActualizacion { get; set; }

        public string Estado { get; set; }
    }


    public class AdministradorInventarioFormularioViewModel
    {
        public int IdInventario { get; set; }

        public int IdProducto { get; set; }

        public int IdSucursal { get; set; }

        public string Talla { get; set; }

        public string Color { get; set; }

        public int Existencias { get; set; }

        public int IdEstado { get; set; }


        public IEnumerable<SelectListItem> Productos { get; set; }

        public IEnumerable<SelectListItem> Sucursales { get; set; }

        public IEnumerable<SelectListItem> Estados { get; set; }
    }

    public class AdministradorSucursalViewModel
    {
        public int IdSucursal { get; set; }

        public string Nombre { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Horario { get; set; }

        public string Estado { get; set; }
    }


    public class AdministradorSucursalFormularioViewModel
    {
        public int IdSucursal { get; set; }

        public string Nombre { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Horario { get; set; }

        public int IdEstado { get; set; }

        public IEnumerable<SelectListItem> Estados { get; set; }
    }

    public class AdministradorPromocionViewModel
    {
        public int IdPromocion { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public decimal Descuento { get; set; }

        public System.DateTime FechaInicio { get; set; }

        public System.DateTime FechaFin { get; set; }

        public string Estado { get; set; }
    }


    public class AdministradorPromocionFormularioViewModel
    {
        public int IdPromocion { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public decimal Descuento { get; set; }

        public System.DateTime FechaInicio { get; set; }

        public System.DateTime FechaFin { get; set; }

        public int IdEstado { get; set; }

        public IEnumerable<SelectListItem> Estados { get; set; }
    }

    public class AdministradorClienteViewModel
    {
        public int Consecutivo { get; set; }

        public string Identificacion { get; set; }

        public string NombreCompleto { get; set; }

        public string CorreoElectronico { get; set; }

        public string Telefono { get; set; }

        public bool Estado { get; set; }
    }

    public class AdministradorClienteFormularioViewModel
    {
        public int Consecutivo { get; set; }

        public string Identificacion { get; set; }

        public string Nombre { get; set; }

        public string Apellido1 { get; set; }

        public string Apellido2 { get; set; }

        public string CorreoElectronico { get; set; }

        public string Telefono { get; set; }

        public bool Estado { get; set; }
    }

    public class AdministradorVendedorViewModel
    {
        public int Consecutivo { get; set; }

        public string Identificacion { get; set; }

        public string NombreCompleto { get; set; }

        public string CorreoElectronico { get; set; }

        public string Telefono { get; set; }

        public bool Estado { get; set; }
    }


    public class AdministradorVendedorFormularioViewModel
    {
        public int Consecutivo { get; set; }

        public string Identificacion { get; set; }

        public string Nombre { get; set; }

        public string Apellido1 { get; set; }

        public string Apellido2 { get; set; }

        public string CorreoElectronico { get; set; }

        public string Telefono { get; set; }

        public string Contrasenna { get; set; }

        public bool Estado { get; set; }
    }

}