using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalPrograIII.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarritoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Carrito Funcionando");
        }
    }
}
