using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class EtapaProcessoAbertoFiltroData : SMCPagerFilterData, ISMCMappable
    {
        [DataMember]
        public SMCLanguage? Idioma { get; set; }

        [DataMember]
        public string DescricaoProcesso { get; set; }

        [DataMember]
        public long? SeqProcesso { get; set; }
    }
}
