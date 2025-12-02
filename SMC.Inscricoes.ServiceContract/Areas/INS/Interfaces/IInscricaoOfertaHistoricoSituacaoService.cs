using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.SEL.Data;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    /// <summary>
    /// Inteface para o serviço que chama o DomainService de Inscrição
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IInscricaoOfertaHistoricoSituacaoService : ISMCService
    {
        /// <summary>
        /// Buscar o historico da inscrição mais atual
        /// </summary>
        /// <param name="seqInscricaoOferta">Sequencial incrição oferta</param>
        /// <returns>Retorna os dados da incrição</returns>
        [OperationContract]
        InscricaoOfertaHistoricoSituacaoData BuscarHistoricosSituacaoAtual(long seqInscricaoOferta);

        /// <summary>
        /// Altera a situação de uma inscrição oferta
        /// </summary>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void AlterarHistoricoSituacao(AlterarHistoricoSituacaoData historico);
    }
}