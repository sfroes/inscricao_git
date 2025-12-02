using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConsultaConsolidadaProcessoListaViewModel : ConsultaConsolidadaListaViewModel
    {
        public new long Seq { get { return base.Seq; } set { base.Seq = value; } }

        [SMCCssClass("smc-gpi-grid-coluna-numeros")]
        [SMCOrder(2)]
        public int OfertasNaoSelecionadas { get; set; }

    }
}