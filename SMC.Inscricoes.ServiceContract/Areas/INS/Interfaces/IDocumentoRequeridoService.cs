using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IDocumentoRequeridoService : ISMCService
    {
        #region Metodos de inscrição

            /// <summary>
        /// Buscar a lista de tipos de documento requeridos que são de upload obrigatório
        /// </summary>
        /// <param name="seqConfiguracaoEtapa">Sequencial de configuração da etapa</param>
        /// <returns>Sequencial dos documentos requeridos que são de upload obrigatório</returns>
        List<InscricaoDocumentoUploadData> BuscarTiposDocumentoRequeridoUploadObrigatorio(long seqConfiguracaoEtapa);

        /// <summary>
        /// Busca a lista de documentos permitidos para um determinado grupo
        /// </summary>        
        List<SMCDatasourceItem> BuscarTiposDocumentoGrupo(long seqGrupoDocumentos);

        /// <summary>
        /// Busca a lista de documentos permitidos para um determinado grupo
        /// </summary>        
        List<SMCDatasourceItem> BuscarDocumentosOpcionais(long seqConfiguracaoEtapa);

        #endregion

        #region CRUD Documentos Requeridos

        /// <summary>
        /// Busca os tipos de documento de uma configuração de etapa para listagem
        /// </summary>        
        SMCPagerData<DocumentoRequeridoListaData> BuscarDocumentosRequeridos(DocumentoRequeridoFiltroData filtro);

        /// <summary>
        /// Busca um documento requerido completo para edição/exibiç/ao
        /// </summary>        
        DocumentoRequeridoData BuscarDocumentoRequerido(long seqDocumentoRequerido);

        
        /// <summary>
        /// Salva um documento requerido realizando as validações
        /// </summary>
        long SalvarDocumentoRequerido(DocumentoRequeridoData documentoRequerido);         

        /// <summary>
        /// Exclui um documento requerido realizando as validações        
        void ExcluirDocumentoRequerido(long seqDocumentoRequerido);

        #endregion

        #region CRUD Grupo de Documentos Requeridos
        
        /// <summary>
        /// Retorna a lista de grupos de documentos de uma configuração de etapa para exibição
        /// </summary>
        List<GrupoDocumentoRequeridoListaData> BuscarGruposDocumentosRequiridos(GrupoDocumentoRequeridoFiltroData filtros);

        /// <summary>
        /// Busca uma lista de documentos requeridos para preencher selects
        /// </summary>       
        IEnumerable<SMCDatasourceItem> BuscarDocumentosRequeridosKeyValue(DocumentoRequeridoFiltroData filtro);        

        /// <summary>
        /// Retorna um grupo de documento requerido para exibição/edição
        /// </summary>
        GrupoDocumentoRequeridoData BuscarGrupoDocumentoRequerido(long seqGrupoDocumentoRequerido);

        /// <summary>
        /// Salva um grupo de documento requerido no banco
        /// </summary>        
        long SalvarGrupoDocumentoRequerido(GrupoDocumentoRequeridoData grupo);

        /// <summary>
        /// Exclui um grupo de documento requerido
        /// </summary>        
        void ExcluirGrupoDocumentoRequerido(long seqGrupoDocumentoRequerido);

        #endregion

        bool VerificaInscricaoComDocumentoCadastrado(long seqDocumentoRequerido);

        DateTime? BuscarDataLimiteEntregaDocumentoRequerido(long seqDocumentoRequerido);
    }
}
