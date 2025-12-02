using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class AssertLancamentoResultadoLoteViewModel : SMCViewModelBase
    {
        public IEnumerable<string> Candidatos { get; set; }
    }
}