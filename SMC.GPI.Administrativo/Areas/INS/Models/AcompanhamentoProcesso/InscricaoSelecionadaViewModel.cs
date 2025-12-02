using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class InscricaoSelecionadaViewModel : SMCPagerViewModel, ISMCMappable
    {
        public long SeqProcesso { get; set; }

        public GPILookupViewModel Oferta { get; set; }

        public long SeqGrupoOferta { get; set; }

        public long SeqTipoProcessoSituacao { get; set; }

        public List<long> GridAnaliseInscricaoLote { get; set; }

        public bool Lote { get; set; }

    
    }
}