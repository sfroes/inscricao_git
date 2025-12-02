using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TipoHierarquiaOfertaViewModel : SMCViewModelBase, ISMCMappable, ISMCStatefulView
    {

        [SMCReadOnly]
        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        public long? Seq { get; set; }

        [SMCRequired]
        [SMCFilter]
        [SMCSize(SMCSize.Grid16_24)]
        [SMCMaxLength(100)]
        public string Descricao { get; set; }       

    }
}