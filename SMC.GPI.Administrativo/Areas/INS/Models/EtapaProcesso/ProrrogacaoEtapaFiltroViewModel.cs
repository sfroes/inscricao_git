using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Administrativo.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ProrrogacaoEtapaFiltroViewModel : SMCViewModelBase, ISMCMappable
    {        
        public long SeqEtapaProcesso { get; set; }
                
        public long SeqProcesso { get; set; }
    }
}