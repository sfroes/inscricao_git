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
    public enum IncludesConfiguracaoEtapaPaginaIdioma
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        Arquivos = 1,

        [EnumMember]
        Arquivos_Arquivo = 2,

        [EnumMember]
        ConfiguracaoEtapaPagina = 4,

        [EnumMember]
        Textos = 8
    }
}
