using SMC.DadosMestres.Common;
using SMC.DadosMestres.Common.Areas.GED.Enums;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.DadosMestres.Common.Areas.SHA.Enums;
using SMC.DadosMestres.ServiceContract.Areas.PES.Data;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Security;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Const;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Models;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject;
using System;
using System.Linq;
using System.Net;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Portfolio
{
    public class ProcessoApiDoaminService : InscricaoContextDomain<SharepointApi>
    {
        #region DOMAINSERVICE
        InscricaoDomainService InscricaoDomainService => Create<InscricaoDomainService>();
        BibliotecaApiDoaminService BibliotecaApiDoaminService => Create<BibliotecaApiDoaminService>();
        #endregion

        /// <summary>
        /// Criar processo caso atenda os parametros necessarios 
        /// </summary>
        /// <returns>Dados do Processo</returns>
        public RetornoProcessoSharepointVO CriarProcesso(CriarProcessoVO modelo)
        {
            RetornoProcessoSharepointVO retorno = new RetornoProcessoSharepointVO();

            var inscricao = this.InscricaoDomainService.SearchProjectionByKey(modelo.SeqInscricao, p => new
            {
                NomeInscrito = p.Inscrito.Nome,
                DescricaoProcesso = p.Processo.Descricao,
                p.Processo.TipoProcesso.SeqHierarquiaClassificacaoGed,
                NomeUnidadeResponsavel = p.Processo.UnidadeResponsavel.Nome,
                p.Processo.AnoReferencia,
                p.Processo.SemestreReferencia,
                p.UidProcessoGed
            });

            //Se existir um portfolio e ainda não existir um dossie para o portfolio cria
            if(!string.IsNullOrEmpty(modelo.IdGedPortfolio) && (inscricao.UidProcessoGed == null || inscricao.UidProcessoGed == Guid.Empty))
            {
                DadosProcessoSharepointVO dadosProcessoSharepointVO = new DadosProcessoSharepointVO();
                dadosProcessoSharepointVO.GuidBiblioteca = Guid.Parse(modelo.GuidBiblioteca);
                dadosProcessoSharepointVO.IdGEDPortfolio = modelo.IdGedPortfolio;
                dadosProcessoSharepointVO.EstruturaPasta = "Inscrições";
                dadosProcessoSharepointVO.Interessado = $"Nome: {inscricao.NomeInscrito} - Número de Inscrição: {modelo.SeqInscricao}, {inscricao.NomeUnidadeResponsavel}";
                dadosProcessoSharepointVO.NivelAcesso = NivelAcesso.Reservado;
                dadosProcessoSharepointVO.NomePastaProcesso = $"GPI NOVO - Inscrição {modelo.SeqInscricao}";
                dadosProcessoSharepointVO.NumeroProtocoloProcesso = $"GPI NOVO - {inscricao.SemestreReferencia}º/{inscricao.AnoReferencia} - Inscrição: {modelo.SeqInscricao}";
                dadosProcessoSharepointVO.PrevisaoDesclassificacao = PrevisaoDesclassificacao.Permanente;
                dadosProcessoSharepointVO.SeqHierarquiaClassificacao = inscricao.SeqHierarquiaClassificacaoGed.Value;
                dadosProcessoSharepointVO.Autor = $"Nome: {inscricao.NomeInscrito} - Número de Inscrição: {modelo.SeqInscricao}";
                dadosProcessoSharepointVO.Titulo = inscricao.DescricaoProcesso;

                retorno = RequisicoesSharePoint.EnviarPostApiGed<RetornoProcessoSharepointVO>(dadosProcessoSharepointVO, ACOES_WEBAPI_SHAREPOINT.SALVAR_PROCESSO);

                if (retorno.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new Exception(retorno.ErroMessage);
                }
            }

            return retorno;
        }

        /// <summary>
        /// Apagar processo caso atenda os parametros necessarios 
        /// </summary>
        /// <returns>Dados do Processo</returns>
        public RetornoProcessoSharepointVO ApagarProcesso(long seqInscricao)
        {
            RetornoProcessoSharepointVO retorno = new RetornoProcessoSharepointVO();

            var inscricao = this.InscricaoDomainService.SearchProjectionByKey(seqInscricao, p => new
            {
                p.Processo.TipoProcesso.SeqContextoBibliotecaGed,
                p.UidProcessoGed
            });

            //Se existir um portfolio e ainda não existir um dossie para o portfolio cria
            if (inscricao.UidProcessoGed.HasValue || inscricao.UidProcessoGed != Guid.Empty)
            {
                if (inscricao.SeqContextoBibliotecaGed != null && inscricao.SeqContextoBibliotecaGed > 0)
                {
                    var retornoApiBiblioteca = BibliotecaApiDoaminService.BuscarGuidBiblioteca(inscricao.SeqContextoBibliotecaGed.Value);

                    if (retornoApiBiblioteca.StatusCode == HttpStatusCode.OK)
                    {
                        DadosProcessoSharepointVO dadosProcessoSharepointVO = new DadosProcessoSharepointVO();
                        dadosProcessoSharepointVO.GuidBiblioteca =Guid.Parse(retornoApiBiblioteca.GuidBiblioteca);
                        dadosProcessoSharepointVO.IdGEDProcesso = inscricao.UidProcessoGed.ToString();

                        retorno = RequisicoesSharePoint.EnviarPostApiGed<RetornoProcessoSharepointVO>(dadosProcessoSharepointVO, ACOES_WEBAPI_SHAREPOINT.APAGAR_PROCESSO);

                        if (retorno.StatusCode == HttpStatusCode.BadRequest)
                        {
                            throw new Exception(retorno.ErroMessage);
                        }
                    }
                }
            }

            return retorno;
        }

    }
}
