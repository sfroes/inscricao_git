using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.Framework.Mapper;
using SMC.Framework.Extensions;
using SMC.GPI.Administrativo.Areas.RES.Models;
using System;
using System.Collections.Generic;
using SMC.Inscricoes.ServiceContract.Areas.RES.Data;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;
using SMC.Framework.UI.Mvc.Html;

namespace SMC.GPI.Administrativo.Areas.RES.Services
{
    public class ClienteControllerService : SMCControllerServiceBase
    {
        #region Services

        private IClienteService ClienteService 
        {
            get { return this.Create<IClienteService>(); }
        }

        #endregion

        public SMCPagerModel<ClienteListaViewModel> BuscarClientes(ClienteFiltroViewModel filtros)
        {
            ClienteFiltroData filtroData = SMCMapperHelper.Create<ClienteFiltroData>(filtros);
            var data = ClienteService.BuscarClientes(filtroData);
            SMCPagerData<ClienteListaViewModel> model = SMCMapperHelper.Create<SMCPagerData<ClienteListaViewModel>>(data);
            return new SMCPagerModel<ClienteListaViewModel>(model, filtros.PageSettings, filtros);
        }

        public ClienteViewModel BuscarCliente(long seqCliente)
        {
            ClienteData data = ClienteService.BuscarClientes(seqCliente);
            return SMCMapperHelper.Create<ClienteViewModel>(data);
        }

        public long SalvarCliente(ClienteViewModel modelo)
        {
            return ClienteService.SalvarCliente(SMCMapperHelper.Create<ClienteData>(modelo));
        }

        public void ExcluirCliente(long seqCliente)
        {
            ClienteService.RemoverCliente(seqCliente);
        }

        public List<SMCSelectListItem> BuscarClientesSelect()
        {
            return ClienteService.BuscarClientesKeyValue().TransformList<SMCSelectListItem>();
        }
    }
}