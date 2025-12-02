using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Controllers;
using SMC.GPI.Administrativo.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConfigurarPaginaIdiomaEtapaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long SeqConfiguracaoEtapaPaginaIdioma { get; set; }

        [SMCHidden]
        public long SeqConfiguracaoEtapa { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long SeqEtapa { get; set; }

        public long SeqUnidadeResponsavel { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid24_24)]
        public string Titulo { get; set; }

        public string Pagina { get; set; }
                
        public string Idioma { get; set; }

        public bool ExibeFormulario { get; set; }

        [SMCRequired]
        [SMCSelect("TiposFormulario")]
        [SMCSize(SMCSize.Grid12_24)]
        public long? SeqTipoFormulario { get; set; }

        public List<SMCDatasourceItem> TiposFormulario { get; set; }

        [SMCRequired]
        [SMCSelect("Formularios")]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCDependency("SeqTipoFormulario", "BuscarFormularios", "ConfiguracaoEtapaPagina", true)]
        public long? SeqFormulario { get; set; }

        public List<SMCDatasourceItem> Formularios { get; set; }

        [SMCRequired]
        [SMCSelect("Visoes", AutoSelectSingleItem = true)]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCDependency("SeqTipoFormulario", nameof(ConfiguracaoEtapaPaginaController.BuscarVisoes), "ConfiguracaoEtapaPagina", true)]
        public long? SeqVisao { get; set; }
        
        [SMCSelect("Visoes")]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCDependency("SeqTipoFormulario", nameof(ConfiguracaoEtapaPaginaController.BuscarVisoes), "ConfiguracaoEtapaPagina", true)]        
        public long? SeqVisaoGestao { get; set; }

        public List<SMCDatasourceItem> Visoes { get; set; }

        [SMCHidden]
        public string PaginaToken { get; set; }
    }
}