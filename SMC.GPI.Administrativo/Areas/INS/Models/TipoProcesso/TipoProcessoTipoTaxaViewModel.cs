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
    public class TipoProcessoTipoTaxaViewModel : SMCViewModelBase
    {
        [SMCHidden]
        public long? SeqTipoProcessoTipoTaxa { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid17_24)]
        [SMCSelect("TiposTaxaSelect")]
        public long SeqTipoTaxa { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCSelect]
        public bool Ativo { get; set; }
    }
}