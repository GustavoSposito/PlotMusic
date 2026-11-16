using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlotMusic2.Models;

public partial class Playlist
{
    [Key]
    public int IdPlaylist { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Descricao { get; set; }

    public bool Publica { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataCriacao { get; set; }

    public int IdUsuario { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Playlist")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [InverseProperty("IdPlaylistNavigation")]
    public virtual ICollection<PlaylistMusica> PlaylistMusica { get; set; } = new List<PlaylistMusica>();
}
