using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class GrupoTaxaFiltroData : SMCPagerFilterData, ISMCMappable
    {
        [DataMember]
        public long? Seq { get; set; }

        [DataMember]
        public long? SeqProcesso { get; set; }

        [DataMember]        
        public string Descricao { get; set; }
    }
}
