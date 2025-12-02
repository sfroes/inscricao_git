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
    public enum IncludesInscricaoDocumento
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        ArquivoAnexado = 1,

        [EnumMember]
        DocumentoRequerido = 2,

        [EnumMember]
        Inscricao = 4
    }
}
