using Microsoft.EntityFrameworkCore;
using PlotMusic.WebAPI.Context;
using PlotMusic.WebAPI.Interfaces;
using PlotMusic.WebAPI.Models;

namespace PlotMusic.WebAPI.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
    private readonly PlotMusicDbContext _context;

    public PlaylistRepository(PlotMusicDbContext context)
    {
        _context = context;
    }

    public async Task Atualizar(int id, Playlist playlist)
    {
        var existente = await _context.Playlist.FindAsync(id);
        if (existente != null)
        {
            existente.Nome = playlist.Nome;
            existente.Descricao = playlist.Descricao;
            existente.Publica = playlist.Publica;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Playlist?> BuscarPorId(int id)
    {
        return await _context.Playlist
            .AsNoTracking()
            .Include(p => p.Usuario)
            .Include(p => p.PlaylistMusicas)
                .ThenInclude(pm => pm.Musica)
                    .ThenInclude(m => m.Artista)
            .FirstOrDefaultAsync(p => p.IdPlaylist == id);
    }

    public async Task Cadastrar(Playlist playlist)
    {
        await _context.Playlist.AddAsync(playlist);
        await _context.SaveChangesAsync();
    }

    public async Task Excluir(int id)
    {
        var existente = await _context.Playlist.FindAsync(id);
        if (existente != null)
        {
            _context.Playlist.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Playlist>> ListarPorUsuario(int idUsuario)
    {
        return await _context.Playlist
            .AsNoTracking()
            .Where(p => p.IdUsuario == idUsuario)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task AdicionarMusica(int idPlaylist, int idMusica)
    {
        var jaExiste = await _context.PlaylistMusica
            .AnyAsync(pm => pm.IdPlaylist == idPlaylist && pm.IdMusica == idMusica);

        if (!jaExiste)
        {
            await _context.PlaylistMusica.AddAsync(new PlaylistMusica
            {
                IdPlaylist = idPlaylist,
                IdMusica = idMusica,
                DataAdicao = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoverMusica(int idPlaylist, int idMusica)
    {
        var item = await _context.PlaylistMusica
            .FirstOrDefaultAsync(pm => pm.IdPlaylist == idPlaylist && pm.IdMusica == idMusica);

        if (item != null)
        {
            _context.PlaylistMusica.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
