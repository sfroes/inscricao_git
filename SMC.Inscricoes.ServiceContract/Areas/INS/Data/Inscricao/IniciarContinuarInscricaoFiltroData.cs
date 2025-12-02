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
    public class IniciarContinuarInscricaoFiltroData : ISMCMappable
    {
        [DataMember]
        public long SeqInscrito { get; set; }

        [DataMember]
        public long SeqConfiguracaoEtapa { get; set; }

        [DataMember]
        public long SeqGrupoOferta { get; set; }

        [DataMember]
        public long? SeqInscricao { get; set; }
    }
}
