using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.RES.Data
{
    [DataContract]
    public class UnidadeResponsavelFiltroData : SMCPagerFilterData, ISMCMappable
    {
        [DataMember]
        public long? Seq { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string Sigla { get; set; }
    }
}
