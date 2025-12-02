using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ProcessoOfertaComTaxaSpecification : SMCSpecification<Oferta>
    {
        public long SeqProcesso { get; set; }

        public override Expression<Func<Oferta, bool>> SatisfiedBy()
        {
            return x => x.SeqProcesso == SeqProcesso
                            && x.Taxas.Any();
        }
    }
}
