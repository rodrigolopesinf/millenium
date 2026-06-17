using Microsoft.EntityFrameworkCore;
using Millenium.Domain.Entity;

namespace Millenium.Infra.Data.Contexto
{
    public partial class ContextMillenium : DbContext
    {
        public ContextMillenium()
        {
        }

        public ContextMillenium(DbContextOptions<ContextMillenium> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Contato> Contato { get; set; }
        public virtual DbSet<Endereco> Endereco { get; set; }
        public virtual DbSet<Faturamento> Faturamento { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Nivel> Nivel { get; set; }
        public virtual DbSet<SituacaoCliente> SituacaoCliente { get; set; }
        public virtual DbSet<Solicitacao> Solicitacao { get; set; }
        public virtual DbSet<TipoCliente> TipoCliente { get; set; }
        public virtual DbSet<TipoSolicitacao> TipoSolicitacao { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<FaturadoMes> FaturadoMes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Fallback de conexão para compatibilidade com instanciação manual
                optionsBuilder.UseSqlServer("Data Source=sql5075.site4now.net;Initial Catalog=db_a82313_cpbarbosa;User Id=db_a82313_cpbarbosa_admin;Password=Palio@131710;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.CodigoCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.NomeFantasia)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.RazaoSocial)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Cpf)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Cnpj)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.TelefonePrincipal)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.RamalPrincipal)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.TelefoneSecundario)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.RamalSecundario)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.EmailPrincipal)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Observacao)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.FaturadoMes)
                .WithOne(e => e.Cliente)
                .HasForeignKey(e => e.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.Faturamento)
                .WithOne(e => e.Cliente)
                .HasForeignKey(e => e.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cliente>()
                .HasOne(e => e.Endereco)
                .WithMany(e => e.Cliente)
                .HasForeignKey(e => e.IdEndereco)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cliente>()
                .HasOne(e => e.Contato)
                .WithMany(e => e.Cliente)
                .HasForeignKey(e => e.IdContato)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cliente>()
                .HasOne(e => e.SituacaoCliente)
                .WithMany(e => e.Cliente)
                .HasForeignKey(e => e.IdSituacaoCliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contato>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Contato>()
                .Property(e => e.Celular)
                .IsUnicode(false);

            modelBuilder.Entity<Contato>()
                .Property(e => e.CelularSecundario)
                .IsUnicode(false);

            modelBuilder.Entity<Contato>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Contato>()
                .Property(e => e.Site)
                .IsUnicode(false);

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Logradouro)
                .IsUnicode(false);

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Numero)
                .IsUnicode(false);

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Complemento)
                .IsUnicode(false);

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Cep)
                .IsUnicode(false);

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Estado)
                .IsUnicode(false);

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Cidade)
                .IsUnicode(false);

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Bairro)
                .IsUnicode(false);

            modelBuilder.Entity<Faturamento>()
                .Property(e => e.Preco)
                .HasPrecision(7, 2);

            modelBuilder.Entity<Faturamento>()
                .Property(e => e.Total)
                .HasPrecision(7, 2);

            modelBuilder.Entity<Faturamento>()
                .HasMany(e => e.FaturadoMes)
                .WithOne(e => e.Faturamento)
                .HasForeignKey(e => e.IdFaturamento)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Menu>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .Property(e => e.Controller)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .Property(e => e.Route)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .Property(e => e.Action)
                .IsUnicode(false);

            modelBuilder.Entity<Nivel>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Nivel>()
                .HasMany(e => e.Usuario)
                .WithOne(e => e.Nivel)
                .HasForeignKey(e => e.IdNivel)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MenuUsuario>()
                .HasKey(mu => new { mu.IdMenu, mu.IdNivel });

            modelBuilder.Entity<MenuUsuario>()
                .HasOne(mu => mu.Menu)
                .WithMany()
                .HasForeignKey(mu => mu.IdMenu)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MenuUsuario>()
                .HasOne(mu => mu.Nivel)
                .WithMany()
                .HasForeignKey(mu => mu.IdNivel)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FaturadoMes>()
                .HasKey(fm => new { fm.IdCliente, fm.Mes, fm.IdFaturamento });

            modelBuilder.Entity<SituacaoCliente>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            //modelBuilder.Entity<SituacaoCliente>()
            //    .HasMany(e => e.Cliente)
            //    .WithOne(e => e.SituacaoCliente)
            //    .HasForeignKey(e => e.IdSituacaoCliente)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Solicitacao>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Solicitacao>()
                .Property(e => e.Cpf)
                .IsUnicode(false);

            modelBuilder.Entity<Solicitacao>()
                .Property(e => e.Rg)
                .IsUnicode(false);

            modelBuilder.Entity<Solicitacao>()
                .Property(e => e.NomePai)
                .IsUnicode(false);

            modelBuilder.Entity<Solicitacao>()
                .Property(e => e.NomeMae)
                .IsUnicode(false);

            modelBuilder.Entity<Solicitacao>()
               .HasOne(mu => mu.Endereco)
               .WithMany()
               .HasForeignKey(mu => mu.IdEndereco)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TipoCliente>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<TipoCliente>()
                .HasMany(e => e.Cliente)
                .WithOne(e => e.TipoCliente)
                .HasForeignKey(e => e.IdTipoCliente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TipoSolicitacao>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Apelido)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Celular)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Login)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Senha)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Cliente)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.IdUsuarioAlteracao);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Cliente1)
                .WithOne(e => e.Usuario1)
                .HasForeignKey(e => e.IdUsuarioCriacao)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Cliente2)
                .WithOne(e => e.Usuario2)
                .HasForeignKey(e => e.IdUsuarioExclusao);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Contato)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.IdUsuarioAlteracao);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Contato1)
                .WithOne(e => e.Usuario1)
                .HasForeignKey(e => e.IdUsuarioCriacao)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Contato2)
                .WithOne(e => e.Usuario2)
                .HasForeignKey(e => e.IdUsuarioExclusao);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Faturamento)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.IdUsuarioCriacao)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Faturamento1)
                .WithOne(e => e.Usuario1)
                .HasForeignKey(e => e.IdUsuarioExclusao);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Faturamento2)
                .WithOne(e => e.Usuario2)
                .HasForeignKey(e => e.IdUsuarioAlteracao);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Solicitacao)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.IdUsuarioAlteracao);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Solicitacao1)
                .WithOne(e => e.Usuario1)
                .HasForeignKey(e => e.IdUsuarioCriacao)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Solicitacao2)
                .WithOne(e => e.Usuario2)
                .HasForeignKey(e => e.IdUsuarioExclusao);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.TipoCliente)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.IdUsuarioAlteracao);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.TipoCliente1)
                .WithOne(e => e.Usuario1)
                .HasForeignKey(e => e.IdUsuarioCriacao)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.TipoCliente2)
                .WithOne(e => e.Usuario2)
                .HasForeignKey(e => e.IdUsuarioExclusao);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.TipoSolicitacao)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.IdUsuarioAlteracao);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.TipoSolicitacao1)
                .WithOne(e => e.Usuario1)
                .HasForeignKey(e => e.IdUsuarioCriacao)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.TipoSolicitacao2)
                .WithOne(e => e.Usuario2)
                .HasForeignKey(e => e.IdUsuarioExclusao);

            modelBuilder.Entity<FaturadoMes>()
                .Property(e => e.Mes)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(f => f.DataHoraCriacao)
                .HasColumnType("datetime2");

            modelBuilder.Entity<Cliente>()
                .Property(f => f.DataHoraAlteracao)
                .HasColumnType("datetime2");

            modelBuilder.Entity<Cliente>()
                .Property(f => f.DataHoraExclusao)
                .HasColumnType("datetime2");

            modelBuilder.Entity<Contato>()
                .Property(f => f.DataHoraCriacao)
                .HasColumnType("datetime2");

            modelBuilder.Entity<Contato>()
                .Property(f => f.DataHoraAlteracao)
                .HasColumnType("datetime2");

            modelBuilder.Entity<Contato>()
                .Property(f => f.DataHoraExclusao)
                .HasColumnType("datetime2");

            modelBuilder.Entity<Solicitacao>()
                .Property(f => f.DataHoraCriacao)
                .HasColumnType("datetime2");

            modelBuilder.Entity<Solicitacao>()
               .Property(f => f.DataHoraAlteracao)
               .HasColumnType("datetime2");

            modelBuilder.Entity<Solicitacao>()
               .Property(f => f.DataHoraExclusao)
               .HasColumnType("datetime2");

            modelBuilder.Entity<TipoSolicitacao>()
                .Property(f => f.DataHoraCriacao)
                .HasColumnType("datetime2");

            modelBuilder.Entity<TipoSolicitacao>()
               .Property(f => f.DataHoraAlteracao)
               .HasColumnType("datetime2");

            modelBuilder.Entity<TipoSolicitacao>()
               .Property(f => f.DataHoraExclusao)
               .HasColumnType("datetime2");

            modelBuilder.Entity<TipoCliente>()
                .Property(f => f.DataHoraCriacao)
                .HasColumnType("datetime2");

            modelBuilder.Entity<TipoCliente>()
               .Property(f => f.DataHoraAlteracao)
               .HasColumnType("datetime2");

            modelBuilder.Entity<TipoCliente>()
               .Property(f => f.DataHoraExclusao)
               .HasColumnType("datetime2");
        }
    }
}
