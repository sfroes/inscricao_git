using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class DadosInscricaoData : ISMCMappable
    {

        public DadosInscricaoData()
        {
            DescricaoOfertas = new List<OpcaoOfertaData>();
        }

        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        public long SeqInscrito { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public string DescricaoEtapaAtual { get; set; }

        [DataMember]
        public string NomeInscrito { get; set; }

        [DataMember]
        public string DescricaoGrupoOferta { get; set; }

        [DataMember]
        public long? SeqArquivoComprovante { get; set; }

        /// <summary>
        /// Indica se a inscrição possui uma situação Finalizada no seu historico
        /// </summary>
        [DataMember]
        public bool Finalizada { get; set; }

        [DataMember]
        public short? NumeroOpcoesDesejadas { get; set; }

        [SMCMapForceFromTo]
        [DataMember]
        public List<OpcaoOfertaData> DescricaoOfertas { get; set; }

        [DataMember]
        public string Observacao { get; set; }

        [DataMember]
        public string SituacaoInscrito { get; set; }

        [DataMember]
        public string TokenSituacaoInscrito { get; set; }

        [DataMember]
        public bool OfertaVigente { get; set; }
        [DataMember]
        public bool CandidatoComBoletoPago { get; set; }

        [DataMember]
        public SituacaoDocumentacao SituacaoDocumentacao { get; set; }

        public string DescricaoTermoEntregaDocumentacao { get; set; }

        [DataMember]
        public DateTime? DataPrazoNovaEntregaDocumentacao { get; set; }

        [DataMember]
        public bool RecebeuBolsa { get; set; }
        [DataMember]
        public string OrientacaoAceiteConversaoArquivosPDF { get; set; }
        [DataMember]
        public string TermoAceiteConversaoArquivosPDF { get; set; }
        [DataMember]
        public bool GestaoEventos { get; set; }
        public Guid? UidInscricaoOferta { get; set; }
        public Guid UidProcesso { get; set; }
    }
}
