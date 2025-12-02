using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [SMCMapKnownType(typeof(SecaoPaginaTextoData))]
    [SMCMapKnownType(typeof(SecaoPaginaArquivoData))]
    [KnownType(typeof(SecaoPaginaTextoData))]
    [KnownType(typeof(SecaoPaginaArquivoData))]
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public abstract class SecaoPaginaData : ISMCMappable
    {
        [DataMember]
        public string Token { get; set; }
    }
}
