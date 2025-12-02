using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ResumoInscricoesProcessoData : ISMCMappable
    {
        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public int Total { get; set; }

        [DataMember]
        public int InscricoesIniciadas { get; set; }

        [DataMember]
        public int InscricoesFinalizadas { get; set; }

        [DataMember]
        public int InscricoesCanceladas { get; set; }

        [DataMember]
        public int InscricoesConfirmadas { get; set; }

        [DataMember]
        public int InscricoesDeferidas { get; set; }

        [DataMember]
        public int InscricoesIndeferidas { get; set; }

    }
}
