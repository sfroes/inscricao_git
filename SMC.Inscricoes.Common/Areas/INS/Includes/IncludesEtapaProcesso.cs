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
    public enum IncludesEtapaProcesso
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        Configuracoes = 1,

        [EnumMember]
        Configuracoes_GruposOferta = 2,

        [EnumMember]
        Configuracoes_GruposOferta_GrupoOferta = 4,

        [EnumMember]
        Configuracoes_DocumentosRequeridos = 8,

        [EnumMember]
        Processo = 16,

        [EnumMember]
        Configuracoes_GruposDocumentoRequerido = 32
    }
}
