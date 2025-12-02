using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ItemGrupoOfertaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCKey]
        [SMCHidden]
        public long Seq { get; set; }
                       
        [SMCDescription]        
        public string DescricaoCompleta { get; set; }

        public string NomeGrupoOferta { get; set; }

        [SMCHidden]
        public bool PossuiGrupo { get; set; }
    }
}