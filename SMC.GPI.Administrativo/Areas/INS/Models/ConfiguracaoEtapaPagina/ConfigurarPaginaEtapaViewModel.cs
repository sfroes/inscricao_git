using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.RES.Models;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConfigurarPaginaEtapaViewModel : SMCViewModelBase, ISMCMappable
    {           
        [SMCHidden]
        public long SeqConfiguracaoEtapaPagina { get; set; }

        [SMCIgnoreProp]
        public string PaginaToken { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        public string DescricaoPagina { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCRequired]
        [SMCRadioButtonList]
        public bool? ExibirConfirmacao { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCRadioButtonList]
        [SMCRequired]
        public bool? ExibirComprovante { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCRadioButtonList]
        [SMCRequired]
        [SMCConditionalRequired(nameof(PaginaToken), SMCConditionalOperation.Equals, TOKENS.PAGINA_COMPROVANTE_INSCRICAO)]
        public bool? ExibeDadosPessoais { get; set; }
    }
}