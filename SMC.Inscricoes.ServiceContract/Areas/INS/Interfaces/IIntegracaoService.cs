using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    /// <summary>
    /// Interface com os serviços de integração do GPI.
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IIntegracaoService : ISMCService
    {
        /// <summary>
        /// Busca os dados de todas as inscrições que estão convocadas.
        /// </summary>
        /// <param name="seqOfertas">Sequenciais das ofertas cujas inscrições devem ser pesquisadas.</param>
        /// <returns>Lista com os dados das inscrições.</returns>
        [OperationContract]
        List<PessoaIntegracaoData> BuscarDadosInscricoes(List<long> seqOfertas);

        [OperationContract]
        bool ExisteInscricoesConvocadas(List<long> seqOfertas);

        /// <summary>
        /// Busca os inscritos dos processos.
        /// </summary>
        [OperationContract]
        SMCPagerData<InscritoProcessoIntegracaoData> BuscarInscritosProcesso(InscritoProcessoIntegracaoFiltroData filtro);

        /// <summary>
        /// Busca um arquivo anexado pelo sequencial.
        /// </summary>
        /// <param name="seqArquivoAnexado">Sequencial do arquivo.</param>
        /// <returns>Um objeto SMCUploadFile com o arquivo.</returns>
        [OperationContract]
        SMCUploadFile BuscarArquivoAnexado(long seqArquivoAnexado);

        /// <summary>
        /// Marca inscrições oferta como exportadas.
        /// </summary>
        /// <param name="inscricaoOfertas">Lista de sequencial de inscrições ofertas.</param>
        [OperationContract]
        void AtualizarExportacaoInscricao(List<long> inscricaoOfertas);

        /// <summary>
        /// Altera a situação da inscrição para Candidato Desistente.
        /// </summary>
        /// <param name="seqInscricaoOferta">Sequencial da inscrição oferta.</param>
        [OperationContract]
        void AlterarSituacaoConvocadoParaDesistente(long seqInscricaoOferta);

        /// <summary>
        /// Buscar o historico da inscrição mais atual
        /// </summary>
        /// <param name="seqInscricaoOferta">Sequencial incrição oferta</param>
        /// <returns>Retorna os dados da incrição</returns>
        [OperationContract]
        InscricaoOfertaHistoricoSituacaoData BuscarHistoricosSituacaoAtual(long seqInscricaoOferta);
    }
}