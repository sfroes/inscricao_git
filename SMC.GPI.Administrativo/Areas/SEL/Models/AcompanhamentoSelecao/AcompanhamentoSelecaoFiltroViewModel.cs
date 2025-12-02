using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class AcompanhamentoSelecaoFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        #region DataSources
        
        public List<SMCDatasourceItem> UnidadesResponsaveis { get; set; }
        
        public List<SMCDatasourceItem> TiposProcessos { get; set; }
        
        #endregion

        [SMCFilter]
        [SMCSelect("UnidadesResponsaveis", AutoSelectSingleItem = true)]
        [SMCSize(SMCSize.Grid6_24)]
        public long? SeqUnidadeResponsavel { get; set; }

        [SMCFilter]
        [SMCSelect("TiposProcessos", AutoSelectSingleItem = true)]
        [SMCDependency(nameof(SeqUnidadeResponsavel), 
                nameof(INS.AcompanhamentoProcessoController.BuscarTiposProcesso), "AcompanhamentoProcesso", "INS", false)]
        [SMCSize(SMCSize.Grid8_24)]
        public long? SeqTipoProcesso { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid3_24)]
        public long? SeqProcesso { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid7_24)]
        public string DescricaoProcesso { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMaxValue(2)]
        [SMCMinValue(1)]
        public int? SemestreReferencia { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMask("0000")]
        public int? AnoReferencia { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid3_24)]
        [SMCMaxDate("DataFim")]
        [SMCConditionalRequired("DataFim", SMCConditionalOperation.NotEqual, "", null)]
        public DateTime? DataInicio { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid3_24)]
        [SMCMinDate("DataInicio")]
        [SMCConditionalRequired("DataInicio", SMCConditionalOperation.NotEqual, "", null)]
        public DateTime? DataFim { get; set; }
    }
}