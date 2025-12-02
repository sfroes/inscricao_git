using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ProcessoCampoInscritoFilterSpecification : SMCSpecification<ProcessoCampoInscrito>
    {

        public long? Seq { get; set; }
        public long? SeqProcesso { get; set; }
        public Guid? GuidProcesso { get; set; }

        public override Expression<Func<ProcessoCampoInscrito, bool>> SatisfiedBy()
        {
            AddExpression(Seq, x => x.Seq == Seq);
            AddExpression(SeqProcesso, x => x.SeqProcesso == SeqProcesso);
            AddExpression(GuidProcesso, x => x.Processo.UidProcesso == GuidProcesso);

            return GetExpression();
        }
    }
}