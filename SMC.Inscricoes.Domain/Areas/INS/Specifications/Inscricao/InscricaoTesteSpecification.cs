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
    public class InscricaoTesteSpecification : SMCSpecification<Inscricao>
    {
        public InscricaoTesteSpecification(long[] seqInscricoes, IEnumerable<long> motivosSituacaoCanceladaTesteSGF)
        {
            SeqInscricoes = seqInscricoes;
            MotivosSituacaoCanceladaTesteSGF = motivosSituacaoCanceladaTesteSGF;
        }

        public long[] SeqInscricoes { get; set; }

        public IEnumerable<long> MotivosSituacaoCanceladaTesteSGF { get; set; }

        public override Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            return x => SeqInscricoes.Contains(x.Seq) &&
                        x.HistoricosSituacao.Any(f => f.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA
                                                        && f.SeqMotivoSituacaoSGF.HasValue
                                                        && MotivosSituacaoCanceladaTesteSGF.Contains(f.SeqMotivoSituacaoSGF.Value)
                                                        && f.Atual);
        }
    }
}
