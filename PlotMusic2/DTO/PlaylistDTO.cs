using System.ComponentModel.DataAnnotations;

namespace PlotMusic2.DTO;

public class PlaylistDTO
{
    [Required(ErrorMessage = "O nome da playlist é obrigatório.")]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Descricao { get; set; }

    public bool Publica { get; set; } = true;
}
