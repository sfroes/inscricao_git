using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{

    public class ConfiguracaoEtapaPorOfertasSpecification : SMCSpecification<ConfiguracaoEtapa>
    {
        public ConfiguracaoEtapaPorOfertasSpecification(long[] seqOfertas)
        {
            SeqOfertas = seqOfertas;
        }
        private long[] SeqOfertas { get; set; }
        public override Expression<Func<ConfiguracaoEtapa, bool>> SatisfiedBy()
        {
            return x => x.GruposOferta.Any(g => g.GrupoOferta.Ofertas.Any(o => SeqOfertas.Any(s => s == o.Seq)));
        }

    }
}
