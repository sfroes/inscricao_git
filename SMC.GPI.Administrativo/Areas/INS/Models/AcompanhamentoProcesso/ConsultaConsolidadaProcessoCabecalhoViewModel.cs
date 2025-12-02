using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConsultaConsolidadaProcessoCabecalhoViewModel : SMCViewModelBase, ISMCMappable
	{
		[SMCHidden]
		public long Seq { get; set; }

		public string DescricaoTipoProcesso { get; set; } 

		public string Descricao { get; set; }

		public string NomeCliente { get; set; }

        public ConsultaConsolidadaProcessoListaViewModel PosicaoConsolidada { get; set; }
	}
} 