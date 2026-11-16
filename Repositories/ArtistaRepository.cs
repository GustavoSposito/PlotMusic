using Microsoft.EntityFrameworkCore;
using PlotMusic.WebAPI.Context;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;

namespace PlotMusic.WebAPI.Repositories;

public class ArtistaRepository : IArtistaRepository
{
    private readonly PlotMusicDbContext _context;

    public ArtistaRepository(PlotMusicDbContext context)
    {
        _context = context;
    }

    public async Task Atualizar(int id, Artista artista)
    {
        var existente = await _context.Artista.FindAsync(id);
        if (existente != null)
        {
            existente.Nome = artista.Nome;
            existente.Biografia = artista.Biografia;
            existente.PaisOrigem = artista.PaisOrigem;
            existente.FotoArtista = artista.FotoArtista;
            existente.Ativo = artista.Ativo;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Artista?> BuscarPorId(int id)
    {
        return await _context.Artista.AsNoTracking().FirstOrDefaultAsync(a => a.IdArtista == id);
    }

    public async Task Cadastrar(Artista artista)
    {
        await _context.Artista.AddAsync(artista);
        await _context.SaveChangesAsync();
    }

    public async Task Excluir(int id)
    {
        var existente = await _context.Artista.FindAsync(id);
        if (existente != null)
        {
            _context.Artista.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(List<Artista> artistas, int total)> ListarPaginado(int pagina = 1, int tamanhoPagina = 10, string? busca = null)
    {
        var query = _context.Artista.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
            query = query.Where(a => a.Nome.Contains(busca));

        int total = await query.CountAsync();

        var items = await query
            .OrderBy(a => a.Nome).ThenBy(a => a.IdArtista)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return (items, total);
    }
}
