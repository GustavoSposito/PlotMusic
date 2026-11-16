using Microsoft.EntityFrameworkCore;
using PlotMusic2.BdContextPlot;
using PlotMusic2.Interfaces;
using PlotMusic2.Models;



namespace PlotMusic2.Repositories;

public class AlbumRepository : IAlbumRepository
{
    private readonly PlotContext _context;

    public AlbumRepository(PlotContext context)
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

   
    

    Task<Album?> IAlbumRepository.BuscarPorId(int id)
    {
        throw new NotImplementedException();
    }

    async Task<(List<Album> albums, int total)> IAlbumRepository.ListarPaginado(int pagina, int tamanhoPagina, string? busca, int? idArtista)
    {
        var query = _context.Album.AsNoTracking().AsQueryable();

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
