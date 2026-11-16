using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlotMusic2.Models;

[PrimaryKey("IdPlaylist", "IdMusica")]
public partial class PlaylistMusica
{
    [Key]
    public int IdPlaylist { get; set; }

    [Key]
    public int IdMusica { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataAdicao { get; set; }

    [ForeignKey("IdMusica")]
    [InverseProperty("PlaylistMusica")]
    public virtual Musica IdMusicaNavigation { get; set; } = null!;

    [ForeignKey("IdPlaylist")]
    [InverseProperty("PlaylistMusica")]
    public virtual Playlist IdPlaylistNavigation { get; set; } = null!;
}
