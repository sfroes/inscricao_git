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
    public class InscricaoDadoFormularioCampoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqInscricaoDadoFormulario { get; set; }

        [DataMember]
        public long SeqElemento { get; set; }

        [DataMember]
        [SMCMapProperty("IdCorrelacao")]
        public Guid? UidCorrelacao { get; set; }

        [DataMember]
        public string Valor { get; set; }

        [DataMember]
        [SMCMapProperty("Token")]
        public string TokenElemento { get; set; }
    }
}
