using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public interface IDocumentoRequeridoControllerService
    {


        /// <summary>
        /// Buscar as documentações requeridas para uma etapa
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        SMCPagerModel<DocumentoRequeridoListaViewModel> BuscarDocumentosRequeridos(DocumentoRequeridoFiltroViewModel filtros);

        /// <summary>
        /// Busca uma documentação requerida para uma etapa 
        /// </summary>
        /// <param name="seqDocumentacaoRequerida"></param>
        /// <returns></returns>
        DocumentoRequeridoViewModel BuscarDocumentoRequerido(long seqDocumentoRequerido);

        /// <summary>
        /// Busca uma lista de documento requeridos para preencher select de acordo com os filtros informados
        /// </summary>        
        List<SMCDatasourceItem> BuscarDocumentosRequeridosSelect(long seqConfiguracaoEtapa, bool? obrigatorio = null, bool? uploadObrigatorio = null); 

        /// <summary>
        /// Salvar uma documentação requerida para uma etapa
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        long SalvarDocumentoRequerido(DocumentoRequeridoViewModel modelo);

        /// <summary>
        /// Excluir uma documentação requerida para uma etapa
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        void ExcluirDocumentoRequerido(long seqDocumentoRequerido);

        bool VerificaApenasInscricoesTeste(long seqConfiguracaoEtapa);

        bool VerificaInscricaoComDocumentoCadastrado(long seqDocumentoRequerido);
    }
}
