using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TaxaPeriodoOfertaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCSize(SMCSize.Grid5_24)]
        public string TipoTaxa { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        public string Periodo { get; set; }

        [SMCSize(SMCSize.Grid2_24)]
        public short? NumeroMinimo { get; set; }

        [SMCSize(SMCSize.Grid2_24)]
        public short? NumeroMaximo { get; set; }

        public long SeqEventoTaxa { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        public string EventoTaxa { get; set; }

        [SMCSize(SMCSize.Grid2_24)]
        public decimal? Valor { get; set; }

        public long SeqParametroCrei { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        public string VencimentoTitulo { get; set; }
    }
}
