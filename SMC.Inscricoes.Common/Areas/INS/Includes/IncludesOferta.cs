using SMC.Framework;
using System;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Common.Areas.INS
{
    [Flags]
    [DataContract(Namespace = NAMESPACES.MODEL)]
    public enum IncludesOferta
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        Taxas = 1,

        [EnumMember]
        CodigosAutorizacao = 2,

        [EnumMember]
        Telefones = 4,

        [EnumMember]
        EnderecosEletronicos = 8,

        [EnumMember]
        HierarquiaOfertaPai = 16,

        [EnumMember]
        ItemHierarquiaOferta = 32,

        [EnumMember]
        InscricoesOferta = 64,

        [EnumMember]
        Taxas_Taxa = 128,

        [EnumMember]
        Taxas_Taxa_TipoTaxa = 256,
        [EnumMember]
        InscricoesOferta_Inscricao_HistoricosSituacao_TipoProcessoSituacao = 512,

        [EnumMember]
        Processo = 1024,
    }
}