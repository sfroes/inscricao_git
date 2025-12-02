using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Localidades.UI.Mvc.Models;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConsultaInscricaoProcessoInscritoListaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long SeqInscrito { get; set; }

        [SMCHidden]
        public long SeqSituacao { get; set; }

        [SMCSortable(true, true)]
        [SMCCssClass("smc-gpi-grid-coluna-texto")]
        public string NomeInscrito { get; set; }

        [SMCCssClass("smc-gpi-grid-coluna-numeros")]
        public string DescricaoSituacaoAtual { get; set; }

        [SMCSortable]
        [SMCCssClass("smc-gpi-grid-coluna-texto")]
        public string DescricaoGrupoOferta { get; set; }

        [SMCHidden]
        public string MotivoSituacaoAtual { get; set; }

        [SMCHidden]
        public string JustificativaSituacaoAtual { get; set; }

        [SMCMapForceFromTo()]
        public List<OfertaInscricaoViewModel> OpcoesOferta { get; set; }

        [SMCSortable]
        [SMCCssClass("smc-gpi-grid-coluna-numeros")]
        public DateTime DataInscricao { get; set; }

        [SMCCssClass("smc-gpi-grid-coluna-numeros")]
        public bool TaxaInscricaoPaga { get; set; }

        [SMCCssClass("smc-gpi-grid-coluna-texto")]
        public bool DocumentacaoEntregue { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Cpf { get; set; }

        public string EmailInscrito { get; set; }

        public PhoneList Telefones { get; set; }

        [SMCCssClass("smc-gpi-grid-coluna-texto")]
        public SituacaoDocumentacao SituacaoDocumentacao { get; set; }
        public string DocumentosPendentes { get; set; }
        public string DocumentosIndeferidos { get; set; }

        public decimal ValorTitulo { get; set; }
    }
}