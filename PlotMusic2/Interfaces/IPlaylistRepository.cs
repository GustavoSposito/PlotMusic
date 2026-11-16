
using PlotMusic2.Models;

namespace PlotMusic2.Interfaces;

public interface IPlaylistRepository
{
    Task Cadastrar(Playlist playlist);
    Task Atualizar(int id, Playlist playlist);
    Task Excluir(int id);
    Task<Playlist?> BuscarPorId(int id);
    Task<List<Playlist>> ListarPorUsuario(int idUsuario);
    Task AdicionarMusica(int idPlaylist, int idMusica);
    Task RemoverMusica(int idPlaylist, int idMusica);
  
}
