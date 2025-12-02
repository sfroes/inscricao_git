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
    public enum IncludesInscricao : long
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        Ofertas = 1 << 0,

        [EnumMember]
        Ofertas_Oferta = 1 << 1,

        [EnumMember]
        Processo = 1 << 2,

        [EnumMember]
        Documentos = 1 << 3,

        [EnumMember]
        Documentos_ArquivoAnexado = 1 << 4,

        [EnumMember]
        HistoricosSituacao = 1 << 5,

        [EnumMember]
        HistoricosSituacao_TipoProcessoSituacao = 1 << 6,

        [EnumMember]
        ConfiguracaoEtapa = 1 << 7,

        [EnumMember]
        ConfiguracaoEtapa_EtapaProcesso = 1 << 8,

        [EnumMember]
        GrupoOferta = 1 << 9,

        [EnumMember]
        ArquivoComprovante = 1 << 10,

        [EnumMember]
        CodigosAutorizacao = 1 << 11,

        [EnumMember]
        Boletos = 1 << 12,

        [EnumMember]
        Processo_PermissoesInscricaoForaPrazo = 1 << 13,

        [EnumMember]
        Processo_PermissoesInscricaoForaPrazo_Inscritos = 1 << 14,

        [EnumMember]
        Processo_GruposOferta = 1 << 15,

        [EnumMember]
        Ofertas_HistoricosSituacao = 1 << 16,

        [EnumMember]
        Ofertas_HistoricosSituacao_TipoProcessoSituacao = 1 << 17,

        [EnumMember]
        Processo_TipoProcesso = 1 << 18,

        [EnumMember]
        HistoricosPagina = 1 << 19,

        [EnumMember]
        HistoricosPagina_ConfiguracaoEtapaPagina = 1 << 20,

        [EnumMember]
        Boletos_Titulos = 1 << 21,

        [EnumMember]
        Processo_GruposOferta_Ofertas = 1 << 22 ,

        [EnumMember]
        Boletos_Taxas = 1 << 23,

        [EnumMember]
        Processo_ConfiguracoesModeloDocumento = 1 << 24,

        [EnumMember]
        Processo_ConfiguracoesModeloDocumento_TipoDocumento = 1 << 25,

    }
}
