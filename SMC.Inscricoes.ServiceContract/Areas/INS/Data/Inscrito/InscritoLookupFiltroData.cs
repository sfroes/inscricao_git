using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class InscritoLookupFiltroData : SMCPagerFilterData, ISMCMappable
    {
        [DataMember]
        public string Inscrito { get; set; }

        [DataMember]
        public string CPF { get; set; }

        [DataMember]
        public string Passaporte { get; set; }

        [DataMember]
        public long? Processo { get; set; }

        [DataMember]
        public long? Situacao { get; set; }
    }
}
