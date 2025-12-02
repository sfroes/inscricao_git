using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common.Areas.RES;
using System;
using System.Globalization;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TaxaTituloInscricaoViewModel : SMCViewModelBase, ISMCMappable
	{
        public string Descricao { get; set; }

        public int NumeroItens { get; set; }
	}
} 