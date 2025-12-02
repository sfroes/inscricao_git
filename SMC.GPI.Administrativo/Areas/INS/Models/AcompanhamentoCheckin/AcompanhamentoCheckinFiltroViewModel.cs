using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class AcompanhamentoCheckinFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {

        public AcompanhamentoCheckinFiltroViewModel()
        {
            TiposProcessos = new List<SMCSelectListItem>();
            Processos = new List<SMCSelectListItem>();
            Unidades = new List<SMCSelectListItem>();
            GestaoEventos = true;
        }

        #region [Itens ocultos]
        [SMCHidden]
        public List<SMCSelectListItem> TiposProcessos { get; set; }

        [SMCHidden]
        public List<SMCSelectListItem> Processos { get; set; }

        [SMCHidden]
        public List<SMCSelectListItem> Unidades { get; set; }

        public bool? GestaoEventos { get; set; }

        #endregion

        #region Processo
        //[SMCFilter]
        //[SMCSelect(nameof(Unidades), AutoSelectSingleItem = true)]
        //[SMCSize(SMCSize.Grid8_24)]
        //public long? SeqUnidadeResponsavel { get; set; }

        //[SMCFilter]
        //[SMCSelect(nameof(TiposProcessos))]
        //[SMCDependency(nameof(SeqUnidadeResponsavel), "VerificaUnidadeResponsavel", "AcompanhamentoCheckin", false, Remote = true)]
        //[SMCSize(SMCSize.Grid8_24)]
        //public long? SeqTipoProcesso { get; set; }

        [SMCFilter]
        [SMCRequired]
        [SMCSelect(nameof(Processos), UseCustomSelect = true)]
        //[SMCDependency(nameof(SeqUnidadeResponsavel), "FiltroProcessos", "AcompanhamentoCheckin", false, Remote = true)]
        //[SMCDependency(nameof(SeqTipoProcesso), "FiltroProcessos", "AcompanhamentoCheckin", false, Remote = true)]
        [SMCSize(SMCSize.Grid8_24)]
        public long? SeqProcesso { get; set; }

        //[SMCFilter]
        //[SMCSize(SMCSize.Grid3_24)]
        //[SMCMinValue(1)]
        //[SMCMaxValue(2)]
        //public long? SemestreReferencia { get; set; }

        //[SMCSize(SMCSize.Grid3_24)]
        //[SMCMask("0000")]
        //public int? AnoReferencia { get; set; }

        #endregion

        #region [Oferta]

        [SMCFilter]
        [LookupHierarquiaOferta]
        [SMCSize(SMCSize.Grid9_24)]
        [SMCDependency(nameof(SeqProcesso))]
        [SMCConditionalReadonly(nameof(SeqProcesso), "", RuleName = "R1")]
        [SMCConditionalRule("R1")]
        public LookupHierarquiaOfertaViewModel SeqItemHierarquiaOferta { get; set; }

        [SMCFilter]
        [LookupSelecaoOferta]
        [SMCSize(SMCSize.Grid9_24)]
        [SMCDependency(nameof(SeqProcesso))]
        //[SMCDependency(nameof(SeqItemHierarquiaOferta))]
        [SMCConditionalReadonly(nameof(SeqProcesso), "", RuleName = "R1")]
        [SMCConditionalRule("R1")]
        public GPILookupViewModel SeqOferta { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid3_24)]
        public DateTime? Data { get; set; }

        #endregion
    }
}