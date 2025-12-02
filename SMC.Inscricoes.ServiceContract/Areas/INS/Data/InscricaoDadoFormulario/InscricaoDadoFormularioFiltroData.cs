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
    public class InscricaoDadoFormularioFiltroData
    {
        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        public long SeqFormulario { get; set; }

        [DataMember]
        public long SeqVisao { get; set; }

        [DataMember]
        public long SeqDadoFormulario { get; set; }
    }
}
