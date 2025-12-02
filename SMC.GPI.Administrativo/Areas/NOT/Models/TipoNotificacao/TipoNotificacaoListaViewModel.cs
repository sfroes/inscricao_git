using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.NOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.NOT.Models
{
    public class TipoNotificacaoListaViewModel : SMCViewModelBase
    {
        [SMCHidden]
        public long Seq { get; set; }

        [SMCSortable(true, true, "Descricao")]
        public string TipoNotificacao { get; set; }

        [SMCSortable]
        [SMCRadioButtonList]
        public bool PermiteAgendamento { get; set; }
        
    }
}