using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class InscricaoForaPrazoControllerService : SMCControllerServiceBase
    {
        #region Services
        private IInscricaoForaPrazoService InscricaoForaPrazoService
        {
            get { return this.Create<IInscricaoForaPrazoService>(); }
        }
        #endregion

        public SMCPagerModel<InscricaoForaPrazoListaViewModel> BuscarInscricoesForaPrazo(InscricaoForaPrazoFiltroViewModel filtro)
        {
            var datas = this.InscricaoForaPrazoService.BuscarInscricoesForaPrazo(
                                    filtro.Transform<InscricaoForaPrazoFiltroData>());
            var pagerData = datas.Transform<SMCPagerData<InscricaoForaPrazoListaViewModel>>();
            return new SMCPagerModel<InscricaoForaPrazoListaViewModel>(pagerData, filtro.PageSettings, filtro);
        }

        public InscricaoForaPrazoViewModel BuscarInscricaoForaPrazo(long seq)
        {
            return InscricaoForaPrazoService.BuscarInscricaoForaPrazo(seq).Transform<InscricaoForaPrazoViewModel>();            
        }

        public void SalvarPermissoes(InscricaoForaPrazoViewModel model)
        {
            InscricaoForaPrazoService.SalvarPermissoes(model.Transform<PermissaoInscricaoForaPrazoData>());
        }

        public void ExcluirPermissao(long seq)
        {
            InscricaoForaPrazoService.ExcluirPermissao(seq);
        }
    }
}