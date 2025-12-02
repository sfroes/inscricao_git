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
    public class OfertaTipoTaxaSpecification : SMCSpecification<OfertaPeriodoTaxa>
    {
        public long SeqProcesso { get; set; }

        public long SeqTipoTaxa { get; set; }

        public override Expression<Func<OfertaPeriodoTaxa, bool>> SatisfiedBy()
        {
            return x => x.Taxa.SeqTipoTaxa == SeqTipoTaxa 
                        && x.Oferta.SeqProcesso == SeqProcesso;
        }
    }
}
