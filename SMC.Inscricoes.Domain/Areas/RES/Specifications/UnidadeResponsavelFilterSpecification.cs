using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.RES.Specifications
{
    public class UnidadeResponsavelFilterSpecification : SMCSpecification<UnidadeResponsavel>
    {

        public UnidadeResponsavelFilterSpecification()
        {
            SetOrderBy(x => x.Nome);
        }

        public long? Seq {get;set;}

        public string Nome { get; set; }

        public string Sigla { get; set; }

        public override Expression<Func<UnidadeResponsavel, bool>> SatisfiedBy()
        {
            return u => (!Seq.HasValue || u.Seq == Seq.Value)
                && (String.IsNullOrEmpty(Nome) || u.Nome.ToLower().Contains(Nome.ToLower()))
                && (String.IsNullOrEmpty(Sigla) || u.Sigla.ToLower().Contains(Sigla.ToLower()));
        }
    }
}
