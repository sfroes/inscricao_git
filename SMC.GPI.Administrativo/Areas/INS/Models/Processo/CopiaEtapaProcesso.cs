using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class CopiaEtapaProcesso : SMCViewModelBase
    {
        public CopiaEtapaProcesso()
        {
            Copiar = true;
        }

        [SMCHidden]
        public long? SeqEtapa { get; set; }

        [SMCHidden]
        public long? SeqEtapaSGF { get; set; }

        [SMCConditionalReadonly("TipoProcessoDesativado", SMCConditionalOperation.Equals, true, RuleName = "R1")]
        [SMCConditionalReadonly("TemplateProcessoDesativado", SMCConditionalOperation.Equals, true, RuleName = "R2")]
        [SMCConditionalRule("R1 || R2")]
        [SMCSize(SMCSize.Grid1_24)]
        public bool Copiar { get; set; }

        [SMCSize(SMCSize.Grid9_24)]
        public string Etapa { get; set; }

        [SMCConditionalReadonly("Copiar", false)]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCConditionalRequired("Copiar", true)]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime? DataInicio { get; set; }

        [SMCConditionalReadonly("Copiar", false)]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMinDate("DataInicio")]
        [SMCConditionalRequired("Copiar", true)]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime? DataFim { get; set; }

        [SMCSize(SMCSize.Grid5_24)]
        [SMCSelect]
        [SMCConditionalReadonly("Copiar", false, PersistentValue = true)]
        [SMCConditionalRequired("Copiar", true)]
        public bool CopiarConfiguracoes { get; set; }
    }
}