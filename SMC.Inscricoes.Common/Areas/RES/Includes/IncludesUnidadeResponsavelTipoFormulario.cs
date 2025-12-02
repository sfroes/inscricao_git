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
    public enum IncludesUnidadeResponsavelTipoFormulario
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,
        
        [EnumMember]
        UnidadeResponsavel = 1,

        [EnumMember]
        UnidadeResponsavel_Processos = 2
    }
}
