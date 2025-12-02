using MC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Financeiro.ServiceContract.BLT.Data;
using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.Inscricao;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    /// <summary>
    /// Inteface para o serviço que chama o DomainService de Inscrição
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IInscricaoService : ISMCService
    {
        #region Regras de Negocio

        /// <summary>
        /// Verifica regras de negócio para iniciar ou continuar uma nova inscrição
        /// </summary>
        /// <param name="filtro">Filtros para validação</param>
        /// <returns>TRUE caso possa iniciar ou continuar a inscrição, FALSE caso contrário</returns>
        bool VerificarPermissaoIniciarContinuarInscricao(IniciarContinuarInscricaoFiltroData filtro);

        /// <summary>
        /// Valida a regra pra registro de documentação entregue
        /// </summary>
        bool VerificarSituacaoRegistrarEntregaDocumentos(long seqInscricao);

        #endregion Regras de Negocio

        #region Buscar Informações de Inscricoes

        /// <summary>
        /// Busca os dados da inscrição resumidos para exibição
        /// </summary>
        /// <param name="seqInscricao"></param>
        DadosInscricaoData BuscarInscricaoResumida(long seqInscricao, bool exibirDescricaoOfertaPorNome = true);

        /// <summary>
        /// Busca as inscrições em processos para um determinado inscrito
        /// </summary>
        /// <param name="filtro">Filtro para pesquisa</param>
        /// <returns>Lista de inscrições por processo</returns>
        SMCPagerData<InscricoesProcessoData> BuscarInscricoesProcesso(InscricaoFiltroData filtro);

        /// <summary>
        /// Busca as situação das isncrições de um processo sumarizadas
        /// </summary>
        /// <param name="filtro">Filtros para pesquisa</param>
        /// <returns>Lista de situações das inscrições de um processo sumarizadas</returns>
        SMCPagerData<SituacaoInscricaoProcessoData> BuscarSituacaoInscricaoProcesso(SituacaoInscricaoProcessoFiltroData filtro);
        /// <summary>
        /// Busca a justificativa da situacao da inscricao
        /// </summary>
        /// <param name="filtro">Filtros para pesquisa</param>
        /// <returns>Lista de situações das inscrições de um processo sumarizadas</returns>
        JustificativaSituacaoInscricaoData BuscarJustificativaSituacao(long seqInscricao);

        /// <summary>
        /// Busca as situação das isncrições de um processo sumarizadas e com os dados do inscrito
        /// </summary>
        /// <param name="filtro">Filtros para pesquisa</param>
        SMCPagerData<SituacaoInscricaoInscritoProcessoData> BuscarSituacaoInscricaoInscritoProcesso(SituacaoInscricaoProcessoFiltroData filtro);

        /// <summary>
        /// Busca as ofertas de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Lista de ofertas de uma inscrição</returns>
        List<InscricaoOfertaData> BuscarInscricaoOfertas(long seqInscricao);

        InscricaoOfertaData BuscarInscricaoOferta(long seqInscricaoOferta);

        /// <summary>
        /// Busca o SeqInscricao de uma InscricaoOferta
        /// </summary>
        /// <param name="seqInscricaoOferta">Seq da InscricaoOferta</param>
        /// <returns>SeqInscricao da InscricaoOferta</returns>
        [OperationContract]
        long BuscarSeqInscricaoPorSeqInscricaoOferta(long seqInscricaoOferta);

        /// <summary>
        /// Busca a lista de codigos de autorização de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Lista de códigos de autorização de uma inscrição</returns>
        List<InscricaoCodigoAutorizacaoData> BuscarInscricaoCodigoAutorizacao(long seqInscricao);

        /// <summary>
        /// Busca o sequencial do dado formulário e uma inscrição
        /// </summary>
        /// <param name="filtroData">Filtro para pesquisa</param>
        /// <returns>Sequencial do dado formulário de uma inscrição</returns>
        long BuscarSeqDadoFormulario(InscricaoDadoFormularioFiltroData filtroData);

        /// <summary>
        /// Buscar os documentos liberados para upload de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="situacoesEntregaDocumentoDiferente">Situações do documento difetente</param>
        /// <returns>Documentos com os dados informados</returns>
        List<InscricaoDocumentoData> BuscarListaInscricaoDocumentoOpcionaisUpload(long seqInscricao, SituacaoEntregaDocumento[] situacoesEntregaDocumentoDiferente);

        /// <summary>
        /// Busca o sequencial do arquivo do comprovante de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Sequencial do arquivo do comprovante</returns>
        long? BuscarSeqArquivoComprovante(long seqInscricao);

        /// <summary>
        /// Busca o formulário já preenchido pelo inscrito.
        /// </summary>
        InscricaoDadoFormularioFiltroData BuscarFormularioInscrito(long seqInscricao, long seqConfiguracaoEtapaPagina);

        bool VerificaApenasInscricoesTeste(long[] seqInscricoes);

        List<string> BuscarNomesInscritos(List<long> seqInscricoes);

        /// <summary>
        /// Busca os inscrições dados formulários para uma inscrição
        /// </summary>
        List<InscricaoDadoFormularioData> BuscarInscricaoDadoFormulario(long seqInscricao);

        ObservacaoInscricaoData BuscarObservacaoInscricao(long seqInscricao);

        /// <summary>
        /// Verifica se já foi pago algum boleto pago para a inscricao
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>retorna se existe ou não um boleto pago para a inscrição</returns>
        bool PossuiBoletoPago(long seqInscricao);

        /// <summary>
        /// Verifica se existe algum boleto para a inscricao
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>retorna se existe ou não um boleto para a inscrição</returns>
        bool PossuiBoleto(long seqInscricao);

        bool PossuiOfertaVigente(long seqInscricao);

        #endregion Buscar Informações de Inscricoes

        #region Buscar Informacoes Pagina

        /// <summary>
        /// Busca as informações da primeira página da etapa de inscrição
        /// </summary>
        /// <param name="seqConfiguracaoEtapa">Sequencial da configuração de etapa</param>
        /// <returns>Informações da primeira página do processo de inscrição</returns>
        ConfiguracaoEtapaPaginaData BuscarConfiguracaoEtapaPrimeiraPagina(long seqConfiguracaoEtapa);

        /// <summary>
        /// Busca as informações da uma página
        /// </summary>
        /// <param name="seqConfiguracaoEtapa">Sequencial da configuração de etapa</param>
        /// <param name="tokenPagina">Token da página a ser recuperada</param>
        /// <returns>Informações de uma página</returns>
        ConfiguracaoEtapaPaginaData BuscarConfiguracaoEtapaPagina(long seqConfiguracaoEtapa, string tokenPagina);

        /// <summary>
        /// Busca as informações da ultima página acessada por uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Informações da ultima página acessada por uma inscrição</returns>
        ContinuarInscricaoData BuscarUltimaPaginaInscricao(long seqInscricao);

        /// <summary>
        /// Busca as informações de uma página e suas seções
        /// </summary>
        /// <param name="filtro">filtros para a página</param>
        /// <returns>Informações da página</returns>
        PaginaData BuscarPagina(PaginaFiltroData filtro);

        string BuscarUrlCss(long seqIncricao);

        #endregion Buscar Informacoes Pagina

        #region Incluir/Alterar Inscrição

        /// <summary>
        /// Inclui uma inscrição
        /// </summary>
        /// <param name="inscricao">Inscrição a ser incluida</param>
        /// <returns>Sequencial da inscrição criada</returns>
        long IncluirInscricao(InscricaoInicialData inscricao);

        /// <summary>
        /// Inclui um historico de acesso a página
        /// </summary>
        /// <param name="historico">Historico a ser incluido</param>
        void IncluirInscricaoHistoricoPagina(InscricaoHistoricoPaginaData historico);

        /// <summary>
        /// Salva as ofertas de uma inscrição
        /// </summary>
        /// <param name="ofertas">Ofertas a serem salvas</param>
        void SalvarInscricaoOferta(List<InscricaoOfertaData> ofertas, short? numeroOpcoesDesejadas, List<InscricaoTaxaOfertaData> taxas = null);
        
        /// <summary>
        /// Salva as ofertas de uma inscrição
        /// </summary>
        /// <param name="ofertas">Ofertas a serem salvas</param>
        void SalvarInscricaoOfertaAngular(List<InscricaoOfertaData> ofertas, short? numeroOpcoesDesejadas,long seqGrupoOferta, List<InscricaoTaxaOfertaData> taxas = null);

        /// <summary>
        /// Salva os códigos de autorização de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="codigos">Códigos a serem salvos</param>
        void SalvarInscricaoCodigosAutorizacao(long seqInscricao, List<InscricaoCodigoAutorizacaoData> codigos);

        /// <summary>
        /// Salva os documentos realizados upload na inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="documentos">Lista de documentos para salvar</param>
        void SalvarInscricaoDocumentoUpload(long seqInscricao, List<InscricaoDocumentoData> documentos);

        /// <summary>
        /// Finaliza uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição a ser finalizada</param>
        /// <param name="arquivoComprovante">Conteudo do arquivo de comprovante</param>
        /// <param name="aceiteConversaoPDF">Aceite do termo conversão do PDF</param>
        void FinalizarInscricao(long seqInscricao, bool aceiteConversaoPDF, bool ConcentimentoLGPD, byte?[] arquivoComprovante);

        /// <summary>
        /// Verifica se uma inscrição está apenas na primeira página (inicial)
        /// </summary>
        bool VerificaInscricaoApenasPrimeiraPagina(long seqInscricao, long seqConfiguracaoEtapaPagina);

        /// <summary>
        /// Exclui uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição a ser excluída</param>
        void ExcluirInscricao(long seqInscricao);

        void AlterarFormularioInscricao(InscricaoDadoFormularioData dadoFormularioData);

        void AlterarAptoBolsa( long seqInscricao, bool apto);

        #endregion Incluir/Alterar Inscrição

        #region Inscrição/ Documentos

        /// <summary>
        /// Busca a lista de documentos da inscrição
        /// </summary>
        InscricaoDocumentosUploadData BuscarDocumentosUploadInscricao(long seqInscricao);

        /// <summary>
        /// Retorna a lista de documentos, com situação e informações de entrega para uma inscrição informada
        /// </summary>
        SumarioDocumentosEntreguesData BuscarSumarioDocumentosEntregue(long seqInscricao);

        /// <summary>
        /// Retorna a lista de documentos, com situação e informações de entrega para uma inscrição informada
        /// </summary>
        List<InscricaoDocumentoData> BuscarDocumentosInscricao(long seqInscricao);

        /// <summary>
        /// Salva um documento para uma determinada inscrição
        /// </summary>
        InscricaoDocumentoData SalvarDocumentoInscricao(InscricaoDocumentoData inscricaoDocumento);

        /// <summary>
        /// Salva todos os documentos entregues
        /// </summary>
        /// <param name="SumarioDocumentosEntreguesViewModel"></param>
        /// <returns>SeqProcesso</returns>
        long SalvarSumarioDocumentosEntreguesInscricao(SumarioDocumentosEntreguesData documentosEntregues);

        bool ValidarSituacaoAtualCandidatoOfertasConfirmadas(SumarioDocumentosEntreguesData documentosEntregues);

        bool ValidarSituacaoAtualCandidatoOfertasDeferidas(SumarioDocumentosEntreguesData documentosEntregues);

        /// <summary>
        /// Duplica o documento em questão
        /// </summary>
        void DuplicarEntregaDocumento(long seqInscricaoDocumento);

        /// <summary>
        /// Exclui um documento para uma inscrição
        /// A operação só deve ser possível se o documento em questão permirtir a entrega de mais de um documento
        /// e se houver outro documento do mesmo tipo já entregue, acs contrário deve lançar uma exceção com
        /// a mensagem
        /// "Para o documento {0} é necessário existir ao menos um registroPara o documento <descrição do tipo de documento excluído>" é necessário existir ao menos um registro"
        /// </summary>
        /// <param name="seqInscricaoDocumento"></param>
        void ExcluirInscricaoDocumento(long seqInscricaoDocumento);

        /// <summary>
        /// Buscar o cabeçalho da tela de nova entrega de documentação
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Dados do cabeçalho da tela de nova entrega da documentação</returns>
        NovaEntregaDocumentacaoCabecalhoData BuscarCabecalhoNovaEntregaDocumentacao(long seqInscricao);

        /// <summary>
        /// Buscar dos documentos para nova entrega da documentação
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Dados dos documentos para nova entrega da documentação</returns>
        NovaEntregaDocumentacaoData BuscarDocumentosNovaEntregaDocumentacao(long seqInscricao);

        /// <summary>
        /// Salvar as novas entregas de documentação de uma inscrição
        /// </summary>
        /// <param name="novaEntregaDocumentacao">Documentos enviados pelo usuário</param>
        /// <returns>Sequencial da inscrição</returns>
        long SalvarNovaEntregaDocumentacao(NovaEntregaDocumentacaoData novaEntregaDocumentacao);

        #endregion Inscrição/ Documentos

        #region Buscar / Persistir Dado Formulario

        /// <summary>
        /// Busca um dado formulário
        /// </summary>
        /// <param name="seqDadoFormulario">Sequencial do dado formulário a ser recuperado</param>
        /// <returns></returns>
        InscricaoDadoFormularioData BuscarDadoFormulario(long seqDadoFormulario);

        InscricaoHistoricoSituacaoData BuscarHistoricoSituacao(long seqHistoricoSituacao);

        /// <summary>
        /// Salva os dados do formulário
        /// </summary>
        /// <param name="dados"></param>
        long SalvarFormularioInscricao(InscricaoDadoFormularioData dados);

        /// <summary>
        /// Salva os dados do formulário impacto
        /// </summary>
        /// <param name="dados">Dados do formulário</param>
        long SalvarFormularioImpacto(InscricaoDadoFormularioData dados);

        #endregion Buscar / Persistir Dado Formulario

        #region Alteração Situação

        bool VerificaSituacaoInscricoesOfertaNaSituacao(List<long> inscricoes, string tokenSituacao);

        /// <summary>
        /// Retorna uma lista de isncrições contendo Seq e Nome do Inscrito de acordo
        /// com os sequenciais informados
        /// </summary>
        List<DetalheAlteracaoSituacaoData> BuscarInscricoesPorSequencial(InscricaoSelecionadaData inscricoes);

        /// <summary>
        /// Altera a situação das inscrições informadas
        /// Todas as situações informadas devem estar na mesma situação atual e no mesmo processo e etapa
        /// </summary>
        /// <param name="seqSituacao">Sequencial do tipo Processo Situação para destino das inscrições</param>
        /// <param name="seqInscricoes">Lista de sequencial das inscrições a serem alteradas</param>
        /// <param name="justificativa">justificativa para a mudança de situação</param>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void AlterarSituacaoInscricoes(AlteracaoSituacaoData data);

        /// <summary>
        /// Altera a justificativa e o motivo de uma situação
        /// </summary>
        /// <param name="seqHistoricoSituacao">Situação a ser alterada</param>
        /// <param name="justificativa">Justificativa informada</param>
        /// <param name="seqMotivo">Motivo informado</param>
        void AlterarMotivoEJustificativaSituacao(long seqHistoricoSituacao, string justificativa, long? seqMotivo);

        #endregion Alteração Situação

        #region Histório de Situação

        List<InscricaoHistoricoSituacaoData> BuscarSituacoesInscricao(long seqInscricao);

        List<InscricaoHistoricoSituacaoData> BuscarSituacoesInscricaoParaValidacaoDeTokens(long seqInscricao);

        List<InscricaoOfertaHistoricoSituacaoData> BuscarSituacoesInscricaoOferta(long seqInscricao);

        InscricaoOfertaHistoricoSituacaoData BuscarSituacaoInscricaoOferta(long seqInscricaoHistoricoSituacao);

        #endregion Histório de Situação

        #region Inscrição / Taxa

        /// <summary>
        /// Busca as informações de taxa pra uma ofeta
        /// </summary>
        IEnumerable<InscricaoTaxaOfertaData> BuscarTaxaInscricaoOfertaVigente(long seqOferta, long seqInscricao);

        /// <summary>
        /// Busca as informações de taxa pra uma inscricao
        /// </summary>
        IEnumerable<InscricaoTaxaOfertaData> BuscarTaxasOfertaInscricao(long seqInscricao);

        /// <summary>
        /// Busca as informações de taxa pra uma inscricao confirmacao
        /// </summary>
        IEnumerable<InscricaoTaxaOfertaData> BuscarTaxasOfertaInscricaoConfirmacao(long seqInscricao);

        ///// <summary>
        ///// Salva as taxas para uma inscrição
        ///// </summary>
        //void SalvarInscricaoTaxasOferta(long seqInscricao, List<InscricaoTaxaOfertaData> inscricaoTaxasOferta);

        /// <summary>
        /// Verifica se uma determinada inscrição pode gerar boleto.
        /// </summary>
        string VerificaPermissaoGerarBoletoInscricao(long seqInscricao);

        /// <summary>
        /// Verifica se uma determinada inscrição pode emitir comprovante.
        /// </summary>
        string VerificarPermissaoEmitirComprovante(long seqInscricao);

        /// <summary>
        /// Busca o boleto para uma determinada inscrição
        /// </summary>
        BoletoData GerarBoletoInscricao(long seqInscricao);

        /// <summary>
        /// Busca os títulos existentes para um inscrição
        /// </summary>
        List<TituloInscricaoData> BuscarTitulosInscricao(long seqInscricao);

        /// <summary>
        /// Verifica se existe taxa para uma determinada inscricao
        /// </summary>
        bool VerificarExistenciaTaxaInscricao(long seqInscricao);

        /// <summary>
        /// Verifica se existe taxa foi paga
        /// </summary>
        bool? VerificarPagamentoTaxaInscricao(long seqInscricao);

        #endregion Inscrição / Taxa

        #region Verificações de informações

        bool VerificaDocumentoObrigatorio(long seqInscricaoDocumento, long seqInscricao);

        bool VerificaDocumentacaoEntregue(long seqInscricao);

        bool VerificaFormularioEmUso(long seqConfiguracaoEtapaPaginaIdioma);

        bool VerificaBoletoInscricaoAlteracaoTaxa(long seqInscricao, List<InscricaoTaxaData> taxas);

        #endregion Verificações de informações

        #region Liberação da Alteração de Inscrição

        void LiberarAlteracaoInscricao(long seqInscricao);

        /// <summary>
        /// Validar RN_INS_141 Liberação da alteração de inscrição
        /// Regras 1 e 2
        /// </summary>
        /// <param name="seqInscricao"></param>
        void ValidarLiberacaoAlteracaoInscricao(long seqInscricao);

        #endregion

        void AlterarObservacaoInscricao(ObservacaoInscricaoData observacaoInscricaoData);

        /// <summary>
        /// Busca os dados do processo de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da Inscrição</param>
        /// <returns>Dados do processo</returns>
        [OperationContract]
        DadosProcessoInscricaoData BuscarDadosProcessoInscricao(long seqInscricao);


        /// <summary>
        /// Recupera os dados de uma inscrição necessários para emissão do comprovante
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Dados da inscrição</returns>
        [OperationContract]
        DadosComprovanteInscricaoData BuscarDadosComprovanteInscricao(long seqInscricao);

        /// <summary>
        /// Atualiza o arquivo do comprovante
        /// </summary>
        /// <param name="dadosComprovanteInscricao">Dados do comprovante</param>
        [OperationContract]
        void AlterarComprovanteInscricao(DadosComprovanteInscricaoData dadosComprovanteInscricao);

        /// <summary>
        /// Validar se um boleto já foi pago e se o valor do mesmo foi alterado pela seleção de novas ofertas
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="seqOferta">Sequencial da oferta opção 1</param>
        void ValidarBoletoPagoAlteracaoValor(long seqInscricao, long seqOferta);

        /// <summary>
        /// Valida se o inscrito está apto a receber a bolsta ex-aluno
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="seqOferta">Sequencial da oferta a ser validada</param>
        /// <returns>True caso esteja apto</returns>
        bool ValidarAptoBolsaNovoTitulo(long seqInscricao, long seqOferta);

        /// <summary>
        /// Buscar dados Formulario Seminario
        /// </summary>
        /// <param name="seqAcao">Sequencial da Ação</param>
        /// <param name="seqProcesso">Sequencial Processo</param>
        /// <param name="seqIncricao">Sequencial da Inscrição</param>
        DadosFormularioSeminarioData DadosFormularioSeminarioSGF(long seqAcao, long seqProcesso, long seqIncricao);

        /// <summary>
        /// Valida se por ventura o formulario é consistente para proseguir
        /// Incluir no botão “Próximo” a regra “RN_INS_187 - Consistência seminários de iniciação científica”:
        /// Se o tipo de processo em questão estiver configurado para integrar com o GPC 
        /// e existir o campo PROJETO no formulário, verificar se existe alguma outra 
        /// inscrição para o processo em questão, com a situação atual da inscrição 
        /// igual a INSCRICAO_FINALIZADA ou INSCRICAO_CONFIRMADA, e com o mesmo projeto 
        /// selecionado.Em caso afirmativo, abortar a operação e emitir a mensagem de erro:
        ///"Já existe uma inscrição para o projeto 'nome do projeto'".
        /// </summary>
        /// <param name="seqProcesso">Sequencial do proceesso no GPI</param>
        /// <param name="seqInscricao">Sequencial da Inscrição</param>
        /// <param name="descricaoProjeto">Descrição do Projeto GPC</param>
        void ValidarFormularioSeminario(long seqProcesso, long seqInscricao, string descricaoProjeto);

        /// <summary>
        /// Buscar situação atual da inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscricao</param>
        /// <returns>Token de segurança</returns>
        string BuscarSituacaoAtualInscricao(long seqInscricao);

        #region[GED]
        void IniciarPortfolio(PortfolioData dadosPortifolio);
        #endregion

        string BuscarNomeInscritosSeqInscricao(long seqInscricao);

        /// <summary>
        /// Buscar ingressos
        /// </summary>
        /// <param name="seqInscricao">Sequencial da Inscricao</param>
        /// <returns>Retorna os ingresso da Inscricao</returns>
        IngressoData BuscarIngressos(long seqInscricao);

        /// <summary>
        /// Busca os campos dados do formulario 
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="tokensCampo">Filtro de tokens: Exemplo: "ESCOLA", "SERIE"...</param>
        /// <returns>Retorna os campos dos dados formulario</returns>
        List<InscricaoDadoFormularioCampoData> BuscarCamposDadoFormularioPorSeqInscricao(long seqInscricao, List<string> tokensCampo);

        bool PossuiDocumentoRequerido(long seqConfiguracaoEtapa);

        string BuscarDescricaoProcessoInscricao(long seqInscricao);

        byte[] EmitirDocumentacao(long seqInscricao, long seqTipoDocumento);
    }
}