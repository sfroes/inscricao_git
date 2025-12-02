using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.NOT.Models
{
    public class ConfigurarNotificacaoListaViewModel : SMCViewModelBase
    {
        [SMCHidden]
        public long Seq { get; set; }
        
        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long SeqTipoNotificacao { get; set; }

        [SMCSortable(true, true,"TipoNotificacao.Descricao", SMCSortDirection.Ascending)]
        public string TipoNotificacao { get; set; }

        public bool EnvioAutomatico { get; set; }

        [SMCSortable(true, false, "TipoNotificacao.PermiteAgendamento")]
        public bool PermiteAgendamento { get; set; }

    }
}