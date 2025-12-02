using Newtonsoft.Json;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Notificacoes.UI.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.NOT.Models
{
    public class ConfigurarNotificacaoViewModel : SMCWizardViewModel, ISMCStatefulView
    {
        public ConfigurarNotificacaoViewModel()
        {
            Idiomas = new List<string>();
            EnvioAutomatico = true;
        }

        [SMCKey]
        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCRequired]
        [SMCSelect("TiposNotificacao", NameDescriptionField = "DescricaoTipoNotificacao")]
        [SMCSize(SMCSize.Grid12_24)]
        public long SeqTipoNotificacao { get; set; }

        public string DescricaoTipoNotificacao { get; set; }

        /// <summary>
        /// Armazena o valor do SeqTipoNotificacao para verificar se houve modificação
        /// </summary>
        [SMCHidden]
        public long OldSeqTipoNotificacao { get; set; }

        [JsonIgnore]
        public List<SMCDatasourceItem> TiposNotificacao { get; set; }

        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCMapForceFromTo]
        public bool EnvioAutomatico { get; set; }

        [JsonIgnore]
        public List<string> Idiomas { get; set; }

        public List<ConfiguracaoNotificacaoIdiomaViewModel> ConfiguracoesEmail { get; set; }
    }
}