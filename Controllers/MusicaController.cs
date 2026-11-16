using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotMusic.WebAPI.DTO;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;

namespace PlotMusic.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MusicaController : ControllerBase
{
    private readonly IMusicaRepository _musicaRepository;
    private readonly IArtistaRepository _artistaRepository;
    private readonly IGeneroRepository _generoRepository;
    private readonly IAlbumRepository _albumRepository;

    public MusicaController(
        IMusicaRepository musicaRepository,
        IArtistaRepository artistaRepository,
        IGeneroRepository generoRepository,
        IAlbumRepository albumRepository)
    {
        _musicaRepository = musicaRepository;
        _artistaRepository = artistaRepository;
        _generoRepository = generoRepository;
        _albumRepository = albumRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Lista(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10,
        [FromQuery] string? busca = null,
        [FromQuery] int? idArtista = null,
        [FromQuery] int? idAlbum = null,
        [FromQuery] int? idGenero = null)
    {
        if (pagina < 1 || tamanhoPagina is < 1 or > 100)
            return BadRequest("Página deve ser positiva e tamanhoPagina deve estar entre 1 e 100.");

        var (items, total) = await _musicaRepository.ListarPaginado(pagina, tamanhoPagina, busca, idArtista, idAlbum, idGenero);
        return Ok(new { items, total, pagina, tamanhoPagina });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var musica = await _musicaRepository.BuscarPorId(id);
        return musica is null ? NotFound() : Ok(musica);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Artista")]
    public async Task<IActionResult> Cadastrar([FromBody] MusicaDTO dto)
    {
        if (await _artistaRepository.BuscarPorId(dto.IdArtista) is null)
            return BadRequest("Artista não encontrado.");

        if (await _generoRepository.BuscarPorId(dto.IdGenero) is null)
            return BadRequest("Gênero não encontrado.");

        if (dto.IdAlbum.HasValue && await _albumRepository.BuscarPorId(dto.IdAlbum.Value) is null)
            return BadRequest("Álbum não encontrado.");

        var musica = new Musica
        {
            Titulo = dto.Titulo,
            DuracaoSegundos = dto.DuracaoSegundos,
            NumeroFaixa = dto.NumeroFaixa,
            AudioUrl = dto.AudioUrl,
            Publicada = dto.Publicada,
            IdAlbum = dto.IdAlbum,
            IdGenero = dto.IdGenero,
            IdArtista = dto.IdArtista
        };

        await _musicaRepository.Cadastrar(musica);
        return CreatedAtAction(nameof(BuscarPorId), new { id = musica.IdMusica }, musica);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Artista")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] MusicaDTO dto)
    {
        if (await _musicaRepository.BuscarPorId(id) is null) return NotFound();

        if (await _artistaRepository.BuscarPorId(dto.IdArtista) is null)
            return BadRequest("Artista não encontrado.");

        if (await _generoRepository.BuscarPorId(dto.IdGenero) is null)
            return BadRequest("Gênero não encontrado.");

        if (dto.IdAlbum.HasValue && await _albumRepository.BuscarPorId(dto.IdAlbum.Value) is null)
            return BadRequest("Álbum não encontrado.");

        await _musicaRepository.Atualizar(id, new Musica
        {
            Titulo = dto.Titulo,
            DuracaoSegundos = dto.DuracaoSegundos,
            NumeroFaixa = dto.NumeroFaixa,
            AudioUrl = dto.AudioUrl,
            Publicada = dto.Publicada,
            IdAlbum = dto.IdAlbum,
            IdGenero = dto.IdGenero,
            IdArtista = dto.IdArtista
        });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Artista")]
    public async Task<IActionResult> Excluir(int id)
    {
        if (await _musicaRepository.BuscarPorId(id) is null) return NotFound();
        await _musicaRepository.Excluir(id);
        return NoContent();
    }
}
