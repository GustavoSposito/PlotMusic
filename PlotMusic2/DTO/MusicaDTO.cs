using System.ComponentModel.DataAnnotations;

namespace PlotMusic2.DTO;

public class MusicaDTO
{
    [Required(ErrorMessage = "O título da música é obrigatório.")]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "A duração deve ser positiva (em segundos).")]
    public int DuracaoSegundos { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O número da faixa deve ser positivo.")]
    public int? NumeroFaixa { get; set; }

    public string? AudioUrl { get; set; }

    public bool Publicada { get; set; } = true;

    /// <summary>Álbum ao qual a música pertence (opcional)</summary>
    public int? IdAlbum { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "IdGenero deve ser um ID válido.")]
    public int IdGenero { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "IdArtista deve ser um ID válido.")]
    public int IdArtista { get; set; }
}
