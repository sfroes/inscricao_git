using SMC.Framework.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.INS.Models;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class DocumentoRequeridoEmUsoSpecification : SMCSpecification<Inscricao>
    {
        public long SeqConfiguracaoEtapa { get; set; }

        public long SeqDocumentoRequerido { get; set; }

        public IEnumerable<long> MotivosSituacaoCanceladaTesteSGF { get; set; }

        public override Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            return x => x.SeqConfiguracaoEtapa == SeqConfiguracaoEtapa
                        && x.HistoricosSituacao.Any(f => f.Atual &&
                                                        (f.TipoProcessoSituacao.Token != TOKENS.SITUACAO_INSCRICAO_CANCELADA ||
                                                        f.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA &&
                                                            (!f.SeqMotivoSituacaoSGF.HasValue || !MotivosSituacaoCanceladaTesteSGF.Contains(f.SeqMotivoSituacaoSGF.Value)))
                                                )
                        && x.Documentos.Any(f => f.SeqDocumentoRequerido == SeqDocumentoRequerido);
        }
    }
}
