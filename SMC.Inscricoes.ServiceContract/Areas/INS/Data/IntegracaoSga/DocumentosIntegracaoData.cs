using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class DocumentosIntegracaoData : ISMCMappable
    {
        [DataMember]
        public long? SeqArquivoAnexado { get; set; }

        [DataMember]
        public long SeqTipoDocumento { get; set; }

        [DataMember]
        public DateTime? DataEntrega { get; set; }

        [DataMember]
        public DateTime? DataPrazoEntrega { get; set; }

        [DataMember]
        public FormaEntregaDocumento? FormaEntregaDocumento { get; set; }

        [DataMember]
        public VersaoDocumento? VersaoDocumento { get; set; }

        [DataMember]
        public string Observacao { get; set; }

        [DataMember]
        public SituacaoEntregaDocumento SituacaoEntregaDocumento { get; set; }

        [DataMember]
        public string DescricaoTipoDocumento { get; set; }
    }
}
