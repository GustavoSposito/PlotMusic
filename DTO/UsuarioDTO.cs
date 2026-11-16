using System.ComponentModel.DataAnnotations;

namespace PlotMusic.WebAPI.DTO;

public class UsuarioDTO : IValidatableObject
{
    [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(8)]
    public string Senha { get; set; } = string.Empty;

    /// <summary>
    /// Admin | Artista | Ouvinte  (apenas Admin pode definir; cadastro público sempre vira "Ouvinte")
    /// </summary>
    public string? TipoUsuario { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var tiposValidos = new[] { "Admin", "Artista", "Ouvinte" };
        if (!string.IsNullOrWhiteSpace(TipoUsuario) && !tiposValidos.Contains(TipoUsuario))
            yield return new ValidationResult(
                "TipoUsuario deve ser Admin, Artista ou Ouvinte.",
                new[] { nameof(TipoUsuario) });
    }
}
