using Microsoft.EntityFrameworkCore;
using Millenium.Domain.Entity;
using Millenium.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Millenium.Infra.Data.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public override Usuario GetById(int id)
        {
            return Db.Usuario.Include(u => u.Nivel).FirstOrDefault(u => u.IdUsuario == id)!;
        }

        public override IEnumerable<Usuario> GetAll()
        {
            return Db.Usuario.Include(u => u.Nivel).ToList();
        }
    }
}
