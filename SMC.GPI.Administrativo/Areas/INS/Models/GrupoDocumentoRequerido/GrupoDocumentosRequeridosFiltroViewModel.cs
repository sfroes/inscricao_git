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
    public class GrupoDocumentosRequeridosFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        public long SeqProcesso { get; set; }

        public long SeqEtapa { get; set; }
        
        [SMCHidden]
        public long SeqConfiguracaoEtapa { get; set; }
    }
}