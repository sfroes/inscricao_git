using System;
using System.Linq.Expressions;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ConfiguracaoModeloDocumentoFilterSpecification : SMCSpecification<ConfiguracaoModeloDocumento>
    {
        public long? SeqProcesso { get; set; }
        public long? SeqTipoDocumento { get; set; }

        public long? SeqUnidadeResponsavel { get; set; }

        public override Expression<Func<ConfiguracaoModeloDocumento, bool>> SatisfiedBy()
        {
            if(SeqProcesso.HasValue)
                AddExpression(SeqProcesso, x => x.SeqProcesso == SeqProcesso);
            
            if (SeqTipoDocumento.HasValue)
                AddExpression(SeqTipoDocumento, x => x.SeqTipoDocumento == SeqTipoDocumento);
            
            if(SeqUnidadeResponsavel.HasValue)
                AddExpression(SeqUnidadeResponsavel, x => x.Processo.SeqUnidadeResponsavel == SeqUnidadeResponsavel);

            return GetExpression();
        }
    }
}
