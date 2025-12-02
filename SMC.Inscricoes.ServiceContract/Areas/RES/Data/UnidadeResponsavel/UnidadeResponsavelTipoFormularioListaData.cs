using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.RES.Data
{
    [DataContract]
    public class UnidadeResponsavelTipoFormularioListaData : SMCPagerFilterData, ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqUnidadeResponsavel { get; set; }

        [DataMember]
        public string DescricaoTipoFormulario { get; set; }

        [DataMember]
        public bool Ativo { get; set; }
    }
}
