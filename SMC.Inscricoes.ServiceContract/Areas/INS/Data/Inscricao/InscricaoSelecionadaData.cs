using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class InscricaoSelecionadaData : SMCPagerFilterData, ISMCMappable
    {
        [DataMember]
        public List<long> GridAnaliseInscricaoLote { get; set; }
    }
}
