namespace Millenium.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    [Table("Cliente")]
    public partial class Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cliente()
        {
            Endereco = new Endereco();
            Contato = new Contato();
            SituacaoCliente = new SituacaoCliente();
            TipoCliente = new TipoCliente();
        }

        [Key]
        public int IdCliente { get; set; }

        [StringLength(25)]
        public string? CodigoCliente { get; set; }

        public int IdTipoCliente { get; set; }

        public int? IdEndereco { get; set; }

        public int? IdContato { get; set; }

        [StringLength(255)]
        public string? NomeFantasia { get; set; }

        [StringLength(255)]
        public string? RazaoSocial { get; set; }

        [StringLength(50)]
        public string? Nome { get; set; }

        private string? _cpf;
        [StringLength(15)]
        public string? Cpf 
        {
            get => _cpf;
            set => _cpf = FormatarCpf(value);
        }

        private string? _cnpj;
        [StringLength(18)]
        public string? Cnpj 
        {
            get => _cnpj;
            set => _cnpj = FormatarCnpj(value);
        }

        private string _telefonePrincipal = "";
        [Required]
        [StringLength(15)]
        public string TelefonePrincipal 
        {
            get => _telefonePrincipal;
            set => _telefonePrincipal = FormatarTelefone(value) ?? "";
        }

        [StringLength(5)]
        public string? RamalPrincipal { get; set; }

        private string? _telefoneSecundario;
        [StringLength(15)]
        public string? TelefoneSecundario 
        {
            get => _telefoneSecundario;
            set => _telefoneSecundario = FormatarTelefone(value);
        }

        [StringLength(5)]
        public string? RamalSecundario { get; set; }

        [Required]
        [StringLength(255)]
        public string EmailPrincipal { get; set; }

        public bool FaturaEmail { get; set; }

        public int IdSituacaoCliente { get; set; }

        [StringLength(1000)]
        public string? Observacao { get; set; }

        public int IdUsuarioCriacao { get; set; }

        public int? IdUsuarioAlteracao { get; set; }

        public int? IdUsuarioExclusao { get; set; }

        public DateTime DataHoraCriacao { get; set; }

        public DateTime? DataHoraAlteracao { get; set; }

        public DateTime? DataHoraExclusao { get; set; }

        public bool Ativo { get; set; }

        public bool? Excluido { get; set; }

        public int? DiaVencimento { get; set; }

        public int? UltimoSequencial { get; set; }

        public virtual Endereco Endereco { get; set; }

        public virtual Contato Contato { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual SituacaoCliente SituacaoCliente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual TipoCliente TipoCliente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual Usuario Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual Usuario Usuario1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual Usuario Usuario2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FaturadoMes> FaturadoMes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Faturamento> Faturamento { get; set; }

        private static string? FormatarCpf(string? cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return cpf;
            var apenasNumeros = new string(cpf.Where(char.IsDigit).ToArray());
            if (apenasNumeros.Length > 11) apenasNumeros = apenasNumeros.Substring(0, 11);

            if (apenasNumeros.Length <= 3) return apenasNumeros;
            if (apenasNumeros.Length <= 6) return $"{apenasNumeros.Substring(0, 3)}.{apenasNumeros.Substring(3)}";
            if (apenasNumeros.Length <= 9) return $"{apenasNumeros.Substring(0, 3)}.{apenasNumeros.Substring(3, 3)}.{apenasNumeros.Substring(6)}";
            return $"{apenasNumeros.Substring(0, 3)}.{apenasNumeros.Substring(3, 3)}.{apenasNumeros.Substring(6, 3)}-{apenasNumeros.Substring(9)}";
        }

        private static string? FormatarCnpj(string? cnpj)
        {
            if (string.IsNullOrEmpty(cnpj)) return cnpj;
            var apenasNumeros = new string(cnpj.Where(char.IsDigit).ToArray());
            if (apenasNumeros.Length > 14) apenasNumeros = apenasNumeros.Substring(0, 14);

            if (apenasNumeros.Length <= 2) return apenasNumeros;
            if (apenasNumeros.Length <= 5) return $"{apenasNumeros.Substring(0, 2)}.{apenasNumeros.Substring(2)}";
            if (apenasNumeros.Length <= 8) return $"{apenasNumeros.Substring(0, 2)}.{apenasNumeros.Substring(2, 3)}.{apenasNumeros.Substring(5)}";
            if (apenasNumeros.Length <= 12) return $"{apenasNumeros.Substring(0, 2)}.{apenasNumeros.Substring(2, 3)}.{apenasNumeros.Substring(5, 3)}/{apenasNumeros.Substring(8)}";
            return $"{apenasNumeros.Substring(0, 2)}.{apenasNumeros.Substring(2, 3)}.{apenasNumeros.Substring(5, 3)}/{apenasNumeros.Substring(8, 4)}-{apenasNumeros.Substring(12)}";
        }

        private static string? FormatarTelefone(string? tel)
        {
            if (string.IsNullOrEmpty(tel)) return tel;
            var apenasNumeros = new string(tel.Where(char.IsDigit).ToArray());
            if (apenasNumeros.Length > 11) apenasNumeros = apenasNumeros.Substring(0, 11);

            if (apenasNumeros.Length <= 2) return apenasNumeros;
            if (apenasNumeros.Length <= 6) return $"({apenasNumeros.Substring(0, 2)}) {apenasNumeros.Substring(2)}";
            if (apenasNumeros.Length <= 10) return $"({apenasNumeros.Substring(0, 2)}) {apenasNumeros.Substring(2, 4)}-{apenasNumeros.Substring(6)}";
            return $"({apenasNumeros.Substring(0, 2)}) {apenasNumeros.Substring(2, 5)}-{apenasNumeros.Substring(7)}";
        }
    }
}
