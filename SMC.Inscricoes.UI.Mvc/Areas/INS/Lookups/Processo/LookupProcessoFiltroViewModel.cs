using SMC.Framework;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupProcessoFiltroViewModel : SMCLookupFilterViewModel
    {
        public LookupProcessoFiltroViewModel()
        {
            Semestres = new List<SMCDatasourceItem>() 
            { 
                new SMCDatasourceItem() { Seq = 1, Descricao = "1" },
                new SMCDatasourceItem() { Seq = 2, Descricao = "2" }
            };
        }

        [SMCKey]
        [SMCSize(SMCSize.Grid4_24)]
        public long? Seq { get; set; }

        [SMCDescription]
        [SMCSize(SMCSize.Grid12_24)]
        public string Descricao { get; set; }

        [SMCSelect("TiposProcesso")]
        [SMCSize(SMCSize.Grid8_24)]
        public long? TipoProcesso { get; set;}

        public List<SMCDatasourceItem> TiposProcesso { get; set; }

        [SMCSelect("UnidadesResponsaveis")]
        [SMCSize(SMCSize.Grid10_24)]
        public long? UnidadeResponsavel { get; set; }

        public List<SMCDatasourceItem> UnidadesResponsaveis { get; set; }

        //[SMCSelect]
        //[SMCDependency("UnidadeResponsavel")]
        //[SMCSize(SMCSize.Grid10_24)]
        //public long TipoProcesso { get; set; }

        [SMCMask("0000")]
        [SMCSize(SMCSize.Grid4_24)]
        public int? AnoReferencia { get; set; }

        [SMCSelect("Semestres")]
        [SMCSize(SMCSize.Grid4_24)]
        public int? SemestreReferencia { get; set; }
        public List<SMCDatasourceItem> Semestres { get; set; }
    }
}
