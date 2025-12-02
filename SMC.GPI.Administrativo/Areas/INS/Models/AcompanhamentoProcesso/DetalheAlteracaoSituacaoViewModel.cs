using SMC.Framework;
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
    public class DetalheAlteracaoSituacaoViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCSize(SMCSize.Grid6_24)]
        [SMCReadOnly]
        public long SeqInscricao { get; set; }
                
        [SMCSize(SMCSize.Grid18_24)]
        [SMCReadOnly]
        public string NomeInscrito { get; set; }
    }
}