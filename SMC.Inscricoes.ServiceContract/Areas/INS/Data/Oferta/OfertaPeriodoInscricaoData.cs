using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class OfertaPeriodoInscricaoData : ISMCMappable
    {
        [DataMember]
        public long SeqOferta { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public DateTime? DataInicio { get; set; }

        [DataMember]
        public DateTime? DataFim { get; set; }

        [DataMember]
        public bool ExigePagamentoTaxa { get; set; }

        [DataMember]
        public bool ExigeEntregaDocumentaaco { get; set; }

        [DataMember]
        public int Vagas { get; set; }

        [DataMember]
        public int InscricoesConfirmadas { get; set; }
    }
}
