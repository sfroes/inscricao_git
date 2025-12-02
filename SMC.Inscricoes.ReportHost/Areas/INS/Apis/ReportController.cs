using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System.Web.Http;
using SMC.Inscricoes.Common.Constants;
using SMC.Framework.UI.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.ReportHost.Areas.INS.Apis
{
    public class ReportController : SMCApiControllerBase
    {

        #region Services

        private IInscritoAtividadeRelatorioService InscritoAtividadeRelatorioService
                => Create<IInscritoAtividadeRelatorioService>();
        #endregion


        [HttpPost]
        [AllowAnonymous]
        public byte[] InscritoAtividadeRelatorio(InscritoAtividadeRelatorioFiltroData filtro)
        {

            filtro.PageSettings = filtro.PageSettings ?? new Framework.Model.SMCPageSetting();
            filtro.PageSettings.PageSize = int.MaxValue;

            var listInscritos = this.InscritoAtividadeRelatorioService.BuscarInscritosAtividades(filtro);

            var retorno = SMCGenerateReport(REPORTS.RDLC_INSCRITO_ATIVIDADE_RELATORIO, listInscritos,
                                            REPORTS.DS_INSCRITO_ATIVIDADE,
                                            new SMCReportViewerHelper(Framework.SMCExportTypes.PDF),
                                            Framework.UI.Mvc.Util.SMCOrientationReport.Portrait, null);

            return retorno;
        }
    }

}
