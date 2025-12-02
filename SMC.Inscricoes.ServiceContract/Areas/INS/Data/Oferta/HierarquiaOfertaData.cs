using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar a posição consolidade de um processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class HierarquiaOfertaData : ISMCMappable
    {
        [SMCMapProperty("Nome")]
        [DataMember]
        public string Descricao { get; set; }

        [SMCMapProperty("HierarquiaOfertaPai.Nome")]
        public string DescricaoPai { get; set; }

        public string DescricaoCompleta { get; set; }

        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long? SeqPai { get; set; }

        [DataMember]
        public bool IsLeaf { get; set; }

        [DataMember]
        public long SeqItemHierarquiaOferta { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public bool EOferta { get; set; }

        [DataMember]
        public bool PossuiGrupo { get; set; }        

        [DataMember]
        public string NomeGrupoOferta { get; set; }

        [DataMember]
        public bool PermiteCadastroOfertaFilha { get; set; }

        [DataMember]
        public bool PermiteCadastroItemFilho { get; set; }

        [DataMember]
        public bool PermiteCadastroNetos { get; set; }

        [DataMember]
        public string DescricaoComplementar { get; set; }

        [DataMember]
        public bool Cancelada { get; set; }

        [DataMember]
        public bool Desativada { get; set; }
    }
}
