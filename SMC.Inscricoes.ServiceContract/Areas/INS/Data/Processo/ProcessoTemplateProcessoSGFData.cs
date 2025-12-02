using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar a posição consolidade de um processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ProcessoTemplateProcessoSGFData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqTemplateProcessoSGF { get; set; }
    }
}