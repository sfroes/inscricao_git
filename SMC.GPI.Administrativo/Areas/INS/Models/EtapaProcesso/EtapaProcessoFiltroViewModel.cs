using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class EtapaProcessoFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        public CabecalhoProcessoViewModel Cabecalho { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        public long SeqProcesso { get; set; }
    }
}