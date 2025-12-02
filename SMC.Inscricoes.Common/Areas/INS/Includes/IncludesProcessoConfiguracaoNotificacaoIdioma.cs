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
    public enum IncludesProcessoConfiguracaoNotificacaoIdioma
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        ProcessoConfiguracaoNotificacao = 1,

        [EnumMember]
        ConfiguracoesIdioma = 2,

        [EnumMember]
        ConfiguracoesIdioma_ProcessoIdioma = 4

    }
}
