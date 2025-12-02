using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ProcessoSelectFilterSpecification : SMCSpecification<Processo>
    {

        public long? SeqTipoProcesso { get; set; }
        public long? SeqUnidadeResponsavel { get; set; }
        public int? AnoReferencia { get; set; }
        public int? SemestreReferencia { get; set; }
        public string DescricaoProcesso { get; set; }
        public bool? GestaoEventos { get; set; }
        public override Expression<Func<Processo, bool>> SatisfiedBy()
        {
            AddExpression(AnoReferencia, x => x.AnoReferencia == AnoReferencia);
            AddExpression(SemestreReferencia, x => x.SemestreReferencia == SemestreReferencia);
            AddExpression(DescricaoProcesso, x => x.Descricao.Contains(DescricaoProcesso));
            AddExpression(SeqUnidadeResponsavel, x => x.SeqUnidadeResponsavel == SeqUnidadeResponsavel);
            AddExpression(SeqTipoProcesso, x => x.SeqTipoProcesso == SeqTipoProcesso);
            AddExpression(GestaoEventos, x => x.TipoProcesso.GestaoEventos == GestaoEventos);
            return GetExpression();
        }
    }
}
