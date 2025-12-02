using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SMC.Inscricoes.Common;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricoesNaoCanceladasPorMotivoTesteSpecification : SMCSpecification<Inscricao>
    {
        public InscricoesNaoCanceladasPorMotivoTesteSpecification(IEnumerable<long> motivosCancelamentoTeste)
        {
            MotivosSituacaoCanceladaTesteSGF = motivosCancelamentoTeste;
        }

        public IEnumerable<long> MotivosSituacaoCanceladaTesteSGF { get; set; }

        public override Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            return x => x.HistoricosSituacao.Any(f => f.Atual &&
                                                        (f.TipoProcessoSituacao.Token != TOKENS.SITUACAO_INSCRICAO_CANCELADA ||
                                                         f.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA &&
                                                               (!f.SeqMotivoSituacaoSGF.HasValue || !MotivosSituacaoCanceladaTesteSGF.Contains(f.SeqMotivoSituacaoSGF.Value)))
                                                );
        }
    }
}
