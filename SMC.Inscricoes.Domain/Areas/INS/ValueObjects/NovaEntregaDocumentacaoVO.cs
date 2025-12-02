using SMC.Framework;
using SMC.Framework.Mapper;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class NovaEntregaDocumentacaoVO : ISMCMappable
    {
        public long SeqInscricao { get; set; }

        public List<NovaEntregaDocumentacaoDocumentoVO> Documentos { get; set; }
    }
}