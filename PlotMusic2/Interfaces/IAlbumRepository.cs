
using PlotMusic2.Models;

namespace PlotMusic2.Interfaces;

public interface IAlbumRepository
{
    Task Cadastrar(Album album);
    Task Atualizar(int id, Album album);
    Task Excluir(int id);
    Task<Album?> BuscarPorId(int id);
    Task<(List<Album> albums, int total)> ListarPaginado(int pagina = 1, int tamanhoPagina = 10, string? busca = null, int? idArtista = null);
}
