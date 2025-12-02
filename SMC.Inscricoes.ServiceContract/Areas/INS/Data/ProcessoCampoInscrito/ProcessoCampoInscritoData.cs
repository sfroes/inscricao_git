using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ProcessoCampoInscritoData : ISMCMappable
    {
        public long Seq { get; set; }
        public long SeqProcesso { get; set; }
        public CampoInscrito CampoInscrito { get; set; }
    }
}
