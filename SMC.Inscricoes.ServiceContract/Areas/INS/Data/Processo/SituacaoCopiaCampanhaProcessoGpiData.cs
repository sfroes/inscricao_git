using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class SituacaoCopiaCampanhaProcessoGpiData : ISMCMappable
    {
        [DataMember]
        public long SeqProcessoSeletivo { get; set; }

        [DataMember]
        public long SeqUnidadeResponsavel { get; set; }

        [DataMember]
        public long SeqTipoProcesso { get; set; }

        [DataMember]
        public bool TipoHierarquiaOfertaAtivo { get; set; }

        [DataMember]
        public bool TipoProcessoAtivo { get; set; }

        [DataMember]
        public bool TemplateProcessoAtivo { get; set; }

        [DataMember]
        public bool TipoProcessoTemplateAtivo { get; set; }
    }
}