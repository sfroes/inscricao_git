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
    public enum IncludesInscricaoOferta
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        Inscricao = 1,

        [EnumMember]
        Oferta = 2,

        [EnumMember]
        Oferta_CodigosAutorizacao = 4,

        [EnumMember]
        Oferta_CodigosAutorizacao_CodigoAutorizacao = 8,

        [EnumMember]
        HistoricosSituacao = 16,

        [EnumMember]
        HistoricosSituacao_TipoProcessoSituacao = 32,

        [EnumMember]
        Oferta_GrupoOferta_Processo_Idiomas = 64,
    }
}
