using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL)]
    public class InscritoProcessoIntegracaoFiltroData : SMCPagerFilterData, ISMCMappable
    {
        [DataMember]
        public long[] SeqProcessos { get; set; }

        [DataMember]
        public bool? Exportado { get; set; }
    }
}