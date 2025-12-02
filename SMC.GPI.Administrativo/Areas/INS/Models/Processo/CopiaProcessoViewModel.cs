using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class CopiaProcessoViewModel : SMCViewModelBase
    {

        public CopiaProcessoViewModel()
        {
            CopiarItens = true;
            CopiarNotificacoes = true;
        }

        [SMCHidden]
        public long SeqProcessoOrigem { get; set; }

        [SMCHidden]
        public long SeqProcessoGpi { get; set; }

        [SMCSize(SMCSize.Grid18_24)]
        public string DescricaoTipoProcesso { get; set; }
                
        [SMCSize(SMCSize.Grid24_24)]
        public string Descricao { get; set; }
                
        [SMCSize(SMCSize.Grid4_24)]        
        public int? AnoReferencia { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        public int? SemestreReferencia { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid16_24)]
        public string NovoProcessoDescricao { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMask("0000")]
        public int? NovoProcessoAnoReferencia { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMaxValue(2)]
        [SMCMinValue(1)]
        public int? NovoProcessoSemestreReferencia { get; set; }

        [SMCSize(SMCSize.Grid10_24)]
        [SMCConditionalReadonly("TipoProcessoDesativado", SMCConditionalOperation.Equals, true, RuleName = "R1")]
        [SMCConditionalReadonly("TipoHierarquiaOfertaDesativado", SMCConditionalOperation.Equals, true, RuleName = "R2")]
        [SMCConditionalRule("R1 || R2")]
        public bool CopiarItens { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCConditionalReadonly("CopiarItens", false)]
        [SMCConditionalRequired("CopiarItens", true)]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime? DataInicioInscricao { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCConditionalReadonly("CopiarItens", false)]
        [SMCConditionalRequired("CopiarItens", true)]
        [SMCMinDate("DataInicioInscricao")]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime? DataFimInscricao { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        public bool CopiarNotificacoes { get; set; }

        [SMCSize(SMCSize.Grid10_24)]
        public bool CopiarFormularioEvento { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCConditionalReadonly(nameof(CopiarFormularioEvento), false)]
        [SMCConditionalRequired(nameof(CopiarFormularioEvento), true)]
        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        public DateTime? DataInicioFormulario { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCConditionalReadonly(nameof(CopiarFormularioEvento), false)]        
        [SMCMinDate(nameof(DataInicioFormulario))]
        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        public DateTime? DataFimFormulario { get; set; }

        public List<CopiaEtapaProcesso> Etapas { get; set; }

        [SMCHidden]
        public bool TipoProcessoDesativado { get; set; }

        [SMCHidden]
        public bool TemplateProcessoDesativado { get; set; }

        [SMCHidden]
        public bool TipoHierarquiaOfertaDesativado { get; set; }

        public string MensagemInformativa { get; set; }

        [SMCHidden]
        public bool PossuiFormularioConfigurado { get; set; }
    }
}