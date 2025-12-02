using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class HistoricoSituacaoListaViewModel : SMCViewModelBase
    {
        [SMCHidden]
        public long Seq { get; set; }

        public string Situacao { get; set; }

        [SMCDateTimeMode(Framework.SMCDateTimeMode.DateTime)]
        public DateTime Data { get; set; }

        public string Responsavel { get; set; }

        public string Motivo { get; set; }

        public string Justificativa { get; set; }
        [SMCHidden]
        public string BackURL { get; set; }
    }
}