using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PlotMusic2.Models;

namespace PlotMusic2.BdContextPlot;

public partial class PlotContext : DbContext
{
    public PlotContext()
    {
    }

    public PlotContext(DbContextOptions<PlotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Album { get; set; }

    public virtual DbSet<Artista> Artista { get; set; }

    public virtual DbSet<Genero> Genero { get; set; }

    public virtual DbSet<Musica> Musica { get; set; }

    public virtual DbSet<Playlist> Playlist { get; set; }

    public virtual DbSet<PlaylistMusica> PlaylistMusica { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=D03S20-1252903\\SQLEXPRESS;Database=PlotMusic;User Id=sa;Password=123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.IdAlbum).HasName("PK__Album__BF9C2A22E5561099");

            entity.HasOne(d => d.IdArtistaNavigation).WithMany(p => p.Album)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlbumArtista");
        });

        modelBuilder.Entity<Artista>(entity =>
        {
            entity.HasKey(e => e.IdArtista).HasName("PK__Artista__0A2C192565E423A2");

            entity.Property(e => e.Ativo).HasDefaultValue(true);
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGenero).HasName("PK__Genero__0F83498819D93BAB");
        });

        modelBuilder.Entity<Musica>(entity =>
        {
            entity.HasKey(e => e.IdMusica).HasName("PK__Musica__F026ACA71984F8A0");

            entity.Property(e => e.Publicada).HasDefaultValue(true);

            entity.HasOne(d => d.IdAlbumNavigation).WithMany(p => p.Musica).HasConstraintName("FK_MusicaAlbum");

            entity.HasOne(d => d.IdArtistaNavigation).WithMany(p => p.Musica)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MusicaArtista");

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.Musica)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MusicaGenero");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.IdPlaylist).HasName("PK__Playlist__72ACF23BBE372AAE");

            entity.Property(e => e.DataCriacao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Publica).HasDefaultValue(true);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Playlist)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlaylistUsuario");
        });

        modelBuilder.Entity<PlaylistMusica>(entity =>
        {
            entity.Property(e => e.DataAdicao).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdMusicaNavigation).WithMany(p => p.PlaylistMusica)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Musica_PlaylistMusica");

            entity.HasOne(d => d.IdPlaylistNavigation).WithMany(p => p.PlaylistMusica).HasConstraintName("FK_Playlist_PlaylistMusica");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97F528C325");

            entity.ToTable(tb => tb.HasTrigger("TR_UsuarioSoftDelete"));

            entity.Property(e => e.Ativo).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
