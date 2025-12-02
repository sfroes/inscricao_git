using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ConfiguracaoEtapaVigenteSpecification : SMCSpecification<ConfiguracaoEtapa>
    {
        public override System.Linq.Expressions.Expression<Func<ConfiguracaoEtapa, bool>> SatisfiedBy()
        {
            return c => c.DataInicio <= DateTime.Now && DateTime.Now <= c.DataFim; 
        }
    }
}
