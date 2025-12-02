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
    public class QuantidadeOfertaProcessoData : ISMCMappable
    {
        [DataMember]
        [SMCMapProperty("EtapaProcesso.Processo.MaximoOpcoesOfertaPorInscricao")]
        public short? MaximoOpcoesOfertaPorInscricao { get; set; }

        [DataMember]
        [SMCMapProperty("EtapaProcesso.Processo.MaximoOfertaPorInscricao")]
        public short? NumeroMaximoOfertaPorInscricao { get; set; }
    }
}
