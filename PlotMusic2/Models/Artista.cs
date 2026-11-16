using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlotMusic2.Models;

public partial class Artista
{
    [Key]
    public int IdArtista { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Unicode(false)]
    public string? Biografia { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PaisOrigem { get; set; }

    [Unicode(false)]
    public string? FotoArtista { get; set; }

    public bool Ativo { get; set; }

    [InverseProperty("IdArtistaNavigation")]
    public virtual ICollection<Album> Album { get; set; } = new List<Album>();

    [InverseProperty("IdArtistaNavigation")]
    public virtual ICollection<Musica> Musica { get; set; } = new List<Musica>();
}
