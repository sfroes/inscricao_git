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
    public class ConfiguracaoEtapaPaginaData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqConfiguracaoEtapa { get; set; }

        [DataMember]
        public long SeqPaginaEtapaSGF { get; set; }

        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public short Ordem { get; set; }

        [DataMember]
        public bool ExibeConfirmacaoInscricao { get; set; }

        [DataMember]
        public bool ExibeComprovanteInscricao { get; set; }
    }
}
