using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class RecuperarPaginaEtapaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long SeqConfiguracaoEtapa { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCOrientation(SMCOrientation.Vertical)]
        [SMCCheckBoxList("PaginasDisponiveis")]
        public List<long> SeqPaginas { get; set; }

        public List<SMCDatasourceItem> PaginasDisponiveis { get; set; }
    }
}