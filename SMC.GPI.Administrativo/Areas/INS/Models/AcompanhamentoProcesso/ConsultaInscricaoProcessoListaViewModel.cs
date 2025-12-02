using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConsultaInscricaoProcessoListaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCSortable(true, false)]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long SeqInscrito { get; set; }

        [SMCHidden]
        public long SeqSituacao { get; set; }

        [SMCSortable(true, true)]
        public string NomeInscrito { get; set; }

        public string DescricaoSituacaoAtual { get; set; }

        [SMCSortable]
        public string DescricaoGrupoOferta { get; set; }

        [SMCHidden]
        public long SeqGrupoOferta { get; set; }

        [SMCHidden]
        public string MotivoSituacaoAtual { get; set; }

        [SMCHidden]
        public string JustificativaSituacaoAtual { get; set; }

        [SMCMapForceFromTo()]
        public List<OfertaInscricaoViewModel> OpcoesOferta { get; set; }

        [SMCSortable]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime DataInscricao { get; set; }

        public bool? TaxaInscricaoPaga { get; set; }

        [SMCHidden]
        public bool? DocumentacaoEntregue { get; set; }

        public SituacaoDocumentacao SituacaoDocumentacao { get; set; }
        public string DocumentosPendentes { get; set; }
        public string DocumentosIndeferidos { get; set; }

        [SMCHidden]
        public string BackURL { get; set; }

        //[SMCSortable]
        //[SMCCssClass("smc-gpi-grid-coluna-numeros")]
        //public decimal? Nota { get; set; }

        //[SMCSortable]
        //[SMCCssClass("smc-gpi-grid-coluna-numeros")]
        //public int? Classificacao { get; set; }               



    }
}