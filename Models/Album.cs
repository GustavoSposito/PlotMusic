using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlotMusic2.Models;

public partial class Album
{
    [Key]
    public int IdAlbum { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Titulo { get; set; } = null!;

    public DateOnly DataLancamento { get; set; }

    [Unicode(false)]
    public string? CapaUrl { get; set; }

    public int IdArtista { get; set; }

    [ForeignKey("IdArtista")]
    [InverseProperty("Album")]
    public virtual Artista IdArtistaNavigation { get; set; } = null!;

    [InverseProperty("IdAlbumNavigation")]
    public virtual ICollection<Musica> Musica { get; set; } = new List<Musica>();
}
