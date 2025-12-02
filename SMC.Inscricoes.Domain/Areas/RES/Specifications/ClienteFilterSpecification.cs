using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.RES;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.RES.Specifications
{
    public class ClienteFilterSpecification : SMCSpecification<Cliente>
    {
        public ClienteFilterSpecification()
        {
            SetOrderBy(x => x.Nome);
        }

        public long? Seq { get; set; }

        public string Nome { get; set; }

        public string Sigla { get; set; }

        public override Expression<Func<Cliente, bool>> SatisfiedBy()
        {
            return u => (!Seq.HasValue || u.Seq == Seq.Value)
                && (string.IsNullOrEmpty(Nome) || u.Nome.ToLower().Contains(Nome.ToLower()))
                && (string.IsNullOrEmpty(Sigla) || u.Sigla.ToLower().Contains(Sigla.ToLower()));
        }
    }
}
