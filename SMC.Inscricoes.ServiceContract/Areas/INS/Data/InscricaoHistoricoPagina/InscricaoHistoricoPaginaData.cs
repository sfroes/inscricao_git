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
    public class InscricaoHistoricoPaginaData : ISMCMappable
    {
        public InscricaoHistoricoPaginaData()
        {
            DataAcesso = DateTime.Now;
            IpAcesso = SMCContext.ClientAddress.Ip;
        }

        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        public long SeqConfiguracaoEtapaPagina { get; set; }

        [DataMember]
        public string IpAcesso { get; set; }

        [DataMember]
        public DateTime DataAcesso { get; set; }
    }
}
