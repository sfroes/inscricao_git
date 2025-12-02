using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupOfertaPeriodoInscricaoFiltroViewModel : SMCLookupFilterViewModel,ISMCMappable
    {       
        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long? SeqGrupoOferta { get { return null; } }

    }
}
