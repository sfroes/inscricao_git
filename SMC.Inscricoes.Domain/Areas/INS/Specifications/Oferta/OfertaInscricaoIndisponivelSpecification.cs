using SMC.Framework.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SMC.Inscricoes.Domain.Areas.INS.Models;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class OfertaInscricaoIndisponivelSpecification : SMCSpecification<Oferta>
    {
        public long SeqInscricao { get; set; }

        public override Expression<Func<Oferta, bool>> SatisfiedBy()
        {
            return x => x.InscricoesOferta.Any(f => f.SeqInscricao == SeqInscricao) &&
                            (!x.Ativo || x.DataCancelamento.HasValue);
        }
    }
}
