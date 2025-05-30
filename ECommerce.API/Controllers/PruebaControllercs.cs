using Microsoft.AspNetCore.Mvc;
using SuperBodega.Models.Dtos;

[ApiController]
[Route("api/[controller]")]
public class PruebaController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("API ECommerce funciona");
}

