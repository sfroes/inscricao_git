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
    public class ItemGrupoOfertaData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        [SMCMapProperty("Nome")]
        public string DescricaoCompleta { get; set; }

        public string NomeGrupoOferta { get; set; }
    }
}
