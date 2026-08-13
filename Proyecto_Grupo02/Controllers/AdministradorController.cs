using Proyecto_Grupo02.Data;
using Proyecto_Grupo02.EF;
using Proyecto_Grupo02.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Proyecto_Grupo02.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly CatalogoDbContext _context =
            new CatalogoDbContext();

        private readonly KA_FASHION_BDEntities _usuariosContext =
            new KA_FASHION_BDEntities();

        private bool EsAdministrador()
        {
            if (Session["ConsecutivoUsuario"] == null)
            {
                return false;
            }

            int idUsuario =
                Convert.ToInt32(
                    Session["ConsecutivoUsuario"]
                );

            var usuario =
                _usuariosContext.tbUsuario
                .FirstOrDefault(u =>
                    u.Consecutivo == idUsuario
                );

            if (usuario == null)
            {
                return false;
            }

            var rol =
                _usuariosContext.tbRol
                .FirstOrDefault(r =>
                    r.Consecutivo ==
                    usuario.ConsecutivoRol
                );

            return rol != null &&
                   rol.Nombre == "Administrador";
        }


        public ActionResult Principal()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idActivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Activo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            int idCliente =
                _usuariosContext.tbRol
                .Where(r =>
                    r.Nombre == "Cliente"
                )
                .Select(r =>
                    r.Consecutivo
                )
                .FirstOrDefault();


            int idVendedor =
                _usuariosContext.tbRol
                .Where(r =>
                    r.Nombre == "Vendedor"
                )
                .Select(r =>
                    r.Consecutivo
                )
                .FirstOrDefault();


            var modelo =
                new AdministradorPrincipalViewModel
                {
                    TotalProductos =
                        _context.Productos.Count(),

                    TotalClientes =
                        _usuariosContext.tbUsuario
                        .Count(u =>
                            u.ConsecutivoRol == idCliente
                        ),

                    TotalVendedores =
                        _usuariosContext.tbUsuario
                        .Count(u =>
                            u.ConsecutivoRol == idVendedor
                        ),

                    TotalPedidos =
                        _context.Pedidos.Count(),

                    TotalSucursales =
                        _context.Sucursales.Count(),

                    PromocionesActivas =
                        _context.Promociones
                        .Count(p =>
                            p.IdEstado == idActivo &&
                            p.FechaInicio <= DateTime.Now &&
                            p.FechaFin >= DateTime.Now
                        )
                };


            return View(modelo);
        }


        public ActionResult Productos()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var productos =
                _context.Productos
                .Select(p =>
                    new AdministradorProductoViewModel
                    {
                        IdProducto =
                            p.IdProducto,

                        Nombre =
                            p.Nombre,

                        Categoria =
                            p.Categoria.Nombre,

                        Precio =
                            p.Precio,

                        Talla =
                            p.Talla,

                        Color =
                            p.Color,

                        Imagen =
                            p.Imagen,

                        Estado =
                            p.Estado.NombreEstado
                    }
                )
                .OrderBy(p =>
                    p.Nombre
                )
                .ToList();


            return View(productos);
        }


        private void CargarCombosProducto(
            AdministradorProductoFormularioViewModel modelo)
        {
            modelo.Categorias =
                _context.Categorias
                .OrderBy(c =>
                    c.Nombre
                )
                .Select(c =>
                    new SelectListItem
                    {
                        Value =
                            c.IdCategoria.ToString(),

                        Text =
                            c.Nombre
                    }
                )
                .ToList();


            modelo.Estados =
                _context.Estados
                .OrderBy(e =>
                    e.NombreEstado
                )
                .Select(e =>
                    new SelectListItem
                    {
                        Value =
                            e.IdEstado.ToString(),

                        Text =
                            e.NombreEstado
                    }
                )
                .ToList();
        }

        [HttpGet]
        public ActionResult AgregarProducto()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idActivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Activo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            var modelo =
                new AdministradorProductoFormularioViewModel
                {
                    IdEstado =
                        idActivo,

                    Destacado =
                        false,

                    Novedad =
                        true
                };


            CargarCombosProducto(modelo);


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarProducto(
            AdministradorProductoFormularioViewModel modelo)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Debe ingresar el nombre del producto."
                );
            }


            if (modelo.Precio <= 0)
            {
                ModelState.AddModelError(
                    "Precio",
                    "El precio debe ser mayor que cero."
                );
            }


            if (modelo.IdCategoria <= 0)
            {
                ModelState.AddModelError(
                    "IdCategoria",
                    "Debe seleccionar una categoría."
                );
            }


            if (modelo.IdEstado <= 0)
            {
                ModelState.AddModelError(
                    "IdEstado",
                    "Debe seleccionar un estado."
                );
            }


            if (!ModelState.IsValid)
            {
                CargarCombosProducto(modelo);

                return View(modelo);
            }


            var producto =
                new tbProducto
                {
                    Nombre =
                        modelo.Nombre,

                    Descripcion =
                        modelo.Descripcion,

                    Precio =
                        modelo.Precio,

                    Imagen =
                        modelo.Imagen,

                    Talla =
                        modelo.Talla,

                    Color =
                        modelo.Color,

                    Destacado =
                        modelo.Destacado,

                    Novedad =
                        modelo.Novedad,

                    IdCategoria =
                        modelo.IdCategoria,

                    IdEstado =
                        modelo.IdEstado
                };


            _context.Productos.Add(producto);

            _context.SaveChanges();


            TempData["Mensaje"] =
                "Producto registrado correctamente.";


            return RedirectToAction("Productos");
        }


        [HttpGet]
        public ActionResult EditarProducto(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var producto =
                _context.Productos
                .FirstOrDefault(p =>
                    p.IdProducto == id
                );


            if (producto == null)
            {
                return HttpNotFound();
            }


            var modelo =
                new AdministradorProductoFormularioViewModel
                {
                    IdProducto =
                        producto.IdProducto,

                    Nombre =
                        producto.Nombre,

                    Descripcion =
                        producto.Descripcion,

                    Precio =
                        producto.Precio,

                    Imagen =
                        producto.Imagen,

                    Talla =
                        producto.Talla,

                    Color =
                        producto.Color,

                    Destacado =
                        producto.Destacado,

                    Novedad =
                        producto.Novedad,

                    IdCategoria =
                        producto.IdCategoria,

                    IdEstado =
                        producto.IdEstado
                };


            CargarCombosProducto(modelo);


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProducto(
            AdministradorProductoFormularioViewModel modelo)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Debe ingresar el nombre del producto."
                );
            }


            if (modelo.Precio <= 0)
            {
                ModelState.AddModelError(
                    "Precio",
                    "El precio debe ser mayor que cero."
                );
            }


            if (modelo.IdCategoria <= 0)
            {
                ModelState.AddModelError(
                    "IdCategoria",
                    "Debe seleccionar una categoría."
                );
            }


            if (modelo.IdEstado <= 0)
            {
                ModelState.AddModelError(
                    "IdEstado",
                    "Debe seleccionar un estado."
                );
            }


            if (!ModelState.IsValid)
            {
                CargarCombosProducto(modelo);

                return View(modelo);
            }


            var producto =
                _context.Productos
                .FirstOrDefault(p =>
                    p.IdProducto ==
                    modelo.IdProducto
                );


            if (producto == null)
            {
                return HttpNotFound();
            }


            producto.Nombre =
                modelo.Nombre;

            producto.Descripcion =
                modelo.Descripcion;

            producto.Precio =
                modelo.Precio;

            producto.Imagen =
                modelo.Imagen;

            producto.Talla =
                modelo.Talla;

            producto.Color =
                modelo.Color;

            producto.Destacado =
                modelo.Destacado;

            producto.Novedad =
                modelo.Novedad;

            producto.IdCategoria =
                modelo.IdCategoria;

            producto.IdEstado =
                modelo.IdEstado;


            _context.SaveChanges();


            TempData["Mensaje"] =
                "Producto actualizado correctamente.";


            return RedirectToAction("Productos");
        }


        public ActionResult CambiarEstadoProducto(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var producto =
                _context.Productos
                .FirstOrDefault(p =>
                    p.IdProducto == id
                );


            if (producto == null)
            {
                return HttpNotFound();
            }


            int idActivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Activo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            int idInactivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Inactivo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            if (producto.IdEstado == idActivo)
            {
                producto.IdEstado =
                    idInactivo;
            }
            else
            {
                producto.IdEstado =
                    idActivo;
            }


            _context.SaveChanges();


            TempData["Mensaje"] =
                "Estado del producto actualizado correctamente.";


            return RedirectToAction("Productos");
        }

        public ActionResult Categorias()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var categorias =
                _context.Categorias
                .Select(c =>
                    new AdministradorCategoriaViewModel
                    {
                        IdCategoria =
                            c.IdCategoria,

                        Nombre =
                            c.Nombre,

                        Descripcion =
                            c.Descripcion,

                        Estado =
                            c.Estado.NombreEstado
                    }
                )
                .OrderBy(c =>
                    c.Nombre
                )
                .ToList();


            return View(categorias);
        }


        private void CargarEstadosCategoria(
            AdministradorCategoriaFormularioViewModel modelo)
        {
            modelo.Estados =
                _context.Estados
                .OrderBy(e =>
                    e.NombreEstado
                )
                .Select(e =>
                    new SelectListItem
                    {
                        Value =
                            e.IdEstado.ToString(),

                        Text =
                            e.NombreEstado
                    }
                )
                .ToList();
        }


        [HttpGet]
        public ActionResult AgregarCategoria()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idActivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Activo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            var modelo =
                new AdministradorCategoriaFormularioViewModel
                {
                    IdEstado = idActivo
                };


            CargarEstadosCategoria(modelo);


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarCategoria(
            AdministradorCategoriaFormularioViewModel modelo)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Debe ingresar el nombre de la categoría."
                );
            }


            bool categoriaExiste =
                _context.Categorias
                .Any(c =>
                    c.Nombre == modelo.Nombre
                );


            if (categoriaExiste)
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Ya existe una categoría con ese nombre."
                );
            }


            if (modelo.IdEstado <= 0)
            {
                ModelState.AddModelError(
                    "IdEstado",
                    "Debe seleccionar un estado."
                );
            }


            if (!ModelState.IsValid)
            {
                CargarEstadosCategoria(modelo);

                return View(modelo);
            }


            var categoria =
                new tbCategoria
                {
                    Nombre =
                        modelo.Nombre,

                    Descripcion =
                        modelo.Descripcion,

                    IdEstado =
                        modelo.IdEstado
                };


            _context.Categorias.Add(categoria);

            _context.SaveChanges();


            TempData["Mensaje"] =
                "Categoría registrada correctamente.";


            return RedirectToAction("Categorias");
        }


        [HttpGet]
        public ActionResult EditarCategoria(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var categoria =
                _context.Categorias
                .FirstOrDefault(c =>
                    c.IdCategoria == id
                );


            if (categoria == null)
            {
                return HttpNotFound();
            }


            var modelo =
                new AdministradorCategoriaFormularioViewModel
                {
                    IdCategoria =
                        categoria.IdCategoria,

                    Nombre =
                        categoria.Nombre,

                    Descripcion =
                        categoria.Descripcion,

                    IdEstado =
                        categoria.IdEstado
                };


            CargarEstadosCategoria(modelo);


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCategoria(
            AdministradorCategoriaFormularioViewModel modelo)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Debe ingresar el nombre de la categoría."
                );
            }


            bool categoriaExiste =
                _context.Categorias
                .Any(c =>
                    c.Nombre == modelo.Nombre &&
                    c.IdCategoria != modelo.IdCategoria
                );


            if (categoriaExiste)
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Ya existe otra categoría con ese nombre."
                );
            }


            if (!ModelState.IsValid)
            {
                CargarEstadosCategoria(modelo);

                return View(modelo);
            }


            var categoria =
                _context.Categorias
                .FirstOrDefault(c =>
                    c.IdCategoria ==
                    modelo.IdCategoria
                );


            if (categoria == null)
            {
                return HttpNotFound();
            }


            categoria.Nombre =
                modelo.Nombre;

            categoria.Descripcion =
                modelo.Descripcion;

            categoria.IdEstado =
                modelo.IdEstado;


            _context.SaveChanges();


            TempData["Mensaje"] =
                "Categoría actualizada correctamente.";


            return RedirectToAction("Categorias");
        }


        public ActionResult CambiarEstadoCategoria(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var categoria =
                _context.Categorias
                .FirstOrDefault(c =>
                    c.IdCategoria == id
                );


            if (categoria == null)
            {
                return HttpNotFound();
            }


            int idActivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Activo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            int idInactivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Inactivo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            if (categoria.IdEstado == idActivo)
            {
                categoria.IdEstado =
                    idInactivo;
            }
            else
            {
                categoria.IdEstado =
                    idActivo;
            }


            _context.SaveChanges();


            TempData["Mensaje"] =
                "Estado de la categoría actualizado correctamente.";


            return RedirectToAction("Categorias");
        }

        public ActionResult Inventario()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var inventario =
                (from i in _context.Inventarios
                 join e in _context.Estados
                     on i.IdEstado equals e.IdEstado

                 select new AdministradorInventarioViewModel
                 {
                     IdInventario =
                         i.IdInventario,

                     Producto =
                         i.Producto.Nombre,

                     Sucursal =
                         i.Sucursal.Nombre,

                     Talla =
                         i.Talla,

                     Color =
                         i.Color,

                     Existencias =
                         i.Existencias,

                     FechaActualizacion =
                         i.FechaActualizacion,

                     Estado =
                         e.NombreEstado
                 })
                .OrderBy(i => i.Producto)
                .ThenBy(i => i.Sucursal)
                .ToList();


            return View(inventario);
        }


        private void CargarCombosInventario(
            AdministradorInventarioFormularioViewModel modelo)
        {
            int idActivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Activo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            modelo.Productos =
                _context.Productos
                .Where(p =>
                    p.IdEstado == idActivo
                )
                .OrderBy(p =>
                    p.Nombre
                )
                .Select(p =>
                    new SelectListItem
                    {
                        Value =
                            p.IdProducto.ToString(),

                        Text =
                            p.Nombre
                    }
                )
                .ToList();


            modelo.Sucursales =
                _context.Sucursales
                .Where(s =>
                    s.IdEstado == idActivo
                )
                .OrderBy(s =>
                    s.Nombre
                )
                .Select(s =>
                    new SelectListItem
                    {
                        Value =
                            s.IdSucursal.ToString(),

                        Text =
                            s.Nombre
                    }
                )
                .ToList();


            modelo.Estados =
                _context.Estados
                .OrderBy(e =>
                    e.NombreEstado
                )
                .Select(e =>
                    new SelectListItem
                    {
                        Value =
                            e.IdEstado.ToString(),

                        Text =
                            e.NombreEstado
                    }
                )
                .ToList();
        }


        [HttpGet]
        public ActionResult AgregarInventario()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idActivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Activo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            var modelo =
                new AdministradorInventarioFormularioViewModel
                {
                    IdEstado =
                        idActivo,

                    Existencias =
                        0
                };


            CargarCombosInventario(modelo);


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarInventario(
            AdministradorInventarioFormularioViewModel modelo)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            if (modelo.IdProducto <= 0)
            {
                ModelState.AddModelError(
                    "IdProducto",
                    "Debe seleccionar un producto."
                );
            }


            if (modelo.IdSucursal <= 0)
            {
                ModelState.AddModelError(
                    "IdSucursal",
                    "Debe seleccionar una sucursal."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Talla))
            {
                ModelState.AddModelError(
                    "Talla",
                    "Debe ingresar una talla."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Color))
            {
                ModelState.AddModelError(
                    "Color",
                    "Debe ingresar un color."
                );
            }


            if (modelo.Existencias < 0)
            {
                ModelState.AddModelError(
                    "Existencias",
                    "Las existencias no pueden ser negativas."
                );
            }


            bool inventarioExiste =
                _context.Inventarios
                .Any(i =>
                    i.IdProducto == modelo.IdProducto &&
                    i.IdSucursal == modelo.IdSucursal &&
                    i.Talla == modelo.Talla &&
                    i.Color == modelo.Color
                );


            if (inventarioExiste)
            {
                ModelState.AddModelError(
                    "",
                    "Ya existe un registro de inventario para ese producto, sucursal, talla y color."
                );
            }


            if (!ModelState.IsValid)
            {
                CargarCombosInventario(modelo);

                return View(modelo);
            }


            var inventario =
                new tbInventario
                {
                    IdProducto =
                        modelo.IdProducto,

                    IdSucursal =
                        modelo.IdSucursal,

                    Talla =
                        modelo.Talla,

                    Color =
                        modelo.Color,

                    Existencias =
                        modelo.Existencias,

                    FechaActualizacion =
                        DateTime.Now,

                    IdEstado =
                        modelo.IdEstado
                };


            _context.Inventarios.Add(inventario);

            _context.SaveChanges();


            TempData["Mensaje"] =
                "Inventario registrado correctamente.";


            return RedirectToAction("Inventario");
        }


        [HttpGet]
        public ActionResult EditarInventario(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var inventario =
                _context.Inventarios
                .FirstOrDefault(i =>
                    i.IdInventario == id
                );


            if (inventario == null)
            {
                return HttpNotFound();
            }


            var modelo =
                new AdministradorInventarioFormularioViewModel
                {
                    IdInventario =
                        inventario.IdInventario,

                    IdProducto =
                        inventario.IdProducto,

                    IdSucursal =
                        inventario.IdSucursal,

                    Talla =
                        inventario.Talla,

                    Color =
                        inventario.Color,

                    Existencias =
                        inventario.Existencias,

                    IdEstado =
                        inventario.IdEstado
                };


            CargarCombosInventario(modelo);


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarInventario(
            AdministradorInventarioFormularioViewModel modelo)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            if (modelo.Existencias < 0)
            {
                ModelState.AddModelError(
                    "Existencias",
                    "Las existencias no pueden ser negativas."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Talla))
            {
                ModelState.AddModelError(
                    "Talla",
                    "Debe ingresar una talla."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Color))
            {
                ModelState.AddModelError(
                    "Color",
                    "Debe ingresar un color."
                );
            }


            bool inventarioExiste =
                _context.Inventarios
                .Any(i =>
                    i.IdProducto == modelo.IdProducto &&
                    i.IdSucursal == modelo.IdSucursal &&
                    i.Talla == modelo.Talla &&
                    i.Color == modelo.Color &&
                    i.IdInventario != modelo.IdInventario
                );


            if (inventarioExiste)
            {
                ModelState.AddModelError(
                    "",
                    "Ya existe otro registro con ese producto, sucursal, talla y color."
                );
            }


            if (!ModelState.IsValid)
            {
                CargarCombosInventario(modelo);

                return View(modelo);
            }


            var inventario =
                _context.Inventarios
                .FirstOrDefault(i =>
                    i.IdInventario ==
                    modelo.IdInventario
                );


            if (inventario == null)
            {
                return HttpNotFound();
            }


            inventario.IdProducto =
                modelo.IdProducto;

            inventario.IdSucursal =
                modelo.IdSucursal;

            inventario.Talla =
                modelo.Talla;

            inventario.Color =
                modelo.Color;

            inventario.Existencias =
                modelo.Existencias;

            inventario.IdEstado =
                modelo.IdEstado;

            inventario.FechaActualizacion =
                DateTime.Now;


            _context.SaveChanges();


            TempData["Mensaje"] =
                "Inventario actualizado correctamente.";


            return RedirectToAction("Inventario");
        }


        public ActionResult CambiarEstadoInventario(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var inventario =
                _context.Inventarios
                .FirstOrDefault(i =>
                    i.IdInventario == id
                );


            if (inventario == null)
            {
                return HttpNotFound();
            }


            int idActivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Activo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            int idInactivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Inactivo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            if (inventario.IdEstado == idActivo)
            {
                inventario.IdEstado =
                    idInactivo;
            }
            else
            {
                inventario.IdEstado =
                    idActivo;
            }


            inventario.FechaActualizacion =
                DateTime.Now;


            _context.SaveChanges();


            TempData["Mensaje"] =
                "Estado del inventario actualizado correctamente.";


            return RedirectToAction("Inventario");
        }


        public ActionResult Sucursales()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var sucursales =
                _context.Sucursales
                .Select(s =>
                    new AdministradorSucursalViewModel
                    {
                        IdSucursal =
                            s.IdSucursal,

                        Nombre =
                            s.Nombre,

                        Direccion =
                            s.Direccion,

                        Telefono =
                            s.Telefono,

                        Horario =
                            s.Horario,

                        Estado =
                            s.Estado.NombreEstado
                    }
                )
                .OrderBy(s =>
                    s.Nombre
                )
                .ToList();


            return View(sucursales);
        }


        private void CargarEstadosSucursal(
            AdministradorSucursalFormularioViewModel modelo)
        {
            modelo.Estados =
                _context.Estados
                .OrderBy(e =>
                    e.NombreEstado
                )
                .Select(e =>
                    new SelectListItem
                    {
                        Value =
                            e.IdEstado.ToString(),

                        Text =
                            e.NombreEstado
                    }
                )
                .ToList();
        }


        [HttpGet]
        public ActionResult AgregarSucursal()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idActivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Activo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            var modelo =
                new AdministradorSucursalFormularioViewModel
                {
                    IdEstado = idActivo
                };


            CargarEstadosSucursal(modelo);


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarSucursal(
            AdministradorSucursalFormularioViewModel modelo)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Debe ingresar el nombre de la sucursal."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Direccion))
            {
                ModelState.AddModelError(
                    "Direccion",
                    "Debe ingresar la dirección."
                );
            }


            bool sucursalExiste =
                _context.Sucursales
                .Any(s =>
                    s.Nombre == modelo.Nombre
                );


            if (sucursalExiste)
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Ya existe una sucursal con ese nombre."
                );
            }


            if (!ModelState.IsValid)
            {
                CargarEstadosSucursal(modelo);

                return View(modelo);
            }


            var sucursal =
                new tbSucursal
                {
                    Nombre =
                        modelo.Nombre,

                    Direccion =
                        modelo.Direccion,

                    Telefono =
                        modelo.Telefono,

                    Horario =
                        modelo.Horario,

                    IdEstado =
                        modelo.IdEstado
                };


            _context.Sucursales.Add(sucursal);

            _context.SaveChanges();


            TempData["Mensaje"] =
                "Sucursal registrada correctamente.";


            return RedirectToAction("Sucursales");
        }


        [HttpGet]
        public ActionResult EditarSucursal(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var sucursal =
                _context.Sucursales
                .FirstOrDefault(s =>
                    s.IdSucursal == id
                );


            if (sucursal == null)
            {
                return HttpNotFound();
            }


            var modelo =
                new AdministradorSucursalFormularioViewModel
                {
                    IdSucursal =
                        sucursal.IdSucursal,

                    Nombre =
                        sucursal.Nombre,

                    Direccion =
                        sucursal.Direccion,

                    Telefono =
                        sucursal.Telefono,

                    Horario =
                        sucursal.Horario,

                    IdEstado =
                        sucursal.IdEstado
                };


            CargarEstadosSucursal(modelo);


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarSucursal(
            AdministradorSucursalFormularioViewModel modelo)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Debe ingresar el nombre de la sucursal."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Direccion))
            {
                ModelState.AddModelError(
                    "Direccion",
                    "Debe ingresar la dirección."
                );
            }


            bool sucursalExiste =
                _context.Sucursales
                .Any(s =>
                    s.Nombre == modelo.Nombre &&
                    s.IdSucursal != modelo.IdSucursal
                );


            if (sucursalExiste)
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Ya existe otra sucursal con ese nombre."
                );
            }


            if (!ModelState.IsValid)
            {
                CargarEstadosSucursal(modelo);

                return View(modelo);
            }


            var sucursal =
                _context.Sucursales
                .FirstOrDefault(s =>
                    s.IdSucursal ==
                    modelo.IdSucursal
                );


            if (sucursal == null)
            {
                return HttpNotFound();
            }


            sucursal.Nombre =
                modelo.Nombre;

            sucursal.Direccion =
                modelo.Direccion;

            sucursal.Telefono =
                modelo.Telefono;

            sucursal.Horario =
                modelo.Horario;

            sucursal.IdEstado =
                modelo.IdEstado;


            _context.SaveChanges();


            TempData["Mensaje"] =
                "Sucursal actualizada correctamente.";


            return RedirectToAction("Sucursales");
        }


        public ActionResult CambiarEstadoSucursal(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var sucursal =
                _context.Sucursales
                .FirstOrDefault(s =>
                    s.IdSucursal == id
                );


            if (sucursal == null)
            {
                return HttpNotFound();
            }


            int idActivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Activo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            int idInactivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Inactivo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            if (sucursal.IdEstado == idActivo)
            {
                sucursal.IdEstado =
                    idInactivo;
            }
            else
            {
                sucursal.IdEstado =
                    idActivo;
            }


            _context.SaveChanges();


            TempData["Mensaje"] =
                "Estado de la sucursal actualizado correctamente.";


            return RedirectToAction("Sucursales");
        }


        public ActionResult Promociones()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var promociones =
                _context.Promociones
                .Select(p =>
                    new AdministradorPromocionViewModel
                    {
                        IdPromocion =
                            p.IdPromocion,

                        Nombre =
                            p.Nombre,

                        Descripcion =
                            p.Descripcion,

                        Descuento =
                            p.Descuento,

                        FechaInicio =
                            p.FechaInicio,

                        FechaFin =
                            p.FechaFin,

                        Estado =
                            p.Estado.NombreEstado
                    }
                )
                .OrderByDescending(p =>
                    p.FechaInicio
                )
                .ToList();


            return View(promociones);
        }


        private void CargarEstadosPromocion(
            AdministradorPromocionFormularioViewModel modelo)
        {
            modelo.Estados =
                _context.Estados
                .OrderBy(e =>
                    e.NombreEstado
                )
                .Select(e =>
                    new SelectListItem
                    {
                        Value =
                            e.IdEstado.ToString(),

                        Text =
                            e.NombreEstado
                    }
                )
                .ToList();
        }


        [HttpGet]
        public ActionResult AgregarPromocion()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idActivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Activo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            var modelo =
                new AdministradorPromocionFormularioViewModel
                {
                    IdEstado =
                        idActivo,

                    FechaInicio =
                        DateTime.Today,

                    FechaFin =
                        DateTime.Today.AddDays(7)
                };


            CargarEstadosPromocion(modelo);


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarPromocion(
            AdministradorPromocionFormularioViewModel modelo)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Debe ingresar el nombre de la promoción."
                );
            }


            if (modelo.Descuento <= 0 ||
                modelo.Descuento > 100)
            {
                ModelState.AddModelError(
                    "Descuento",
                    "El descuento debe estar entre 1 y 100."
                );
            }


            if (modelo.FechaFin < modelo.FechaInicio)
            {
                ModelState.AddModelError(
                    "FechaFin",
                    "La fecha final no puede ser menor que la fecha inicial."
                );
            }


            if (!ModelState.IsValid)
            {
                CargarEstadosPromocion(modelo);

                return View(modelo);
            }


            var promocion =
                new tbPromocion
                {
                    Nombre =
                        modelo.Nombre,

                    Descripcion =
                        modelo.Descripcion,

                    Descuento =
                        modelo.Descuento,

                    FechaInicio =
                        modelo.FechaInicio,

                    FechaFin =
                        modelo.FechaFin,

                    IdEstado =
                        modelo.IdEstado
                };


            _context.Promociones.Add(promocion);

            _context.SaveChanges();


            TempData["Mensaje"] =
                "Promoción registrada correctamente.";


            return RedirectToAction("Promociones");
        }


        [HttpGet]
        public ActionResult EditarPromocion(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var promocion =
                _context.Promociones
                .FirstOrDefault(p =>
                    p.IdPromocion == id
                );


            if (promocion == null)
            {
                return HttpNotFound();
            }


            var modelo =
                new AdministradorPromocionFormularioViewModel
                {
                    IdPromocion =
                        promocion.IdPromocion,

                    Nombre =
                        promocion.Nombre,

                    Descripcion =
                        promocion.Descripcion,

                    Descuento =
                        promocion.Descuento,

                    FechaInicio =
                        promocion.FechaInicio,

                    FechaFin =
                        promocion.FechaFin,

                    IdEstado =
                        promocion.IdEstado
                };


            CargarEstadosPromocion(modelo);


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPromocion(
            AdministradorPromocionFormularioViewModel modelo)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Debe ingresar el nombre de la promoción."
                );
            }


            if (modelo.Descuento <= 0 ||
                modelo.Descuento > 100)
            {
                ModelState.AddModelError(
                    "Descuento",
                    "El descuento debe estar entre 1 y 100."
                );
            }


            if (modelo.FechaFin < modelo.FechaInicio)
            {
                ModelState.AddModelError(
                    "FechaFin",
                    "La fecha final no puede ser menor que la fecha inicial."
                );
            }


            if (!ModelState.IsValid)
            {
                CargarEstadosPromocion(modelo);

                return View(modelo);
            }


            var promocion =
                _context.Promociones
                .FirstOrDefault(p =>
                    p.IdPromocion ==
                    modelo.IdPromocion
                );


            if (promocion == null)
            {
                return HttpNotFound();
            }


            promocion.Nombre =
                modelo.Nombre;

            promocion.Descripcion =
                modelo.Descripcion;

            promocion.Descuento =
                modelo.Descuento;

            promocion.FechaInicio =
                modelo.FechaInicio;

            promocion.FechaFin =
                modelo.FechaFin;

            promocion.IdEstado =
                modelo.IdEstado;


            _context.SaveChanges();


            TempData["Mensaje"] =
                "Promoción actualizada correctamente.";


            return RedirectToAction("Promociones");
        }


        public ActionResult CambiarEstadoPromocion(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var promocion =
                _context.Promociones
                .FirstOrDefault(p =>
                    p.IdPromocion == id
                );


            if (promocion == null)
            {
                return HttpNotFound();
            }


            int idActivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Activo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            int idInactivo =
                _context.Estados
                .Where(e =>
                    e.NombreEstado == "Inactivo"
                )
                .Select(e =>
                    e.IdEstado
                )
                .FirstOrDefault();


            if (promocion.IdEstado == idActivo)
            {
                promocion.IdEstado =
                    idInactivo;
            }
            else
            {
                promocion.IdEstado =
                    idActivo;
            }


            _context.SaveChanges();


            TempData["Mensaje"] =
                "Estado de la promoción actualizado correctamente.";


            return RedirectToAction("Promociones");
        }


        public ActionResult Clientes()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idRolCliente =
                _usuariosContext.tbRol
                .Where(r =>
                    r.Nombre == "Cliente"
                )
                .Select(r =>
                    r.Consecutivo
                )
                .FirstOrDefault();


            var clientes =
                _usuariosContext.tbUsuario
                .Where(u =>
                    u.ConsecutivoRol == idRolCliente
                )
                .Select(u =>
                    new AdministradorClienteViewModel
                    {
                        Consecutivo =
                            u.Consecutivo,

                        Identificacion =
                            u.Identificacion,

                        NombreCompleto =
                            u.Nombre + " " +
                            u.Apellido1 + " " +
                            u.Apellido2,

                        CorreoElectronico =
                            u.CorreoElectronico,

                        Telefono =
                            u.Telefono,

                        Estado =
                            u.Estado
                    }
                )
                .OrderBy(u =>
                    u.NombreCompleto
                )
                .ToList();


            return View(clientes);
        }


        public ActionResult CambiarEstadoCliente(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var cliente =
                _usuariosContext.tbUsuario
                .FirstOrDefault(u =>
                    u.Consecutivo == id
                );


            if (cliente == null)
            {
                return HttpNotFound();
            }


            int idRolCliente =
                _usuariosContext.tbRol
                .Where(r =>
                    r.Nombre == "Cliente"
                )
                .Select(r =>
                    r.Consecutivo
                )
                .FirstOrDefault();


            if (cliente.ConsecutivoRol != idRolCliente)
            {
                return RedirectToAction("Clientes");
            }


            cliente.Estado =
                !cliente.Estado;


            _usuariosContext.SaveChanges();


            if (cliente.Estado)
            {
                TempData["Mensaje"] =
                    "Cliente activado correctamente.";
            }
            else
            {
                TempData["Mensaje"] =
                    "Cliente desactivado correctamente.";
            }


            return RedirectToAction("Clientes");
        }


        [HttpGet]
        public ActionResult EditarCliente(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idRolCliente =
                _usuariosContext.tbRol
                .Where(r =>
                    r.Nombre == "Cliente"
                )
                .Select(r =>
                    r.Consecutivo
                )
                .FirstOrDefault();


            var cliente =
                _usuariosContext.tbUsuario
                .FirstOrDefault(u =>
                    u.Consecutivo == id &&
                    u.ConsecutivoRol == idRolCliente
                );


            if (cliente == null)
            {
                return HttpNotFound();
            }


            var modelo =
                new AdministradorClienteFormularioViewModel
                {
                    Consecutivo =
                        cliente.Consecutivo,

                    Identificacion =
                        cliente.Identificacion,

                    Nombre =
                        cliente.Nombre,

                    Apellido1 =
                        cliente.Apellido1,

                    Apellido2 =
                        cliente.Apellido2,

                    CorreoElectronico =
                        cliente.CorreoElectronico,

                    Telefono =
                        cliente.Telefono,

                    Estado =
                        cliente.Estado
                };


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCliente(
            AdministradorClienteFormularioViewModel modelo)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Identificacion))
            {
                ModelState.AddModelError(
                    "Identificacion",
                    "Debe ingresar la identificación."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Debe ingresar el nombre."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Apellido1))
            {
                ModelState.AddModelError(
                    "Apellido1",
                    "Debe ingresar el primer apellido."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.CorreoElectronico))
            {
                ModelState.AddModelError(
                    "CorreoElectronico",
                    "Debe ingresar el correo electrónico."
                );
            }


            bool identificacionExiste =
                _usuariosContext.tbUsuario
                .Any(u =>
                    u.Identificacion == modelo.Identificacion &&
                    u.Consecutivo != modelo.Consecutivo
                );


            if (identificacionExiste)
            {
                ModelState.AddModelError(
                    "Identificacion",
                    "Ya existe otro usuario con esa identificación."
                );
            }


            bool correoExiste =
                _usuariosContext.tbUsuario
                .Any(u =>
                    u.CorreoElectronico == modelo.CorreoElectronico &&
                    u.Consecutivo != modelo.Consecutivo
                );


            if (correoExiste)
            {
                ModelState.AddModelError(
                    "CorreoElectronico",
                    "Ya existe otro usuario con ese correo electrónico."
                );
            }


            if (!ModelState.IsValid)
            {
                return View(modelo);
            }


            int idRolCliente =
                _usuariosContext.tbRol
                .Where(r =>
                    r.Nombre == "Cliente"
                )
                .Select(r =>
                    r.Consecutivo
                )
                .FirstOrDefault();


            var cliente =
                _usuariosContext.tbUsuario
                .FirstOrDefault(u =>
                    u.Consecutivo == modelo.Consecutivo &&
                    u.ConsecutivoRol == idRolCliente
                );


            if (cliente == null)
            {
                return HttpNotFound();
            }


            cliente.Identificacion =
                modelo.Identificacion;

            cliente.Nombre =
                modelo.Nombre;

            cliente.Apellido1 =
                modelo.Apellido1;

            cliente.Apellido2 =
                modelo.Apellido2;

            cliente.CorreoElectronico =
                modelo.CorreoElectronico;

            cliente.Telefono =
                modelo.Telefono;

            cliente.Estado =
                modelo.Estado;


            _usuariosContext.SaveChanges();


            TempData["Mensaje"] =
                "Cliente actualizado correctamente.";


            return RedirectToAction("Clientes");
        }


        public ActionResult Vendedores()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idRolVendedor =
                _usuariosContext.tbRol
                .Where(r =>
                    r.Nombre == "Vendedor"
                )
                .Select(r =>
                    r.Consecutivo
                )
                .FirstOrDefault();


            var vendedores =
                _usuariosContext.tbUsuario
                .Where(u =>
                    u.ConsecutivoRol == idRolVendedor
                )
                .Select(u =>
                    new AdministradorVendedorViewModel
                    {
                        Consecutivo =
                            u.Consecutivo,

                        Identificacion =
                            u.Identificacion,

                        NombreCompleto =
                            u.Nombre + " " +
                            u.Apellido1 + " " +
                            u.Apellido2,

                        CorreoElectronico =
                            u.CorreoElectronico,

                        Telefono =
                            u.Telefono,

                        Estado =
                            u.Estado
                    }
                )
                .OrderBy(u =>
                    u.NombreCompleto
                )
                .ToList();


            return View(vendedores);
        }


        [HttpGet]
        public ActionResult AgregarVendedor()
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var modelo =
                new AdministradorVendedorFormularioViewModel
                {
                    Estado = true
                };


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarVendedor(
            AdministradorVendedorFormularioViewModel modelo)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Identificacion))
            {
                ModelState.AddModelError(
                    "Identificacion",
                    "Debe ingresar la identificación."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Debe ingresar el nombre."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Apellido1))
            {
                ModelState.AddModelError(
                    "Apellido1",
                    "Debe ingresar el primer apellido."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.CorreoElectronico))
            {
                ModelState.AddModelError(
                    "CorreoElectronico",
                    "Debe ingresar el correo electrónico."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Contrasenna))
            {
                ModelState.AddModelError(
                    "Contrasenna",
                    "Debe ingresar una contraseña."
                );
            }


            bool identificacionExiste =
                _usuariosContext.tbUsuario
                .Any(u =>
                    u.Identificacion == modelo.Identificacion
                );


            if (identificacionExiste)
            {
                ModelState.AddModelError(
                    "Identificacion",
                    "Ya existe un usuario con esa identificación."
                );
            }


            bool correoExiste =
                _usuariosContext.tbUsuario
                .Any(u =>
                    u.CorreoElectronico == modelo.CorreoElectronico
                );


            if (correoExiste)
            {
                ModelState.AddModelError(
                    "CorreoElectronico",
                    "Ya existe un usuario con ese correo electrónico."
                );
            }


            if (!ModelState.IsValid)
            {
                return View(modelo);
            }


            int idRolVendedor =
                _usuariosContext.tbRol
                .Where(r =>
                    r.Nombre == "Vendedor"
                )
                .Select(r =>
                    r.Consecutivo
                )
                .FirstOrDefault();


            if (idRolVendedor == 0)
            {
                ModelState.AddModelError(
                    "",
                    "No se encontró el rol Vendedor."
                );

                return View(modelo);
            }


            var vendedor =
                new tbUsuario
                {
                    Identificacion =
                        modelo.Identificacion,

                    Nombre =
                        modelo.Nombre,

                    Apellido1 =
                        modelo.Apellido1,

                    Apellido2 =
                        modelo.Apellido2,

                    CorreoElectronico =
                        modelo.CorreoElectronico,

                    Telefono =
                        modelo.Telefono,

                    Contrasenna =
                        modelo.Contrasenna,

                    Estado =
                        modelo.Estado,

                    TieneContrasennaTemp =
                        false,

                    ConsecutivoRol =
                        idRolVendedor
                };


            _usuariosContext.tbUsuario.Add(vendedor);

            _usuariosContext.SaveChanges();


            TempData["Mensaje"] =
                "Vendedor registrado correctamente.";


            return RedirectToAction("Vendedores");
        }


        [HttpGet]
        public ActionResult EditarVendedor(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idRolVendedor =
                _usuariosContext.tbRol
                .Where(r =>
                    r.Nombre == "Vendedor"
                )
                .Select(r =>
                    r.Consecutivo
                )
                .FirstOrDefault();


            var vendedor =
                _usuariosContext.tbUsuario
                .FirstOrDefault(u =>
                    u.Consecutivo == id &&
                    u.ConsecutivoRol == idRolVendedor
                );


            if (vendedor == null)
            {
                return HttpNotFound();
            }


            var modelo =
                new AdministradorVendedorFormularioViewModel
                {
                    Consecutivo =
                        vendedor.Consecutivo,

                    Identificacion =
                        vendedor.Identificacion,

                    Nombre =
                        vendedor.Nombre,

                    Apellido1 =
                        vendedor.Apellido1,

                    Apellido2 =
                        vendedor.Apellido2,

                    CorreoElectronico =
                        vendedor.CorreoElectronico,

                    Telefono =
                        vendedor.Telefono,

                    Estado =
                        vendedor.Estado
                };


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarVendedor(
            AdministradorVendedorFormularioViewModel modelo)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Identificacion))
            {
                ModelState.AddModelError(
                    "Identificacion",
                    "Debe ingresar la identificación."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                ModelState.AddModelError(
                    "Nombre",
                    "Debe ingresar el nombre."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.Apellido1))
            {
                ModelState.AddModelError(
                    "Apellido1",
                    "Debe ingresar el primer apellido."
                );
            }


            if (string.IsNullOrWhiteSpace(modelo.CorreoElectronico))
            {
                ModelState.AddModelError(
                    "CorreoElectronico",
                    "Debe ingresar el correo electrónico."
                );
            }


            bool identificacionExiste =
                _usuariosContext.tbUsuario
                .Any(u =>
                    u.Identificacion == modelo.Identificacion &&
                    u.Consecutivo != modelo.Consecutivo
                );


            if (identificacionExiste)
            {
                ModelState.AddModelError(
                    "Identificacion",
                    "Ya existe otro usuario con esa identificación."
                );
            }


            bool correoExiste =
                _usuariosContext.tbUsuario
                .Any(u =>
                    u.CorreoElectronico == modelo.CorreoElectronico &&
                    u.Consecutivo != modelo.Consecutivo
                );


            if (correoExiste)
            {
                ModelState.AddModelError(
                    "CorreoElectronico",
                    "Ya existe otro usuario con ese correo electrónico."
                );
            }


            if (!ModelState.IsValid)
            {
                return View(modelo);
            }


            int idRolVendedor =
                _usuariosContext.tbRol
                .Where(r =>
                    r.Nombre == "Vendedor"
                )
                .Select(r =>
                    r.Consecutivo
                )
                .FirstOrDefault();


            var vendedor =
                _usuariosContext.tbUsuario
                .FirstOrDefault(u =>
                    u.Consecutivo == modelo.Consecutivo &&
                    u.ConsecutivoRol == idRolVendedor
                );


            if (vendedor == null)
            {
                return HttpNotFound();
            }


            vendedor.Identificacion =
                modelo.Identificacion;

            vendedor.Nombre =
                modelo.Nombre;

            vendedor.Apellido1 =
                modelo.Apellido1;

            vendedor.Apellido2 =
                modelo.Apellido2;

            vendedor.CorreoElectronico =
                modelo.CorreoElectronico;

            vendedor.Telefono =
                modelo.Telefono;

            vendedor.Estado =
                modelo.Estado;


            _usuariosContext.SaveChanges();


            TempData["Mensaje"] =
                "Vendedor actualizado correctamente.";


            return RedirectToAction("Vendedores");
        }


        public ActionResult CambiarEstadoVendedor(int id)
        {
            if (!EsAdministrador())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idRolVendedor =
                _usuariosContext.tbRol
                .Where(r =>
                    r.Nombre == "Vendedor"
                )
                .Select(r =>
                    r.Consecutivo
                )
                .FirstOrDefault();


            var vendedor =
                _usuariosContext.tbUsuario
                .FirstOrDefault(u =>
                    u.Consecutivo == id &&
                    u.ConsecutivoRol == idRolVendedor
                );


            if (vendedor == null)
            {
                return HttpNotFound();
            }


            vendedor.Estado =
                !vendedor.Estado;


            _usuariosContext.SaveChanges();


            if (vendedor.Estado)
            {
                TempData["Mensaje"] =
                    "Vendedor activado correctamente.";
            }
            else
            {
                TempData["Mensaje"] =
                    "Vendedor desactivado correctamente.";
            }


            return RedirectToAction("Vendedores");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();

                _usuariosContext.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}