using SMC.Framework.DataAnnotations;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.NOT.Models
{
    public class ParametroNotificacaoViewModel : SMCViewModelBase
    {
        [SMCKey]
        [SMCHidden]
        public long Seq { get; set; }

        public string TipoNotificacao { get; set; }

        public bool EnvioAutomatico { get; set; }

        public List<SMCDatasourceItem> AtributosDisponiveis { get; set; }

        [SMCDetail(SMCDetailType.Tabular)]
        public SMCMasterDetailList<ParametrosNotificacaoItemViewModel> ParametrosEnvioNotificacao { get; set; }
    }
}