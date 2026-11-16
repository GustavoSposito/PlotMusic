using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlotMusic2.Models;

[Index("Nome", Name = "UQ__Genero__7D8FE3B20B5E0608", IsUnique = true)]
public partial class Genero
{
    [Key]
    public int IdGenero { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [InverseProperty("IdGeneroNavigation")]
    public virtual ICollection<Musica> Musica { get; set; } = new List<Musica>();
}
