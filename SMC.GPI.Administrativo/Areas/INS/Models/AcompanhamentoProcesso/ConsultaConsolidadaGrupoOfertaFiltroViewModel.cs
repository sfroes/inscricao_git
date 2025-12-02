using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
	public class ConsultaConsolidadaGrupoOfertaFiltroViewModel : SMCPagerViewModel, ISMCMappable
	{
        #region Datasources
        public List<SMCDatasourceItem> GrupoOfertas { get; set; }
        #endregion

        public ConsultaConsolidadaProcessoCabecalhoViewModel Cabecalho { get; set; }

		[SMCHidden]
        [SMCFilterKey]
		public long SeqProcesso { get; set; }

		[SMCFilter]
		[SMCSelect("GrupoOfertas")]
		[SMCSize(SMCSize.Grid4_24)]
        [SMCSingleFill("oferta")]
        public long? SeqGrupoOferta { get; set; }

        [SMCFilter]
        [LookupHierarquiaOferta]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCDependency("SeqProcesso")]
        [SMCSingleFill("oferta")]
        public LookupHierarquiaOfertaViewModel SeqItemHierarquiaOferta { get; set; }

        /// <summary>
        /// Renderiza um Lookup
        /// </summary>
        [SMCFilter]
        [LookupSelecaoOferta]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCDependency("SeqGrupoOferta")]
        [SMCDependency("SeqItemHierarquiaOferta")]
        [SMCDependency("SeqProcesso")]
        [SMCConditionalReadonly("SeqGrupoOferta", "", RuleName = "R1")]
        [SMCConditionalReadonly("SeqItemHierarquiaOferta", "", RuleName = "R2")]
        [SMCConditionalRule("R1 && R2")]
        public GPILookupViewModel OpcaoLookupOfertas { get; set; }

	}
}