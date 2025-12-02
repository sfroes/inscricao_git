using SMC.Framework.Model;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public interface IEtapaProcessoControllerService
    {
        EtapaProcessoViewModel BuscarEtapaProcesso(long seqEtapaProcesso);
        SMCPagerModel<EtapaProcessoListaViewModel> BuscarEtapasProcesso(EtapaProcessoFiltroViewModel filtros);
        void ExcluirAssociacaoEtapa(long seqEtapaProcesso);
        long SalvarAssociacaoEtapa(EtapaProcessoViewModel modelo);
        List<SMCDatasourceItem> BuscarEtapasSelect(long seqProcesso);
        List<SMCDatasourceItem> BuscarSituacoesPermitidas(long seqEtapaProcesso);
        
        
        /// <summary>
        /// Busca o cabecalho com as informacoes do processo e etapa
        /// </summary>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        CabecalhoProcessoEtapaViewModel BuscarCabecalhoProcessoEtapa(long seqEtapaProcesso);

        
        /// <summary>
        /// Verifica se a inclusão de etapas é permitida
        /// </summary>        
        void VerificarPermissaoCadastrarEtapa(long seqProcesso);

        List<ConfiguracaoProrrogacaoViewModel> BuscarConfiguracoesProrrogacao(long seqEtapaProcesso
            , long[] seqOfertas);

        /// <summary>
        /// Recupera o sumário de uma prorrogação de processo para ser exibido para o usuário
        /// </summary>
        ProrrogacaoEtapaViewModel SumarioProrrogacao(ProrrogacaoEtapaViewModel etapaProrrogar);


        /// <summary>
        /// Prorroga a etapa informada com os dados passados no DTO
        /// </summary>
        /// <param name="etapaProrrogar"></param>
        void ProrrogarEtapa(ProrrogacaoEtapaViewModel etapaProrrogar);

        /// <summary>
        /// Verifica se é possível prorrogar a etapa informada
        /// </summary>        
        void VerificarPossibilidadeProrrogacao(long seqEtapaProcesso);
    }
}
