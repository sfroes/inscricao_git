using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.Common.Areas.NOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.NOT.Models
{
    public class TipoNotificacaoViewModel : SMCViewModelBase
    {
        [SMCSelect("TipoNotificacao")]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCRequired]
        public long Seq { get; set; }

        [SMCHidden]
        public long OldSeq { get; set; }

        public List<SMCDatasourceItem> TipoNotificacao { get; set; }

        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid12_24)]
        public bool PermiteAgendamento { get; set; }
        
        [SMCDetail(SMCDetailType.Block)]
        [SMCSize(SMCSize.Grid24_24)]
        public SMCMasterDetailList<TipoNotificacaoDetalheViewModel> Atributos { get; set; }
    }
}