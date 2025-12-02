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
    public class TipoProcessoConsistenciaSpecification : SMCSpecification<TipoProcesso>
    {
        public long? SeqInscricao { get; set; }

        public long? SeqTipoProcesso { get; set; }

        public long? SeqProcesso { get; set; }

        public override Expression<Func<TipoProcesso, bool>> SatisfiedBy()
        {
            AddExpression(SeqTipoProcesso, x => x.Seq == SeqTipoProcesso);
            AddExpression(SeqProcesso, x => x.Processos.Any(f => f.Seq == SeqProcesso));
            AddExpression(SeqInscricao, x => x.Processos.Any(f => f.Inscricoes.Any(g => g.Seq == SeqInscricao)));
            return GetExpression();
        }
    }
}
