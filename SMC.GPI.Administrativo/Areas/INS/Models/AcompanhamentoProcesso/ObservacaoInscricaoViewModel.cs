using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ObservacaoInscricaoViewModel : SMCViewModelBase
    {
        [SMCHidden]
        public long Seq { get; set; }
        public long SeqProcesso { get; set; }
        public string Nome { get; set; }

        [SMCMultiline(Rows = 5)]
        [SMCRequired]
        [SMCSize(Framework.SMCSize.Grid24_24)]
        public string Observacao { get; set; }

    }
}