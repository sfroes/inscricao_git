using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.SEL.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.SEL.Interfaces
{
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface ISelecaoService : ISMCService
    {
        #region Acompanhamento Selecao

        SMCPagerData<AcompanhamentoSelecaoData> ConsultaPosicaoConsolidada(AcompanhamentoSelecaoFiltroData filtro);

        SMCPagerData<PosicaoConsolidadaPorGrupoOfertaListaData> ConsultaPosicaoConsolidadaGrupoOferta(PosicaoConsolidadaPorGrupoOfertaFiltroData filtro);

        SMCPagerData<ConsultaCandidatosProcessoListaData> BuscarCandidatosProcesso(ConsultaCandidatosProcessoFiltroData filtro);

        CabecalhoSelecaoData BuscarCabecalhoSelecaoProcesso(long seqProcesso);

        CabecalhoSelecaoData BuscarCabecalhoSelecaoOferta(long seqOferta);

        CabecalhoSelecaoData BuscarCabecalhoInscricaoOferta(long seqInscricaoOferta);

        #endregion Acompanhamento Selecao

        #region Analise Seleção

        List<LancamentoResultadoItemData> BuscarInscricoesOfertaParaSelecao(long seqProcesso, List<long> inscricoesOfertas);

        void SalvarLancamentos(List<LancamentoResultadoItemData> list);

        bool VerificaDisponibilidadeVagas(long seqOferta, List<LancamentoResultadoItemData> lancamentos);

        void DesfazerLancamentoResultado(List<long> seqsInscricaoOferta);

        List<HistoricoSituacaoData> BuscarHistoricosSituacao(long seqInscricaoOferta);

        HistoricoSituacaoData BuscarHistoricoSituacao(long seqHistoricoSituacao);

        long SalvarAlteracaoSituacao(HistoricoSituacaoData data);

        List<OpcoesInscricaoData> BuscarOpcoesInscricao(long seqInscricao);

        List<ConvocadoData> BuscarInscricoesOfertaParaConvocacao(long seqProcesso, List<long> seqsInscricaoOferta);

        #endregion Analise Seleção

        #region Alteração Oferta

        OfertaCabecalhoData BuscarOfertaCabecalho(OfertaFiltroData filtro);

        OfertaAlteracaoData BuscarOferta(OfertaFiltroData filtro);

        void SalvarAlteracaoOferta(OfertaAlteracaoData oferta);

        #endregion Alteração Oferta

        OfertaAlteracaoData BuscarDadosOferta(OfertaFiltroData filtro);
    }
}