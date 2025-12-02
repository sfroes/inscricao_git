using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.RES.Models
{
    public class UnidadeResponsavelListaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCSortable]
        public long Seq { get; set; }

        [SMCSortable]
        public string Nome { get; set; }

        [SMCSortable]
        public string Sigla { get; set; }
    }
}