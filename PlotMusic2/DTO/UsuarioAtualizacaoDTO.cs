using System.ComponentModel.DataAnnotations;

namespace PlotMusic2.DTO;

public class UsuarioAtualizacaoDTO
{
    [Required, StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [MinLength(8)]
    public string? Senha { get; set; }
}
