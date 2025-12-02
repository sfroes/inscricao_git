using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class InscritoProcessoIntegracaoData : ISMCMappable
    {
        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        public long SeqHierarquiaOferta { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string Processo { get; set; }

        [DataMember]
        public long SeqInscricaoOferta { get; set; }

        [DataMember]
        public string Oferta { get; set; }
    }
}