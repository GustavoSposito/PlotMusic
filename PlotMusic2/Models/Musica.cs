using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlotMusic2.Models;

public partial class Musica
{
    [Key]
    public int IdMusica { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Titulo { get; set; } = null!;

    public int DuracaoSegundos { get; set; }

    public int? NumeroFaixa { get; set; }

    [Unicode(false)]
    public string? AudioUrl { get; set; }

    public bool Publicada { get; set; }

    public int? IdAlbum { get; set; }

    public int IdGenero { get; set; }

    public int IdArtista { get; set; }

    [ForeignKey("IdAlbum")]
    [InverseProperty("Musica")]
    public virtual Album? IdAlbumNavigation { get; set; }

    [ForeignKey("IdArtista")]
    [InverseProperty("Musica")]
    public virtual Artista IdArtistaNavigation { get; set; } = null!;

    [ForeignKey("IdGenero")]
    [InverseProperty("Musica")]
    public virtual Genero IdGeneroNavigation { get; set; } = null!;

    [InverseProperty("IdMusicaNavigation")]
    public virtual ICollection<PlaylistMusica> PlaylistMusica { get; set; } = new List<PlaylistMusica>();
}
