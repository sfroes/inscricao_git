using SMC.Framework;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Common.Areas.INS.Enums
{
    [DataContract(Namespace = NAMESPACES.MODEL)]
    public enum TipoEmissao : short
    {
        [SMCIgnoreValue]
        [EnumMember]
        [Description("")]
        Nenhum = 0,

        [EnumMember]
        Consolidada = 1,

        [EnumMember]
        [Description("Por oferta")]
        PorOferta = 2
    }
}