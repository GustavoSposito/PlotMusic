using Microsoft.EntityFrameworkCore;
using PlotMusic.WebAPI.Context;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;

namespace PlotMusic.WebAPI.Repositories;

public class AlbumRepository : IAlbumRepository
{
    private readonly PlotMusicDbContext _context;

    public AlbumRepository(PlotMusicDbContext context)
    {
        _context = context;
    }

    public async Task Atualizar(int id, Album album)
    {
        var existente = await _context.Album.FindAsync(id);
        if (existente != null)
        {
            existente.Titulo = album.Titulo;
            existente.DataLancamento = album.DataLancamento;
            existente.CapaUrl = album.CapaUrl;
            existente.IdArtista = album.IdArtista;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Album?> BuscarPorId(int id)
    {
        return await _context.Album
            .AsNoTracking()
            .Include(a => a.Artista)
            .FirstOrDefaultAsync(a => a.IdAlbum == id);
    }

    public async Task Cadastrar(Album album)
    {
        await _context.Album.AddAsync(album);
        await _context.SaveChangesAsync();
    }

    public async Task Excluir(int id)
    {
        var existente = await _context.Album.FindAsync(id);
        if (existente != null)
        {
            _context.Album.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(List<Album> albums, int total)> ListarPaginado(int pagina = 1, int tamanhoPagina = 10, string? busca = null, int? idArtista = null)
    {
        var query = _context.Album.AsNoTracking().Include(a => a.Artista).AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
            query = query.Where(a => a.Titulo.Contains(busca));

        if (idArtista.HasValue)
            query = query.Where(a => a.IdArtista == idArtista.Value);

        int total = await query.CountAsync();

        var items = await query
            .OrderBy(a => a.Titulo).ThenBy(a => a.IdAlbum)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return (items, total);
    }
}
