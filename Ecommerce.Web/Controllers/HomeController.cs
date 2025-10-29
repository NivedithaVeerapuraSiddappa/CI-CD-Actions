using Microsoft.AspNetCore.Mvc;
using Ecommerce.Web.Models;
using System.Collections.Generic;

namespace Ecommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99M },
                new Product { Id = 2, Name = "Phone", Price = 499.99M }
            };
            return View(products);
        }
    }
}
