using SMC.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.RES
{
    [Flags]
    [DataContract(Namespace = NAMESPACES.MODEL)]
    public enum IncludesUnidadeResponsavelTipoProcesso
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        TipoProcesso = 1,

        [EnumMember]
        UnidadeResponsavel = 2,

        [EnumMember]
        TiposHierarquiaOferta = 4,

        [EnumMember]
        TiposHierarquiaOferta_TipoHierarquiaOferta = 8
    }
}
