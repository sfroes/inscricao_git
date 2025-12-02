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
    public class EtapaProcessoVigenteSpecification : SMCSpecification<EtapaProcesso>
    {
        public override Expression<Func<EtapaProcesso, bool>> SatisfiedBy()
        {
            return o => o.DataInicioEtapa <= DateTime.Now && DateTime.Now <= o.DataFimEtapa;
        }
    }
}
