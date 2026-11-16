using PlotMusic.WebAPI.Models;

namespace PlotMusic.WebAPI.Interfaces;

public interface IGeneroRepository
{
    Task Cadastrar(Genero genero);
    Task Atualizar(int id, Genero genero);
    Task Excluir(int id);
    Task<Genero?> BuscarPorId(int id);
    Task<List<Genero>> Listar();
}
