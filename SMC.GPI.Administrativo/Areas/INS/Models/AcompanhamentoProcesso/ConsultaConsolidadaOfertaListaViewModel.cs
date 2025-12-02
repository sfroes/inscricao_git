using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConsultaConsolidadaOfertaListaViewModel : ConsultaConsolidadaListaViewModel, ISMCMappable
	{

        public long SeqProcesso { get; set; }

        public long SeqGrupoOferta {get;set;}

	}
}