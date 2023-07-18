using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FullStack.API.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginRequest : ControllerBase
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
