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
    public class CopiaEtapaProcessoData : ISMCMappable
    {
        [DataMember]
        public long SeqEtapa { get; set; }

        [DataMember]
        public long SeqEtapaSGF { get; set; }        

        [DataMember]
        public bool Copiar { get; set; }

        [DataMember]
        public string Etapa { get; set; }

        [DataMember]
        public DateTime? DataInicio { get; set; }

        [DataMember]
        public DateTime? DataFim { get; set; }

        [DataMember]
        public bool CopiarConfiguracoes { get; set; }
    }
}
