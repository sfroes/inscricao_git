using SMC.Framework;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Common.Areas.INS.Enums
{
    [DataContract(Namespace = NAMESPACES.MODEL)]
    public enum TipoCobranca : short
    {
        [SMCIgnoreValue]
        [EnumMember]
        [Description("")]
        Nenhum = 0,

        [EnumMember]
        [Description("Por inscrição")]
        PorInscricao = 1,

        [EnumMember]
        [Description("Por oferta")]
        PorOferta = 2,

        [EnumMember]
        [Description("Por quantidade de ofertas")]
        PorQuantidadeOfertas = 3
    }
}