using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ItemHierarquiaOfertaArvoreData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long? SeqPai { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public string Token { get; set; }
    }
}