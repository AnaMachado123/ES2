using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace back.Models;

public partial class GestaoServicosClientesContext : DbContext
{
    public GestaoServicosClientesContext()
    {
    }

    public GestaoServicosClientesContext(DbContextOptions<GestaoServicosClientesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Convite> Convites { get; set; }

    public virtual DbSet<Projeto> Projetos { get; set; }

    public virtual DbSet<Projeto1> Projetos1 { get; set; }

    public virtual DbSet<Relatorio> Relatorios { get; set; }

    public virtual DbSet<Relatorioproj> Relatorioprojs { get; set; }

    public virtual DbSet<Tarefa> Tarefas { get; set; }

    public virtual DbSet<Utilizador> Utilizadors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=GestaoServicosClientes;Username=postgres;Password=Adriana");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cliente_pkey");

            entity.ToTable("cliente");

            entity.HasIndex(e => e.Email, "cliente_email_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Telefone)
                .HasMaxLength(20)
                .HasColumnName("telefone");
        });

        modelBuilder.Entity<Convite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("convite_pkey");

            entity.ToTable("convite");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Dataenvio).HasColumnName("dataenvio");
            entity.Property(e => e.Dataresposta).HasColumnName("dataresposta");
            entity.Property(e => e.Projetoid).HasColumnName("projetoid");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Utilizadorid).HasColumnName("utilizadorid");

            entity.HasOne(d => d.Projeto).WithMany(p => p.Convites)
                .HasForeignKey(d => d.Projetoid)
                .HasConstraintName("convite_projetoid_fkey");

            entity.HasOne(d => d.Utilizador).WithMany(p => p.Convites)
                .HasForeignKey(d => d.Utilizadorid)
                .HasConstraintName("convite_utilizadorid_fkey");
        });

        modelBuilder.Entity<Projeto1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projeto_pkey");

            entity.ToTable("projeto");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Clienteid).HasColumnName("clienteid");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Utilizadorid).HasColumnName("utilizadorid");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Projeto1s)
                .HasForeignKey(d => d.Clienteid)
                .HasConstraintName("projeto_clienteid_fkey");

            entity.HasOne(d => d.Utilizador).WithMany(p => p.Projeto1s)
                .HasForeignKey(d => d.Utilizadorid)
                .HasConstraintName("projeto_utilizadorid_fkey");
        });

        modelBuilder.Entity<Relatorio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("relatorio_pkey");

            entity.ToTable("relatorio");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Ano).HasColumnName("ano");
            entity.Property(e => e.Mes).HasColumnName("mes");
            entity.Property(e => e.Numprojeto).HasColumnName("numprojeto");
            entity.Property(e => e.Precohora)
                .HasPrecision(10, 2)
                .HasColumnName("precohora");
            entity.Property(e => e.Precototal)
                .HasPrecision(10, 2)
                .HasColumnName("precototal");
            entity.Property(e => e.Totalhoras)
                .HasPrecision(10, 2)
                .HasColumnName("totalhoras");
            entity.Property(e => e.Utilizadorid).HasColumnName("utilizadorid");

            entity.HasOne(d => d.Utilizador).WithMany(p => p.Relatorios)
                .HasForeignKey(d => d.Utilizadorid)
                .HasConstraintName("relatorio_utilizadorid_fkey");
        });

        modelBuilder.Entity<Relatorioproj>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("relatorioproj_pkey");

            entity.ToTable("relatorioproj");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Clientid).HasColumnName("clientid");
            entity.Property(e => e.Precotarefas)
                .HasPrecision(10, 2)
                .HasColumnName("precotarefas");
            entity.Property(e => e.Precotot)
                .HasPrecision(10, 2)
                .HasColumnName("precotot");
            entity.Property(e => e.Projetoid).HasColumnName("projetoid");

            entity.HasOne(d => d.Client).WithMany(p => p.Relatorioprojs)
                .HasForeignKey(d => d.Clientid)
                .HasConstraintName("relatorioproj_clientid_fkey");

            entity.HasOne(d => d.Projeto).WithMany(p => p.Relatorioprojs)
                .HasForeignKey(d => d.Projetoid)
                .HasConstraintName("relatorioproj_projetoid_fkey");
        });

        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tarefa_pkey");

            entity.ToTable("tarefa");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Datafim).HasColumnName("datafim");
            entity.Property(e => e.Datainicio).HasColumnName("datainicio");
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.Preco)
                .HasPrecision(10, 2)
                .HasColumnName("preco");
            entity.Property(e => e.Projetoid).HasColumnName("projetoid");
            entity.Property(e => e.Relatorioprojid).HasColumnName("relatorioprojid");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Utilizadorid).HasColumnName("utilizadorid");

            entity.HasOne(d => d.Projeto).WithMany(p => p.Tarefas)
                .HasForeignKey(d => d.Projetoid)
                .HasConstraintName("tarefa_projetoid_fkey");

            entity.HasOne(d => d.Relatorioproj).WithMany(p => p.Tarefas)
                .HasForeignKey(d => d.Relatorioprojid)
                .HasConstraintName("fk_relatorioproj");

            entity.HasOne(d => d.Utilizador).WithMany(p => p.Tarefas)
                .HasForeignKey(d => d.Utilizadorid)
                .HasConstraintName("tarefa_utilizadorid_fkey");
        });

        modelBuilder.Entity<Utilizador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("utilizador_pkey");

            entity.ToTable("utilizador");

            entity.HasIndex(e => e.Email, "utilizador_email_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Horasdia)
                .HasPrecision(5, 2)
                .HasColumnName("horasdia");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");

            entity.HasMany(d => d.Projetos).WithMany(p => p.Utilizadors)
                .UsingEntity<Dictionary<string, object>>(
                    "Utilizadorprojeto",
                    r => r.HasOne<Projeto1>().WithMany()
                        .HasForeignKey("Projetoid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("utilizadorprojeto_projetoid_fkey"),
                    l => l.HasOne<Utilizador>().WithMany()
                        .HasForeignKey("Utilizadorid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("utilizadorprojeto_utilizadorid_fkey"),
                    j =>
                    {
                        j.HasKey("Utilizadorid", "Projetoid").HasName("utilizadorprojeto_pkey");
                        j.ToTable("utilizadorprojeto");
                        j.IndexerProperty<int>("Utilizadorid").HasColumnName("utilizadorid");
                        j.IndexerProperty<int>("Projetoid").HasColumnName("projetoid");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
