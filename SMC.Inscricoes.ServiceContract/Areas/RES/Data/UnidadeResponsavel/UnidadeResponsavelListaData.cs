using SMC.Framework.Mapper;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.RES.Data
{
    [DataContract]
    public class UnidadeResponsavelListaData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string Sigla { get; set; }
    }
}
