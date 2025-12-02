using SMC.Framework;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups.INS.Inscrito
{
    public class LookupInscritoController : SMCControllerBase
    {
        private IProcessoService ProcessoService
        {
            get { return Create<IProcessoService>(); }
        }

        [SMCAllowAnonymous]
        public ActionResult BuscarSituacoes(long processo)
        {
            var situacoes = this.ProcessoService.BuscarSituacoesProcessoKeyValue(processo).
                                            TransformList<SMCDatasourceItem>();
            return Json(situacoes);
        }
    }
}
