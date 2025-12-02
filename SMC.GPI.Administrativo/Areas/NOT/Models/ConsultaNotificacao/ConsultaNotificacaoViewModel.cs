using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.NOT.Models
{
    public class ConsultaNotificacaoViewModel : SMCViewModelBase
    {
        public ConsultaNotificacaoViewModel()
        {
            Arquivos = new List<SMCDatasourceItem>();
        }

        public long Seq { get; set; }

        public string Processo { get; set; }

        public string GrupoOferta { get; set; }

        public List<string> Oferta { get; set; }

        public string Inscrito { get; set; }

        #region Detalhes
        public string DescricaoTipoNotificacao { get; set; }

        public string NomeRementente { get; set; }

        public string EmailRemetente { get; set; }

        public string EmailResposta { get; set; }

        public string AssuntoNotificacao { get; set; }

        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime DataPrevistaEnvio { get; set; }

        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime? DataEnvio { get; set; }

        public bool? SucessoEnvio { get; set; }

        public string EmailDestinatario { get; set; }

        public string EmailComCopia { get; set; }

        public string EmailComCopiaOculta { get; set; }

        public string DescricaoErroEnvio { get; set; }
        #endregion

        public string Mensagem { get; set; }

        [SMCMapForceFromTo]
        public List<SMCDatasourceItem> Arquivos { get; set; }

        public string BackUrl { get; set; }

    }
}