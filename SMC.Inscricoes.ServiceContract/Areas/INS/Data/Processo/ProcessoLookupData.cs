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
    /// <summary>
    /// Data para busca por lookup
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ProcessoLookupData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public string UnidadeResponsavel { get; set; }

        [DataMember]
        public string TipoProcesso { get; set; }

        [DataMember]
        public int AnoReferencia { get; set; }

        [DataMember]
        public int SemestreReferencia { get; set; }
    }
}
