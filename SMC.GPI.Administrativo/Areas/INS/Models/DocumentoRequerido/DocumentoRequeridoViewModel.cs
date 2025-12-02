using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Areas.INS.Controllers;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class DocumentoRequeridoViewModel : SMCViewModelBase, ISMCMappable
    {
        public DocumentoRequeridoViewModel()
        {
            TiposDocumento = new List<SMCDatasourceItem>();
        }

        #region Auxiliares

        [SMCHidden]
        public long SeqConfiguracaoEtapa { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long SeqEtapaProcesso { get; set; }

        [SMCHidden]
        public long TipoDocumentoOriginal { get; set; }

        public List<SMCDatasourceItem> TiposDocumento { get; set; }

        [SMCHidden]
        public short VersaoDocumentoOriginal { get; set; }

        [SMCHidden]
        public bool? UploadObrigatorioOriginal { get; set; }

        #endregion Auxiliares

        [SMCKey]
        [SMCHidden]
        [SMCSize(SMCSize.Grid4_24)]
        public long Seq { get; set; }

        [SMCSelect("TiposDocumento")]
        [SMCRequired]
        [SMCSize(SMCSize.Grid10_24)]
        public long SeqTipoDocumento { get; set; }

        [SMCRequired]
        [SMCSelect]
        [SMCSize(SMCSize.Grid6_24)]
        public VersaoDocumento VersaoDocumento { get; set; }

        /// <summary>
        /// Permite mais de um arquivo?
        /// </summary>
        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid4_24, SMCSize.Grid24_24, SMCSize.Grid6_24,SMCSize.Grid8_24)]
        public bool PermiteVarios { get; set; }

        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid4_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid8_24)]
        public bool? PermiteUploadArquivo { get; set; }

        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid4_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid8_24)]
        [SMCConditionalReadonly("PermiteUploadArquivo", false)]
        [SMCConditionalRequired("PermiteUploadArquivo", true)]
        public bool? UploadObrigatorio { get; set; }

        /// <summary>
        /// Permite entrega posterior?
        /// </summary>
        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid4_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid8_24)]
        public bool PermiteEntregaPosterior { get; set; }

        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid4_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid8_24)]
        [SMCConditionalRequired(nameof(UploadObrigatorio), SMCConditionalOperation.Equals, true, RuleName = "R1")]
        [SMCConditionalRequired(nameof(PermiteEntregaPosterior), SMCConditionalOperation.Equals, true, RuleName = "R2")]
        [SMCConditionalReadonly(nameof(UploadObrigatorio), SMCConditionalOperation.Equals, false, PersistentValue = false, RuleName = "R3")]
        [SMCConditionalReadonly(nameof(PermiteEntregaPosterior), SMCConditionalOperation.Equals, false, PersistentValue = false, RuleName = "R4")]
        [SMCConditionalReadonly(nameof(UploadObrigatorio), SMCConditionalOperation.Equals, null, PersistentValue = false, RuleName = "R5")]
        [SMCConditionalReadonly(nameof(PermiteEntregaPosterior), SMCConditionalOperation.Equals, null, PersistentValue = false, RuleName = "R6")]
        [SMCDependency(nameof(UploadObrigatorio), nameof(DocumentoRequeridoController.ValorPadraoCampoExibeTermoResponsabilidadeEntrega), "DocumentoRequerido", true, includedProperties:new string[] { nameof(PermiteEntregaPosterior)})]
        [SMCDependency(nameof(PermiteEntregaPosterior), nameof(DocumentoRequeridoController.ValorPadraoCampoExibeTermoResponsabilidadeEntrega), "DocumentoRequerido", true, includedProperties: new string[] { nameof(UploadObrigatorio) })]
        [SMCConditionalRule("(R1 && R2)")]
        [SMCConditionalRule("((R3 && R4) || (!R3 && R4) || (R3 && !R4)) || ((R5 && R6) || (!R5 && R6) || (R5 && !R6))")]
        public bool? ExibeTermoResponsabilidadeEntrega { get; set; }

        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        [SMCSize(SMCSize.Grid4_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid8_24)]
        [SMCMinDateNow]
        [SMCConditionalReadonly(nameof(ExibeTermoResponsabilidadeEntrega), SMCConditionalOperation.Equals, false, PersistentValue = false, RuleName = "R1")]
        [SMCConditionalReadonly(nameof(ExibeTermoResponsabilidadeEntrega), SMCConditionalOperation.Equals, null, PersistentValue = false, RuleName = "R2")]
        [SMCConditionalRule("R1 || R2")]
        public DateTime? DataLimiteEntrega { get; set; }

        /// <summary>
        /// Permite validação por outro setor
        /// </summary>
        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid4_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid8_24)]
        public bool ValidacaoOutroSetor { get; set; }



        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid4_24)]
        public bool? Obrigatorio { get; set; }

        [SMCSelect]
        [SMCConditionalReadonly("Obrigatorio", SMCConditionalOperation.NotEqual, true)]
        [SMCSize(SMCSize.Grid4_24)]
        public Sexo? Sexo { get; set; }
    }
}