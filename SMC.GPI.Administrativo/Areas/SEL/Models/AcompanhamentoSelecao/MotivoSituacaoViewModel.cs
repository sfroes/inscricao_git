using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class MotivoSituacaoViewModel
    {
        public string Justificativa { get; set; }

        public string Motivo { get; set; }

        public long? SeqMotivoSituacao { get; set; }
    }
}