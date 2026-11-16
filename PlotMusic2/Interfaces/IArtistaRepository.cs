
using PlotMusic2.Models;

namespace PlotMusic2.Interfaces;

public interface IArtistaRepository
{
    Task Cadastrar(Artista artista);
    Task Atualizar(int id, Artista artista);
    Task Excluir(int id);
    Task<Artista?> BuscarPorId(int id);
    Task<(List<Artista> artistas, int total)> ListarPaginado(int pagina = 1, int tamanhoPagina = 10, string? busca = null);
}
