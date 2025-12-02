using SMC.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS
{
    [Flags]
    [DataContract(Namespace = NAMESPACES.MODEL)]
    public enum IncludesItemHierarquiaOferta
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        ItemHierarquiaOfertaPai = 1,

        [EnumMember]
        ItemHierarquiaOfertaPai_TipoItemHierarquiaOferta = 2
    }
}
