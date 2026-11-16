using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotMusic.WebAPI.DTO;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PlotMusic.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PlaylistController : ControllerBase
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IMusicaRepository _musicaRepository;

    public PlaylistController(IPlaylistRepository playlistRepository, IMusicaRepository musicaRepository)
    {
        _playlistRepository = playlistRepository;
        _musicaRepository = musicaRepository;
    }

    /// <summary>Lista todas as playlists do usuário autenticado</summary>
    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var idUsuario = ObterIdUsuario();
        if (idUsuario is null) return Forbid();

        var playlists = await _playlistRepository.ListarPorUsuario(idUsuario.Value);
        return Ok(playlists);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var playlist = await _playlistRepository.BuscarPorId(id);
        if (playlist is null) return NotFound();
        if (!PodeAcessar(playlist)) return Forbid();
        return Ok(playlist);
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] PlaylistDTO dto)
    {
        var idUsuario = ObterIdUsuario();
        if (idUsuario is null) return Forbid();

        var playlist = new Playlist
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Publica = dto.Publica,
            DataCriacao = DateTime.UtcNow,
            IdUsuario = idUsuario.Value
        };

        await _playlistRepository.Cadastrar(playlist);
        return CreatedAtAction(nameof(BuscarPorId), new { id = playlist.IdPlaylist }, playlist);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] PlaylistDTO dto)
    {
        var atual = await _playlistRepository.BuscarPorId(id);
        if (atual is null) return NotFound();
        if (!PodeAcessar(atual)) return Forbid();

        await _playlistRepository.Atualizar(id, new Playlist
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Publica = dto.Publica
        });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var playlist = await _playlistRepository.BuscarPorId(id);
        if (playlist is null) return NotFound();
        if (!PodeAcessar(playlist)) return Forbid();

        await _playlistRepository.Excluir(id);
        return NoContent();
    }

    /// <summary>Adiciona uma música à playlist</summary>
    [HttpPost("{id:int}/musicas/{idMusica:int}")]
    public async Task<IActionResult> AdicionarMusica(int id, int idMusica)
    {
        var playlist = await _playlistRepository.BuscarPorId(id);
        if (playlist is null) return NotFound("Playlist não encontrada.");
        if (!PodeAcessar(playlist)) return Forbid();

        if (await _musicaRepository.BuscarPorId(idMusica) is null)
            return NotFound("Música não encontrada.");

        await _playlistRepository.AdicionarMusica(id, idMusica);
        return NoContent();
    }

    /// <summary>Remove uma música da playlist</summary>
    [HttpDelete("{id:int}/musicas/{idMusica:int}")]
    public async Task<IActionResult> RemoverMusica(int id, int idMusica)
    {
        var playlist = await _playlistRepository.BuscarPorId(id);
        if (playlist is null) return NotFound("Playlist não encontrada.");
        if (!PodeAcessar(playlist)) return Forbid();

        await _playlistRepository.RemoverMusica(id, idMusica);
        return NoContent();
    }

    private int? ObterIdUsuario()
    {
        var idTexto = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return int.TryParse(idTexto, out var id) ? id : null;
    }

    private bool PodeAcessar(Playlist playlist) =>
        User.IsInRole("Admin") ||
        (int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) && playlist.IdUsuario == id);
}
