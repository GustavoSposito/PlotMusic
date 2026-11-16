using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlotMusic2.Models;

[Index("Email", Name = "UQ__Usuario__A9D1053406B13BA9", IsUnique = true)]
public partial class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Senha { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string TipoUsuario { get; set; } = null!;

    public bool Ativo { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Playlist> Playlist { get; set; } = new List<Playlist>();
}
