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
    public class ConsultaConsolidadaGrupoOfertaListaViewModel : ConsultaConsolidadaListaViewModel, ISMCMappable
	{

        public ConsultaConsolidadaGrupoOfertaListaViewModel()
        {
            PosicoesConsolidadasOfertas = new List<ConsultaConsolidadaOfertaListaViewModel>();
        }
        public long SeqProcesso { get; set; }

        public int OfertasNaoSelecionadas { get; set; }

        [SMCMapForceFromTo]
        public List<ConsultaConsolidadaOfertaListaViewModel> PosicoesConsolidadasOfertas { get; set; }
	}
}