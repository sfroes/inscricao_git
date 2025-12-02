using SMC.Framework.Mapper;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class SelecaoOfertaInscricaoData
    {
        public long Seq { get; set; }

        [SMCMapProperty("DescricaoCompleta")]
        [DataMember]
        public string Descricao { get; set; }

        [SMCMapProperty("HierarquiaOfertaPai.Nome")]
        [DataMember]
        public long SeqPai { get; set; }

        [DataMember]
        public bool IsLeaf { get; set; }

        [DataMember]
        public string DescricaoComplementar { get; set; }
    }
}
