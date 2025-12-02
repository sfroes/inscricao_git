using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ProcessoFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        [SMCHidden]
        public bool ListagemInicial { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMaxValue(long.MaxValue)]
        public long? Seq { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid10_24)]
        [SMCMaxLength(255)]
        public string Descricao { get; set; }
               
        [SMCFilter]
        [SMCSize(SMCSize.Grid10_24)]
        [SMCSelect("UnidadesResponsaveis")]
        public long? SeqUnidadeResponsavel { get; set; }

        public List<SMCSelectListItem> UnidadesResponsaveis { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid9_24)]
        [SMCSelect("TiposProcesso")]
        [SMCDependency("SeqUnidadeResponsavel","BuscarTiposProcesso","Processo","INS",false)]
        public long? SeqTipoProcesso { get; set; }

        public List<SMCDatasourceItem> TiposProcesso { get; set; }

        [SMCFilter]
        [SMCMask("0000")]
        [SMCSize(SMCSize.Grid4_24)]
        public int? AnoReferencia { get; set; }

        [SMCFilter]
        [SMCMinValue(1)]
        [SMCMaxValue(2)]
        [SMCMask("0")]
        [SMCSize(SMCSize.Grid4_24)]
        public int? SemestreReferencia { get; set; }

    }
}