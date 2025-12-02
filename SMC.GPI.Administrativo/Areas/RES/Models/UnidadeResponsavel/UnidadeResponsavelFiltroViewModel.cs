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
    public class UnidadeResponsavelFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        [SMCFilter]
        [SMCSize(SMCSize.Grid3_24)]
        [SMCMaxValue(long.MaxValue)]
        public long? Seq { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid10_24)]
        [SMCMaxLength(100)]
        public string Nome { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid3_24)]
        [SMCMaxLength(15)]
        public string Sigla { get; set; }
    }
}