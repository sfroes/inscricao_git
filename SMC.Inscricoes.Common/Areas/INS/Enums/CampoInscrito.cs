using SMC.Framework;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Common.Areas.INS.Enums
{
    [DataContract(Namespace = NAMESPACES.MODEL)]
    public enum CampoInscrito : short
    {
        [SMCIgnoreValue]
        [EnumMember]
        [Description("")]
        Nenhum = 0,

        [EnumMember]
        Nome = 1,

        [EnumMember]
        [Description("Data de nascimento")]
        DataNascimento = 2,

        [EnumMember]
        Sexo = 3,

        [EnumMember]
        [Description("Estado civil")]
        EstadoCivil = 4,

        [EnumMember]
        Nacionalidade = 5,

        [EnumMember]
        [Description("País de origem")]
        PaisOrigem = 6,

        [EnumMember]
        Naturalidade = 7,

        [EnumMember]
        CPF = 8,

        [EnumMember]
        Passaporte = 9,

        [EnumMember]
        [Description("Nº de identidade")]
        NumeroIdentidade = 10,

        [EnumMember]
        [Description("Órgão emissor identidade")]
        OrgaoEmissorIdentidade = 11,

        [EnumMember]
        [Description("UF identidade")]
        UfIdentidade = 12,

        [EnumMember]
        [Description("Filiação")]
        Filiacao = 13,

        [EnumMember]
        [Description("Endereço")]
        Endereco = 14,

        [EnumMember]
        Telefone = 15,

        [EnumMember]
        [Description("E-mail")]
        Email = 16,

        [EnumMember]
        [Description("Outros endereços eletrônicos")]
        OutrosEndereçosEletronicos = 17
    }
}