using BancoSENAIAPI.Data;
using BancoSENAIAPI.Dtos;
using BancoSENAIAPI.Models;
using BancoSENAIAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFramewokCore;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegisterRequest dto)
        {
            if (await _context Usuario AnyAsync(uint => u.N)
            if (await _context Usuario AnyAsync(uint => u.N)

        }
    }
}