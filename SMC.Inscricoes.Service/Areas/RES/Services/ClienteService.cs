using SMC.Framework;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common.Areas.RES;
using SMC.Inscricoes.Domain.Areas.RES;
using SMC.Inscricoes.Domain.Areas.RES.DomainServices;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using SMC.Inscricoes.Domain.Areas.RES.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.RES.Data;
using System.Collections.Generic;

namespace SMC.Inscricoes.Service.Areas.RES.Services
{
    public class ClienteService : SMCServiceBase, IClienteService
    {

        #region DomainService

        private ClienteDomainService ClienteDomainService
        {
            get { return this.Create<ClienteDomainService>(); }
        }

        #endregion

        public SMCPagerData<ClienteListaData> BuscarClientes(ClienteFiltroData filtro)
        {
            return this.ClienteDomainService
                .SearchBySpecification<ClienteFilterSpecification, ClienteFiltroData, ClienteListaData, Cliente>(filtro);
        }

        public IEnumerable<SMCDatasourceItem> BuscarClientesKeyValue()
        {
            return ClienteDomainService.BuscarClientesKeyValue();
        }

        public ClienteData BuscarClientes(long seqCliente)
        {
            return this.ClienteDomainService.SearchByKey<Cliente, ClienteData>(seqCliente, IncludesCliente.Enderecos |
                                                                                           IncludesCliente.EnderecosEletronicos |
                                                                                           IncludesCliente.Telefones);
        }

        public long SalvarCliente(ClienteData clienteData)
        {
            clienteData.Cnpj = clienteData.Cnpj.SMCRemoveNonDigits();
            foreach (var endereco in clienteData.Enderecos)
            {
                endereco.Cep = endereco.Cep.SMCRemoveNonDigits();
            }
            return this.ClienteDomainService.SaveEntity(clienteData);
        }

        public void RemoverCliente(long seqCliente)
        {
            this.ClienteDomainService.DeleteEntity(seqCliente);
        }
    }
}
