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
    public enum IncludesInscrito
    {
        [SMCIgnoreValue]
        [EnumMember]
        Nenhum = 0,

        [EnumMember]
        Enderecos = 1,

        [EnumMember]
        Telefones = 2,

        [EnumMember]
        EnderecosEletronicos = 4
    }
}
