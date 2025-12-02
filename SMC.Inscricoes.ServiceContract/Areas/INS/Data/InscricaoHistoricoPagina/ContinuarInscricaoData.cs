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
    public class ContinuarInscricaoData : ISMCMappable
    {
        [DataMember]
        [SMCMapProperty("ConfiguracaoEtapaPagina.Token")]
        public string TokenPagina { get; set; }

        [DataMember]
        public long SeqConfiguracaoEtapaPagina { get; set; }

        [DataMember]
        [SMCMapProperty("Inscricao.SeqConfiguracaoEtapa")]
        public long SeqConfiguracaoEtapa { get; set; }

        [DataMember]
        [SMCMapProperty("Inscricao.SeqGrupoOferta")]
        public long SeqGrupoOferta { get; set; }

        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        [SMCMapProperty("Inscricao.Idioma")]
        public SMCLanguage Idioma { get; set; }
    }
}
