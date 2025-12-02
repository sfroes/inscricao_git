using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class SituacaoInscricaoProcessoData : ISMCMappable
    {

        public SituacaoInscricaoProcessoData()
        {
            OpcoesOferta = new List<OpcaoOfertaData>();
        }

        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public long SeqInscrito { get; set; }

        [DataMember]
        public long SeqSituacao { get; set; }

        [DataMember]
        public string NomeInscrito { get; set; }

        [DataMember]
        public string DescricaoSituacaoAtual { get; set; }

        [DataMember]
        public string MotivoSituacaoAtual { get; set; }

        [DataMember]
        public string JustificativaSituacaoAtual { get; set; }

        [DataMember]
        public string DescricaoGrupoOferta { get; set; }

        [DataMember]
        public long SeqGrupoOferta { get; set; }

        [DataMember]
        [SMCMapForceFromTo()]
        public List<OpcaoOfertaData> OpcoesOferta { get; set; }

        [DataMember]
        public DateTime DataInscricao { get; set; }

        [DataMember]
        public bool? TaxaInscricaoPaga { get; set; }

        [DataMember]
        public bool? DocumentacaoEntregue { get; set; }

        [DataMember]
        public SituacaoDocumentacao SituacaoDocumentacao { get; set; }
        [DataMember]
        public string DocumentosPendentes { get; set; }
        [DataMember]
        public string DocumentosIndeferidos { get; set; }
        
        [DataMember]
        public long? SeqTipoTaxa { get; set; }

    }
}
