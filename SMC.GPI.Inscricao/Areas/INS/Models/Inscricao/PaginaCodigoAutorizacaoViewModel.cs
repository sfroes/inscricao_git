using SMC.Framework;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class PaginaCodigoAutorizacaoViewModel : PaginaViewModel
    {
        // Token da página
        public override string Token
        {
            get
            {
                return TOKENS.PAGINA_CODIGO_AUTORIZACAO;
            }
        }

        public PaginaCodigoAutorizacaoViewModel()
        {
            CodigosAutorizacao = new SMCMasterDetailList<InscricaoCodigoAutorizacaoViewModel>();
        }

        [SMCDetail(SMCDetailType.Block, min:1)]
        public SMCMasterDetailList<InscricaoCodigoAutorizacaoViewModel> CodigosAutorizacao { get; set; }

    }
}