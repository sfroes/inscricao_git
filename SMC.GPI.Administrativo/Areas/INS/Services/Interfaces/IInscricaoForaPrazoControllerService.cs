using SMC.Framework.Model;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMC.Framework.UI.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public interface IInscricaoForaPrazoControllerService
    {
        SMCPagerModel<InscricaoForaPrazoListaViewModel> BuscarInscricoesForaPrazo(InscricaoForaPrazoFiltroViewModel filtro);

        InscricaoForaPrazoViewModel BuscarInscricaoForaPrazo(long seq);

        void SalvarPermissoes(InscricaoForaPrazoViewModel model);

        void ExcluirPermissao(long seq);        
    }
}