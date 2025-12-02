using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.RES.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces
{
    /// <summary>
    /// Inteface para o serviço que chama o DomainService de Inscrito
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IClienteService : ISMCService
    {
        /// <summary>
        /// Retorna todos os clientes que atendem a um determinado filtro.
        /// </summary>
        SMCPagerData<ClienteListaData> BuscarClientes(ClienteFiltroData filtro);

        /// <summary>
        /// Retorna todos os clientes com apenas seq  e descrição (nome) preenchidos
        /// </summary>     
        IEnumerable<SMCDatasourceItem> BuscarClientesKeyValue();

        /// <summary>
        /// Busca um cliente pelo seu Seq.
        /// </summary>
        ClienteData BuscarClientes(long seqCliente);

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        long SalvarCliente(ClienteData clienteData);

        /// <summary>
        /// Remove um cliente.
        /// </summary>
        void RemoverCliente(long seqCliente);
    }
}
