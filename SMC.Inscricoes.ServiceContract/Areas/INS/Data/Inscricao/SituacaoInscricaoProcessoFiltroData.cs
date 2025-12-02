using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Formularios.ServiceContract.Areas.FRM.Data;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class SituacaoInscricaoProcessoFiltroData : SMCPagerFilterData, ISMCMappable
    {
     
        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public long? SeqInscricao { get; set; }

        [DataMember]
        public long? SeqTipoProcessoSituacao { get; set; }

        public long? SeqMotivo { get; set; }

        [DataMember]
        public long? SeqGrupoOferta { get; set; }

        [DataMember]
        [SMCMapProperty("SeqItemHierarquiaOferta.Seq")]
        public long? SeqItemHierarquiaOferta { get; set; }

        [DataMember]
        [SMCMapProperty("Oferta.Seq")]
        public long? SeqOferta { get; set; }

        [DataMember]
        public string NomeInscrito { get; set; }

        [DataMember]
        public SGFFilterData FiltroSGF { get; set; }

        [DataMember]
        public SituacaoDocumentacao? SituacaoDocumentacao { get; set; }

        [DataMember]
        public bool? RecebeuBolsa { get; set; }

        [DataMember]
        public long? SeqTipoTaxa { get; set; }

        [DataMember]
        public bool? CheckinRealizado { get; set; }
    }
}
