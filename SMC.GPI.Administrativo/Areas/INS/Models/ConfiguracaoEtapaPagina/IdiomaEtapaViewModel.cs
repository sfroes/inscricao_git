using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class IdiomaEtapaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        [SMCKey]
        [SMCSize(SMCSize.Grid4_24)]
        public long Seq { get; set; }

        [SMCHidden]
        [SMCSize(SMCSize.Grid4_24)]
        public long SeqEtapa { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCDescription]
        public string Descricao { get; set; }

        public bool EPadrao { get; set; }
    }
}