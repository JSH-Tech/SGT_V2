using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SGT_V2.Models;

public partial class DbSgtContext : DbContext
{
    public DbSgtContext()
    {
    }

    public DbSgtContext(DbContextOptions<DbSgtContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Commentaire> Commentaires { get; set; }

    public virtual DbSet<Personne> Personnes { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     => optionsBuilder.UseMySQL("Server=127.0.0.1; Database=db_sgt; Uid=root; Pwd=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Commentaire>(entity =>
        {
            entity.HasKey(e => e.Idcommentaire).HasName("PRIMARY");

            entity.ToTable("commentaires");

            entity.HasIndex(e => e.IdticketCommentaire, "IDTICKET_COMMENTAIRE");

            entity.Property(e => e.Idcommentaire).HasColumnName("IDCOMMENTAIRE");
            entity.Property(e => e.Contenu)
                .HasColumnType("text")
                .HasColumnName("CONTENU");
            entity.Property(e => e.Datecommentaire)
                .HasColumnType("date")
                .HasColumnName("DATECOMMENTAIRE");
            entity.Property(e => e.IdticketCommentaire).HasColumnName("IDTICKET_COMMENTAIRE");

            entity.HasOne(d => d.IdticketCommentaireNavigation).WithMany(p => p.Commentaires)
                .HasForeignKey(d => d.IdticketCommentaire)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("commentaires_ibfk_1");
        });

        modelBuilder.Entity<Personne>(entity =>
        {
            entity.HasKey(e => e.Idpersonne).HasName("PRIMARY");

            entity.ToTable("personnes");

            entity.Property(e => e.Idpersonne).HasColumnName("IDPERSONNE");
            entity.Property(e => e.Courriel)
                .HasMaxLength(50)
                .HasColumnName("COURRIEL");
            entity.Property(e => e.Departement)
                .HasMaxLength(40)
                .HasColumnName("DEPARTEMENT");
            entity.Property(e => e.Matricule).HasColumnName("MATRICULE");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .HasColumnName("NOM");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Idticket).HasName("PRIMARY");

            entity.ToTable("tickets");

            entity.HasIndex(e => e.IdpersonneTickets, "IDPERSONNE_TICKETS");

            entity.Property(e => e.Idticket).HasColumnName("IDTICKET");
            entity.Property(e => e.Categorie)
                .HasMaxLength(25)
                .HasColumnName("CATEGORIE");
            entity.Property(e => e.Datecreation)
                .HasColumnType("date")
                .HasColumnName("DATECREATION");
            entity.Property(e => e.Datefermeture)
                .HasColumnType("date")
                .HasColumnName("DATEFERMETURE");
            entity.Property(e => e.IdpersonneTickets).HasColumnName("IDPERSONNE_TICKETS");
            entity.Property(e => e.Priorite)
                .HasMaxLength(25)
                .HasColumnName("PRIORITE");
            entity.Property(e => e.Status)
                .HasMaxLength(25)
                .HasColumnName("STATUS");
            entity.Property(e => e.Titre)
                .HasMaxLength(50)
                .HasColumnName("TITRE");
            entity.Property(e => e.Type)
                .HasMaxLength(25)
                .HasColumnName("TYPE");

            entity.HasOne(d => d.IdpersonneTicketsNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdpersonneTickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tickets_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
