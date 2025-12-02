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
    public class AlterarIdiomaEtapaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long SeqConfiguracaoEtapa { get; set; }

        [SMCHidden]
        [SMCSize(SMCSize.Grid24_24)]
        public SMCLanguage IdiomaPadrao { get; set; }

        public List<SMCLanguage> IdiomasDisponiveis { get; set; }        

        [SMCOrientation(SMCOrientation.Vertical)]
        [SMCCheckBoxList("IdiomasDisponiveis")]
        [SMCSize(SMCSize.Grid24_24)]
        public List<SMCLanguage> IdiomasEmUso { get; set; }
    }
}