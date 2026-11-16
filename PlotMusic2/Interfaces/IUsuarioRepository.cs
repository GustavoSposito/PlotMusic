
using PlotMusic2.Models;

namespace PlotMusic2.Interfaces;

public interface IUsuarioRepository
{
    Task Cadastrar(Usuario usuario);
    Task Atualizar(int id, Usuario usuario);
    Task Excluir(int id);
    Task<Usuario?> BuscarPorId(int id);
    Task<List<Usuario>> Listar();
    Task<Usuario?> BuscarPorEmailESenha(string email, string senha);
   
}
