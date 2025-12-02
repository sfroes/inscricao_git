using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.NOT.Models
{
    public class ConfigurarNotificacaoFiltroViewModel : SMCPagerViewModel
    {
        [SMCFilterKey]
        public long SeqProcesso { get; set; }
    }
}