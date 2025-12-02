
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
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
    public class ConsultaNotificacaoFiltroData : SMCPagerFilterData, ISMCMappable, ISMCLookupData
    {
        [DataMember]
        public long? SeqProcesso { get; set; }

        [DataMember]
        public long? SeqGrupoOferta { get; set; }

        [DataMember]
        public long? SeqOferta { get; set; }

        [DataMember]
        public long? SeqTipoNotificacao { get; set; }

        [DataMember]
        public long? SeqInscricao { get; set; }

        [DataMember]
        public string Inscrito { get; set; }

        [DataMember]
        public string Assunto { get; set; }

        [DataMember]
        public DateTime? DataInicio { get; set; }

        [DataMember]
        public DateTime? DataFim { get; set; }

        [DataMember]
        public List<long> SeqUnidadeResponsavel { get; set; }
    }
}
