using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract]
    public class TipoProcessoFiltroData : SMCPagerFilterData, ISMCMappable
    {
        [DataMember]
        [SMCMapProperty("SeqTipoProcesso")]
        public long? Seq { get; set; }

        [DataMember]
        public string Descricao { get; set; }
    }
}
