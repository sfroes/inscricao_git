using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo (usado em InscricoesProcessoVO)
    /// </summary>
    public class DadosInscricaoVO : ISMCMappable
    {

        public DadosInscricaoVO()
        {
            DescricaoOfertas = new List<OpcaoOfertaVO>();
        }

        public long SeqInscricao { get; set; }

        public long SeqInscrito { get; set; }

        public long SeqProcesso { get; set; }

        public string DescricaoEtapaAtual { get; set; }

        public string NomeInscrito { get; set; }

        public string DescricaoGrupoOferta { get; set; }

        public long? SeqArquivoComprovante { get; set; }

        public short? NumeroOpcoesDesejadas { get; set; }

        public List<OpcaoOfertaVO> DescricaoOfertas { get; set; }

        /// <summary>
        /// Indica se a inscrição possui uma situação Finalizada no seu historico
        /// </summary>
        public bool Finalizada { get; set; }

        public string Observacao { get; set; }

        public string SituacaoInscrito { get; set; }

        public string TokenSituacaoInscrito { get; set; }

        public bool OfertaVigente { get; set; }

        public bool CandidatoComBoletoPago { get; set; }

        public SituacaoDocumentacao SituacaoDocumentacao { get; set; }

        public string DescricaoTermoEntregaDocumentacao { get; set; }

        public DateTime? DataPrazoNovaEntregaDocumentacao { get; set; }

        public bool RecebeuBolsa { get; set; }

        public string OrientacaoAceiteConversaoArquivosPDF { get; set; }
        public string TermoAceiteConversaoArquivosPDF { get; set; }
        public bool GestaoEventos { get; set; }
        public Guid? UidInscricaoOferta { get; set; }
        public Guid UidProcesso { get; set; }

    }
}
