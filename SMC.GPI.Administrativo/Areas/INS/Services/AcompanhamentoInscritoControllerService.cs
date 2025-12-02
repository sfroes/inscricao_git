using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class AcompanhamentoInscritoControllerService : SMCControllerServiceBase
    {
        private AcompanhamentoInscritoService AcompanhamentoInscritoService
        {
            get { return this.Create<AcompanhamentoInscritoService>(); }
        }


        #region Listar Inscritos

        public SMCPagerModel<AcompanhamentoInscritoListaViewModel> BuscarInscritos(AcompanhamentoInscritoFiltroViewModel filtros)
        { 
            AcompanhamentoInscritoFiltroData filtroData = SMCMapperHelper.Create<AcompanhamentoInscritoFiltroData>(filtros);
            SMCPagerData<AcompanhamentoInscritoListaData> datas = AcompanhamentoInscritoService.BuscarInscrito(filtroData);
            SMCPagerData<AcompanhamentoInscritoListaViewModel> model = SMCMapperHelper.Create<SMCPagerData<AcompanhamentoInscritoListaViewModel>>(datas);
            return new SMCPagerModel<AcompanhamentoInscritoListaViewModel>(model, filtros.PageSettings, filtros);
        }
        #endregion
    }
}