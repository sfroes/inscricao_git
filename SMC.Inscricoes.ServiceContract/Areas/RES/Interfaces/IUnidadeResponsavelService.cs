using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.RES.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces
{
    /// <summary>
    /// Inteface para o serviço que chama o DomainService de Inscrito
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IUnidadeResponsavelService : ISMCService
    {

        /// <summary>
        /// Retorna todos os tipos de processo com apenas seq  e descricao preenchidos
        /// Serviço utilizado pelo Novo SGA nos cadastros de Entidade (Exposto WCF)
        /// </summary> 
        /// <returns>Lista de Unidades responsáveis</returns>
        [OperationContract]
        List<SMCDatasourceItem> BuscarUnidadesResponsaveisKeyValue();

        List<SMCDatasourceItem> BuscarUnidadesResponsaveisSelect(long? seqUnidadeResponsavel);

        /// <summary>
        /// Retorna a lista de unidades responsáveis para exibição em listagem
        /// </summary>        
        SMCPagerData<UnidadeResponsavelListaData> BuscarUnidadesResponsaveis(UnidadeResponsavelFiltroData filtro);

        /// <summary>
        /// Retorna uma unidade responsável pelo sequencial
        /// </summary>        
        UnidadeResponsavelData BuscarUnidadeResponsavel(long seqUnidadeResponsavel);

        /// <summary>
        /// Retorna o nome e a sigla de uma unidades responsável
        /// </summary>
        UnidadeResponsavelData BuscarUnidadeResponsavelSimplificado(long seqUnidadeResponsavel);

        /// <summary>
        /// Salva a unidade responsável e retorna o sequencial da mesma
        /// </summary>                
        long SalvarUnidadeResponsavel(UnidadeResponsavelData unidadeResponsavel);

        /// <summary>
        /// Salva a unidade responsável e retorna o sequencial da mesma
        /// </summary>                
        void ExcluirUnidadeResponsavel(long seqUnidadeResponsavel);

        /// <summary>
        /// Busca os tipos de formulário associados à unidade responsável
        /// </summary>
        SMCPagerData<UnidadeResponsavelTipoFormularioListaData> BuscarUnidadeResponsavelTiposFormularios(long seqUnidadeResponsavel);

        /// <summary>
        /// Busca o tipo de formulário associado à unidade responsável
        /// </summary>
        UnidadeResponsavelTipoFormularioData BuscarUnidadeResponsavelTipoFormulario(long seqFormulario);

        /// <summary>
        /// Salva uma associação entre uma unidade responsável e um tipo de formulário
        /// </summary>        
        long SalvarTipoFormularioUnidadeResponsavel(UnidadeResponsavelTipoFormularioData tipoFormulario);

        /// <summary>
        /// Exclui uma associação entre uma unidade responsável e um tipo de formulário
        /// </summary>
        void ExcluirUnidadeResponsavelTipoFormulario(long seqConfiguracaoTipoFormulario);

        /// <summary>
        /// Busca os tipos de processo associados à unidade responsável
        /// </summary>
        SMCPagerData<UnidadeResponsavelTipoProcessoListaData> BuscarUnidadeResponsavelTiposProcessos(long seqUnidadeResponsavel);

        /// <summary>
        /// Busca o tipo de processo associado à unidade responsável
        /// </summary>
        UnidadeResponsavelTipoProcessoData BuscarUnidadeResponsavelTipoProcesso(long seqUnidadeResponsavelTipoProcesso);

        /// <summary>
        /// Salva uma associação entre uma unidade responsável e um tipo de processo
        /// </summary> 
        long SalvarUnidadeResponsavelTipoProcessoTipoHierarquiaOferta(UnidadeResponsavelTipoProcessoData unidadeResponsavelTipoProcessoData);

        /// <summary>
        /// Exclui uma associação entre uma unidade responsável e um tipo de processo
        /// </summary>
        void ExcluirUnidadeResponsavelTipoProcessoTipoHierarquiaOferta(long seqConfiguracaoTipoProcessoTipoHierarquiaOferta);

        List<SMCDatasourceItem> BuscarTiposProcessoAssociados(long? seqUnidadeResponsavel);

        List<SMCDatasourceItem> BuscarTiposHieraquiaOfertaAssociados(long seqUnidadeResponsavel, long seqTipoProcesso);

        /// <summary>
        /// Busca a lista de tipos de formulário associados a unidade responsável
        /// </summary>
        List<SMCDatasourceItem> BuscarTiposFormularioKeyValue(long seqUnidadeResponsavel);
        long? BuscarSeqUnidadeResponsavelSgf(long seqUnidadeGestora);

        /// <summary>
        /// Buscar os sistemas origem com a sigla "GPI"
        /// </summary>
        List<SMCDatasourceItem<string>> BuscarSistemaOrigemGADSelect(string sigla);

        /// <summary>
        /// Busca Token de css Alternativo
        /// </summary>
        /// <param name="seqTipoProcesso"></param>
        /// <returns></returns>
        string BuscaTokenCssAlternativo(long seqUnidadeResponsavelTipoProcessoIdVisual);

        /// <summary>
        /// Busca todos todos os Layouts de mensagem de e-mail do Grupo de aplicação do SAS, cuja Sigla é "GPI", em ordem alfabética.
        /// </summary>
        /// <returns></returns>
        List<SMCDatasourceItem> BuscarLayoutNotificacaoEmailPorSiglaGrupoAplicacao();

        /// <summary>
        /// Buscar todas as identidades visuais por unidade responsavel
        /// </summary>
        /// <param name="seqUnidadeResponsavel"></param>
        /// <returns></returns>
        List<SMCDatasourceItem> BuscarIdentidadesVisuais(long seqUnidadeResponsavel, long seqTipoProcesso);

    }
}
