using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class CheckinLoteFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        public long? SeqInscricao { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid10_24)]
        public string NomeInscrito { get; set; }

        [SMCFilter(true, true)]
        [Required]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCSelect()]        
        public bool? CheckinRealizado { get; set; }

        [SMCHidden]
        [SMCFilter(true, true)]
        public long SeqOferta { get; set; }
    }
}