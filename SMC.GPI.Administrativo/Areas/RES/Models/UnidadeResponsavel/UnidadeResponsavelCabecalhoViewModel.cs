using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.RES.Models
{
    public class UnidadeResponsavelCabecalhoViewModel : SMCViewModelBase, ISMCMappable
    {
        public long Seq { get; set; }

        public string Nome { get; set; }

        public string Sigla { get; set; }

        public int CodigoUnidade { get; set; }

        public string UnidadeResponsavel
        {
            get { return $"{Nome} - {Sigla}"; }
            set { }
        }
    }
}