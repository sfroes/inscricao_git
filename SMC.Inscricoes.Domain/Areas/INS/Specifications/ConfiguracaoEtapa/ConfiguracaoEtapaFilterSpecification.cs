using System;
using System.Linq.Expressions;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ConfiguracaoEtapaFilterSpecification : SMCSpecification<ConfiguracaoEtapa>
    {

        public ConfiguracaoEtapaFilterSpecification()
        {
            SetOrderBy(x => x.DataInicio);
        }

        public ConfiguracaoEtapaFilterSpecification(long seqEtapaProcesos)
        {
            SetOrderBy(x => x.DataInicio);
        }
        public long? SeqEtapaProcesso { get; set; }
        public long? SeqProcesso { get; set; }
        public Nullable<DateTime> DataAtual { get; set; }

        public override Expression<Func<ConfiguracaoEtapa, bool>> SatisfiedBy()
        {
            if(SeqEtapaProcesso.HasValue)
                AddExpression(d => d.SeqEtapaProcesso == SeqEtapaProcesso.Value);
            if(SeqProcesso.HasValue)
                AddExpression(d => d.EtapaProcesso.SeqProcesso == SeqProcesso.Value);
            if(DataAtual.HasValue)
                AddExpression(d => d.DataInicio <= DataAtual.Value && d.DataFim >= DataAtual.Value);

            return GetExpression();
        }
    }
}
