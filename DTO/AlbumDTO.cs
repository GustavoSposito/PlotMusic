using System.ComponentModel.DataAnnotations;

namespace PlotMusic.WebAPI.DTO;

public class AlbumDTO : IValidatableObject
{
    [Required(ErrorMessage = "O título do álbum é obrigatório.")]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;

    public DateOnly DataLancamento { get; set; }

    /// <summary>URL externa da capa (alternativa ao upload de arquivo)</summary>
    public string? CapaUrl { get; set; }

    /// <summary>Arquivo de imagem para upload via Cloudinary</summary>
    public IFormFile? ArquivoImagem { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "IdArtista deve ser um ID válido.")]
    public int IdArtista { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ArquivoImagem is not null && !string.IsNullOrWhiteSpace(CapaUrl))
            yield return new ValidationResult(
                "Envie ArquivoImagem ou CapaUrl, não os dois.",
                new[] { nameof(ArquivoImagem), nameof(CapaUrl) });

        if (DataLancamento.Year < 1900 || DataLancamento.Year > DateTime.UtcNow.Year)
            yield return new ValidationResult(
                "O ano de lançamento deve estar entre 1900 e o ano atual.",
                new[] { nameof(DataLancamento) });
    }
}
