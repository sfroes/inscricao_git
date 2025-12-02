using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class OfertaFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {

        public long? SeqGrupoOferta { get; set; }

        public long? SeqOferta { get; set; }

        public bool? Ativo { get; set; }

        public bool? Vigente { get; set; }

        public long? SeqProcesso { get; set; }
    
    }
}
