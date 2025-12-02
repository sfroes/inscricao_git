using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public interface ITipoDocumentoControllerService
    {
        /// <summary>
        /// Buscar os Tipos de Documentos associados
        /// </summary>
        /// <returns>Lista de Tipos de Processo</returns>
        SMCPagerModel<TipoDocumentoListaViewModel> BuscarTiposDocumento(TipoDocumentoFiltroViewModel filtros);
        
        /// <summary>
        /// Buscar o Tipo de Documento desejado
        /// </summary>        
        TipoDocumentoViewModel BuscarTipoDocumento(long seqTipoDocumento);
     
        /// <summary>
        /// Salva um Tipo de Documento
        /// </summary>        
        long SalvarTipoDocumento(TipoDocumentoViewModel modelo);

        /// <summary>
        /// Exclui um Tipo de Documento
        /// </summary>
        void ExcluirTipoDocumento(long seqTipoDocumento);

        /// <summary>
        /// Busca os tipos de documento para select
        /// </summary>        
        List<SMCDatasourceItem> BuscarTiposDocumentoSelect();

        /// <summary>
        /// Busca os tipos de documento não utilizados para select
        /// </summary>
        /// <returns></returns>
        List<SMCDatasourceItem> BuscarTiposDocumentoNaoUtilizadosSelect();

    }
}
