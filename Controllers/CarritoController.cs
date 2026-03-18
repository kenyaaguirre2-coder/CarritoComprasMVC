// Controllers/CarritoController.cs
using CarritoComprasMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CarritoComprasMVC.Controllers
{
    public class CarritoController : Controller
    {
        private const string SessionKeyProductos = "ProductosCarrito";
        private const string SessionKeyComprador = "NombreComprador";

        // GET: Carrito/Index
        public IActionResult Index()
        {
            // Obtener el nombre del comprador de la sesión
            ViewBag.NombreComprador = HttpContext.Session.GetString(SessionKeyComprador) ?? "Invitado";

            // Obtener la lista de productos de la sesión
            var productos = GetProductosFromSession();
            return View("~/Views/Carritos/Index.cshtml", productos);
        }

        // GET: Carrito/Create
        public IActionResult Create()
        {
            return View("~/Views/Carritos/Create.cshtml");
        }

        // POST: Carrito/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Nombre,Precio")] Producto producto, string nombreComprador)
        {
            // Guardar el nombre del comprador en sesión si se proporciona
            if (!string.IsNullOrEmpty(nombreComprador))
            {
                HttpContext.Session.SetString(SessionKeyComprador, nombreComprador);
            }

            if (ModelState.IsValid)
            {
                // Obtener productos actuales de la sesión
                var productos = GetProductosFromSession();

                // Asignar nuevo ID (basado en el máximo existente + 1)
                producto.Id = productos.Any() ? productos.Max(p => p.Id) + 1 : 1;

                // Agregar el producto
                productos.Add(producto);

                // Guardar en sesión
                SaveProductosToSession(productos);

                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Carritos/Create.cshtml", producto);
        }

        // GET: Carrito/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var productos = GetProductosFromSession();
            var producto = productos.FirstOrDefault(p => p.Id == id);

            if (producto == null) return NotFound();

            return View("~/Views/Carritos/Delete.cshtml", producto);
        }

        // POST: Carrito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var productos = GetProductosFromSession();
            var producto = productos.FirstOrDefault(p => p.Id == id);

            if (producto != null)
            {
                productos.Remove(producto);
                SaveProductosToSession(productos);
            }

            return RedirectToAction(nameof(Index));
        }

        // Métodos auxiliares para manejar la sesión
        private List<Producto> GetProductosFromSession()
        {
            var sessionData = HttpContext.Session.GetString(SessionKeyProductos);
            return sessionData == null
                ? new List<Producto>()
                : JsonConvert.DeserializeObject<List<Producto>>(sessionData);
        }

        private void SaveProductosToSession(List<Producto> productos)
        {
            HttpContext.Session.SetString(SessionKeyProductos, JsonConvert.SerializeObject(productos));
        }

        // GET: Carrito/Clear (opcional - para limpiar la sesión)
        public IActionResult Clear()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }
    }
}
