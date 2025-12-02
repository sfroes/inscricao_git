using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Notificacoes.UI.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.NOT.Models
{
    public class ConfiguracaoNotificacaoIdiomaViewModel : SMCViewModelBase
    {
        public SMCLanguage Idioma { get; set; }

        public ConfiguracaoNotificacaoEmailViewModel ConfiguracaoNotificacao { get; set; }
    }
}