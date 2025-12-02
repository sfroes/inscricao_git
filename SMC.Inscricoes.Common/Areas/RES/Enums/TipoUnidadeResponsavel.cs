using SMC.Framework;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Common.Areas.RES.Enums
{
    [DataContract(Namespace = NAMESPACES.MODEL)]
    public enum TipoUnidadeResponsavel : short
    {
        [EnumMember]
        [SMCIgnoreValue]
        [Description("")]
        Nenhum = 0,

        [EnumMember]
        [Description("Departamento Administrativo")]
        DepartamentoAdministrativo = 1,

        [EnumMember]
        [Description("Programa")]
        Programa = 2,

        [EnumMember]
        [Description("Departamento Pedagógico")]
        DepartamentoPedagogico = 3
    }
}