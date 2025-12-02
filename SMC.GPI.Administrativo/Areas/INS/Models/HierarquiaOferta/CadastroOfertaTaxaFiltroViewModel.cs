using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class CadastroOfertaTaxaFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        [SMCHidden]
        [SMCFilterKey]
        public long SeqProcesso { get; set; }

        [SMCFilter]
        [SMCSelect("TiposTaxa")]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCRequired]
        public long? SeqTipoTaxa { get; set; }

        [SMCFilter]
        [SMCSelect("Periodos")]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCRequired]
        public string Periodo { get; set; }


        [SMCFilter]
        [SMCSelect("GruposOferta")]
        [SMCSize(SMCSize.Grid6_24)]        
        public long? SeqGrupoOferta { get; set; }

        public List<SMCDatasourceItem<string>> Periodos { get; set; }

        public List<SMCDatasourceItem> TiposTaxa { get; set; }

        public List<SMCDatasourceItem> GruposOferta { get; set; }

        public bool PossuiTaxa { get; set; }
    
    }
}
