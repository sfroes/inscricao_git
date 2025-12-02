using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TipoProcessoTemplateProcessoViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long? SeqTipoProcessoTemplate { get; set; }

        [SMCRequired]
        [SMCSelect("TemplatesProcessoSelect")]
        [SMCSize(SMCSize.Grid17_24)]
        public long? SeqTemplateProcessoSGF { get; set; }

        [SMCRequired]
        [SMCSelect]
        [SMCSize(SMCSize.Grid4_24)]
        public bool? Ativo { get; set; }

    }
}