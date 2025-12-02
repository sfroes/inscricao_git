using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TipoHierarquiaOfertaFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMaxValue(long.MaxValue)]        
        public long? Seq { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid14_24)]
        [SMCMaxLength(100)]
        public string Descricao { get; set; }
    }
}