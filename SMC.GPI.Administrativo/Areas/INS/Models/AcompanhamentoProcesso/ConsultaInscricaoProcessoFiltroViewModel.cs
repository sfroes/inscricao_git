using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Formularios.UI.Mvc.Controls;
using SMC.Formularios.UI.Mvc.Model;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConsultaInscricaoProcessoFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        #region DataSources

        public List<SMCDatasourceItem> GruposOferta { get; set; }

        public List<SMCDatasourceItem> Motivos { get; set; }

        public List<SMCDatasourceItem> TipoTaxa { get; set; }
        
        public List<SMCDatasourceItem> SituacoesProcesso { get; set; }
        
        #endregion DataSources

        [SMCOrder(0)]
        [SMCFilter]
        [SMCSGFFilter]
        [SMCSize(Framework.SMCSize.Grid24_24)]
        public SGFFilterModel FiltroSGF { get; set; }

        [SMCHidden]
        [SMCFilter]
        [SMCFilterKey]
        public long SeqProcesso { get; set; }

        [SMCOrder(1)]
        [SMCFilter]
        [SMCSize(SMCSize.Grid5_24)]
        public long? SeqInscricao { get; set; }

        [SMCOrder(2)]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCSelect(nameof(SituacoesProcesso))]
        [SMCFilter]
        public long? SeqTipoProcessoSituacao { get; set; }

        [SMCOrder(3)]
        [SMCSelect(nameof(Motivos))]
        [SMCSize(SMCSize.Grid5_24)]
        [SMCFilter]
        [SMCDependency(nameof(SeqTipoProcessoSituacao), nameof(AcompanhamentoProcessoController.BuscarMotivos), "AcompanhamentoProcesso", true)]
        public long? SeqMotivo { get; set; }

        [SMCOrder(4)]
        [SMCSize(SMCSize.Grid10_24)]
        [SMCFilter]
        public string NomeInscrito { get; set; }

        [SMCOrder(5)]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCSelect(nameof(GruposOferta))]
        [SMCFilter]
        [SMCSingleFill("oferta")]
        public long? SeqGrupoOferta { get; set; }

        [SMCOrder(6)]
        [SMCFilter]
        [LookupHierarquiaOferta]
        [SMCSize(SMCSize.Grid10_24)]
        [SMCDependency(nameof(SeqProcesso))]
        [SMCSingleFill("oferta")]
        public LookupHierarquiaOfertaViewModel SeqItemHierarquiaOferta { get; set; }

        [SMCOrder(7)]
        [SMCFilter]
        [LookupSelecaoOferta]
        [SMCSize(SMCSize.Grid10_24)]
        [SMCDependency(nameof(SeqGrupoOferta))]
        [SMCDependency(nameof(SeqItemHierarquiaOferta))]
        [SMCDependency(nameof(SeqProcesso))]
        [SMCConditionalReadonly(nameof(SeqGrupoOferta), "", RuleName = "R1")]
        [SMCConditionalReadonly(nameof(SeqItemHierarquiaOferta), "", RuleName = "R2")]
        [SMCConditionalRule("R1 && R2")]
        public GPILookupViewModel Oferta { get; set; }

        [SMCOrder(8)]
        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCSelect]
        public SituacaoDocumentacao? SituacaoDocumentacao { get; set; }

        [SMCHidden]
        public bool? PossuiTaxa { get; set; }

        [SMCOrder(9)]
        [SMCConditionalDisplay(nameof(PossuiTaxa), true)]
        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCSelect(nameof(TipoTaxa))]
        public long? SeqTipoTaxa { get; set; }

        [SMCHidden]
        public bool? BolsaExAluno { get; set; }
       
        [SMCOrder(10)]
        [SMCConditionalDisplay(nameof(BolsaExAluno), true)]
        [SMCFilter]
        [SMCSize(SMCSize.Grid2_24)]
        [SMCSelect]
        public bool? RecebeuBolsa { get; set; }

        public int? ExportAction { get; set; }

        [SMCHidden]
        public  bool? HabilitaGestaoEventos { get; set; }

        [SMCOrder(11)]
        [SMCConditionalDisplay(nameof(HabilitaGestaoEventos), true)]
        [SMCFilter]        
        [SMCSize(SMCSize.Grid4_24)]
        [SMCSelect()]
        public bool? CheckinRealizado { get; set; }


        [SMCHidden]
        public string BackUrl { get; set; }

    }
}