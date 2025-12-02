using SMC.Framework.Extensions;
using SMC.Framework.Service;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.SEL.DomainServices;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System.ServiceModel;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class InscricaoOfertaHistoricoSituacaoService : SMCServiceBase, IInscricaoOfertaHistoricoSituacaoService
    {
        #region DomainService

        private InscricaoOfertaHistoricoSituacaoDomainService InscricaoOfertaHistoricoSituacaoDomainService { get => Create<InscricaoOfertaHistoricoSituacaoDomainService>(); }

        #endregion DomainService

        /// <summary>
        /// Buscar o historico da inscrição mais atual
        /// </summary>
        /// <param name="seqInscricaoOferta">Sequencial incrição oferta</param>
        /// <returns>Retorna os dados da incrição</returns>
        public InscricaoOfertaHistoricoSituacaoData BuscarHistoricosSituacaoAtual(long seqInscricaoOferta)
        {
            return InscricaoOfertaHistoricoSituacaoDomainService.BuscarHistoricosSituacaoAtual(seqInscricaoOferta).Transform<InscricaoOfertaHistoricoSituacaoData>();
        }

        /// <summary>
        /// Altera a situação de uma inscrição oferta
        /// </summary>
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void AlterarHistoricoSituacao(AlterarHistoricoSituacaoData historico)
        {
            InscricaoOfertaHistoricoSituacaoDomainService.AlterarHistoricoSituacao(historico.Transform<AlterarHistoricoSituacaoVO>());
        }
    }
}