using SMC.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Enums
{
    [DataContract(Namespace = NAMESPACES.MODEL)]
    public enum SituacaoValidacaoOferta : short
    {
        [EnumMember]
        [SMCIgnoreValue]
        [Description("")]
        Nenhum = 0,

        [EnumMember]
        [Description("Deferido")]
        Deferido = 1,

        [EnumMember]
        [Description("Indeferido")]
        Indeferido = 2
    }
}
