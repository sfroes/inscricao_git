using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.RES.Models
{
    public class ListarConfiguracoesTipoFormularioViewModel : SMCViewModelBase
    {
        public SMCPagerModel<UnidadeResponsavelTipoFormularioListaViewModel> Pager { get; set; }

        public int CodigoUnidade { get; set; }
    }
}