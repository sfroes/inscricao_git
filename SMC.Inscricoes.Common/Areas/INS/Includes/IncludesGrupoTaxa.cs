using SMC.Framework;
using SMC.Inscricoes.Common;
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
    public enum IncludesGrupoTaxa
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        Itens = 1,

        [EnumMember]
        Itens_Taxa = 2,

        [EnumMember]
        Itens_Taxa_TipoTaxa = 4,

        [EnumMember]
        Itens_Taxa_Ofertas = 6
        
    }
}
