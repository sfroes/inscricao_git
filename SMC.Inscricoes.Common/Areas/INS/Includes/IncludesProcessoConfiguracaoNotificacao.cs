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
    public enum IncludesProcessoConfiguracaoNotificacao
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        ParametrosEnvioNotificacao = 1,

        [EnumMember]
        ConfiguracoesIdioma = 2,

        [EnumMember]
        Processo = 4,

        [EnumMember]
        Processo_EtapasProcesso = 8
    }
}
