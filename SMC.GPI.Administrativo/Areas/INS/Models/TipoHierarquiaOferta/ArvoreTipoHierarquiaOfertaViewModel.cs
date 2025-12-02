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
    public class ArvoreTipoHierarquiaOfertaViewModel : SMCViewModelBase, ISMCMappable
    {

        public ArvoreTipoHierarquiaOfertaViewModel()
        {
            ItensHierarquia = new List<SMCTreeViewNode<NoArvoreTipoHierarquiaOfertaViewModel>>();
        }

        [SMCHidden]
        public long Seq { get; set; }

        public List<SMCTreeViewNode<NoArvoreTipoHierarquiaOfertaViewModel>> ItensHierarquia { get; set; }

    }
}