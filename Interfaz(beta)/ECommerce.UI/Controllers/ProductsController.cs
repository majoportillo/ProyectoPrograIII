using Microsoft.AspNetCore.Mvc;

namespace ECommerce.UI.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            string html = @"
                <html>
                    <head>
                        <title>Productos</title>
                    </head>
                    <body>
                        <h1>¡Hola! Esta es la página de productos simple.</h1>
                        <p>Esto es HTML enviado desde el controlador.</p>
                    </body>
                </html>";

            return Content(html, "text/html");
        }
    }
}
