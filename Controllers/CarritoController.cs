// Controllers/CarritoController.cs
using CarritoComprasMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CarritoComprasMVC.Controllers
{
    public class CarritoController : Controller
    {
        // Simularemos una base de datos con una lista estática
        private static List<Producto> productos = new List<Producto>();
        private static int nextId = 1;

        // GET: Carrito/Index
        public IActionResult Index()
        {
            // Retorna la vista con la lista de productos
            return View(productos);
        }

        // GET: Carrito/Create
        public IActionResult Create()
        {
            // Retorna el formulario vacío para crear un nuevo producto
            return View();
        }

        // POST: Carrito/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Nombre,Precio")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                // Asignar un ID único al producto
                producto.Id = nextId++;

                // Agregar el producto a la lista
                productos.Add(producto);

                // Redirigir al Index para ver la lista actualizada
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores, regresa el mismo formulario con los datos
            return View(producto);
        }

        // GET: Carrito/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Buscar el producto por ID
            var producto = productos.FirstOrDefault(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Carrito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Buscar y eliminar el producto
            var producto = productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                productos.Remove(producto);
            }

            return RedirectToAction(nameof(Index));
        }

        // Método adicional para obtener el total del carrito
        public IActionResult Resumen()
        {
            ViewBag.TotalProductos = productos.Count;
            ViewBag.TotalPrecio = productos.Sum(p => p.Precio);
            return View(productos);
        }
    }
}
