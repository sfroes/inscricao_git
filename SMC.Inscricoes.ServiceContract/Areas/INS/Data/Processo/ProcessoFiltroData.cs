using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Specification para filtrar ConfiguracaoEtapa
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ProcessoFiltroData : SMCPagerFilterData, ISMCMappable
    {
        [SMCMapProperty("Seq")]
        [DataMember]
        public long? SeqProcesso { get; set; }

        [SMCMapProperty("Descricao")]
        [DataMember]
        public string DescricaoProcesso { get; set; }

        [DataMember]
        public long? SeqTipoProcesso { get; set; }

        [DataMember]
        public long? SeqUnidadeResponsavel { get; set; }

        [DataMember]
        public long? SeqCliente { get; set; }

        [DataMember]
        public int? SemestreReferencia { get; set; }

        [DataMember]
        public int? AnoReferencia { get; set; }

        [DataMember]
        public DateTime? DataInicio { get; set; }

        [DataMember]
        public DateTime? DataFim { get; set; }
    }
}
