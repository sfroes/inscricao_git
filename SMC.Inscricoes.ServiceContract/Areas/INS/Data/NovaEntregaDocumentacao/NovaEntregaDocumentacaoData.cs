using SMC.Framework;
using SMC.Framework.Mapper;
using System.Collections.Generic;

namespace MC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class NovaEntregaDocumentacaoData : ISMCMappable
    {
        public long SeqInscricao { get; set; }

        public List<NovaEntregaDocumentacaoDocumentoData> Documentos { get; set; }

    }
}