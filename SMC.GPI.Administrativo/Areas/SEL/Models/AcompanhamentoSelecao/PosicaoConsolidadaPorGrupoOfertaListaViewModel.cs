using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class PosicaoConsolidadaPorGrupoOfertaListaViewModel : AcompanhamentoSelecaoListaViewModel
    {
        public long SeqProcesso { get; set; }

        public long? SeqGrupoOferta { get; set; }

        public List<PosicaoConsolidadaPorOfertaListaViewModel> PosicoesConsolidadasOfertas { get; set; }
    }
}