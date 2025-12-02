using System.Runtime.Serialization;

namespace SMC.Inscricoes.Common.Enums
{
    public enum TipoSistema : short
    {
        [EnumMember]
        Inscricao = 0,

        [EnumMember]
        Administrativo = 1,
    }
}
