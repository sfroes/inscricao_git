using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ProcessoSituacaoTipoHierarquiaTipoProcessoUnidadeResponsavelSpecification : SMCSpecification<Processo>
    {
        public long[] SeqsUnidadesResponsaveis { get; set; }

        public long[] SeqsTiposProcessos { get; set; }

        public override Expression<Func<Processo, bool>> SatisfiedBy()
        {
            AddExpression(SeqsUnidadesResponsaveis, p => p.TipoProcesso.UnidadeResponsavelTipoProcesso.Any(u => SeqsUnidadesResponsaveis.Contains(u.SeqUnidadeResponsavel)));
            AddExpression(SeqsTiposProcessos, p => p.TipoProcesso.UnidadeResponsavelTipoProcesso.Any(u => SeqsTiposProcessos.Contains(u.SeqTipoProcesso)));

            return GetExpression();
        }
    }
}