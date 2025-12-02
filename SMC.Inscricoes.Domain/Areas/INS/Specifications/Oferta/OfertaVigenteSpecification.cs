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
    public class OfertaVigenteSpecification : SMCSpecification<Oferta>
    {
        public override Expression<Func<Oferta, bool>> SatisfiedBy()
        {
            return o => o.DataInicio <= DateTime.Now && DateTime.Now <= o.DataFim;
        }
    }
}
