using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.Inscricao
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class DadosProcessoInscricaoData : ISMCMappable
    {
        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public long SeqTipoTemplateProcessoSGF { get; set; }
    }
}