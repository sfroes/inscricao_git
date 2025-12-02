using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Domain.Models;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class NovaEntregaDocumentacaoDocumentoVO : ISMCMappable
    {
        public long Seq { get; set; }
        public long SeqDocumentoRequerido { get; set; }
        public string DescricaoTipoDocumento { get; set; }
        public string DescricaoArquivoAnexado { get; set; }
        public long? SeqArquivoAnexado { get; set; }
        public SMCUploadFile ArquivoAnexado { get; set; }
        public long? SeqArquivoAnexadoAnterior { get; set; }
        public string Observacao { get; set; }
        public SituacaoEntregaDocumento SituacaoEntregaDocumento { get; set; }
    }
}