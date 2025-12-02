using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    /// <summary>
    /// Criado para tratar a regra 9 do da RN_INS_097
    /// </summary>
    public class InscricaoDocumentoGrupoAlteradoSpecification : SMCSpecification<InscricaoDocumento>
    {

        public long? SeqDocumentoRequerido { get; set; }

        public bool DocumentacaoEntregue { get; set; }

        public IEnumerable<long> MotivosSituacaoCanceladaTesteSGF { get; set; }

        public override Expression<Func<InscricaoDocumento, bool>> SatisfiedBy()
        {
            if (MotivosSituacaoCanceladaTesteSGF == null)
                MotivosSituacaoCanceladaTesteSGF = new List<long>();

            return i => (SeqDocumentoRequerido.HasValue && i.SeqDocumentoRequerido == SeqDocumentoRequerido)
                && i.Inscricao.DocumentacaoEntregue == DocumentacaoEntregue
                && i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Deferido
                && (!MotivosSituacaoCanceladaTesteSGF.Any() ||
                                i.Inscricao.HistoricosSituacao.Any(f => f.Atual &&
                                                             (f.TipoProcessoSituacao.Token != TOKENS.SITUACAO_INSCRICAO_CANCELADA ||
                                                              f.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA &&
                                                                (!f.SeqMotivoSituacaoSGF.HasValue || !MotivosSituacaoCanceladaTesteSGF.Contains(f.SeqMotivoSituacaoSGF.Value)))
                                                        )
                           );
        }
    }
}
