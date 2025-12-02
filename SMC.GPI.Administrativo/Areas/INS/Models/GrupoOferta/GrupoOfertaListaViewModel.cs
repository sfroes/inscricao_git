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
    public class GrupoOfertaListaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCSortable]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCSortable]
        public string Descricao { get; set; }
    }
}