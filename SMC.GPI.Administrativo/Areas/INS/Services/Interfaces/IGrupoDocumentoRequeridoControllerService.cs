using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public interface IGrupoDocumentoRequeridoControllerService
    {

        /// <summary>
        /// Buscar os grupos de documentação requeridas
        /// </summary>        
        List<GrupoDocumentosRequeridosListaViewModel> BuscarGruposDocumentacoesRequeridas(GrupoDocumentosRequeridosFiltroViewModel filtros);

        /// <summary>
        /// Buscar um grupo de documentação requerida
        /// </summary>
        /// <param name="seqGrupoDocumentacaoRequerida"></param>
        /// <returns></returns>
        GrupoDocumentosRequeridosViewModel BuscarGrupoDocumentacaoRequerida(long seqGrupoDocumentacaoRequerida);

        /// <summary>
        /// Salvar um grupo de documentação requerida
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        long SalvarGrupoDocumentacaoRequerida(GrupoDocumentosRequeridosViewModel modelo);

        /// <summary>
        /// Excluir um grupo de documentação requerida
        /// </summary>
        /// <param name="seqGrupoDocumentacaoRequerida"></param>
        void ExcluirGrupoDocumentacaoRequerida(long seqGrupoDocumentacaoRequerida);
        
        bool VerificaApenasInscricoesTeste(long seqConfiguracaoEtapa);
    }
}
