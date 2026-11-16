using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotMusic.WebAPI.DTO;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;

namespace PlotMusic.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GeneroController : ControllerBase
{
    private readonly IGeneroRepository _generoRepository;

    public GeneroController(IGeneroRepository generoRepository)
    {
        _generoRepository = generoRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var generos = await _generoRepository.Listar();
        return Ok(generos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var genero = await _generoRepository.BuscarPorId(id);
        return genero is null ? NotFound() : Ok(genero);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cadastrar([FromBody] GeneroDTO dto)
    {
        var genero = new Genero { Nome = dto.Nome };
        await _generoRepository.Cadastrar(genero);
        return CreatedAtAction(nameof(BuscarPorId), new { id = genero.IdGenero }, genero);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] GeneroDTO dto)
    {
        if (await _generoRepository.BuscarPorId(id) is null) return NotFound();
        await _generoRepository.Atualizar(id, new Genero { Nome = dto.Nome });
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Excluir(int id)
    {
        if (await _generoRepository.BuscarPorId(id) is null) return NotFound();
        await _generoRepository.Excluir(id);
        return NoContent();
    }
}
