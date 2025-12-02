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
    public class TipoProcessoFilterSpecification : SMCSpecification<TipoProcesso>
    {
        public TipoProcessoFilterSpecification()
        {
            SetOrderBy(x => x.Descricao);
        }

        public long? Seq { get; set; }
        public string Descricao { get; set; }
        public long? SeqProcesso { get; set; }
        public long? SeqInscricao { get; set; }

        public override Expression<Func<TipoProcesso, bool>> SatisfiedBy()
        {
            AddExpression(Seq, a => a.Seq == Seq);
            AddExpression(Descricao, a => a.Descricao.ToLower().Contains(Descricao.ToLower()));
            AddExpression(SeqProcesso, a => a.Processos.Any(aa => aa.Seq == SeqProcesso));
            AddExpression(SeqInscricao, a => a.Processos.Any(ap => ap.Inscricoes.Any(ai => ai.Seq == SeqInscricao)));
            return GetExpression();
        }
    }
}
