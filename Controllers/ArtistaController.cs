using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlotMusic.WebAPI.DTO;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;
using PlotMusic.WebAPI.Services;

namespace PlotMusic.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArtistaController : ControllerBase
{
    private readonly IArtistaRepository _artistaRepository;
    private readonly IImageService _imageService;

    public ArtistaController(IArtistaRepository artistaRepository, IImageService imageService)
    {
        _artistaRepository = artistaRepository;
        _imageService = imageService;
    }

    [HttpGet]
    public async Task<IActionResult> Lista(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10,
        [FromQuery] string? busca = null)
    {
        if (pagina < 1 || tamanhoPagina is < 1 or > 100)
            return BadRequest("Página deve ser positiva e tamanhoPagina deve estar entre 1 e 100.");

        var (items, total) = await _artistaRepository.ListarPaginado(pagina, tamanhoPagina, busca);
        return Ok(new { items, total, pagina, tamanhoPagina });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var artista = await _artistaRepository.BuscarPorId(id);
        return artista is null ? NotFound() : Ok(artista);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(11 * 1024 * 1024)]
    public async Task<IActionResult> Cadastrar([FromForm] ArtistaDTO dto)
    {
        if (dto.ArquivoImagem is not null && !_imageService.IsConfigured)
            return Problem("Configure as credenciais do Cloudinary para enviar imagens.", statusCode: 503);

        string? novaUrl = null;
        try
        {
            if (dto.ArquivoImagem is not null)
                novaUrl = await _imageService.UploadImageAsync(dto.ArquivoImagem, ImageFolder.Artistas);
        }
        catch (ArgumentException erro) { return BadRequest(erro.Message); }
        catch (HttpRequestException) { return Problem("Falha ao enviar imagem ao Cloudinary.", statusCode: 502); }

        var foto = novaUrl ?? (string.IsNullOrWhiteSpace(dto.FotoArtista) ? null : dto.FotoArtista);

        var artista = new Artista
        {
            Nome = dto.Nome,
            Biografia = dto.Biografia,
            PaisOrigem = dto.PaisOrigem,
            FotoArtista = foto,
            Ativo = dto.Ativo
        };

        try { await _artistaRepository.Cadastrar(artista); }
        catch
        {
            if (novaUrl is not null) await _imageService.DeleteImageAsync(novaUrl);
            throw;
        }

        return CreatedAtAction(nameof(BuscarPorId), new { id = artista.IdArtista }, artista);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(11 * 1024 * 1024)]
    public async Task<IActionResult> Atualizar(int id, [FromForm] ArtistaDTO dto)
    {
        var atual = await _artistaRepository.BuscarPorId(id);
        if (atual is null) return NotFound();

        if (dto.ArquivoImagem is not null && !_imageService.IsConfigured)
            return Problem("Configure as credenciais do Cloudinary para enviar imagens.", statusCode: 503);

        string? novaUrl = null;
        try
        {
            if (dto.ArquivoImagem is not null)
                novaUrl = await _imageService.UploadImageAsync(dto.ArquivoImagem, ImageFolder.Artistas);
        }
        catch (ArgumentException erro) { return BadRequest(erro.Message); }
        catch (HttpRequestException) { return Problem("Falha ao enviar imagem ao Cloudinary.", statusCode: 502); }

        var foto = novaUrl ?? (string.IsNullOrWhiteSpace(dto.FotoArtista) ? atual.FotoArtista : dto.FotoArtista);

        try
        {
            await _artistaRepository.Atualizar(id, new Artista
            {
                Nome = dto.Nome,
                Biografia = dto.Biografia,
                PaisOrigem = dto.PaisOrigem,
                FotoArtista = foto,
                Ativo = dto.Ativo
            });
        }
        catch
        {
            if (novaUrl is not null) await _imageService.DeleteImageAsync(novaUrl);
            throw;
        }

        if (!string.IsNullOrWhiteSpace(atual.FotoArtista) && atual.FotoArtista != foto)
            await _imageService.DeleteImageAsync(atual.FotoArtista);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Excluir(int id)
    {
        var artista = await _artistaRepository.BuscarPorId(id);
        if (artista is null) return NotFound();
        await _artistaRepository.Excluir(id);
        if (!string.IsNullOrWhiteSpace(artista.FotoArtista))
            await _imageService.DeleteImageAsync(artista.FotoArtista);
        return NoContent();
    }
}
