using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class HistoricoSituacaoFiltroViewModel : SMCViewModelBase
    {
        public long SeqInscrito { get; set; }

        public long SeqInscricao { get; set; }

        public long SeqProcesso { get; set; }

        public string Origem { get; set; }

        [SMCHidden]
        public string BackURL { get; set; }
    }
}