using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
	public class ConsultaConsolidadaProcessoFiltroViewModel : SMCPagerViewModel, ISMCMappable
	{      
		public ConsultaConsolidadaProcessoFiltroViewModel()
		{
			TiposProcessos = new List<SMCSelectListItem>();
			Clientes = new List<SMCSelectListItem>();
			Unidades = new List<SMCSelectListItem>();
		}

		[SMCFilter]
		[SMCSelect("TiposProcessos")]
        [SMCDependency(nameof(SeqUnidadeResponsavel),
                nameof(INS.AcompanhamentoProcessoController.BuscarTiposProcesso), "AcompanhamentoProcesso", "INS", false)]
        [SMCSize(SMCSize.Grid10_24)]
		public long? SeqTipoProcesso { get; set; }

		public List<SMCSelectListItem> TiposProcessos { get; set; }

		[SMCFilter]
		[SMCSelect("Clientes")]
		[SMCSize(SMCSize.Grid6_24)]
		public long? SeqCliente { get; set; }

		public List<SMCSelectListItem> Clientes { get; set; }

		[SMCFilter]
		[SMCSelect("Unidades")]
		[SMCSize(SMCSize.Grid8_24)]
		public long? SeqUnidadeResponsavel { get; set; }

		public List<SMCSelectListItem> Unidades { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        public long? SeqProcesso { get; set; }

		[SMCFilter]
		[SMCSize(SMCSize.Grid12_24)]
        [SMCMaxLength(100)]
		public string DescricaoProcesso { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]        
        [SMCMask("0000")]
        public int? AnoReferencia { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]          
        [SMCMaxValue(2)]
        [SMCMinValue(1)]
        public int? SemestreReferencia { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCConditionalRequired("DataFim", SMCConditionalOperation.NotEqual, "", null)]
        [SMCMaxDate("DataFim")]
        public DateTime? DataInicio { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMinDate("DataInicio")]
        [SMCConditionalRequired("DataInicio", SMCConditionalOperation.NotEqual, "", null)]
        public DateTime? DataFim { get; set; }

        [SMCHidden]
        public bool AutoPesquisa
        {
            get
            {
                bool retorno = true;
                if (SeqProcesso > 0 ||
                    SeqUnidadeResponsavel != null ||
                    SeqCliente != null ||
                    DataInicio != null ||
                    DataFim != null ||
                    SeqTipoProcesso != null)
                {
                    retorno = false;
                }

                return retorno;
            }
        }
    }
}