using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.NOT;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.NOT.Models
{
    public class TipoNotificacaoFiltroViewModel : SMCPagerViewModel
    {
        [SMCSortable(true, true, "Descricao")]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCMaxLength(100)]
        public string TipoNotificacao { get; set; }
        
        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid12_24)]
        public bool? PermiteAgendamento { get; set; }

    }
}