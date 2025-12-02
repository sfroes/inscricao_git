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
    public class TipoProcessoCampoInscritoFilterSpecification : SMCSpecification<TipoProcessoCampoInscrito>
    {
        public long? Seq { get; set; }
        public long? SeqTipoProcesso { get; set; }

        public override Expression<Func<TipoProcessoCampoInscrito, bool>> SatisfiedBy()
        {
            AddExpression(Seq, a => a.Seq == Seq);
            AddExpression(SeqTipoProcesso, a => a.TipoProcesso.Seq == SeqTipoProcesso);
            return GetExpression();
        }
    }
}
