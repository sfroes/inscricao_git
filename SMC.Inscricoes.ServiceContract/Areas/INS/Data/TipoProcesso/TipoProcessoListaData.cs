using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract]
    public class TipoProcessoListaData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public string Descricao { get; set; }
    }
}
