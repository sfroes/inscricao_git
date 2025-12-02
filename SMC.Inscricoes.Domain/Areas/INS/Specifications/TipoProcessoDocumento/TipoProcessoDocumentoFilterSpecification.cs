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
    public class TipoProcessoDocumentoFilterSpecification : SMCSpecification<TipoProcessoDocumento>
    {
        public long? SeqTipoProcesso { get; set; }

        public override Expression<Func<TipoProcessoDocumento, bool>> SatisfiedBy()
        {
            AddExpression(SeqTipoProcesso, t => t.SeqTipoProcesso == SeqTipoProcesso);

            return GetExpression();
        }
    }
}
