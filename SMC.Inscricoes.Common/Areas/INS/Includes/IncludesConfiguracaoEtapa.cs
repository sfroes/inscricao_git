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
    public enum IncludesConfiguracaoEtapa
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        ArquivoImagem = 1,

        [EnumMember]
        EtapaProcesso = 2,

        [EnumMember]
        EtapaProcesso_Processo = 4,

        [EnumMember]
        EtapaProcesso_Processo_Idiomas = 8,

        [EnumMember]
        GruposOferta = 16,

        [EnumMember]
        GruposOferta_GrupoOferta = 32,

        [EnumMember]
        Paginas = 64,

        [EnumMember]
        EtapaProcesso_Processo_GruposOferta = 128,

        [EnumMember]
        EtapaProcesso_Processo_TipoProcesso = 256,

        [EnumMember]
        EtapaProcesso_Processo_Inscricoes = 512,
        [EnumMember]
        Inscricoes = 1024
    }
}
