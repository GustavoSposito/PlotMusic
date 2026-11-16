using System.ComponentModel.DataAnnotations;

namespace PlotMusic.WebAPI.DTO;

public class ArtistaDTO : IValidatableObject
{
    [Required(ErrorMessage = "O nome do artista é obrigatório.")]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    public string? Biografia { get; set; }

    [StringLength(50)]
    public string? PaisOrigem { get; set; }

    /// <summary>URL externa da foto do artista (alternativa ao upload de arquivo)</summary>
    public string? FotoArtista { get; set; }

    /// <summary>Arquivo de imagem para upload via Cloudinary</summary>
    public IFormFile? ArquivoImagem { get; set; }

    public bool Ativo { get; set; } = true;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ArquivoImagem is not null && !string.IsNullOrWhiteSpace(FotoArtista))
            yield return new ValidationResult(
                "Envie ArquivoImagem ou FotoArtista (URL), não os dois.",
                new[] { nameof(ArquivoImagem), nameof(FotoArtista) });
    }
}
