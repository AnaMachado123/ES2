using Microsoft.EntityFrameworkCore;
using BackendTesteESII.Models;

namespace BackendTesteESII.Data;

public class GestaoServicosClientesContext : DbContext
{
    public GestaoServicosClientesContext(DbContextOptions<GestaoServicosClientesContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Projeto> Projetos { get; set; }  
    public DbSet<Utilizador> Utilizadores { get; set; }
    public DbSet<Tarefa> Tarefas { get; set; }
    public DbSet<Convite> Convites { get; set; }
    public DbSet<Relatorio> Relatorios { get; set; }
    public DbSet<RelatorioProjeto> RelatoriosProjeto { get; set; }
    public DbSet<UtilizadorProjeto> UtilizadoresProjeto { get; set; }
    public DbSet<RelatorioProjeto> RelatorioProjetos { get; set; }





    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Projeto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Utilizador>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Tarefa>(entity =>
        {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Convite>(entity =>
        {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Relatorio>(entity =>
        {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<RelatorioProjeto>(entity =>
        {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<UtilizadorProjeto>()
            .HasKey(up => new { up.UtilizadorId, up.ProjetoId });

        modelBuilder.Entity<UtilizadorProjeto>()
            .HasOne(up => up.Utilizador)
            .WithMany(u => u.UtilizadorProjetos)
            .HasForeignKey(up => up.UtilizadorId);

        modelBuilder.Entity<UtilizadorProjeto>()
            .HasOne(up => up.Projeto)
            .WithMany(p => p.UtilizadorProjetos)
            .HasForeignKey(up => up.ProjetoId);
 
        
    }
}
