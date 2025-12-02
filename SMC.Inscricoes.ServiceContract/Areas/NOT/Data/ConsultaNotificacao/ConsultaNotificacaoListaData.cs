using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.NOT.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ConsultaNotificacaoListaData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqUnidadeResponsavel { get; set; }

        [DataMember]
        public string GrupoOferta { get; set; }

        [DataMember]
        public List<string> Oferta { get; set; }

        [DataMember]
        public string Inscrito { get; set; }

        [DataMember]
        public string TipoNotificacao { get; set; }

        [DataMember]
        public string Assunto { get; set; }

        [DataMember]
        public bool? Sucesso { get; set; }

        [DataMember]
        public DateTime? DataEnvio { get; set; }
    }
}
