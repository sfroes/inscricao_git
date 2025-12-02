using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.NOT;
using SMC.Inscricoes.Common.Areas.NOT.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.NOT.Models
{
    public class TipoNotificacaoDetalheViewModel : SMCViewModelBase
    {
        [SMCHidden]
        public long? SeqAtributo { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCSelect]
        public AtributoAgendamento Atributo { get; set; }
    }
}