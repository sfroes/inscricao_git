using System.Runtime.Serialization;

namespace SMC.Inscricoes.Common.Enums
{
    public enum MetodoHttp : short
    {
        [EnumMember]
        GET = 0,

        [EnumMember]
        POST = 1,

        [EnumMember]
        PUT = 2,

        [EnumMember]
        DELETE = 3,

        [EnumMember]
        HEAD = 4,

        [EnumMember]
        OPTIONS = 5,

        [EnumMember]
        PATCH = 6,

        [EnumMember]
        MERGE = 7
    }
}
