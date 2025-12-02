using SMC.Framework;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Common.Areas.INS.Enums
{
    [DataContract(Namespace = NAMESPACES.MODEL)]
    public enum TipoCheckin : short
    {
        [SMCIgnoreValue]
        [EnumMember]
        [Description("")]
        Nenhum = 0,

        [EnumMember]
        [Description("QR Code")]
        QRCode = 1,

        [EnumMember]
        Manual = 2,

        [EnumMember]
        [Description("Em lote")]
        EmLote = 3
    }
}