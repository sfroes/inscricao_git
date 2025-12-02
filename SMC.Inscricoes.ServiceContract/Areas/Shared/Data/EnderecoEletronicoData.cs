using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Service.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class EnderecoEletronicoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public TipoEnderecoEletronico TipoEnderecoEletronico { get; set; }
    }
}
