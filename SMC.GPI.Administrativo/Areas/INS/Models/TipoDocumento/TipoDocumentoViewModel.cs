using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{    
    public class TipoDocumentoViewModel : SMCViewModelBase
    {                   
        [SMCSize(SMCSize.Grid10_24)]        
        [SMCSelect("TiposDocumento")]
        [SMCReadOnly(SMCViewMode.Edit)]
        [SMCRequired]
        public long Seq { get; set; }

        public List<SMCDatasourceItem> TiposDocumento { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCSelect]
        public TipoEmissao? TipoEmissao { get; set; }

        [SMCHidden]
        public bool Novo { get; set; }
    }
}
