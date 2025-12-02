using SMC.Framework;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Common.Areas.INS.Enums
{
    [DataContract(Namespace = NAMESPACES.MODEL)]
    public enum TipoConsistencia : short
    {
        [SMCIgnoreValue]
        [EnumMember]
        [Description("")]
        Nenhum = 0,

        [EnumMember]
        [Description("Débito financeiro")]
        DebitoFinanceiro = 1,

        [EnumMember]
        Desligamento = 2,

        [EnumMember]
        [Description("Desligamento acadêmico")]
        DesligamentoAcademico = 3,

        [EnumMember]
        [Description("Legitimidade do fiador")]
        LegitimidadeFiador = 4,

        [EnumMember]
        [Description("Ocupação de vagas do financiamento estudantil")]
        OcupacaoVagasFinanciamentoEstudantil = 5,

        [EnumMember]
        [Description("Vínculo acadêmico")]
        VinculoAcademico = 6,

        [EnumMember]
        [Description("Cálculos da bolsa social")]
        CalculoBolsaSocial = 7, 

        [EnumMember]
        [Description("Professor da casa")]
        ProfessorDaCasa = 8
    }
}