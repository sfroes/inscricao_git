using SMC.Framework.Mapper;
using SMC.Framework.Model;
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
    public class ProcessoLookupFiltroData : SMCPagerFilterData, ISMCMappable
    {
        [DataMember]
        [SMCMapProperty("SeqProcesso")]
        public long? Seq { get; set; }

        [DataMember]
        [SMCMapProperty("DescricaoProcesso")]
        public string Descricao { get; set; }

        [DataMember]
        [SMCMapProperty("SeqUnidadeResponsavel")]
        public long? UnidadeResponsavel { get; set; }

        [DataMember]
        public int? AnoReferencia { get; set; }

        [DataMember]
        public int? SemestreReferencia { get; set; }

        [DataMember]
        public long? TipoProcesso { get; set; }
    }
}
