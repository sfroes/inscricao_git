using SMC.Framework;
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
    public class InscricaoInicialData : ISMCMappable
    {
        [DataMember]
        [SMCMapProperty("SeqInscricao")]
        public long Seq { get; set; }

        [DataMember]
        public long SeqInscrito { get; set; }

        [DataMember]
        public long SeqConfiguracaoEtapa { get; set; }

        [DataMember]
        public long SeqGrupoOferta { get; set; }

        [DataMember]
        public SMCLanguage Idioma { get; set; }

        [DataMember]
        public bool ConsentimentoLGPD { get; set; }

        [DataMember]
        public bool AptoBolsa { get; set; }

    }
}
