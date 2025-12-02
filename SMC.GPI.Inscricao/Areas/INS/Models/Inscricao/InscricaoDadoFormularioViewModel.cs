using SMC.Formularios.UI.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class InscricaoDadoFormularioViewModel : DadoFormularioViewModel
    {
        public long SeqInscricao { get; set; }

        public long SeqConfiguracaoEtapaPaginaIdioma { get; set; }

        public string UidProcesso { get; set; }

        public string TokenCssAlternativoSas { get; set; }

        public Guid? UidInscricaoOferta { get; set; }
    }
}