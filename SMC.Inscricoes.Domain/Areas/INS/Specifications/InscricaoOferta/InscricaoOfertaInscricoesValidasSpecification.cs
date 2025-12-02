using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoOfertaInscricoesValidasSpecification : SMCSpecification<InscricaoOferta>
    {
        public InscricaoOfertaInscricoesValidasSpecification(long seqOferta)
        {
            SeqOferta = seqOferta;
        }

        public long SeqOferta { get; set; }

        public override Expression<Func<InscricaoOferta, bool>> SatisfiedBy()
        {
            AddExpression(x => x.SeqOferta == SeqOferta);
            AddExpression(x => x.Inscricao.HistoricosSituacao.Any(f => f.Atual &&
                                                                    (f.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_FINALIZADA ||
                                                                     f.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA ||
                                                                     f.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_DEFERIDA)));
            return GetExpression();
        }
    }
}
