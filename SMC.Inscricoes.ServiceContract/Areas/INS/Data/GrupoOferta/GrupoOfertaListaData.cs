using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class GrupoOfertaListaData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        [SMCMapProperty("Descricao")]
        public string Nome { get; set; }
    }
}
