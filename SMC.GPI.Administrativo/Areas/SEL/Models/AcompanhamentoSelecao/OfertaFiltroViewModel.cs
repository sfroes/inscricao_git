using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class OfertaFiltroViewModel : SMCViewModelBase
    {
        public long? SeqInscricaoOferta { get; set; }

        public long? SeqOferta { get; set; }

        public long? SeqInscricao { get; set; }

        public long? SeqGrupoOferta { get; set; }

        public long? SeqItemHierarquiaOferta { get; set; }

        public long? SeqProcesso { get; set; }

        public string BackUrl { get; set; }

    }
}