using Microsoft.AspNetCore.Mvc;

namespace DogShelterService.Api.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("/health")]
        public IActionResult GetHealth()
        {
            return Ok(new { Status = "available" });
        }
    }
}