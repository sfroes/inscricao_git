using SMC.Framework.Mapper;
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
    public class LookupInscritoData : ISMCMappable
    {
        [DataMember]
        public long? Seq { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public long SeqInscrito { get; set; }

        [DataMember]
        public DateTime DataNascimento { get; set; }

        [DataMember]
        public string CPF { get; set; }

        [DataMember]
        public string NumeroPassaporte { get; set; }
    }
}
