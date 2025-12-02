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
    public enum IncludesInscricaoHistoricoSituacao
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        TipoProcessoSituacao = 1,

        [EnumMember]
        Inscricao = 2,

        [EnumMember]
        EtapaProcesso = 4,

        [EnumMember]
        Inscricao_Inscrito = 5
    }
}
