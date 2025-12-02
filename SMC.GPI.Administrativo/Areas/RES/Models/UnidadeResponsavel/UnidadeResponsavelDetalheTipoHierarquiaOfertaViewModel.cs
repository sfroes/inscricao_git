using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.RES.Models
{
    public class UnidadeResponsavelDetalheTipoHierarquiaOfertaViewModel : SMCViewModelBase, ISMCMappable
    {

        public UnidadeResponsavelDetalheTipoHierarquiaOfertaViewModel()
        {
            TipoHierarquiaAtiva = true;
        }

        [SMCHidden]
        public long SeqUnidadeTipoHierarquia { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid16_24)]
        [SMCSelect("TiposHierarquiaOferta")]        
        public long SeqTipoHierarquiaOferta { get; set; }

        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCRequired]
        [SMCMapForceFromTo]
        public bool? TipoHierarquiaAtiva { get; set; }
    }
}