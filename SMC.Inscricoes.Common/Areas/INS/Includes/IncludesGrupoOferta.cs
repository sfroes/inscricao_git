using SMC.Framework;
using SMC.Inscricoes.Common;
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
    public enum IncludesGrupoOferta
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        Ofertas = 1,

        [EnumMember]
        Ofertas_HierarquiaOfertaPai = 2,

        [EnumMember]
        Ofertas_InscricoesOferta = 4,

        [EnumMember]
        ConfiguracoesEtapa = 8,

        [EnumMember]
        ConfiguracoesEtapa_ConfiguracaoEtapa = 16,

        [EnumMember]
        Processo = 32,

        [EnumMember]
        Processo_EtapasProcesso = 64,

        [EnumMember]
        InscricoesGrupoOferta = 128,
        
        [EnumMember]
        Ofertas_Taxas = 256,

        [EnumMember]
        Ofertas_Taxas_Taxa = 512,

        [EnumMember]
        Ofertas_Taxas_Taxa_TipoTaxa = 1024,
        
    }
}
