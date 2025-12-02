using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.AcompanhamentoCheckin;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.InscricaoOferta;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    /// <summary>
    /// Inteface para o serviço que chama o DomainService de Inscrição oferta
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IInscricaoOfertaService : ISMCService
    {

        /// <summary>
        /// Efeturar o checkin do inscrito
        /// </summary>
        /// <param name="dados">Dados do checkin</param>
        /// <returns>Mensagem com o status code</returns>
        RespostaCheckinData EfetuarCheckin(CheckinData dados);

        /// <summary>
        /// Efeturar o checkin manual do inscrito
        /// </summary>
        /// <param name="dados">Dados do checkin</param>
        /// <returns>Mensagem com o status code</returns>
        RespostaCheckinData EfetuarCheckinManual(CheckinData dados);

        /// <summary>
        /// Pesquisar Inscrito para checkin manual
        /// </summary>
        /// <param name="dados">Dados do checkin</param>
        /// <returns>Mensagem com o status code</returns>
        RespostaCheckinData PesquisaNomeCheckinManual(FiltroCheckinData dados);

        /// <summary>
        /// Pesquisar inscrito checkin manual
        /// </summary>
        /// <param name="dados">Dados do inscrito</param>
        /// <returns>Mensagem com o status code</returns>
        List<RespostaPesquiaOfertaCheckinData> PesquisaOfertaCheckinManual(FiltroCheckinData dados);

        /// <summary>
        /// Verifica se o inscrito possui checkin
        /// </summary>
        /// <param name="seqInscricao"></param>
        /// <returns></returns>
        bool VerificaPossuiCkeckin(long seqInscricao);

        /// <summary>
        /// Efeturar o checkout do inscrito
        /// </summary>
        /// <param name="guid">Guid inscrição oferta</param>
        /// <returns></returns>
        RespostaCheckinData EfetuarCheckout(string guid);

        /// <summary>
        /// Busca o cabecalho do ckeckin em lote
        /// </summary>
        /// <param name="seqOferta"></param>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        CabecalhoCheckinLoteData BuscarCabecalhoCheckinLote(long seqOferta, long seqProcesso);

        /// <summary>
        /// Busca o cabecalho do ckeckin em lote
        /// </summary>
        /// <param name="seqOferta"></param>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        SMCPagerData<ListarCheckinLoteData> BuscarInscritosCheckinLote(CheckinLoteFiltroData seqOferta);

        /// <summary>
        /// Efeturar o checkin em lote
        /// </summary>
        /// <param name="dados"></param>
        /// <returns></returns>
        RespostaCheckinData EfetuarCheckinLote(CheckinData dados);

        RespostaCheckinData DesfazerCheckinLote(CheckinData dados);

        /// <summary>
        /// Buscar dados da inscrição oferta por guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>Dados da inscricao oferta</returns>
        InscricaoOfertaData BuscarInscricaoOfertaPorGuid(Guid guid);
    }
}