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
    public class ConfiguracaoPaginaData : ISMCMappable
    {
        [DataMember]
        public long SeqConfiguracaoEtapaPagina { get; set; }

        [DataMember]
        public string DescricaoPagina { get; set; }

        [DataMember]
        public string PaginaToken { get; set; }

        [DataMember]
        public bool? ExibirConfirmacao { get; set; }

        [DataMember]
        public bool? ExibirComprovante { get; set; }

        [DataMember]
        public bool? ExibeDadosPessoais { get; set; }
    }
}
