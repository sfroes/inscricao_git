using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.RES.Data
{
    [DataContract]
    public class UnidadeResponsavelDetalheTipoHierarquiaOfertaData : ISMCMappable
    {
        [DataMember]
        [SMCMapProperty("Seq")]
        public long SeqUnidadeTipoHierarquia { get; set; }

        [DataMember]
        public long SeqTipoHierarquiaOferta { get; set; }

        [DataMember]
        [SMCMapProperty("Ativo")]
        public bool TipoHierarquiaAtiva { get; set; }
    }
}
