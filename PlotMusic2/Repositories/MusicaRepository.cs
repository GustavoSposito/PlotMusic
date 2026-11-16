using Microsoft.EntityFrameworkCore;
using PlotMusic2.BdContextPlot;
using PlotMusic2.Interfaces;
using PlotMusic2.Models;


namespace PlotMusic2.Repositories;

public class MusicaRepository : IMusicaRepository
{
    private readonly PlotContext _context;

    public MusicaRepository(PlotContext context)
    {
        _context = context;
    }

    public async Task Atualizar(int id, Musica musica)
    {
        var existente = await _context.Musica.FindAsync(id);
        if (existente != null)
        {
            existente.Titulo = musica.Titulo;
            existente.DuracaoSegundos = musica.DuracaoSegundos;
            existente.NumeroFaixa = musica.NumeroFaixa;
            existente.AudioUrl = musica.AudioUrl;
            existente.Publicada = musica.Publicada;
            existente.IdAlbum = musica.IdAlbum;
            existente.IdGenero = musica.IdGenero;
            existente.IdArtista = musica.IdArtista;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Musica?> BuscarPorId(int id)
    {
        return await _context.Musica
            .AsNoTracking()
            .Include(m => m.IdArtista)
            .Include(m => m.IdGenero)
            .Include(m => m.IdAlbum)
            .FirstOrDefaultAsync(m => m.IdMusica == id);
    }

    public async Task Cadastrar(Musica musica)
    {
        await _context.Musica.AddAsync(musica);
        await _context.SaveChangesAsync();
    }

    public async Task Excluir(int id)
    {
        var existente = await _context.Musica.FindAsync(id);
        if (existente != null)
        {
            _context.Musica.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(List<Musica> musicas, int total)> ListarPaginado(int pagina = 1, int tamanhoPagina = 10, string? busca = null, int? idArtista = null, int? idAlbum = null, int? idGenero = null)
    {
        var query = _context.Musica
            .AsNoTracking()
            .Include(m => m.IdArtista)
            .Include(m => m.IdGenero)
            .Include(m => m.IdAlbum)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(busca))
            query = query.Where(m => m.Titulo.Contains(busca));

        if (idArtista.HasValue)
            query = query.Where(m => m.IdArtista == idArtista.Value);

        if (idAlbum.HasValue)
            query = query.Where(m => m.IdAlbum == idAlbum.Value);

        if (idGenero.HasValue)
            query = query.Where(m => m.IdGenero == idGenero.Value);

        int total = await query.CountAsync();

        var items = await query
            .OrderBy(m => m.Titulo).ThenBy(m => m.IdMusica)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return (items, total);
    }
}
