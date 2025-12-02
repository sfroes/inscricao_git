using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Service.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class TelefoneData : ISMCMappable
    {
        [DataMember]
        [SMCMapProperty("SeqTelefone")]
        public long Seq { get; set; }

        [DataMember]
        [SMCMapProperty("DDI")]
        public int CodigoPais { get; set; }

        [DataMember]
        [SMCMapProperty("DDD")]
        public int CodigoArea { get; set; }

        [DataMember]
        [SMCMapProperty("Telefone")]
        public string Numero { get; set; }

        [DataMember]
        public TipoTelefone TipoTelefone { get; set; }
    }
}
