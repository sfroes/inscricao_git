using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class OfertaTaxaViewModel : SMCViewModelBase, ISMCMappable
    {
        public long SeqOferta { get; set; }

        public string Descricao { get; set; }

        public string Periodo { get; set; }

        public List<TaxaPeriodoOfertaViewModel> Taxas { get; set; }
    }
}
