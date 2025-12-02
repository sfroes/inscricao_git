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
    public class ConsultaNotificacaoListaViewModel : SMCViewModelBase
    {
        [SMCHidden]
        public long Seq { get; set; }

        [SMCSortable(true, false, "Inscricao.GrupoOferta.Nome")]
        public string GrupoOferta { get; set; }

        [SMCSortable]
        public List<string> Oferta { get; set; }

        [SMCSortable(true, false, "Inscricao.Inscrito.Nome")]
        public string Inscrito { get; set; }

        [SMCSortable(true, false, "DescricaoTipoNotificacao")]
        public string TipoNotificacao { get; set; }

        public string Assunto { get; set; }

        [SMCSortable(true, false, "SucessoEnvio")]
        public bool Sucesso { get; set; }

        [SMCSortable]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime DataEnvio { get; set; }

        [SMCHidden]
        public string BackUrl { get; set; }
    }
}