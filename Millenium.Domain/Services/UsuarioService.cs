using Millenium.Domain.Entity;
using Millenium.Domain.Interfaces.Repositories;
using Millenium.Domain.Interfaces.Services;
using Millenium.Util;
using System.Collections.Generic;
using System.Linq;

namespace Millenium.Domain.Services
{
    public class UsuarioService : ServiceBase<Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
            : base(usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }       

        public IEnumerable<Usuario> ObterUsuariosAtivos(IEnumerable<Usuario> usuarios)
        {
            return usuarios.Where(c => c.Ativo == true);
        }

        public Usuario AutenticarUsuario(IEnumerable<Usuario> usuarios, string login, string senha)
        {
            return usuarios.Where(u => u.Login == login && u.Senha == Md5Crypt.Criptografar(senha) && u.Ativo == true && u.Excluido != true).FirstOrDefault();
        }

        public Usuario ObterUsuarioLogin(IEnumerable<Usuario> usuarios, string nome)
        {
            return usuarios.Where(u => u.Nome == nome.ToUpper()).FirstOrDefault();
        }
    }
}
