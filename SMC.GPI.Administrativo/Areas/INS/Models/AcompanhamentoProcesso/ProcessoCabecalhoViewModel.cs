using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ProcessoCabecalhoViewModel : SMCPagerViewModel, ISMCMappable
    {
        [SMCHidden]
        public long Seq { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCMaxLength(100)]
        public string DescricaoTipoProcesso { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCMaxLength(100)]
        public string NomeCliente { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCMaxLength(100)]
        public string Descricao { get; set; }

    }
}