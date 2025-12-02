using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Controllers;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class GrupoDocumentosRequeridosViewModel : SMCViewModelBase, ISMCMappable
    {
        public GrupoDocumentosRequeridosViewModel()
        {
            DocumentosRequeridos = new List<SMCDatasourceItem>();
            Itens = new SMCMasterDetailList<GrupoDocumentosRequeridosDetalheViewModel>();
        }

        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqEtapa { get; set; }

        [SMCHidden]
        [SMCSize(SMCSize.Grid4_24)]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        [SMCSize(SMCSize.Grid4_24)]
        public long SeqConfiguracaoEtapa { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid10_24)]
        public string Descricao { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCMask("0999")]
        [SMCMinValue(1)]
        public short? MinimoObrigatorio { get; set; }

        [SMCHidden]
        public short? MinimoObrigatorioOriginal { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCOrientation(SMCOrientation.Horizontal)]
        [SMCRadioButtonList]
        public bool? UploadObrigatorio { get; set; }        

        [SMCHidden]
        public bool? UploadObrigatorioOriginal { get; set; }

        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCConditionalRequired(nameof(UploadObrigatorio), SMCConditionalOperation.Equals, true)]
        [SMCConditionalReadonly(nameof(UploadObrigatorio), SMCConditionalOperation.Equals, false, PersistentValue = false, RuleName ="R1")]
        [SMCConditionalReadonly(nameof(UploadObrigatorio), SMCConditionalOperation.Equals, null, PersistentValue = false, RuleName ="R2")]
        [SMCDependency(nameof(UploadObrigatorio), nameof(GrupoDocumentoRequeridoController.ValorPadraoCampoExibeTermoResponsabilidadeEntrega), "GrupoDocumentoRequerido", true)]
        [SMCConditionalRule("R1 || R2")]
        public bool? ExibeTermoResponsabilidadeEntrega { get; set; }

        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMinDateNow]
        [SMCConditionalReadonly(nameof(ExibeTermoResponsabilidadeEntrega), SMCConditionalOperation.Equals, false, PersistentValue = false, RuleName = "R1")]
        [SMCConditionalReadonly(nameof(ExibeTermoResponsabilidadeEntrega), SMCConditionalOperation.Equals, null, PersistentValue = false, RuleName = "R2")]
        [SMCConditionalRule("R1 || R2")]
        public DateTime? DataLimiteEntrega { get; set; }

        public List<SMCDatasourceItem> DocumentosRequeridos { get; set; }

        [SMCMapForceFromTo]
        [SMCDetail(SMCDetailType.Tabular)]
        public SMCMasterDetailList<GrupoDocumentosRequeridosDetalheViewModel> Itens { get; set; }

        [SMCHidden]
        public string ItensHash { get; set; }
    }
}