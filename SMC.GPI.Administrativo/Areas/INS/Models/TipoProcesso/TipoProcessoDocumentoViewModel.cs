using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models.TipoProcesso
{
    public class TipoProcessoDocumentoViewModel : SMCViewModelBase, ISMCMappable
    {
        public TipoProcessoDocumentoViewModel()
        {
            Ativo = true;
        }

        [SMCRequired]
        [SMCSize(SMCSize.Grid14_24)]
        [SMCSelect("TiposDocumentoSelect")]
        public long SeqTipoDocumento { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCRadioButtonList]
        [SMCMapForceFromTo]
        public bool Ativo {get;set;}
    }
}