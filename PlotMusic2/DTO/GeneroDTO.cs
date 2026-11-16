using System.ComponentModel.DataAnnotations;

namespace PlotMusic2.DTO;

public class GeneroDTO
{
    [Required(ErrorMessage = "O nome do gênero é obrigatório.")]
    [StringLength(50)]
    public string Nome { get; set; } = string.Empty;
}
