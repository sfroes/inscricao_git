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
    public enum IncludesProcesso
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        TipoProcesso = 1 << 0,

        [EnumMember]
        Cliente = 1 << 1,

        [EnumMember]
        Inscricoes = 1 << 2,

        [EnumMember]
        EtapasProcesso = 1 << 3,

        [EnumMember]
        Taxas = 1 << 4,

        [EnumMember]
        Idiomas = 1 << 5,

        [EnumMember]
        EnderecosEletronicos = 1 << 6,

        [EnumMember]
        Telefones = 1 << 7,

        [EnumMember]
        GruposOferta = 1 << 8,

        [EnumMember]
        HierarquiasOferta = 1 << 9,
        
        [EnumMember]
        ConfiguracoesNotificacao = 1 << 10,

        [EnumMember]
        ConfiguracoesNotificacao_ConfiguracoesIdioma = 1 << 11,  
      
        [EnumMember]
        ConfiguracoesNotificacao_ParametrosEnvioNotificacao = 1 << 12,

        [EnumMember]
        ConfiguracoesNotificacao_TipoNotificacao = 1 << 13,

        [EnumMember]
        UnidadeResponsavel = 1 << 14,

        [EnumMember]
        EtapasProcesso_Configuracoes = 1 << 15,

        [EnumMember]
        ConfiguracoesNotificacao_ConfiguracoesIdioma_ProcessoIdioma = 1 << 16,

        [EnumMember]
        ConfiguracoesModeloDocumento = 1 << 17,

        [EnumMember]
        ConfiguracoesFormulario = 1 << 18,

        [EnumMember]
        TipoProcesso_UnidadeResponsavelTipoProcesso = 1 << 19,

        [EnumMember]
        GruposOferta_Ofertas = 1 << 20,

        [EnumMember]
        CamposInscrito = 1 << 21,

        [EnumMember]
        Inscricoes_Ofertas = 1 << 22,

        [EnumMember]
        Inscricoes_Ofertas_HistoricosSituacao = 1<< 23,

        [EnumMember]
        Inscricoes_Ofertas_HistoricosSituacao_TipoProcessoSituacao = 1 << 24,

        [EnumMember]
        Inscricoes_HistoricosSituacao_TipoProcessoSituacao = 1 << 25,
        
        [EnumMember]
        EtapasProcesso_Configuracoes_Paginas_Idiomas = 1 << 26,
        
        [EnumMember]
        UnidadeResponsavelTipoProcessoIdVisual = 1 << 27,

        [EnumMember]
        Taxas_TipoTaxa = 1 << 28,

        [EnumMember]
        GruposOferta_Ofertas_Taxas = 1 << 29,

        [EnumMember]
        GruposOferta_Ofertas_Taxas_Taxa = 1 << 30,

        [EnumMember]
        GruposOferta_Ofertas_Taxas_Taxa_TipoTaxa = 1 << 31,

    }
}
