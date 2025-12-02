using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class HistoricoSituacaoCabecalhoViewModel : SMCViewModelBase
    {
        public long Seq { get; set; }

        public string DescricaoTipoProcesso { get; set; }

        public string NomeCliente { get; set; }

        public string Descricao { get; set; }

        public string Inscrito { get; set; }
    }
}