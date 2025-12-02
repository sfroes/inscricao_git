using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Inscricao.Areas.INS.Controllers;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class InscricaoOfertaViewModel : SMCViewModelBase, ISMCMappable
	{

        public InscricaoOfertaViewModel()
        {
            SeqOferta = new GPILookupViewModel();
        }

        [SMCHidden]
        public long Seq { get; set; }

        //[SMCSize(SMCSize.Grid2_24)]
        [SMCHidden]
        public short NumeroOpcao { get; set; }

		/// <summary>
		/// Renderiza um Lookup
		/// </summary>		
        [LookupSelecaoOfertaInscricao]
		[SMCSize(SMCSize.Grid24_24)]
        [SMCDependency("SeqGrupoOferta", true)]
        [SMCDependency("SeqProcesso",  true)]
        [SMCMapForceFromTo]
        [SMCRequired]
        [SMCUnique]
        public GPILookupViewModel SeqOferta { get; set; }

        [SMCHidden]
        [SMCDependency(nameof(SeqOferta), nameof(InscricaoController.ValidarBolsaExAlunoOferta), "Inscricao", true, includedProperties: new[] { nameof(PaginaSelecaoOfertaViewModel.BolsaExAluno), nameof(PaginaSelecaoOfertaViewModel.SeqInscricaoEncrypted) })]
        public bool ExibirMensagemOferta { get; set; }

        /// <summary>
        /// Renderiza um Lookup
        /// </summary>		
        [SMCSize(SMCSize.Grid24_24)]
        [SMCConditionalDisplay(nameof(ExibirMensagemOferta), true)]
        [SMCHideLabel]
        [SMCDisplay]
        public string MensagemOferta { get; set; }

        [SMCConditionalDisplay(nameof(PaginaSelecaoOfertaViewModel.ExigeJustificativaOferta), true)]
        [SMCConditionalRequired(nameof(PaginaSelecaoOfertaViewModel.ExigeJustificativaOferta), true)]        
        [SMCSize(SMCSize.Grid24_24)]
        [SMCMultiline]
        public string JustificativaInscricao { get; set; }

        [SMCHidden]
        public bool Ativo { get; set; }
        
        [SMCHidden]
        public bool OfertaImpedida { get; set; }

    }
} 