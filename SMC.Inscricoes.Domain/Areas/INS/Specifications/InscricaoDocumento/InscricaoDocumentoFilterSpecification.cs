using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoDocumentoFilterSpecification : SMCSpecification<InscricaoDocumento>
    {
        public long? Seq { get; set; }

        public long? SeqInscricao { get; set; }

        public long? SeqDocumentoRequerido { get; set; }

        public long? SeqConfiguracaoEtapa { get; set; }

        public bool? PermiteUploadArquivo { get; set; }

        public bool? UploadObrigatorio { get; set; }

        public SituacaoEntregaDocumento? SituacaoEntregaDocumento { get; set; }

        public SituacaoEntregaDocumento[] SituacoesEntregaDocumentoDiferentes { get; set; }
        public SituacaoEntregaDocumento[] SituacaesEntregaDocumentos { get; set; }

        public VersaoDocumento? VersaoDocumento { get; set; }

        public bool? PossuiArquivo { get; set; }

        public IEnumerable<long> MotivosSituacaoCanceladaTesteSGF { get; set; }
        public List<long> Seqs { get; set; }
        public List<string> GuidsGED { get; set; }

        public override Expression<Func<InscricaoDocumento, bool>> SatisfiedBy()
        {
            if (Seq.HasValue) return i => i.Seq == Seq.Value;
            
            AddExpression(SeqInscricao, w => w.SeqInscricao == this.SeqInscricao.Value);
            AddExpression(SeqDocumentoRequerido, w => w.SeqDocumentoRequerido == this.SeqDocumentoRequerido);
            AddExpression(SeqConfiguracaoEtapa, w => w.Inscricao.SeqConfiguracaoEtapa == this.SeqConfiguracaoEtapa);
            AddExpression(PermiteUploadArquivo, w => w.DocumentoRequerido.PermiteUploadArquivo == this.PermiteUploadArquivo);
            AddExpression(SituacaoEntregaDocumento, w => w.SituacaoEntregaDocumento == this.SituacaoEntregaDocumento); 
            AddExpression(VersaoDocumento, w => w.VersaoDocumento == this.VersaoDocumento);
            AddExpression(UploadObrigatorio, w => w.DocumentoRequerido.UploadObrigatorio == this.UploadObrigatorio.Value);
            AddExpression(PossuiArquivo, w => PossuiArquivo.Value == w.SeqArquivoAnexado.HasValue);
            AddExpression(MotivosSituacaoCanceladaTesteSGF, w =>
                                                        w.Inscricao.HistoricosSituacao.Any(f => f.Atual &&
                                                             (f.TipoProcessoSituacao.Token != TOKENS.SITUACAO_INSCRICAO_CANCELADA ||
                                                              f.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA &&
                                                                (!f.SeqMotivoSituacaoSGF.HasValue || !MotivosSituacaoCanceladaTesteSGF.Contains(f.SeqMotivoSituacaoSGF.Value)))
                                                        )
            );
            AddExpression(SituacoesEntregaDocumentoDiferentes, w => !this.SituacoesEntregaDocumentoDiferentes.Contains(w.SituacaoEntregaDocumento));
            AddExpression(SituacaesEntregaDocumentos, w => this.SituacaesEntregaDocumentos.Contains(w.SituacaoEntregaDocumento));
            AddExpression(Seqs, a => Seqs.Contains(a.Seq));
            AddExpression(GuidsGED, a => GuidsGED.Contains(a.ArquivoAnexado.UidArquivoGed.ToString()));

            return GetExpression();
        }
    }
}
