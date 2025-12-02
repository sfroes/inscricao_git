using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Enums
{
    [DataContract(Namespace = NAMESPACES.MODEL)]
    public enum TipoItemPaginaEtapa : short
    {
        [EnumMember]
        [Description("")]
        Nenhum = 0,

        [EnumMember]
        [Description("pagina")]
        Pagina = 1,

        [EnumMember]
        [Description("idioma")]
        Idioma = 2,

        [EnumMember]
        [Description("texto")]
        Secao = 4,

        [EnumMember]
        [Description("arquivo")]
        Arquivo = 8
    }

}
