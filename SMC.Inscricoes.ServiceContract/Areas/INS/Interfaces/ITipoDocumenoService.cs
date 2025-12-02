using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    /// <summary>
    /// Inteface para o serviço que chama o DomainService de Inscrito
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface ITipoDocumentoService : ISMCService
    {
        /// <summary>
        /// Buscar os tipos de documentos configurados
        /// </summary>        
        SMCPagerData<TipoDocumentoData> BuscarTiposDocumento(TipoDocumentoFiltroData filtro);

        List<SMCDatasourceItem> BuscarTiposDocumentoKeyValue();

        /// <summary>
        /// Busca a configuração pra um tipo de documento pelo sequencial
        /// </summary>        
        TipoDocumentoData BuscarTipoDocumento(long seqTipoDocumento);

        /// <summary>
        /// Salva a configuração de um tipo de documento
        /// </summary>        
        long SalvarTipoDocumento(TipoDocumentoData tipoDocumento);

        /// <summary>
        /// Exclui a configuração para um tipo de documento
        /// </summary>        
        void ExcluirTipoDocumento(long seqTipoDocumento);
    }
}
