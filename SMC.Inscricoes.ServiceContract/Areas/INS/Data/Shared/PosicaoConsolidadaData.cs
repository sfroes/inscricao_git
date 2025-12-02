using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar a posição consolidade de um processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class PosicaoConsolidadaData : ISMCMappable
    {

        [DataMember]
        public int Total { get; set; }

        [DataMember]
        public int Deferidos { get; set; }

        [DataMember]
        public int Indeferidos { get; set; }

        [DataMember]
        public int Finalizadas { get; set; }

        [DataMember]
        public int Iniciadas { get; set; }

        [DataMember]
        public int Confirmadas { get; set; }

        [DataMember]
        public int NaoConfirmadas { get; set; }

        [DataMember]
        public int DocumentacoesEntregues { get; set; }

        [DataMember]
        public int Pagas { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public int Canceladas { get; set; }

        [DataMember]
        public int OfertasNaoSelecionadas { get; set; }
    }
}
