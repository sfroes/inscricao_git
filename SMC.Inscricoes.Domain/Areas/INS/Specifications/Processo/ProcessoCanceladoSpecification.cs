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
    public class ProcessoCanceladoSpecification : SMCSpecification<Processo>
    {
        public override Expression<Func<Processo, bool>> SatisfiedBy()
        {
            return p => p.DataCancelamento.HasValue && p.DataCancelamento.Value <= DateTime.Now;
        }
    }
}
