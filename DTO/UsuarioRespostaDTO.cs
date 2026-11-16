using PlotMusic.WebAPI.Models;

namespace PlotMusic.WebAPI.DTO;

public record UsuarioRespostaDTO(int IdUsuario, string Nome, string Email, string TipoUsuario, bool Ativo)
{
    public static UsuarioRespostaDTO Converter(Usuario usuario) => new(
        usuario.IdUsuario,
        usuario.Nome,
        usuario.Email,
        usuario.TipoUsuario,
        usuario.Ativo);
}
