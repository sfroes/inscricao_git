using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Castle.Core.Internal;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Financeiro.ServiceContract.BLT;
using SMC.Formularios.Common.Areas.FRM.Includes;
using SMC.Formularios.ServiceContract.Areas.FRM.Interfaces;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Framework.Util;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Exceptions.Processo;
using SMC.Inscricoes.Common.Constants;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.Validators;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.NOT.DomainServices;
using SMC.Inscricoes.Domain.Areas.RES.DomainServices;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using SMC.Inscricoes.Domain.Areas.RES.Specifications;
using SMC.Inscricoes.Domain.Areas.SEL.DomainServices;
using SMC.Inscricoes.Domain.Areas.SEL.Specifications;
using SMC.Inscricoes.Domain.Models;
using SMC.Inscricoes.Rest.Helper;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Notificacoes.Common.Areas.NTF.Enums;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Data;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;
using StackExchange.Profiling.Internal;
using Unity.Interception.Utilities;
using ProcessoData = SMC.Inscricoes.ServiceContract.Areas.INS.Data.ProcessoData;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class ProcessoDomainService : InscricaoContextDomain<Processo>
    {
        #region Domain Services

        private TipoProcessoSituacaoDomainService TipoProcessoSituacaoDomainService
        {
            get { return this.Create<TipoProcessoSituacaoDomainService>(); }
        }

        private TipoTaxaDomainService TipoTaxaDomainService
        {
            get { return this.Create<TipoTaxaDomainService>(); }
        }

        private UnidadeResponsavelDomainService UnidadeResponsavelDomainService
        {
            get { return this.Create<UnidadeResponsavelDomainService>(); }
        }

        private ProcessoIdiomaDomainService ProcessoIdiomaDomainService
        {
            get { return this.Create<ProcessoIdiomaDomainService>(); }
        }

        private TipoProcessoDomainService TipoProcessoDomainService
        {
            get { return this.Create<TipoProcessoDomainService>(); }
        }

        private HierarquiaOfertaDomainService HierarquiaOfertaDomainService
        {
            get { return this.Create<HierarquiaOfertaDomainService>(); }
        }

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        private UnidadeResponsavelTipoProcessoDomainService UnidadeResponsavelTipoProcessoDomainService
        {
            get { return this.Create<UnidadeResponsavelTipoProcessoDomainService>(); }
        }

        private GrupoOfertaDomainService GrupoOfertaDomainService
        {
            get { return this.Create<GrupoOfertaDomainService>(); }
        }

        private EtapaProcessoDomainService EtapaProcessoDomainService
        {
            get { return this.Create<EtapaProcessoDomainService>(); }
        }

        private ConfiguracaoEtapaDomainService ConfiguracaoEtapaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaDomainService>(); }
        }

        private ConfiguracaoEtapaPaginaDomainService ConfiguracaoEtapaPaginaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaPaginaDomainService>(); }
        }

        private ConfiguracaoEtapaPaginaIdiomaDomainService ConfiguracaoEtapaPaginaIdiomaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaPaginaIdiomaDomainService>(); }
        }

        private ProcessoConfiguracaoNotificacaoDomainService ProcessoConfiguracaoNotificacaoDomainService
        {
            get { return this.Create<ProcessoConfiguracaoNotificacaoDomainService>(); }
        }

        private ProcessoConfiguracaoNotificacaoIdiomaDomainService ProcessoConfiguracaoNotificacaoIdiomaDomainService
        {
            get { return this.Create<ProcessoConfiguracaoNotificacaoIdiomaDomainService>(); }
        }

        private ParametroEnvioNotificacaoDomainService ParametroEnvioNotificacaoDomainService
        {
            get { return this.Create<ParametroEnvioNotificacaoDomainService>(); }
        }

        private OfertaDomainService OfertaDomainService
        {
            get { return this.Create<OfertaDomainService>(); }
        }

        private OfertaPeriodoTaxaDomainService OfertaPeriodoTaxaDomainService
        {
            get { return Create<OfertaPeriodoTaxaDomainService>(); }
        }

        private InscricaoHistoricoSituacaoDomainService InscricaoHistoricoSituacaoDomainService { get => Create<InscricaoHistoricoSituacaoDomainService>(); }

        private InscricaoOfertaHistoricoSituacaoDomainService InscricaoOfertaHistoricoSituacaoDomainService { get => Create<InscricaoOfertaHistoricoSituacaoDomainService>(); }

        private ConfiguracaoModeloDocumentoDomainService ConfiguracaoModeloDocumentoDomainService { get => Create<ConfiguracaoModeloDocumentoDomainService>(); }

        private ArquivoAnexadoDomainService ArquivoAnexadoDomainService { get => Create<ArquivoAnexadoDomainService>(); }

        private ProcessoCampoInscritoDomainService ProcessoCampoInscritoDomainService { get => Create<ProcessoCampoInscritoDomainService>(); }

        private TipoProcessoCampoInscritoDomainService TipoProcessoCampoInscritoDomainService { get => Create<TipoProcessoCampoInscritoDomainService>(); }
        private InscricaoDocumentoEmitidoDomainService InscricaoDocumentoEmitidoDomainService { get => Create<InscricaoDocumentoEmitidoDomainService>(); }
        private InscricaoDadoFormularioDomainService InscricaoDadoFormularioDomainService => Create<InscricaoDadoFormularioDomainService>();
        private ProcessoConfiguracaoFormularioDomainService ProcessoConfiguracaoFormularioDomainService => Create<ProcessoConfiguracaoFormularioDomainService>();
        private GrupoTaxaDomainService GrupoTaxaDomainService => Create<GrupoTaxaDomainService>();
        private TipoProcessoDocumentoDomainService TipoProcessoDocumentoDomainService => Create<TipoProcessoDocumentoDomainService>();
        private TipoDocumentoDomainService TipoDocumentoDomainService { get => Create<TipoDocumentoDomainService>(); }
        private InscricaoOfertaDomainService InscricaoOfertaDomainService { get => Create<InscricaoOfertaDomainService>(); }

        #endregion Domain Services

        #region Services

        private INotificacaoService NotificacaoService
        {
            get { return this.Create<INotificacaoService>(); }
        }

        private IIntegracaoFinanceiroService FinanceiroService
        {
            get { return this.Create<IIntegracaoFinanceiroService>(); }
        }

        private ISituacaoService SituacaoService
        {
            get { return this.Create<ISituacaoService>(); }
        }

        private IEtapaService EtapaService
        {
            get { return Create<IEtapaService>(); }
        }

        private IFormularioService FormularioService
        {
            get { return this.Create<IFormularioService>(); }
        }

        private ITemplateProcessoService TemplateProcessoService => Create<ITemplateProcessoService>();

        private SMC.DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService TipoDocumentoService => Create<SMC.DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService>();

        private ITipoProcessoService TipoProcessoService
        {
            get { return Create<ITipoProcessoService>(); }
        }

        #endregion Services

        #region Buscar informações do processo

        /// <summary>
        /// Busca a descrição complementar do processo no idioma informado na specification
        /// Caso o idioma não seja informado retorna a descrição complementar no idioma padrão do processo
        /// </summary>
        public string BuscarURLComplementar(long seqProcesso)
        {
            var enderecoEletronico = this.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso),
                x => x.EnderecosEletronicos.FirstOrDefault(y => y.TipoEnderecoEletronico == TipoEnderecoEletronico.Website));
            if (enderecoEletronico == null) return null;
            return enderecoEletronico.Descricao;
        }

        /// <summary>
        /// Retorna a chave valor, para as situações de um tipo processo (seq da situação e descrião)
        /// </summary>
        /// <param name="seqTipoProcesso"></param>
        /// <returns></returns>
        public IEnumerable<SMCDatasourceItem> BuscarSituacoesProcessoKeyValue(long seqProcesso)
        {
            var seqTipoProcesso = this.SearchProjectionByKey(
                new SMCSeqSpecification<Processo>(seqProcesso), x => x.SeqTipoProcesso);
            return this.TipoProcessoSituacaoDomainService.BuscarTiposProcessoSituacaoKeyValue(seqTipoProcesso);
        }

        /// <summary>
        /// Retorna os TipoProcessoSituação permitidos para uma inscrição no processo e etapas informados
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <returns>Sequencial e descrição dos TipoProcessoSituação</returns>
        public IEnumerable<SMCDatasourceItem> BuscarSituacoesProcessoPorEtapaKeyValue(long seqProcesso, params string[] tokensEtapa)
        {
            var processo = this.SearchProjectionByKey(
                new SMCSeqSpecification<Processo>(seqProcesso), x => new { SeqTipoProcesso = x.SeqTipoProcesso, SeqTemplateProcesso = x.SeqTemplateProcessoSGF });
            return this.TipoProcessoSituacaoDomainService.BuscarTiposProcessoSituacaoPorEtapaKeyValue(processo.SeqTipoProcesso, processo.SeqTemplateProcesso, tokensEtapa);
        }

        public List<ProcessoTemplateProcessoSGFVO> BuscarSeqsTemplatesProcessosSGF(long[] seqsProcessos)
        {
            var spec = new ProcessoFilterSpecification() { SeqsProcessos = seqsProcessos };

            var data = SearchProjectionBySpecification(spec, x => new ProcessoTemplateProcessoSGFVO() { Seq = x.Seq, SeqTemplateProcessoSGF = x.SeqTemplateProcessoSGF }).ToList();

            return data;
        }

        #endregion Buscar informações do processo

        #region Posição consolidada inscrição

        /// <summary>
        /// Consulta a posição consolidada por processo para listagem
        /// </summary>
        public List<PosicaoConsolidadaVO> BuscarPosicaoConsolidadaProcesso(ProcessoFilterSpecification filter, out int total)
        {
            var result = ResultadoPosicaoConsolidada(filter, out total);

            var posicoes = MontarContabilizadorPosicaoConsoliada(result);

            return posicoes;
        }

        public List<ResultadoPosicaoConsolidadaVO> ResultadoPosicaoConsolidada(ProcessoFilterSpecification filter, out int total)
        {
            var result = this.SearchProjectionBySpecification(filter, p => new ResultadoPosicaoConsolidadaVO
            {
                Seq = p.Seq,
                Descricao = p.Descricao,
                GruposOfertas = p.GruposOferta.Select(s => new PosicaoConsolidadaGrupoOfertaVo()
                {
                    SeqGrupoOferta = s.Seq,
                    NomeGrupo = s.Nome,
                    Ofertas = s.Ofertas.Select(so => new OfertaVO
                    {
                        SeqOferta = so.Seq,
                        DescricaoOferta = so.DescricaoCompleta,
                        SeqGrupoOferta = so.SeqGrupoOferta,
                        HierarquiaCompleta = so.HierarquiaCompleta

                    }).ToList()

                }).ToList(),
                Inscricoes = p.Inscricoes.Select(s => new ResultadoInscricaoPosicaoConsolidadaVO
                {
                    SeqInscricao = s.Seq,
                    SeqGrupoOferta = s.SeqGrupoOferta,
                    TituloPago = s.TituloPago,
                    DocumentacaoEntregue = s.DocumentacaoEntregue,
                    HistoricosSituacao = s.HistoricosSituacao.OrderByDescending(o => o.DataInclusao).Select(sh => new ResultadoHistoricoSituacaoPosicaoConsolidadaVO
                    {
                        Atual = sh.Atual,
                        AtualEtapa = sh.AtualEtapa,
                        Token = sh.TipoProcessoSituacao.Token,
                        SeqMotivoSituacaoSGF = sh.SeqMotivoSituacaoSGF
                    }).ToList(),
                    Ofertas = s.Ofertas.Select(so => new ResultadoInscricaoOfertaOfertaPosicaoConsolidadaVO
                    {
                        Seq = so.Seq,
                        SeqOferta = so.SeqOferta,
                        DescricaoOferta = so.Oferta.DescricaoCompleta,
                        DescricaoGrupoOferta = so.Oferta.GrupoOferta.Nome,
                        SeqGrupoOferta = so.Oferta.SeqGrupoOferta,
                        HierarquiaCompleta = so.Oferta.HierarquiaCompleta
                    }).ToList()

                }).ToList()
            }, out total).ToList();

            return result;
        }

        public List<PosicaoConsolidadaVO> MontarContabilizadorPosicaoConsoliada(List<ResultadoPosicaoConsolidadaVO> listaPosicao, long? seqGrupo = null)
        {
            List<PosicaoConsolidadaVO> retorno = new List<PosicaoConsolidadaVO>();
            var motivosCanceladoTeste = SituacaoService.BuscarSeqMotivosSituacaoPorToken(TOKENS.MOTIVO_INSCRICAO_CANCELADA_TESTE);

            foreach (var posicao in listaPosicao)
            {
                PosicaoConsolidadaVO posicaoConsolidada = new PosicaoConsolidadaVO();

                posicaoConsolidada.Descricao = posicao.Descricao;
                posicaoConsolidada.Descricao = posicao.Descricao;
                posicaoConsolidada.Seq = posicao.Seq;

                //Quantidade de inscrições: total de inscrições no processo, exceto as canceladas cujo motivo é CANCELADA_TESTE.
                posicaoConsolidada.Total = posicao.Inscricoes.Count(i => !i.HistoricosSituacao.Any(f => f.Atual && f.SeqMotivoSituacaoSGF.HasValue
                                                                          && motivosCanceladoTeste.Contains(f.SeqMotivoSituacaoSGF.Value)));

                //Deferidas: o total de inscrições cuja situação atual da etapa de inscrição é INSCRICAO_DEFERIDA.
                posicaoConsolidada.Deferidos = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.AtualEtapa &&
                                                                        s.Token == TOKENS.SITUACAO_INSCRICAO_DEFERIDA));

                //Indeferidas: o total de inscrições cuja situação atual é INSCRICAO_INDEFERIDA
                posicaoConsolidada.Indeferidos = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual &&
                                                                            s.Token == TOKENS.SITUACAO_INSCRICAO_INDEFERIDA));

                //Iniciadas: o total de inscrições com a situação atual igual à INSCRICAO_INICIADA e cuja oferta foi selecionada.
                posicaoConsolidada.Iniciadas = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual &&
                                                                        s.Token == TOKENS.SITUACAO_INSCRICAO_INICIADA && i.Ofertas.Any()));

                if (seqGrupo == null)
                {
                    //Oferta não Selecionada: o total de inscrições cuja oferta não foi selecionada.
                    posicaoConsolidada.OfertasNaoSelecionadas = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual &&
                                                                                          s.Token == TOKENS.SITUACAO_INSCRICAO_INICIADA && !i.Ofertas.Any()));
                }
                else
                {
                    //Oferta não Selecionada: o total de inscrições cuja oferta não foi selecionada.
                    posicaoConsolidada.OfertasNaoSelecionadas = posicao.Inscricoes.Count(i => i.SeqGrupoOferta == seqGrupo && i.HistoricosSituacao.Any(s => s.Atual &&
                                                                                          s.Token == TOKENS.SITUACAO_INSCRICAO_INICIADA && !i.Ofertas.Any()));
                }

                //Finalizadas: o total de inscrições que possuem no “histórico de situação” a situação INSCRICAO_FINALIZADA e que a situação atual não é INSCRICAO_CANCELADA e tampouco INSCRICAO_INICIADA.
                posicaoConsolidada.Finalizadas = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual &&
                                                                                       s.Token != TOKENS.SITUACAO_INSCRICAO_CANCELADA &&
                                                                                       s.Token != TOKENS.SITUACAO_INSCRICAO_INICIADA)
                                                                                       && i.HistoricosSituacao.Any(s => s.Token == TOKENS.SITUACAO_INSCRICAO_FINALIZADA));

                //Pagas: o total de inscrições com o campo “inscrições pagas” igual a “Sim”, cuja situação atual não é INSCRICAO_CANCELADA.
                posicaoConsolidada.Pagas = posicao.Inscricoes.Count(i => i.TituloPago && !i.HistoricosSituacao
                                   .Any(s => s.Atual && s.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA));

                //Documentação Entregue: o total de inscrições com o campo “documentação entregue” igual a “Sim”, cuja situação atual não é INSCRICAO_CANCELADA.
                posicaoConsolidada.DocumentacoesEntregues = posicao.Inscricoes.Count(i => i.DocumentacaoEntregue &&
                                                                                     !i.HistoricosSituacao.Any(s => s.Atual && s.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA));

                //Confirmadas: o total de inscrições cuja a situação atual da inscrição é INSCRICAO_CONFIRMADA, INSCRICAO_DEFERIDA ou INSCRICAO_INDEFERIDA (com situação anterior INSCRICAO_CONFIRMADA).
                posicaoConsolidada.Confirmadas = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual &&
                                                                               (s.Token == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA ||
                                                                                s.Token == TOKENS.SITUACAO_INSCRICAO_DEFERIDA ||
                                                                               (s.Token == TOKENS.SITUACAO_INSCRICAO_INDEFERIDA &&
                                                                                i.HistoricosSituacao.Skip(1).FirstOrDefault().Token == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA))));

                //Não Confirmadas: o total de inscrições cuja a situação atual da inscrição é INSCRICAO_FINALIZADA, ou INSCRICAO_INDEFERIDA (com situação anterior diferente de INSCRICAO_CONFIRMADA).
                posicaoConsolidada.NaoConfirmadas = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual && (
                                                                             s.Token == TOKENS.SITUACAO_INSCRICAO_FINALIZADA ||
                                                                            (s.Token == TOKENS.SITUACAO_INSCRICAO_INDEFERIDA &&
                                                                             i.HistoricosSituacao.Skip(1).FirstOrDefault().Token != TOKENS.SITUACAO_INSCRICAO_CONFIRMADA))));

                //Canceladas: o total de inscrições cuja situação atual é INSCRICAO_CANCELADA e o motivo, quando existir, não é CANCELADA_TESTE.
                posicaoConsolidada.Canceladas = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual
                                                                          && s.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA)
                                                                          && !i.HistoricosSituacao.Any(f => f.Atual && f.SeqMotivoSituacaoSGF.HasValue
                                                                          && motivosCanceladoTeste.Contains(f.SeqMotivoSituacaoSGF.Value)));
                retorno.Add(posicaoConsolidada);
            }

            return retorno;
        }
        #endregion Posição consolidada inscrição

        #region Buscar posição consolidada da seleção

        public List<AcompanhamentoSelecaoVO> ConsultaPosicaoConsolidadaSelecao(ProcessoFilterSpecification filter, out int total)
        {
            var seqsEtapasSituacaoDeferida = EtapaService.BuscarSeqEtapasPorTokenSituacao(TOKENS.SITUACAO_INSCRICAO_DEFERIDA);

            var vos = this.SearchProjectionBySpecification(filter, x => new AcompanhamentoSelecaoVO
            {
                Seq = x.Seq,
                Descricao = x.Descricao,
                TipoProcesso = x.TipoProcesso.Descricao,
                CandidatosConfirmados = x.Inscricoes.Where(i => i.HistoricosSituacao.Any(
                            g => g.Atual &&
                            // Verifica se o processo possui INSCRICAO_DEFERIDA em seu template. Se existir, utiliza essa situação para candidado confirmado,
                            // caso contrário, utiliza INSCRICAO_CONFIRMADA
                            g.TipoProcessoSituacao.Token == ((i.Processo.EtapasProcesso.Select(e => e.SeqEtapaSGF).Intersect(seqsEtapasSituacaoDeferida).Any())
                                                                ? TOKENS.SITUACAO_INSCRICAO_DEFERIDA : TOKENS.SITUACAO_INSCRICAO_CONFIRMADA)

                        )
                    ).SelectMany(i => i.Ofertas.SelectMany(f => f.HistoricosSituacao.Where(g => g.Atual))).Count(),
                //CandidatosConfirmados = x.Inscricoes.SelectMany(i => i.Ofertas.SelectMany(f => f.HistoricosSituacao.Where(g => g.Atual))).Count(),

                CandidatosDesistentes = x.Inscricoes.SelectMany(i => i.Ofertas.SelectMany(
                        f => f.HistoricosSituacao.Where(g => g.Atual && g.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CANDIDATO_DESISTENTE))).Count(),

                CandidatosReprovados = x.Inscricoes.SelectMany(i => i.Ofertas.SelectMany(
                        f => f.HistoricosSituacao.Where(g => g.Atual && g.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CANDIDATO_REPROVADO))).Count(),

                CandidatosSelecionados = x.Inscricoes.SelectMany(i => i.Ofertas.SelectMany(
                        f => f.HistoricosSituacao.Where(g => g.AtualEtapa && g.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CANDIDATO_SELECIONADO))).Count(),

                CandidatosExcedentes = x.Inscricoes.SelectMany(i => i.Ofertas.SelectMany(
                        f => f.HistoricosSituacao.Where(g => g.AtualEtapa && g.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CANDIDATO_EXCEDENTE))).Count(),

                Convocados = x.Inscricoes.SelectMany(i => i.Ofertas.SelectMany(
                        f => f.HistoricosSituacao.Where(g => g.Atual && g.EtapaProcesso.Token == TOKENS.ETAPA_CONVOCACAO))).Count(),

                ConvocadosDesistentes = x.Inscricoes.SelectMany(i => i.Ofertas.SelectMany(
                        f => f.HistoricosSituacao.Where(g => g.Atual && g.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CONVOCADO_DESISTENTE))).Count(),

                ConvocadosConfirmados = x.Inscricoes.SelectMany(i => i.Ofertas.SelectMany(
                        f => f.HistoricosSituacao.Where(g => g.Atual && g.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CONVOCADO_CONFIRMADO))).Count()
            }, out total);

            return vos.ToList();
        }

        #endregion Buscar posição consolidada da seleção

        #region Buscar processos

        /// <summary>
        /// Retorna a chave valor, para as situações de um tipo processo (seq da situação e descrião)
        /// </summary>
        /// <param name="inscricaoDocumentoEntregueSpec">Filtros para pesquisa</param>
        /// <param name="idioma">Idioma para retornar as informações</param>
        /// <returns>Informações do processo para apresentação na Home</returns>
        public ProcessoHomeVO BuscarProcessoHome(ProcessoFilterSpecification filtros, SMCLanguage idioma)
        {
            // Busca o processo
            Processo processo = this.SearchByKey(filtros, IncludesProcesso.EtapasProcesso |
                                                        IncludesProcesso.TipoProcesso_UnidadeResponsavelTipoProcesso |
                                                        IncludesProcesso.GruposOferta_Ofertas |
                                                        IncludesProcesso.ConfiguracoesFormulario |
                                                        IncludesProcesso.Inscricoes_HistoricosSituacao_TipoProcessoSituacao |
                                                        IncludesProcesso.Inscricoes_Ofertas_HistoricosSituacao_TipoProcessoSituacao |
                                                        IncludesProcesso.UnidadeResponsavelTipoProcessoIdVisual);

            // Transforma o processo em VO
            SMCLanguage idiomaProcesso;
            ProcessoHomeVO vo = new ProcessoHomeVO()
            {
                SeqProcesso = processo.Seq,
                DescricaoProcesso = processo.Descricao,
                UrlInformacaoComplementar = this.BuscarURLComplementar(processo.Seq),
                DescricaoComplementarProcesso = this.ProcessoIdiomaDomainService.BuscarDescricaoComplementarProcesso(new ProcessoIdiomaFilterSpecification(processo.Seq, idioma), out idiomaProcesso),
                IdiomaAtual = idiomaProcesso,
                IdiomasDisponiveis = this.ProcessoIdiomaDomainService.BuscarIdiomasDisponiveis(processo.Seq),
                ProcessoCancelado = processo.Cancelado,
                SeqTipoProcesso = processo.SeqTipoProcesso,
                SituacaoEtapaInscricao = processo.EtapasProcesso.Where(e => e.Token == TOKENS.ETAPA_INSCRICAO).First().SituacaoEtapa,
                TokenResource = processo.TipoProcesso.TokenResource,
                UrlCss = processo.UnidadeResponsavelTipoProcessoIdVisual.CssAplicacao,
                TokenCssAlternativoSas = processo.UnidadeResponsavelTipoProcessoIdVisual.TokenCssAlternativoSas,

                TodasOfertasProcessoInativas = processo.GruposOferta.SelectMany(sm => sm.Ofertas).All(al => !al.Ativo),
                ProcessoEncerrado = processo.DataEncerramento.HasValue && processo.DataEncerramento <= DateTime.Now,
                OrientacaoCadastroInscrito = processo.TipoProcesso.OrientacaoCadastroInscrito
            };


            if (!vo.UrlCss.Contains(CSS_PROCESSO.CSS_PROCESSO_INSCRICAO))
            {
                vo.UrlCss += vo.UrlCss[vo.UrlCss.Length - 1] == '/' ? CSS_PROCESSO.CSS_PROCESSO_INSCRICAO : "/" + CSS_PROCESSO.CSS_PROCESSO_INSCRICAO;
            }


            vo.TituloInscricoes = $"Titulo_Inscricoes_{vo.TokenResource.ToLower().SMCToPascalCase()}";

            if (filtros.SeqInscrito.HasValue)
            {
                vo.FormularioImpacto = ValidarFormularioImpacto(processo, filtros.SeqInscrito.Value);
            }
            return vo;
        }

        public string BuscarCssProcesso(Guid uidProcesso)
        {
            var spec = new ProcessoFilterSpecification()
            {
                UidProcesso = uidProcesso
            };

            return this.SearchProjectionByKey(spec, s => s.UnidadeResponsavelTipoProcessoIdVisual.CssAplicacao);
        }


        /// <summary>
        /// Verificar se o formulario de impacto
        /// </summary>
        /// <param name="processo">Dados do processo</param>
        /// <param name="seqInscrito">Sequencial do inscrito</param>
        /// <returns>Dados para exibição do formulário de impacto</returns>
        public FormularioImpactoVO ValidarFormularioImpacto(Processo processo, long seqInscrito)
        {
            //Apresentar o formulário apenas para os inscritos cujo processo em questão não possua data de cancelamento 
            //maior ou igual à data vigente, seja de gestão de eventos e possua um template de formulário configurado, 
            //que a data vigente se enquadre no período estabelecido de disponibilidade do formulário.É necessário que o 
            //inscrito ainda não tenha respondido ao formulário e que ele possua uma situação que permite responder o 
            //formulário: INSCRICAO_DEFERIDA, no caso do template do processo possuir deferimento de inscrição, ou 
            //INSCRICAO_CONFIRMADA, no caso do template do processo não possuir deferimento de inscrição..

            //Caso não haja nenhuma oferta com check -in, exibir o formulário no período estabelecido para candidatos que
            //possuam a situação INSCRICAO_DEFERIDA, no caso do template do processo possuir deferimento de inscrição, 
            //ou INSCRICAO_CONFIRMADA, no caso do template do processo não possuir deferimento de inscrição.

            // Caso o flag do formulário "Disponível apenas para quem fez check-in?" seja igual a "Sim", é necessário que o 
            //participante tenha realizado o check -in em ao menos uma das ofertas inscritas, além de estar na situação que 
            //permite responder ao formulário de impacto. Caso o flag seja igual a "Não", é necessário apenas que o participante 
            //esteja na situação que permite responder ao formulário de impacto."

            FormularioImpactoVO retorno = new FormularioImpactoVO();
            var gestaoEvento = processo.TipoProcesso.GestaoEventos;
            var configuracoesFormulario = processo.ConfiguracoesFormulario.Any() ? processo.ConfiguracoesFormulario.FirstOrDefault() : null;

            if (configuracoesFormulario == null || !gestaoEvento)
            {
                retorno.ExibirFormularioImpacto = false;
                retorno.MensagemFormularioImpactoIndisponivel = "Este formulário encontra-se indisponível";
                return retorno;
            }

            if (processo.DataCancelamento.HasValue && processo.DataCancelamento.Value.Date <= DateTime.Now.Date)
            {
                retorno.ExibirFormularioImpacto = false;
                retorno.MensagemFormularioImpactoIndisponivel = "Este processo encontra-se cancelado";
                return retorno;
            }

            if (!((configuracoesFormulario.DataInicioFormulario.Date <= DateTime.Now.Date && configuracoesFormulario.DataFimFormulario.HasValue && configuracoesFormulario.DataFimFormulario.Value.Date >= DateTime.Now.Date)
                || (configuracoesFormulario.DataInicioFormulario.Date <= DateTime.Now.Date && !configuracoesFormulario.DataFimFormulario.HasValue)))
            {
                retorno.ExibirFormularioImpacto = false;
                if (configuracoesFormulario.DataInicioFormulario.Date >= DateTime.Now.Date)
                {
                    retorno.MensagemFormularioImpactoIndisponivel = $"Avaliação disponível a partir de {configuracoesFormulario.DataInicioFormulario.Date.ToShortDateString()}";
                }
                else
                {
                    retorno.MensagemFormularioImpactoIndisponivel = $"Avaliação expirada";
                }

                return retorno;
            }

            var ofertas = processo.GruposOferta.SelectMany(sm => sm.Ofertas).ToList();
            var somenteCheckin = configuracoesFormulario.DisponivelApenasComCheckin;
            var situacoesTemplateProcesso = TemplateProcessoService.BuscarSituacoesPorTemplateProcesso(processo.SeqTemplateProcessoSGF);
            var tokenSituacao = situacoesTemplateProcesso.Contains(TOKENS.SITUACAO_INSCRICAO_DEFERIDA) ? TOKENS.SITUACAO_INSCRICAO_DEFERIDA : TOKENS.SITUACAO_INSCRICAO_CONFIRMADA;
            var inscricoesInscrito = processo.Inscricoes.Where(w => w.SeqInscrito == seqInscrito && w.HistoricosSituacao.Any(a => a.Atual && a.TipoProcessoSituacao.Token == tokenSituacao));
            var inscricoesOferta = inscricoesInscrito.SelectMany(sm => sm.Ofertas).ToList();

            var formularioRespondido = false;
            foreach (var item in inscricoesOferta)
            {
                var spec = new InscricaoDadoFormularioFilterSpecification() { SeqFormulario = configuracoesFormulario.SeqFormularioSgf, SeqVisao = configuracoesFormulario.SeqVisaoSgf, SeqInscricao = item.SeqInscricao };
                var inscricoesFormulario = InscricaoDadoFormularioDomainService.SearchBySpecification(spec).ToList();
                if (inscricoesFormulario.Any())
                {
                    formularioRespondido = true;
                }
            }

            if (formularioRespondido)
            {
                retorno.ExibirFormularioImpacto = false;
                retorno.MensagemFormularioImpactoIndisponivel = "Esse formulário de avaliação já foi respondido";
                return retorno;
            }

            if (!inscricoesInscrito.Any())
            {
                retorno.ExibirFormularioImpacto = false;
                retorno.MensagemFormularioImpactoIndisponivel = "Inscrição não encontrada";
                return retorno;
            }

            if (somenteCheckin)
            {
                if (inscricoesOferta.Any())
                {
                    var possuiCheckin = inscricoesOferta.Any(a => a.DataCheckin.HasValue);
                    if (!possuiCheckin)
                    {
                        retorno.ExibirFormularioImpacto = false;
                        retorno.MensagemFormularioImpactoIndisponivel = "Registro de participação no evento não encontrado";
                        return retorno;
                    }
                }
                else
                {
                    if (!((configuracoesFormulario.DataInicioFormulario.Date <= DateTime.Now.Date && configuracoesFormulario.DataFimFormulario.HasValue && configuracoesFormulario.DataFimFormulario.Value.Date >= DateTime.Now.Date)
                        || (configuracoesFormulario.DataInicioFormulario.Date <= DateTime.Now.Date && !configuracoesFormulario.DataFimFormulario.HasValue)))
                    {
                        retorno.ExibirFormularioImpacto = false;
                        if (configuracoesFormulario.DataInicioFormulario.Date >= DateTime.Now.Date)
                        {
                            retorno.MensagemFormularioImpactoIndisponivel = $"Avaliação disponível a partir de {configuracoesFormulario.DataInicioFormulario.Date.ToShortDateString()}";
                        }
                        else
                        {
                            retorno.MensagemFormularioImpactoIndisponivel = $"Avaliação expirada";
                        }
                        return retorno;
                    }

                    retorno.SeqFormulario = configuracoesFormulario.SeqFormularioSgf;
                    retorno.DataFimResposta = configuracoesFormulario.DataFimFormulario;
                    retorno.SeqVisaoSGF = configuracoesFormulario.SeqVisaoSgf;
                    retorno.SeqInscricao = inscricoesOferta.FirstOrDefault().SeqInscricao;
                    retorno.ExibirFormularioImpacto = true;
                    retorno.MensagemInformativa = configuracoesFormulario.Mensagem;
                    retorno.DescricaoMensagemInformativa = configuracoesFormulario.Descricao;
                    return retorno;
                }
            }

            retorno.SeqFormulario = configuracoesFormulario.SeqFormularioSgf;
            retorno.DataFimResposta = configuracoesFormulario.DataFimFormulario;
            retorno.SeqVisaoSGF = configuracoesFormulario.SeqVisaoSgf;
            retorno.SeqInscricao = inscricoesOferta.FirstOrDefault().SeqInscricao;
            retorno.ExibirFormularioImpacto = true;
            retorno.MensagemInformativa = configuracoesFormulario.Mensagem;
            retorno.DescricaoMensagemInformativa = configuracoesFormulario.Descricao;

            return retorno;
        }


        /// <summary>
        /// Busca um processo completo para edição
        /// </summary>
        public ProcessoData BuscarProcessoComTipoProcesso(long seqProcesso)
        {
            var retorno = this.SearchByKey<Processo, ProcessoData>(seqProcesso,
             IncludesProcesso.TipoProcesso | IncludesProcesso.Idiomas);

            return retorno;
        }
        #endregion Buscar processos

        #region Salvar processo

        /// <summary>
        /// Salva o processo validando todas as regras do caso de uso
        /// </summary>
        /// <param name="processo"></param>
        /// <returns></returns>
        public long SalvarProcesso(Processo processo)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {

                    var gestaoEventos = this.TipoProcessoDomainService.ConferirHabilitaGestaoEvento(processo.SeqTipoProcesso);

                    /*
                     * Se o Tipo de processo do Processo em questão Habilita gestão de eventos,
                     * existir Taxa associada ao Processo e o Evento SAE não tiver sido informado,
                     * abortar a operação e emitir a mensagem de erro:
                     * "Em processos que possuem gestão de eventos e taxa associada,
                     * o Evento SAE deve ser informado, para o controle orçamentário das receitas."  
                     */
                    if (gestaoEventos && processo.Taxas.Any() && processo.CodigoEventoSAE == null)
                    {
                        throw new ProcessoPossuiTaxaENaoPossuiEventoSaeException();
                    }

                    /*
                     Se o Tipo de processo do Processo em questão Habilita gestão de eventos, 
                     o Evento SAE tiver sido informado e não existir Taxa associada ao Processo,
                     abortar a operação e emitir a mensagem de erro:
                     "Em processos que possuem gestão de eventos e Evento SAE, a taxa deve ser informada."
                     */
                    if (gestaoEventos && processo.CodigoEventoSAE != null && !processo.Taxas.Any())
                    {
                        throw new ProcessoNaoPossuiTaxaEPossuiEventoSaeException();
                    }

                    var ehAlteracao = processo.Seq != 0;
                    if (ehAlteracao)
                    {
                        ExecutaAlteracoesProcesso(processo);
                    }
                    else
                    {
                        ValidarInclusaoProcesso(processo);
                    }

                    // Armazena os idiomas que foram adicionados ao processo.
                    var idiomasAdicionados = processo.Idiomas.Where(x => x.Seq == 0);

                    //Salvar Arquivo anexado
                    processo = this.PreencherValidacaoArquivo(processo);

                    //Mapeia campo inscrito
                    this.MapearCamposInscrito(processo);


                    this.SaveEntity(processo, new ProcessoValidator());


                    if (ehAlteracao)
                    {
                        if (idiomasAdicionados.Any())
                        {
                            // Adiciona os idiomas que foram adicionados ao processo nas notificações já existentes para este mesmo processo.
                            AdicionaNovosIdiomaNasNotificacoes(processo.Seq, idiomasAdicionados);
                        }
                    }

                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
                return processo.Seq;
            }
        }

        private void ValidaCamposPadronizadosExistentes(IList<ConfiguracaoModeloDocumento> configuracoesDocumento)
        {

            Dictionary<string, List<string>> camposErro = new Dictionary<string, List<string>>();

            foreach (var documento in configuracoesDocumento)
            {
                if (documento.ArquivoModelo.Conteudo != null)
                {
                    var listaCampos = SautinsoftHelper.FindFieldsMerge(documento.ArquivoModelo.Conteudo);
                    var camposNaoPadronizados = new List<string>();

                    foreach (var campo in listaCampos)
                    {
                        if (!CAMPOS_PADRONIZADOS.ARRAY_COM_TODOS_CAMPOS.Contains(campo))
                        {
                            camposNaoPadronizados.Add(campo);
                        }
                    }

                    if (camposNaoPadronizados.Any())
                    {
                        camposErro.Add((camposErro.Count() + 1) + "º - " + documento.ArquivoModelo.Nome, camposNaoPadronizados);
                    }
                }
            }
            if (camposErro.Any())
            {
                var mensagemErro = new StringBuilder();
                mensagemErro.AppendLine($"\nDocumento(s):");
                foreach (var erro in camposErro)
                {
                    mensagemErro.AppendLine($"\r\n{erro.Key} - Campos: {string.Join(", ", erro.Value)}");
                }

                throw new CamposNaoIdentificadosException(mensagemErro.ToString());
            }
        }

        //Valida se: O período do evento contempla o período de atividade configurado em todas as ofertas associadas ao processo.
        private bool PeriodoAtividadeUltrapassaPeriodoEvento(Processo processo)
        {
            var ofertas = this.OfertaDomainService.BuscarOfertasProcesso(processo.Seq);

            var retorno = ofertas.Any(a =>
            {
                if (a.DataInicioAtividade.HasValue && a.DataFimAtividade.HasValue)
                {

                    if (a.DataInicioAtividade.Value.Date < processo.DataInicioEvento.Value.Date ||
                        a.DataFimAtividade.Value.Date > processo.DataFimEvento.Value.Date)
                    {
                        return true;
                    }
                }
                return false;
            });

            return retorno;
        }
        private void MapearCamposInscrito(Processo processo)
        {
            ValidarConsistenciasCamposInscrito(processo);

            if (processo.Seq == 0)
            {
                return;
            }

            var camposInscritosInDB = this.SearchProjectionByKey(processo.Seq, p => p.CamposInscrito);
            //remove o que esta no banco e não esta na tela
            foreach (var item in camposInscritosInDB)
            {
                if (!processo.CamposInscrito.Any(a => a.CampoInscrito == item.CampoInscrito))
                {
                    ProcessoCampoInscritoDomainService.DeleteEntity(item);
                }
            }

            //adiciona o que esta na tela e não esta no banco
            foreach (var item in processo.CamposInscrito)
            {
                if (!camposInscritosInDB.Any(a => a.CampoInscrito == item.CampoInscrito))
                {
                    var itemSerSalvo = new ProcessoCampoInscrito();
                    itemSerSalvo.SeqProcesso = processo.Seq;
                    itemSerSalvo.CampoInscrito = item.CampoInscrito;

                    ProcessoCampoInscritoDomainService.SaveEntity(itemSerSalvo);
                }
            }

            //limpa o objeto uma vez que o tramento foi feito manualmente
            processo.CamposInscrito = null;
        }

        private Processo PreencherValidacaoArquivo(Processo model)
        {
            var arquivoAnexado = new ArquivoAnexado();

            foreach (var item in model.ConfiguracoesModeloDocumento)
            {

                if (item.ArquivoModelo.Conteudo == null)
                {
                    // Busacamos os dados do arquivo para preencher os dados obrigatórios
                    var spec = new SMCSeqSpecification<ArquivoAnexado>(item.SeqArquivoModelo);
                    var ArquivoModelo = ArquivoAnexadoDomainService.SearchByKey(spec);
                    arquivoAnexado.Conteudo = ArquivoModelo.Conteudo;
                    arquivoAnexado.Seq = ArquivoModelo.Seq;
                }
                else
                {
                    // Caso o arquivo do VO não seja nulo, devemos criar um novo arquivo
                    arquivoAnexado.Conteudo = item.ArquivoModelo.Conteudo;
                }
                arquivoAnexado.Nome = item.ArquivoModelo.Nome;
                arquivoAnexado.Tamanho = (int)item.ArquivoModelo.Tamanho;
                arquivoAnexado.Tipo = item.ArquivoModelo.Tipo;
                arquivoAnexado.Seq = item.ArquivoModelo.Seq;
                ArquivoAnexadoDomainService.SaveEntity(arquivoAnexado);

                item.SeqArquivoModelo = arquivoAnexado.Seq;

            }
            return model;
        }

        public SMCUploadFile BuscarArquivoAnexadoConfigurancaoEmissaoDocumento(long seqArquivo)
        {
            var arquivo = ArquivoAnexadoDomainService.SearchByKey(seqArquivo);

            if (arquivo == null)
            {
                throw new Exception("Arquivo não encontrado");
            }

            return arquivo.Transform<SMCUploadFile>();
        }

        private void ValidarConsistenciasCamposInscrito(Processo processo)
        {
            List<string> mensagemErro = new List<string>();
            var camposInscritoPorTipoProcesso = TipoProcessoCampoInscritoDomainService.BuscarTiposProcessoCamposInscritoPorTipoProcesso(processo.SeqTipoProcesso);
            foreach (var campo in camposInscritoPorTipoProcesso)
            {
                if (!processo.CamposInscrito.Any(a => a.CampoInscrito == campo.CampoInscrito))
                {
                    mensagemErro.Add(string.IsNullOrEmpty(campo.CampoInscrito.SMCGetDescription()) ? campo.CampoInscrito.ToString() : campo.CampoInscrito.SMCGetDescription());
                }
            }

            if (mensagemErro.Any())
            {
                string descricaoTipoProcesso = this.TipoProcessoDomainService.SearchProjectionByKey(processo.SeqTipoProcesso, p => p.Descricao);

                throw new CamposInscritoFaltandoPorTipoProcessoException(string.Join(", ", mensagemErro), descricaoTipoProcesso);
            }

            if (processo.CamposInscrito.Any(a => a.CampoInscrito == CampoInscrito.OrgaoEmissorIdentidade || a.CampoInscrito == CampoInscrito.UfIdentidade) && !processo.CamposInscrito.Any(a => a.CampoInscrito == CampoInscrito.NumeroIdentidade))
            {
                throw new CamposInscritoIncosistentesRGException();
            }

            if (processo.CamposInscrito.Any(a => a.CampoInscrito == CampoInscrito.Naturalidade) && !processo.CamposInscrito.Any(a => a.CampoInscrito == CampoInscrito.PaisOrigem))
            {
                throw new CamposInscritoIncosistentesNaturalidadeException();
            }
        }

        private void AdicionaNovosIdiomaNasNotificacoes(long seqProcesso, IEnumerable<ProcessoIdioma> idiomasAdicionados)
        {
            // Tratamento de adição de novos idiomas para notificações
            var processo = this.SearchByKey(
                new SMCSeqSpecification<Processo>(seqProcesso), x => x.ConfiguracoesNotificacao,
                x => x.Idiomas, x => x.ConfiguracoesNotificacao[0].TipoNotificacao,
                x => x.UnidadeResponsavel, x => x.UnidadeResponsavel.EnderecosEletronicos,
                x => x.EnderecosEletronicos);

            foreach (var config in processo.ConfiguracoesNotificacao)
            {
                var vo = this.ProcessoConfiguracaoNotificacaoDomainService.BuscarConfiguracaoNotificacao(config.Seq);
                foreach (var configIdioma in vo.ConfiguracoesEmail)
                {
                    configIdioma.ConfiguracaoNotificacao =
                        NotificacaoService.BuscarConfiguracaoNotificacaoEmail(configIdioma.ConfiguracaoNotificacao.Seq);
                }
                var email =
                    processo.EnderecosEletronicos
                    .FirstOrDefault(x => x.TipoEnderecoEletronico == TipoEnderecoEletronico.Email) == null ?
                    processo.UnidadeResponsavel.EnderecosEletronicos
                        .FirstOrDefault(x => x.TipoEnderecoEletronico == TipoEnderecoEletronico.Email).Descricao
                    : processo.EnderecosEletronicos
                        .FirstOrDefault(x => x.TipoEnderecoEletronico == TipoEnderecoEletronico.Email).Descricao;
                foreach (var idioma in idiomasAdicionados)
                {
                    vo.ConfiguracoesEmail.Add(new ConfiguracaoNotificacaoIdiomaVO
                    {
                        Idioma = idioma.Idioma,
                        ConfiguracaoNotificacao = new ConfiguracaoNotificacaoEmailData
                        {
                            DataInicioValidade = DateTime.Now,
                            TipoEnvioNotificacao = TipoEnvioNotificacao.Email,
                            SeqTipoNotificacao = config.SeqTipoNotificacao,
                            EmailOrigem = email,
                            NomeOrigem = processo.UnidadeResponsavel.Nome,
                            Mensagem = Resources.MessagesResource.Mensagem_Notificacao_Padrao,
                            Assunto = Resources.MessagesResource.Assunto_Notificacao_Padrao
                        }
                    });
                }
                ProcessoConfiguracaoNotificacaoDomainService.SalvarConfiguracaoNotificacao(vo);
            }
        }

        private void ValidarInclusaoProcesso(Processo processo)
        {
            ValidaCamposPadronizadosExistentes(processo.ConfiguracoesModeloDocumento);

            ValidarTipoProcessoDesativado(processo);

            ValidarTipoHierarquiaDesativada(processo);

            ValidarTipoDocumentoDesativado(processo);

            ValidaIdentidadeVisual(processo.SeqUnidadeResponsavelTipoProcessoIdVisual);

            if (processo.ConfiguracoesFormulario != null && processo.ConfiguracoesFormulario.Count() > 0)
            {
                processo.ConfiguracoesFormulario.ForEach(formulario => ValidarTipoFormularioDesativado(formulario, processo.SeqUnidadeResponsavel));
            }

            ValidarEvento(processo);

            ValidarTemplateProcesso(processo);

            var taxas = this.TipoProcessoDomainService
               .SearchProjectionByKey(new SMCSeqSpecification<TipoProcesso>(processo.SeqTipoProcesso), x => x.TiposTaxa);

            foreach (var taxa in processo.Taxas)
            {
                if (taxas.Any(x => x.SeqTipoTaxa == taxa.SeqTipoTaxa && !x.Ativo))
                {
                    var descricao = TipoTaxaDomainService.SearchProjectionByKey(new SMCSeqSpecification<TipoTaxa>(taxa.SeqTipoTaxa)
                        , x => x.Descricao);
                    throw new AlteracaoProcessoTipoTaxaDesativadaException(descricao);
                }
            }
            processo.UidProcesso = Guid.NewGuid();
        }

        private Processo BuscarProcessoAntigo(Processo processo)
        {
            var spec = new SMCSeqSpecification<Processo>(processo.Seq);
            var processoOld = this.SearchByKey(spec, IncludesProcesso.EtapasProcesso |
                                                     IncludesProcesso.Idiomas |
                                                     IncludesProcesso.Taxas |
                                                     IncludesProcesso.EtapasProcesso_Configuracoes |
                                                     IncludesProcesso.ConfiguracoesNotificacao |
                                                     IncludesProcesso.ConfiguracoesNotificacao_ConfiguracoesIdioma |
                                                     IncludesProcesso.ConfiguracoesNotificacao_ConfiguracoesIdioma_ProcessoIdioma |
                                                     IncludesProcesso.HierarquiasOferta |
                                                     IncludesProcesso.ConfiguracoesModeloDocumento |
                                                     IncludesProcesso.ConfiguracoesFormulario |
                                                     IncludesProcesso.EtapasProcesso_Configuracoes_Paginas_Idiomas |
                                                     IncludesProcesso.GruposOferta_Ofertas_Taxas |
                                                     IncludesProcesso.GruposOferta_Ofertas_Taxas_Taxa |
                                                     IncludesProcesso.GruposOferta_Ofertas_Taxas_Taxa_TipoTaxa
                                                     );
            return processoOld;
        }

        private void ExecutaAlteracoesProcesso(Processo processo)
        {
            var processoOld = BuscarProcessoAntigo(processo);

            if (processo.ConfiguracoesModeloDocumento.Any(a => a.Seq == 0 || a.ArquivoModelo.Conteudo != null))
            {
                ValidaCamposPadronizadosExistentes(processo.ConfiguracoesModeloDocumento);
            }

            if (processo.ConfiguracoesFormulario.Any())
            {

                var possuiFormularioInscricao = processoOld.EtapasProcesso.Any(ep =>
                                                          ep.Configuracoes.Any(c =>
                                                                 c.Paginas.Where(w => w.Token == TOKENS.PAGINA_FORMULARIO_INSCRICAO)
                                                                          .Any(p =>
                                                                 p.Idiomas.Any(i =>
                                          processo.ConfiguracoesFormulario.Any(cf => cf.SeqFormularioSgf == i.SeqFormularioSGF)))));

                if (possuiFormularioInscricao)
                {
                    throw new UsarMesmoFormularioException();
                }
            }

            processoOld.ConfiguracoesFormulario.ForEach(f => f.Processo = null);

            if (processoOld.EtapasProcesso != null && processoOld.EtapasProcesso.Count > 0)
            {
                if (processoOld.EtapasProcesso.Any(x => x.SituacaoEtapa == SituacaoEtapa.Liberada
                        && x.DataFimEtapa > DateTime.Today)
                && (processoOld.Descricao != processo.Descricao
                || processoOld.MaximoInscricoesPorInscrito != processo.MaximoInscricoesPorInscrito
                || processoOld.DataCancelamento != processo.DataCancelamento
                || processoOld.SeqEvento != processo.SeqEvento
                || VerificarMudancaIdiomaAlteracaoProcesso(processo, processoOld)
                || VerificarMudancaTaxaAlteracaoProcesso(processo, processoOld)))
                {
                    throw new AlteracaoProcessoInscricaoLiberadaException();
                }
            }

            if (processoOld.SeqUnidadeResponsavelTipoProcessoIdVisual != processo.SeqUnidadeResponsavelTipoProcessoIdVisual)
            {
                ValidaIdentidadeVisual(processo.SeqUnidadeResponsavelTipoProcessoIdVisual);
            }

            if (processo.SeqTemplateProcessoSGF != processoOld.SeqTemplateProcessoSGF)
            {
                if (processoOld.EtapasProcesso.Any())
                {
                    throw new AlteracaoProcessoTemplateProcessoException();
                }
                //Validar template ativo
                ValidarTemplateProcesso(processo);
            }

            if (processo.SeqTipoHierarquiaOferta != processoOld.SeqTipoHierarquiaOferta)
            {
                var specHierarquia = new HierarquiaOfertaFilterSpecification
                {
                    SeqProcesso = processo.Seq
                };
                if (HierarquiaOfertaDomainService.Count(specHierarquia) > 0)
                {
                    throw new AlteracaoProcessoHieraquiaCadastradaException();
                }
                ValidarTipoHierarquiaDesativada(processo);
            }

            if (processo.MaximoInscricoesPorInscrito > 0 && processoOld.MaximoInscricoesPorInscrito != processo.MaximoInscricoesPorInscrito)
            {
                var inscricaoSpec = new InscricaoFilterSpecification
                {
                    SeqProcesso = processoOld.Seq,
                };
                inscricaoSpec.SetOrderByDescending(x => x.DataInscricao);
                var dicInscritosProcesso = InscricaoDomainService.GroupBy(inscricaoSpec, x => x.SeqInscrito);
                if (dicInscritosProcesso.Any(x => x.Value.Count() > processo.MaximoInscricoesPorInscrito))
                {
                    var inscritoComMaisInscricoes = dicInscritosProcesso.OrderByDescending(o => o.Value.Count()).FirstOrDefault();

                    throw new AlteracaoProcessoMaximoInscricoesInvalidoException(inscritoComMaisInscricoes.Value.Count(), processo.MaximoInscricoesPorInscrito);
                }
            }

            if (processo.SeqTipoProcesso != processoOld.SeqTipoProcesso)
                ValidarTipoProcessoDesativado(processo);

            var taxasOfertasProcesso = processoOld.GruposOferta.SelectMany(x => x.Ofertas.SelectMany(oferta => oferta.Taxas)).ToList();

            VerificaAlteracaoTipoCobTaxaAssocPeriodoTaxaOferta(processoOld, processo.Taxas);

            var gruposTaxaProcesso = GrupoTaxaDomainService.BuscarGruposTaxaPorSeqProcesso(processoOld.Seq).ToList();

            if (gruposTaxaProcesso.Any())
            {
                VerificaAlteracaoTipoCobrTaxaAssocGrupoTaxa(gruposTaxaProcesso, processoOld.Taxas, processo.Taxas);

                VerificaGrupoTaxaComTaxaTipoCobrDistintos(gruposTaxaProcesso, processoOld.Taxas, processo.Taxas);
            }


            var tipoProcesso = this.TipoProcessoDomainService
                .SearchByKey(new SMCSeqSpecification<TipoProcesso>(processo.SeqTipoProcesso), x => x.TiposTaxa);
            foreach (var taxa in processo.Taxas)
            {
                ProcessaAlteraçõesTaxas(processoOld, tipoProcesso, taxa);
            }

            if (processo.SeqEvento != processoOld.SeqEvento)
            {
                var ofertaFilterSpec = new OfertaFilterSpecification() { SeqProcesso = processo.Seq };
                ofertaFilterSpec.SetOrderBy(x => x.Nome);
                var existeTaxa = OfertaDomainService.SearchProjectionBySpecification(ofertaFilterSpec,
                        x => x.Taxas.Any());
                if (existeTaxa.Any(f => f == true))
                {
                    throw new AlteracaoProcessoEventoTaxaCadastradaException();
                }
                ValidarEvento(processo);
            }

            // Tratamento de mudanças em idiomas do processo
            foreach (var etapa in processoOld.EtapasProcesso)
            {
                foreach (var config in etapa.Configuracoes)
                {
                    var voIdiomas = new IdiomasPaginasProcessoVO
                    {
                        IdiomasEmUso = processo.Idiomas.Select(x => x.Idioma).ToList(),
                        SeqConfiguracaoEtapa = config.Seq
                    };
                    this.ConfiguracaoEtapaDomainService.AlterarIdiomasPaginas(voIdiomas);
                }
            }

            if (processoOld.ConfiguracoesNotificacao.Any())
            {
                var idiomasExcluidos = processoOld.Idiomas
                    .Where(x => !processo.Idiomas.Any(i => i.Idioma == x.Idioma)).Select(x => x.Idioma);
                foreach (var not in processoOld.ConfiguracoesNotificacao)
                {
                    var configuracoesAExcluir = not.ConfiguracoesIdioma.Where(x => idiomasExcluidos.Contains(x.ProcessoIdioma.Idioma));
                    foreach (var config in configuracoesAExcluir)
                    {
                        this.ProcessoConfiguracaoNotificacaoIdiomaDomainService.ExcluirConfiguracaoNotificacaoIdioma(config);
                    }
                }
            }

            if (processo.DataInicioEvento != processoOld.DataInicioEvento || processo.DataFimEvento != processoOld.DataFimEvento)
            {
                //Valida se Período do Evento contempla o período das Atividades das ofertas do processo.
                if (TipoProcessoService.ConferirHabilitaGestaoEvento(processo.SeqTipoProcesso))
                {
                    if (PeriodoAtividadeUltrapassaPeriodoEvento(processo))
                    {
                        throw new PeriodoEventoAtividadeException();
                    }

                }
            }

            ValidaAlteracoes(processo, processoOld);

        }


        private void ValidaIdentidadeVisual(long seqUnidadeResponsavelTipoProcessoIdVisual)
        {
            var identidadeVisual = this.UnidadeResponsavelTipoProcessoDomainService.BuscarIdentidadeVisual(seqUnidadeResponsavelTipoProcessoIdVisual);

            if (!identidadeVisual.Ativo)
            {
                throw new IdVisualDesativadaException(identidadeVisual.Descricao);
            }
        }

        //Valida alterações ConfiguracoesModeloDocumento e ConfiguracoesFormulario
        private void ValidaAlteracoes(Processo processoAtual, Processo processoAntigo)
        {
            // Verifica se o tipo de processo foi alterado.
            if (processoAtual.SeqTipoProcesso != processoAntigo.SeqTipoProcesso)
            {
                ExecutaAlteracaoTipoProcessoSituacao(processoAtual);
            }

            //Verifica se a Master Detail Configuração Modelo Documento teve alteração no Tipo de documento
            foreach (var item in processoAtual.ConfiguracoesModeloDocumento)
            {
                if (!processoAntigo.ConfiguracoesModeloDocumento.Where(w => w.SeqTipoDocumento == item.SeqTipoDocumento).Any())
                {
                    ValidarTipoDocumentoDesativado(processoAtual);
                }
            }


            if (processoAtual.ConfiguracoesFormulario != null)
            {
                //Verifica se a Master Detail Formulário teve alteração no formulário
                foreach (var formulario in processoAtual.ConfiguracoesFormulario)
                {
                    //Se o formulario existir o seq é diferente de zero. Serve para editar/incluir
                    if (formulario.Seq != 0)
                    {
                        //busca o formulario antigo para conferir a situação
                        var configuracaoFormulario = processoAntigo.ConfiguracoesFormulario.Where(w => w.Seq == formulario.Seq && w.SeqFormularioSgf != formulario.SeqFormularioSgf).FirstOrDefault();
                        if (configuracaoFormulario != null)
                        {
                            ConferirSituacaoInscricoesFormulario(configuracaoFormulario);

                            if (configuracaoFormulario.SeqFormularioSgf != formulario.SeqFormularioSgf)
                            {
                                ValidarTipoFormularioDesativado(formulario, processoAtual.SeqUnidadeResponsavel);
                            }
                        }
                    }
                    else
                    {
                        ValidarTipoFormularioDesativado(formulario, processoAtual.SeqUnidadeResponsavel);

                    }
                }

                //Consistencia no caso de exlusão do formulario
                var diferenca = processoAntigo.ConfiguracoesFormulario.Select(s => s.Seq).Except(processoAtual.ConfiguracoesFormulario.Select(s => s.Seq)).ToList();

                if (diferenca != null && diferenca.Count() > 0)
                {
                    var formularioExcluido = processoAntigo.ConfiguracoesFormulario.Where(w => diferenca.Contains(w.Seq)).ToList();

                    //Verifica se a Master Detail Formulário teve alteração no formulário no caso de EXCLUSÃO
                    foreach (var formulario in formularioExcluido)
                    {
                        ConferirSituacaoInscricoesFormulario(formulario);
                    }
                }
            }
        }

        private void ExecutaAlteracaoTipoProcessoSituacao(Processo processo)
        {
            // Armazena o novo sequencial do tipo_processo_situacao
            var tipoProcessoSituacaoCache = new Dictionary<long, long>();

            // Altera o tipo processo situação da tabela inscricao_historico_situacao
            var hsSpec = new InscricaoHistoricoSituacaoPorProcessoSpecification() { SeqProcesso = processo.Seq };
            var historicoSituacoes = InscricaoHistoricoSituacaoDomainService.SearchBySpecification(hsSpec);
            foreach (var historicoSituacao in historicoSituacoes)
            {
                if (!tipoProcessoSituacaoCache.ContainsKey(historicoSituacao.SeqTipoProcessoSituacao))
                {
                    var seqNovoTipoProcessoSituacao = BuscaSeqTipoProcessoSituacao(processo, historicoSituacao.SeqTipoProcessoSituacao);
                    tipoProcessoSituacaoCache.Add(historicoSituacao.SeqTipoProcessoSituacao, seqNovoTipoProcessoSituacao);
                }

                // Modifica o seq do tipo processo situação para o novo valor
                historicoSituacao.SeqTipoProcessoSituacao = tipoProcessoSituacaoCache[historicoSituacao.SeqTipoProcessoSituacao];
                InscricaoHistoricoSituacaoDomainService.SaveEntity(historicoSituacao);
            }

            // Altera o tipo processo situação da tabela inscricao_oferta_historico_situacao
            var ohsSpec = new InscricaoOfertaHistoricoSituacaoPorProcessoSpecification() { SeqProcesso = processo.Seq };
            var ofertaHistoricoSituacoes = InscricaoOfertaHistoricoSituacaoDomainService.SearchBySpecification(ohsSpec);
            foreach (var ofertaHistoricoSituacao in ofertaHistoricoSituacoes)
            {
                if (!tipoProcessoSituacaoCache.ContainsKey(ofertaHistoricoSituacao.SeqTipoProcessoSituacao))
                {
                    var seqNovoTipoProcessoSituacao = BuscaSeqTipoProcessoSituacao(processo, ofertaHistoricoSituacao.SeqTipoProcessoSituacao);
                    tipoProcessoSituacaoCache.Add(ofertaHistoricoSituacao.SeqTipoProcessoSituacao, seqNovoTipoProcessoSituacao);
                }

                // Modifica o seq do tipo processo situação para o novo valor
                ofertaHistoricoSituacao.SeqTipoProcessoSituacao = tipoProcessoSituacaoCache[ofertaHistoricoSituacao.SeqTipoProcessoSituacao];
                InscricaoOfertaHistoricoSituacaoDomainService.SaveEntity(ofertaHistoricoSituacao);
            }
        }

        private long BuscaSeqTipoProcessoSituacao(Processo processo, long seqTipoProcessoSituacao)
        {
            var tipoProcessoSituacao = TipoProcessoSituacaoDomainService.SearchProjectionByKey(new SMCSeqSpecification<TipoProcessoSituacao>(seqTipoProcessoSituacao),
                                                                                                x => new { x.SeqSituacao, x.Token });

            // Busca a situação que corresponde ao mesmo seq_situacao_SGF e ao mesmo dsc_token_situacao do seq_tipo_processo_situacao
            // que estava setado nestas tabelas de histórico e que estava associado ao tipo de processo anterior.
            return TipoProcessoSituacaoDomainService.SearchProjectionByKey(
                                                new TipoProcessoSituacaoFilterSpecification()
                                                {
                                                    SeqSituacaoSGF = tipoProcessoSituacao.SeqSituacao,
                                                    Token = tipoProcessoSituacao.Token,
                                                    SeqTipoProcesso = processo.SeqTipoProcesso
                                                },
                                                x => x.Seq);
        }

        /// <summary>
        /// Verifica Alteração: O campo Tipo de cobrança não pode ser alterado se a Taxa estiver associada a algum Período de taxa da oferta do Processo.
        /// Ao violar esta regra, abortar a operação e emitir a mensagem de erro. 
        /// </summary>
        /// <param name="processoOld"></param>
        /// <param name="taxasLast"></param>
        /// <exception cref="AlteracaoProcessoTipoCobrancaTaxaAssociadaPeriodoTaxaOfertaException"></exception>
        private void VerificaAlteracaoTipoCobTaxaAssocPeriodoTaxaOferta(Processo processoOld, IList<Taxa> taxasLast)
        {

            if (processoOld != null && taxasLast.Any())
            {

                var seqsTaxasOfertasProcessoOld = processoOld.GruposOferta
                                                .SelectMany(grpo => grpo.Ofertas)
                                                .SelectMany(oferta => oferta.Taxas)
                                                .Select(ofpt => ofpt.SeqTaxa)
                                                .ToList();

                if (processoOld.Taxas.Any() && seqsTaxasOfertasProcessoOld.Any())
                {
                    var taxaAssociada = (from taxaOld in processoOld.Taxas
                                         from taxaLast in taxasLast
                                         where taxaOld.Seq == taxaLast.Seq
                                            && taxaOld.TipoCobranca != taxaLast.TipoCobranca
                                            && seqsTaxasOfertasProcessoOld.Contains(taxaLast.Seq)
                                         select taxaLast.TipoTaxa.Descricao
                                         ).FirstOrDefault();

                    if (!taxaAssociada.IsNullOrEmpty() && !taxaAssociada.IsNullOrWhiteSpace())
                        throw new AlteracaoProcessoTipoCobrancaTaxaAssociadaPeriodoTaxaOfertaException(taxaAssociada);

                }
            }
        }


        /// <summary>
        /// Verifica Alteração: O campo Tipo de cobrança não pode ser alterado de “Por inscrição” ou “Por oferta” para "Por quantidade de ofertas"
        /// se a Taxa fizer parte de um Grupo de taxas do Processo.
        /// Ao violar esta regra, abortar a operação e emitir mensagem de erro.
        /// </summary>
        /// <param name="gruposTaxaProcesso"></param>
        /// <param name="taxasOld"></param>
        /// <param name="taxasLast"></param>
        /// <exception cref="AlteracaoProcessoTipoCobrancaTaxaAssociadaGrupoTaxaException"></exception>
        private void VerificaAlteracaoTipoCobrTaxaAssocGrupoTaxa(List<GrupoTaxa> gruposTaxaProcesso, IList<Taxa> taxasOld, IList<Taxa> taxasLast)
        {
            if (gruposTaxaProcesso.Any() && taxasLast.Any() && taxasOld.Any())
            {
                //var gruposTaxaProcesso = GrupoTaxaDomainService.BuscarGruposTaxaPorSeqProcesso(processoOld.Seq).ToList();

                var seqsTaxasGruposTaxaProcessoOld = gruposTaxaProcesso
                                                    .SelectMany(grpt => grpt.Itens
                                                    .Select(x => x.SeqTaxa))
                                                    .ToList();

                if (seqsTaxasGruposTaxaProcessoOld.Any())
                {

                    //O campo Tipo de cobrança não pode ser alterado de “Por inscrição” ou “Por oferta” para "Por quantidade de ofertas"
                    //se a Taxa fizer parte de um Grupo de taxas do Processo.
                    //Ao violar esta regra, abortar a operação e emitir a mensagem de erro:

                    //“Operação não permitida.A taxa<Descrição do Tipo de taxa> está associada ao grupo de taxas <Descrição do grupo de taxa>.
                    //Taxas cujo tipo de cobrança é por quantidade de ofertas não podem fazer parte de um grupo de taxas.


                    var grupoTaxaeTipoTaxaAssociada = (from taxaOld in taxasOld
                                                       from taxaLast in taxasLast
                                                       from grupoTaxa in gruposTaxaProcesso
                                                       from taxaGrupo in grupoTaxa.Itens
                                                       where taxaGrupo.SeqTaxa == taxaLast.Seq
                                                          && (taxaOld.TipoCobranca == TipoCobranca.PorInscricao ||
                                                              taxaOld.TipoCobranca == TipoCobranca.PorOferta)
                                                          && taxaLast.TipoCobranca == TipoCobranca.PorQuantidadeOfertas
                                                       select new
                                                       {
                                                           DescTipoTaxaAssociada = taxaLast.TipoTaxa.Descricao,
                                                           DescGrupoTaxa = grupoTaxa.Descricao
                                                       }

                                        ).FirstOrDefault();

                    if (grupoTaxaeTipoTaxaAssociada != null)
                        throw new AlteracaoProcessoTipoCobrancaTaxaAssociadaGrupoTaxaException(grupoTaxaeTipoTaxaAssociada.DescTipoTaxaAssociada, grupoTaxaeTipoTaxaAssociada.DescGrupoTaxa);

                }

            }
        }

        /// <summary>
        /// Verifica Alteração: O campo Tipo de cobrança não pode ser alterado se a Taxa fizer parte de algum Grupo de taxas do Processo e,
        /// ao final da operação, seu respectivo Grupo de taxas tiver Taxas com Tipos de cobrança distintos.
        /// Ao violar esta regra, abortar a operação e emitir a mensagem de erro.
        /// </summary>
        /// <param name="gruposTaxaProcesso"></param>
        /// <param name="taxasOld"></param>
        /// <param name="taxasLast"></param>
        /// <exception cref="AlteracaoProcessoGrupoTaxaComTaxaTipoCobrDistintosException"></exception>
        private void VerificaGrupoTaxaComTaxaTipoCobrDistintos(List<GrupoTaxa> gruposTaxaProcesso, IList<Taxa> taxasOld, IList<Taxa> taxasLast)
        {
            //Obter taxas alteradas
            if (gruposTaxaProcesso.Any() && taxasOld.Any() && taxasLast.Any())
            {
                if (gruposTaxaProcesso.Any())
                {
                    var taxasAlteradas = (from taxaOld in taxasOld
                                          from taxaLast in taxasLast
                                          where ((taxaLast.TipoCobranca != taxaOld.TipoCobranca) && (taxaLast.Seq == taxaOld.Seq))//Mudou Tipo Cobrança?                                                 
                                          select (taxaLast)
                                          ).ToList();

                    if (taxasAlteradas.Any())
                    {

                        //Se houve alteração, 
                        //Listar os grupos de taxa do processo que tem essas taxas
                        var gruposTaxaComTaxasAlteradas = (from taxaAlterada in taxasAlteradas
                                                           from grupoTaxa in gruposTaxaProcesso
                                                           from taxaGrupo in grupoTaxa.Itens
                                                           where taxaGrupo.SeqTaxa == taxaAlterada.Seq //Pertence ao grupoTaxa
                                                           select (grupoTaxa)
                                                           ).ToList();


                        if (gruposTaxaComTaxasAlteradas.Any())
                        {
                            //Atualizar os gruposTaxa apenas em memoria conforme taxasAlteradas para checar 
                            //como o grupoTaxa ficará
                            foreach (var grupo in gruposTaxaComTaxasAlteradas)
                            {
                                grupo.Itens
                                    .Join(taxasAlteradas,
                                            grupoTaxaItem => grupoTaxaItem.SeqTaxa,
                                            taxaAlterada => taxaAlterada.Seq,
                                            (grupoTaxaItem, taxaAlterada) => new { grupoTaxaItem, taxaAlterada })
                                    .ToList()
                                    .ForEach(par =>
                                    {
                                        if (par.grupoTaxaItem.SeqTaxa == par.taxaAlterada.Seq)
                                            par.grupoTaxaItem.Taxa.TipoCobranca = par.taxaAlterada.TipoCobranca;
                                    });

                                //Grupo de taxas tiver Taxas com Tipos de cobrança distintos.                                    
                                var grupoTemTaxasComTipoCobrancaDistintos = grupo.Itens
                                                                              .Select(i => i.Taxa.TipoCobranca)
                                                                              .Distinct().Count() > 1;

                                if (grupoTemTaxasComTipoCobrancaDistintos)
                                    throw new AlteracaoProcessoGrupoTaxaComTaxaTipoCobrDistintosException();


                            }
                        }
                    }
                }

            }
        }

        private void ProcessaAlteraçõesTaxas(Processo processoOld, TipoProcesso tipoProcesso, Taxa taxa)
        {
            if (tipoProcesso.TiposTaxa.Any(x => x.SeqTipoTaxa == taxa.SeqTipoTaxa && !x.Ativo)
                                            && (taxa.Seq == 0 || (processoOld.Taxas.Any(x => x.Seq == taxa.Seq && x.SeqTipoTaxa != taxa.SeqTipoTaxa))))
            {
                var descricao = TipoTaxaDomainService.SearchProjectionByKey(new SMCSeqSpecification<TipoTaxa>(taxa.SeqTipoTaxa)
                    , x => x.Descricao);
                throw new AlteracaoProcessoTipoTaxaDesativadaException(descricao);
            }

            // Valida alteração do campo CobrarPorOferta
            if (taxa.Seq != 0)
            {
                var taxaOld = processoOld.Taxas.Where(f => f.Seq == taxa.Seq).FirstOrDefault();

                //if (taxaOld.TipoCobranca != taxa.TipoCobranca)
                //{
                //    var possuiInscricoes = this.InscricaoDomainService.BuscarInscricoesQuePossuiTaxaAssociadaAoBoleto(filtro: new InscricaoFilterSpecification { SeqProcesso = processoOld.Seq, SeqTaxa = taxa.Seq });

                //    if (possuiInscricoes)
                //    {
                //        throw new ExisteInscricaoComTaxaDentreOsBoletos(taxa.TipoTaxa.Descricao);
                //    }
                //}

                if (taxaOld != null && taxa.TipoCobranca != TipoCobranca.Nenhum)
                {
                    // Busca todas as ofertas taxas com o mesmo tipo de taxa
                    var spec = new OfertaTipoTaxaSpecification()
                    {
                        SeqProcesso = processoOld.Seq,
                        SeqTipoTaxa = taxaOld.SeqTipoTaxa
                    };
                    var ofertaTaxas = OfertaPeriodoTaxaDomainService.SearchBySpecification(spec).ToList();

                    if (taxaOld.SeqTipoTaxa != taxa.SeqTipoTaxa && ofertaTaxas.Any())
                    {
                        throw new TaxaAssaociadaAOfertaException(taxa.TipoTaxa.Descricao);
                    }

                    // Verifica se taxas foram alteradas de outro tipo para "Por quantidade de ofertas"
                    if (taxaOld.TipoCobranca != TipoCobranca.PorQuantidadeOfertas && taxa.TipoCobranca == TipoCobranca.PorQuantidadeOfertas)
                    {

                        //TODO Lembrete para quando a Maira alterar a regra, tem que fazer ajuste no VerificaDivergenciaEntreTaxasAssociadasNasOfertas()
                        //if (VerificarOfertaTaxaDeValoresDiferentesNaMesmaData(ofertaTaxas)
                        //    || VerificaDivergenciaEntreTaxasAssociadasNasOfertas(processoOld))
                        //{
                        //    throw new AlterarCobrancaPorOfertaException();
                        //}

                        //if (VerificarOfertaTaxaDeValoresDiferentesNaMesmaData(ofertaTaxas))
                        //{
                        //    throw new AlterarCobrancaPorOfertaException();
                        //}

                        foreach (var ofertaTaxa in ofertaTaxas)
                        {
                            // Apaga os valores de máximo e mínimo da taxa alterada
                            ofertaTaxa.NumeroMaximo = null;
                            ofertaTaxa.NumeroMinimo = null;
                            OfertaPeriodoTaxaDomainService.SaveEntity(ofertaTaxa);
                        }
                    }
                }
            }
        }

        #endregion Salvar processo

        #region Validações

        public bool VerificarOfertaTaxaDeValoresDiferentesNaMesmaData(IList<OfertaPeriodoTaxa> ofertaTaxas)
        {
            var seqEventosTaxa = ofertaTaxas.Select(f => f.SeqEventoTaxa).Distinct();
            var eventosTaxa = FinanceiroService.BuscarEventosTaxaPorSeq(seqEventosTaxa.ToArray());

            // Verifica se as taxas das ofertas possuem datas sobrepostas
            for (int i = 0; i < ofertaTaxas.Count; i++)
            {
                for (int g = i + 1; g < ofertaTaxas.Count; g++)
                {
                    // Verifica apenas as ofertax taxa de outras ofertas
                    if (ofertaTaxas[i].SeqOferta != ofertaTaxas[g].SeqOferta)
                    {
                        // Verifica se os períodos coincidem
                        if (SMCDateTimeHelper.HasOverlap(true,
                                            new SMCDateOverlap(ofertaTaxas[i].DataInicio, ofertaTaxas[i].DataFim),
                                            new SMCDateOverlap(ofertaTaxas[g].DataInicio, ofertaTaxas[g].DataFim)))
                        {
                            // Se possuirem taxas sobrepostas, verifica se o valor é diferente.
                            var eventoTaxa1 = eventosTaxa.FirstOrDefault(f => f.SeqEventoTaxa == ofertaTaxas[i].SeqEventoTaxa);
                            var eventoTaxa2 = eventosTaxa.FirstOrDefault(f => f.SeqEventoTaxa == ofertaTaxas[g].SeqEventoTaxa);
                            if (eventoTaxa1.Valor != eventoTaxa2.Valor)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Verifica se há ofertas que possuem taxas associadas e outras que não possuem.
        /// </summary>
        private bool VerificaDivergenciaEntreTaxasAssociadasNasOfertas(Processo processoOld)
        {
            var taxas = SearchProjectionByKey(new SMCSeqSpecification<Processo>(processoOld.Seq),
                                            x => x.HierarquiasOferta.Select(f => (f as Oferta).Taxas)).ToList();

            if (taxas.Count == 0)
                return false;

            bool possuiTaxaBase = taxas[0].Count > 0;

            foreach (var oferta in taxas)
            {
                var possuiTaxa = oferta.Count > 0;
                if (possuiTaxaBase != possuiTaxa)
                    return true;
            }
            return false;
        }

        private void ValidarTemplateProcesso(Processo processo)
        {
            var templates = this.TipoProcessoDomainService
              .SearchProjectionByKey(new SMCSeqSpecification<TipoProcesso>(processo.SeqTipoProcesso),
                x => x.Templates);
            var templateSelecionado = templates.FirstOrDefault(x => x.SeqTemplateProcessoSGF == processo.SeqTemplateProcessoSGF);
            if (!templateSelecionado.Ativo)
            {
                throw new ProcessoTemplateDesativadoException();
            }
        }

        private void ValidarTipoProcessoDesativado(Processo processo)
        {
            var tipoProcessoSpec = new UnidadeResponsavelTipoProcessoFilterSpecification
            {
                SeqTipoProcesso = processo.SeqTipoProcesso,
                SeqUnidadeResponsavel = processo.SeqUnidadeResponsavel
            };
            var ativo = this.UnidadeResponsavelTipoProcessoDomainService.SearchProjectionByKey(tipoProcessoSpec,
                x => x.Ativo);
            if (!ativo)
            {
                throw new AlteracaoProcessoTipoProcessoDesativadoException();
            }
        }

        private void ValidarTipoHierarquiaDesativada(Processo processo)
        {
            var tipoProcessoSpec = new UnidadeResponsavelTipoProcessoFilterSpecification
            {
                SeqTipoProcesso = processo.SeqTipoProcesso,
                SeqUnidadeResponsavel = processo.SeqUnidadeResponsavel
            };
            var desativado = this.UnidadeResponsavelTipoProcessoDomainService.SearchProjectionByKey(tipoProcessoSpec,
               x => x.TiposHierarquiaOferta.Any(t => t.SeqTipoHierarquiaOferta == processo.SeqTipoHierarquiaOferta
                    && !t.Ativo));
            if (desativado)
            {
                throw new AlteracaoProcessoTipoHierarquiaDesativadoException();
            }
        }

        private void ValidarTipoDocumentoDesativado(Processo processo)
        {
            foreach (var item in processo.ConfiguracoesModeloDocumento)
            {
                var situacaoTipoDocumento = TipoProcessoDomainService.BuscarSituacaoTipoDocumentoDoProcesso(processo.SeqTipoProcesso, item.SeqTipoDocumento);
                if (situacaoTipoDocumento == true)
                {
                    throw new TipoDocumentoDesativadoException();
                }
            }
        }

        private void ValidarTipoFormularioDesativado(ConfiguracaoFormulario formulario, long seqUnidadeResponsavel)
        {

            var tipoFormulario = FormularioService.BuscarFormulario(formulario.SeqFormularioSgf, IncludesFormulario.Nenhum);
            var seqTipoFormularioSGF = tipoFormulario.SeqTipoFormulario;
            var situacaoTipoFormulario = UnidadeResponsavelDomainService.BuscarSituacaoTipoFormularioDaUnidadeResponsavel(seqUnidadeResponsavel, seqTipoFormularioSGF);
            if (situacaoTipoFormulario == true)
            {
                throw new TipoFormularioDesativadoException();
            }
        }

        //Caso existam inscrições como formulário respondido, e todas estas inscrições estejam na situação INSCRICAO_CANCELADA, com o motivo da situação INSCRICAO_CANCELADA_TESTE, chame o assert
        public bool ValidarFormulariosAssert(Processo processo)
        {
            bool retorno = false;
            var processoAntigo = BuscarProcessoAntigo(processo);

            //A validação deve ser feita pelo formulario antigo e não pelo novo
            foreach (var formulario in processo.ConfiguracoesFormulario)
            {

                if (formulario.Seq != 0)
                {
                    //busca o formulario antigo para conferir a situação
                    var configuracaoFormularioAntigo = processoAntigo.ConfiguracoesFormulario.Where(w => w.Seq == formulario.Seq && w.SeqFormularioSgf != formulario.SeqFormularioSgf).FirstOrDefault();
                    if (configuracaoFormularioAntigo != null)
                    {
                        var specInscricao = new InscricaoFilterSpecification() { SeqFormulario = configuracaoFormularioAntigo.SeqFormularioSgf, SeqProcesso = processo.Seq, FormularioRespondido = true };
                        var inscricoes = InscricaoDomainService.SearchProjectionBySpecification(specInscricao, i => new
                        {
                            i.Seq,
                            TokenProcessoSituacaoHistoricoAtual = i.HistoricosSituacao.FirstOrDefault(h => h.Atual).TipoProcessoSituacao.Token,
                            SeqMotivoSituacaoSGF = i.HistoricosSituacao.FirstOrDefault(h => h.Atual).SeqMotivoSituacaoSGF,
                        }).ToList();

                        if (inscricoes != null && inscricoes.Count() > 0)
                        {
                            //Verifica se todas inscrições estão com situação CANCELADA
                            if (inscricoes.All(h => h.TokenProcessoSituacaoHistoricoAtual == TOKENS.SITUACAO_INSCRICAO_CANCELADA))
                            {
                                // busca todos os motivos que estejam com situação canceladacanceladaTeste
                                var listaSeqMotivo = SituacaoService.BuscarSeqMotivosSituacaoPorToken(TOKENS.MOTIVO_INSCRICAO_CANCELADA_TESTE);

                                bool canceladaTeste = false;
                                foreach (var item in inscricoes)
                                {
                                    if (item.SeqMotivoSituacaoSGF.HasValue)
                                    {
                                        canceladaTeste = listaSeqMotivo.Contains(item.SeqMotivoSituacaoSGF.Value);
                                    }
                                    else
                                        canceladaTeste = true;

                                    //Nem todas são CANCELADA_TESTE
                                    if (!canceladaTeste)
                                        return false;
                                }

                                //Todas são CANCELADA_TESTE
                                if (canceladaTeste)
                                {
                                    retorno = true;
                                }
                            }
                        }
                    }

                }
            }

            return retorno;
        }

        public bool ValidarAssertDocumentoEmitido(Processo model)
        {
            var specInscricao = new InscricaoFilterSpecification()
            {
                SeqProcesso = model.Seq,
            };

            //Busca os seq das inscrições relacionadas ao processo sendo alterado
            var inscricoes = InscricaoDomainService.SearchProjectionBySpecification(specInscricao, i => i.Seq).ToList();

            var specDocEmitido = new InscricaoDocumentoEmitidoFilterSpecification()
            {
                SeqInscricoes = inscricoes
            };

            // Busca documentos com os seq das inscrições informados
            var documentosEmitidos = InscricaoDocumentoEmitidoDomainService.SearchProjectionBySpecification(specDocEmitido, x => new { x.Seq, x.SeqDocumentoGad }).ToList();

            return documentosEmitidos.Count() > 0;
        }

        //Caso existam inscrições como formulário respondido, que não esteja na situação INSCRICAO_CANCELADA, ou esteja, mas o motivo da situação não é INSCRICAO_CANCELADA_TESTE, chame a exception
        public void ConferirSituacaoInscricoesFormulario(ConfiguracaoFormulario formulario)
        {

            var specInscricao = new InscricaoFilterSpecification() { SeqFormulario = formulario.SeqFormularioSgf, SeqProcesso = formulario.SeqProcesso, FormularioRespondido = true };
            specInscricao.SetOrderByDescending(x => x.DataInscricao);
            var inscricoes = InscricaoDomainService.SearchProjectionBySpecification(specInscricao, i => new
            {
                i.Seq,
                SeqFormulario = i.Formularios.Select(x => x.SeqFormulario),
                TokenProcessoSituacaoHistoricoAtual = i.HistoricosSituacao.FirstOrDefault(h => h.Atual).TipoProcessoSituacao.Token,
                SeqMotivoSituacaoSGF = i.HistoricosSituacao.FirstOrDefault(h => h.Atual).SeqMotivoSituacaoSGF,

            }).ToList();

            // busca todos os motivos que estejam com situação canceladacanceladaTeste
            var listaSeqMotivo = SituacaoService.BuscarSeqMotivosSituacaoPorToken(TOKENS.MOTIVO_INSCRICAO_CANCELADA_TESTE);

            if (inscricoes != null && inscricoes.Count() > 0)
            {
                foreach (var item in inscricoes)
                {

                    //verifica se o token é INSCRICAO_CANCELADA
                    bool cancelada = item.TokenProcessoSituacaoHistoricoAtual == TOKENS.SITUACAO_INSCRICAO_CANCELADA;

                    if (!cancelada)
                    {
                        throw new InscricoesValidasFormularioPreenchidoException();
                    }

                    if (item.SeqMotivoSituacaoSGF.HasValue)
                    {
                        //verifica se o motivo é INSCRICAO_CANCELADA_TESTE
                        bool canceladaTeste = listaSeqMotivo.Contains(item.SeqMotivoSituacaoSGF.Value);

                        /*Ao trocar o formulário associado à página, caso existam inscrições como formulário respondido, que 
                          NÃO esteja na situação INSCRICAO_CANCELADA, ou esteja, mas o motivo da situação NÃO é 
                          INSCRICAO_CANCELADA_TESTE */
                        if (cancelada && !canceladaTeste)
                        {
                            throw new InscricoesValidasFormularioPreenchidoException();
                        }
                    }
                }
            }

        }

        private void ValidarEvento(Processo processo)
        {
            if (processo.SeqEvento.HasValue)
            {
                var evento = this.FinanceiroService.BuscarEvento(processo.SeqEvento.Value);
                if (evento.DataFimValidade.Date < DateTime.Today)
                {
                    throw new ProcessoEventoVencidoException();
                }
                var unidadeSpec = new SMCSeqSpecification<UnidadeResponsavel>(processo.SeqUnidadeResponsavel);
                var desativado = this.UnidadeResponsavelDomainService.SearchProjectionByKey(unidadeSpec,
                  x => x.CentrosCusto.Any(c => c.CodigoCentroCusto == evento.CodigoCentroCusto.Value && !c.Ativo));
                if (desativado)
                {
                    throw new ProcessoCentroCustoDesativadoException();
                }
            }
        }

        /// <summary>
        /// Retorna true se a validação for inválida
        /// </summary>
        private bool VerificarMudancaIdiomaAlteracaoProcesso(Processo novoProcesso, Processo processoBanco)
        {
            if (novoProcesso.Idiomas.Count != processoBanco.Idiomas.Count) return true;
            var idiomasOld = processoBanco.Idiomas.OrderBy(x => x.Seq).ToArray();
            var idiomasNovo = novoProcesso.Idiomas.OrderBy(x => x.Seq).ToArray();
            for (int i = 0; i < novoProcesso.Idiomas.Count; i++)
            {
                if (idiomasNovo[i].Idioma != idiomasOld[i].Idioma
                    || idiomasNovo[i].DescricaoComplementar != idiomasOld[i].DescricaoComplementar
                    || idiomasNovo[i].LabelCodigoAutorizacao != idiomasOld[i].LabelCodigoAutorizacao
                    || idiomasNovo[i].LabelGrupoOferta != idiomasOld[i].LabelGrupoOferta
                    || idiomasNovo[i].LabelOferta != idiomasOld[i].LabelOferta)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Retorna true se a validação for inválida
        /// </summary>
        private bool VerificarMudancaTaxaAlteracaoProcesso(Processo novoProcesso, Processo processoBanco)
        {
            if (novoProcesso.Taxas.Count != processoBanco.Taxas.Count) return true;
            var taxasOld = processoBanco.Taxas.OrderBy(x => x.Seq).ToArray();
            var taxasNovo = novoProcesso.Taxas.OrderBy(x => x.Seq).ToArray();
            for (int i = 0; i < novoProcesso.Taxas.Count; i++)
            {
                if (taxasNovo[i].SeqTipoTaxa != taxasOld[i].SeqTipoTaxa
                    || taxasNovo[i].SelecaoInscricao != taxasOld[i].SelecaoInscricao
                    || taxasNovo[i].TipoCobranca != taxasOld[i].TipoCobranca)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Verifica se uma das raizes da hierarquia de ofertas do processo é oferta
        /// </summary>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        public bool VerificarRaizHierarquiaOfertaPermiteOferta(long seqProcesso)
        {
            return this.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso),
                x => x.TipoHierarquiaOferta.Itens.Any(i => !i.SeqPai.HasValue && i.HabilitaCadastroOferta));
        }

        /// <summary>
        /// Verifica se uma das raizes da hierarquia de ofertas do processo é oferta
        /// </summary>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        public bool VerificarRaizHierarquiaOfertaPermiteItem(long seqProcesso)
        {
            return this.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso),
                x => x.TipoHierarquiaOferta.Itens.Any(i =>
                    (!i.SeqPai.HasValue && i.HabilitaCadastroOferta && i.ItensHierarquiaOfertaFihos.Any())
                    || (!i.SeqPai.HasValue && !i.HabilitaCadastroOferta)));
        }

        #endregion Validações

        public IEnumerable<SMCDatasourceItem> BuscarTiposItemHierarquiaOfertaKeyValue(long seqProcesso,
            long? SeqItemSuperior, bool HabilitaCadastroOferta)
        {
            var seqPai = SeqItemSuperior.HasValue ? this.HierarquiaOfertaDomainService.SearchProjectionByKey(
                new SMCSeqSpecification<HierarquiaOferta>(SeqItemSuperior.Value), x => x.SeqItemHierarquiaOferta)
                : new Nullable<long>();
            return this.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso),
                x => x.TipoHierarquiaOferta.Itens
                    .Where(i => i.SeqPai == seqPai && ((HabilitaCadastroOferta && i.HabilitaCadastroOferta) || (!HabilitaCadastroOferta))).Select(h => new SMCDatasourceItem
                    {
                        Seq = h.Seq,
                        Descricao = h.TipoItemHierarquiaOferta.Descricao,
                    }));
        }

        public List<SMCDatasourceItem> BuscarProcessoSelect(ProcessoSelectFilterSpecification filtro)
        {
            var spec = filtro.SetOrderByDescending(o => o.AnoReferencia)
                                .SetOrderByDescending(o => o.SemestreReferencia)
                                .SetOrderBy(o => o.Descricao);

            var lista = this.SearchProjectionBySpecification(spec, x => new SMCDatasourceItem
            {
                Seq = x.Seq,
                Descricao = x.Descricao
            }).ToList();
            return lista;
        }

        #region Copia de processo

        /// <summary>
        /// Realiza a cópia completa de processo de acordo com os parâmetros do VO
        /// </summary>
        /// <param name="copiaProcessoVO"></param>
        public CopiaProcessoRetornoVO CopiarProcesso(CopiaProcessoVO copiaProcessoVO)
        {
            var retorno = new CopiaProcessoRetornoVO() { SeqProcessoOrigem = copiaProcessoVO.SeqProcessoOrigem, ProcessosGpi = new Dictionary<long, long?>(), ItensOfertasHierarquiasOfertas = new Dictionary<long, long?>() };

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    //5.Se o item “Hierarquia de Oferta e Grupo de Oferta” for selecionado, copiar toda a hierarquia,
                    //suas ofertas, os grupos aos quais elas pertencem, seus telefones, endereços eletrônicos e códigos de autorização.
                    //Copiar todos os dados das ofertas, com exceção das datas início e fim, que deverão ser preenchidas respectivamente
                    //com as datas início e fim de inscrição informadas, da data de cancelamento e o motivo que deverão ser
                    //setados para null, do indicador de ativo que deverá ser "sim", das taxas que não deverão ser copiadas e do
                    //indicador exige pagamento de taxa que deverá ser "não"*******.Ao copiar os grupos de ofertas, associá - los
                    //às novas configurações da etapa.

                    var spec = new SMCSeqSpecification<Processo>(copiaProcessoVO.SeqProcessoGpi);
                    var processo = SearchByKey(spec, IncludesProcesso.Idiomas |
                                                        IncludesProcesso.EnderecosEletronicos |
                                                        IncludesProcesso.EtapasProcesso |
                                                        IncludesProcesso.EtapasProcesso_Configuracoes |
                                                        IncludesProcesso.GruposOferta |
                                                        IncludesProcesso.HierarquiasOferta |
                                                        IncludesProcesso.Taxas |
                                                        IncludesProcesso.Telefones |
                                                        IncludesProcesso.ConfiguracoesNotificacao |
                                                        IncludesProcesso.ConfiguracoesNotificacao_ConfiguracoesIdioma |
                                                        IncludesProcesso.ConfiguracoesNotificacao_ParametrosEnvioNotificacao |
                                                        IncludesProcesso.ConfiguracoesNotificacao_TipoNotificacao |
                                                        IncludesProcesso.ConfiguracoesFormulario
                                                        );
                    var etapasProcesso = processo.EtapasProcesso;
                    var notificacoes = processo.ConfiguracoesNotificacao;
                    var idiomasProcessoOld = processo.Idiomas.SMCClone();
                    var formularios = processo.ConfiguracoesFormulario;

                    processo.Seq = 0;
                    processo.Descricao = copiaProcessoVO.NovoProcessoDescricao;
                    processo.AnoReferencia = copiaProcessoVO.NovoProcessoAnoReferencia;
                    processo.SemestreReferencia = copiaProcessoVO.NovoProcessoSemestreReferencia;
                    processo.UidProcesso = Guid.NewGuid();
                    processo.DataCancelamento = null;
                    processo.DataEncerramento = null;
                    processo.SeqEvento = null;
                    processo.EtapasProcesso = null;
                    processo.ConfiguracoesNotificacao = null;
                    processo.DataAlteracao = null;
                    processo.UsuarioAlteracao = null;
                    processo.ConfiguracoesFormulario = null;
                    processo.DataFimEvento = null;
                    processo.DataInicioEvento = null;

                    foreach (var idioma in processo.Idiomas)
                    {
                        idioma.Seq = 0;
                        idioma.DataAlteracao = null;
                        idioma.UsuarioAlteracao = null;
                    }

                    foreach (var enderecoEletronico in processo.EnderecosEletronicos)
                    {
                        enderecoEletronico.Seq = 0;
                        enderecoEletronico.DataAlteracao = null;
                        enderecoEletronico.UsuarioAlteracao = null;
                    }

                    foreach (var taxa in processo.Taxas)
                    {
                        taxa.Seq = 0;
                        taxa.DataAlteracao = null;
                        taxa.UsuarioAlteracao = null;
                    }

                    foreach (var telefone in processo.Telefones)
                    {
                        telefone.Seq = 0;
                        telefone.DataAlteracao = null;
                        telefone.UsuarioAlteracao = null;
                    }


                    var seqNovoProcesso = ExecutaCopia(processo);

                    //Adiciona no retorno o Seq processo origem e o Seq do novo processo que foi gerado na cópia
                    retorno.ProcessosGpi.Add(copiaProcessoVO.SeqProcessoGpi, seqNovoProcesso);

                    if (processo.Taxas.Any())
                    {
                        var taxasProcessoNovo = processo.Taxas.Select(t => new TaxaVO()
                        {
                            Seq = t.Seq,
                            SeqTipoTaxa = t.SeqTipoTaxa,
                            DataInclusao = t.DataInclusao,
                            SeqProcesso = t.SeqProcesso,
                            DescricaoComplementar = t.DescricaoComplementar
                        }).ToList();

                        var copiaGrupoTaxaVO = new CopiaGrupoTaxaVO()
                        {
                            SeqProcessoCopia = seqNovoProcesso,
                            SeqProcessoOrigem = copiaProcessoVO.SeqProcessoGpi,
                            TaxasProcessoCopia = taxasProcessoNovo
                        };
                        //Copiar Grupos de Taxa
                        GrupoTaxaDomainService.CopiarGruposTaxa(copiaGrupoTaxaVO);
                    }
                    // Adiciona os campos do inscrito logo após a criação do novo processo que foi copiado
                    var camposInscrito = ProcessoCampoInscritoDomainService.BuscarCamposIncritosPorProcesso(copiaProcessoVO.SeqProcessoOrigem);

                    foreach (var item in camposInscrito)
                    {
                        var copiaCampoIncrito = new ProcessoCampoInscrito();
                        copiaCampoIncrito.SeqProcesso = processo.Seq;
                        copiaCampoIncrito.DataInclusao = DateTime.Now;
                        copiaCampoIncrito.CampoInscrito = item.CampoInscrito;

                        ProcessoCampoInscritoDomainService.SaveEntity(copiaCampoIncrito);
                    }

                    Dictionary<long, long> grupoOfertaMapping = null;
                    if (!copiaProcessoVO.CopiarItens)
                    {
                        processo.HierarquiasOferta = new List<HierarquiaOferta>();
                        processo.GruposOferta = new List<GrupoOferta>();
                    }
                    else
                    {
                        //Testar cópia de etapa de inscrição
                        var etapaInscricao = etapasProcesso.FirstOrDefault(x => x.Token == TOKENS.ETAPA_INSCRICAO);

                        if (copiaProcessoVO.CopiarItens && etapaInscricao != null)
                        {
                            var copiaEtapaInscricao = copiaProcessoVO.Etapas
                                .FirstOrDefault(x => x.SeqEtapaSGF == etapaInscricao.SeqEtapaSGF);

                            if (copiaEtapaInscricao != null)
                            {
                                if (copiaEtapaInscricao.Copiar && (copiaProcessoVO.DataInicioInscricao < copiaEtapaInscricao.DataInicio
                                                                    || copiaProcessoVO.DataFimInscricao > copiaEtapaInscricao.DataFim))
                                {
                                    throw new CopiaProcessoDataOfertaExcedeDataEtapaException();
                                }
                            }
                        }
                        grupoOfertaMapping = new Dictionary<long, long>();

                        // ordenação primeiro para melhorar a performance durante o loop
                        var grupoOrdenado = processo.GruposOferta.OrderByDescending(x => x.Seq).ToList();

                        foreach (var grupoOferta in grupoOrdenado)
                        {
                            long seq = grupoOferta.Seq;
                            grupoOferta.Seq = 0;
                            grupoOferta.SeqProcesso = processo.Seq;

                            var novoseq = GrupoOfertaDomainService.InsertEntity(grupoOferta).Seq;
                            grupoOfertaMapping.Add(seq, novoseq);
                        }

                        Dictionary<long, long?> hierarquiaOfertaMapping = new Dictionary<long, long?>();

                        if (copiaProcessoVO.MontarHierarquiaOfertaGPI)
                        {
                            retorno.ItensOfertasHierarquiasOfertas = HierarquiaOfertaDomainService.MontarHiererquiaOferta(processo.Seq, copiaProcessoVO.ItensOfertasHierarquiasOfertas, grupoOfertaMapping, hierarquiaOfertaMapping, copiaProcessoVO.DataInicioInscricao, copiaProcessoVO.DataFimInscricao, unitOfWork);
                        }
                        else
                        {
                            retorno.ItensOfertasHierarquiasOfertas = CopiaHierarquias(processo.Seq, processo.HierarquiasOferta.Where(f => !f.SeqPai.HasValue), grupoOfertaMapping, hierarquiaOfertaMapping, processo.HierarquiasOferta, copiaProcessoVO.DataInicioInscricao, copiaProcessoVO.DataFimInscricao, unitOfWork);
                        }
                    }

                    processo = this.SearchByKey(new SMCSeqSpecification<Processo>(processo.Seq),
                                                            IncludesProcesso.Inscricoes |
                                                            IncludesProcesso.TipoProcesso |
                                                            IncludesProcesso.Cliente |
                                                            IncludesProcesso.EtapasProcesso |
                                                            IncludesProcesso.Taxas |
                                                            IncludesProcesso.Idiomas |
                                                            IncludesProcesso.Telefones |
                                                            IncludesProcesso.EnderecosEletronicos |
                                                            IncludesProcesso.GruposOferta |
                                                            IncludesProcesso.HierarquiasOferta |
                                                            IncludesProcesso.UnidadeResponsavel);

                    //Cópia de notificações
                    if (copiaProcessoVO.CopiarNotificacoes)
                    {
                        foreach (var not in notificacoes)
                        {
                            var parametros = not.ParametrosEnvioNotificacao;
                            var tipoNotificacao = not.TipoNotificacao;
                            var idiomasNot = not.ConfiguracoesIdioma;
                            not.ConfiguracoesIdioma = null;
                            not.ParametrosEnvioNotificacao = null;
                            not.Seq = 0;
                            not.SeqProcesso = processo.Seq;
                            var novaConfiguracaoNot = ProcessoConfiguracaoNotificacaoDomainService.InsertEntity(not);
                            foreach (var parametro in parametros)
                            {
                                parametro.Seq = 0;
                                parametro.SeqProcessoConfiguracaoNotificacao = novaConfiguracaoNot.Seq;
                                ParametroEnvioNotificacaoDomainService.InsertEntity(parametro);
                            }
                            foreach (var idiomaNotificacao in idiomasNot)
                            {
                                var processoIdiomaOld = idiomasProcessoOld.FirstOrDefault(
                                            i => i.Seq == idiomaNotificacao.SeqProcessoIdioma);
                                var seqProcessoIdiomaNovo = processo.Idiomas.FirstOrDefault(
                                    x => x.Idioma == processoIdiomaOld.Idioma).Seq;
                                idiomaNotificacao.Seq = 0;
                                idiomaNotificacao.SeqProcessoConfiguracaoNotificacao = novaConfiguracaoNot.Seq;
                                idiomaNotificacao.SeqProcessoIdioma = seqProcessoIdiomaNovo;
                                idiomaNotificacao.ProcessoIdioma = null;

                                //Criar as notificações no Notifação
                                var configuracaoTipoNotificao = this.NotificacaoService
                                    .BuscarConfiguracaoTipoNotificacao(idiomaNotificacao.SeqConfiguracaoTipoNotificacao);
                                if (configuracaoTipoNotificao != null)
                                {
                                    configuracaoTipoNotificao.DataInicioValidade = DateTime.Now;
                                    configuracaoTipoNotificao.DataFimValidade = null;
                                    configuracaoTipoNotificao.Descricao =
                                        String.Format("{0}({1}) - ({2}){3}",
                                        configuracaoTipoNotificao.DescricaoTipoNotificacao,
                                        SMCEnumHelper.GetDescription(processoIdiomaOld.Idioma),
                                        processo.Seq,
                                        processo.Descricao);
                                    configuracaoTipoNotificao.Seq = 0;
                                    // A copia do processo não valida as tags da notificação.
                                    configuracaoTipoNotificao.ValidaTags = false;
                                    var seqConfiguracaoTipoNotificacaoNova =
                                        this.NotificacaoService.SalvarConfiguracaoTipoNotificacao(configuracaoTipoNotificao);

                                    idiomaNotificacao.SeqConfiguracaoTipoNotificacao = seqConfiguracaoTipoNotificacaoNova;
                                }

                                this.ProcessoConfiguracaoNotificacaoIdiomaDomainService.InsertEntity(idiomaNotificacao);
                            }
                        }
                    }

                    //Se o item Copiar formulário do evento for selecionado, copiar todos os dados do formulário,
                    //substituindo os valores dos campos Início para respostas e Fim para respostas pelos valores
                    //informados na tela de cópia
                    if (copiaProcessoVO.CopiarFormularioEvento)
                    {

                        foreach (var formulario in formularios)
                        {

                            formulario.Seq = 0;
                            formulario.SeqProcesso = processo.Seq;
                            formulario.DataInicioFormulario = copiaProcessoVO.DataInicioFormulario.Value;
                            formulario.DataFimFormulario = copiaProcessoVO.DataFimFormulario;

                            ProcessoConfiguracaoFormularioDomainService.InsertEntity(formulario);
                        }

                    }

                    var etapas = new List<EtapaProcesso>();
                    if (copiaProcessoVO.Etapas != null)
                    {
                        foreach (var etapa in copiaProcessoVO.Etapas)
                        {
                            if (etapa.Copiar)
                            {
                                var processoEtapa = etapasProcesso.FirstOrDefault(f => f.SeqEtapaSGF == etapa.SeqEtapaSGF);
                                long seqEtapaProcesso = 0;
                                if (processoEtapa != null)
                                {
                                    //var a = processoEtapa.Configuracoes.Select(x => x.GruposOferta.Select(t => t.SeqGrupoOferta));
                                    seqEtapaProcesso = processoEtapa.Seq;
                                    processoEtapa.Seq = 0;
                                    processoEtapa.SeqProcesso = processo.Seq;
                                    processoEtapa.DataInicioEtapa = etapa.DataInicio;
                                    processoEtapa.DataFimEtapa = etapa.DataFim;
                                    processoEtapa.SituacaoEtapa = SituacaoEtapa.AguardandoLiberacao;
                                    EtapaProcessoDomainService.SalvarEtapaProcesso(processoEtapa);
                                    if (etapa.CopiarConfiguracoes)
                                    {
                                        //Copia Configurações Xmoela
                                        CopiarConfiguracoesEtapaVO vo = new CopiarConfiguracoesEtapaVO
                                        {
                                            CopiarDocumentacao = true,
                                            CopiarPaginas = true,
                                            SeqProcessoDestino = processo.Seq,
                                            SeqProcessoOrigem = copiaProcessoVO.SeqProcessoOrigem,
                                            SeqEtapaProcesso = seqEtapaProcesso,
                                        };
                                        //Resgatar as configurações pra cópia
                                        vo.Configuracoes =
                                            EtapaProcessoDomainService.SearchProjectionByKey(
                                                    new SMCSeqSpecification<EtapaProcesso>(seqEtapaProcesso),
                                                x => x.Configuracoes.Select(
                                                    c => new CopiarConfiguracoesEtapaDetalheVO
                                                    {
                                                        SeqConfiguracaoEtapa = c.Seq,
                                                        DataInicio = processoEtapa.DataInicioEtapa,
                                                        DataFim = processoEtapa.DataFimEtapa,
                                                        DataLimiteDocumentacao = processoEtapa.DataFimEtapa,
                                                        NumeroEdital = null,
                                                        Descricao = c.Nome,
                                                        NumeroMaximoOfertaPorInscricao = c.NumeroMaximoOfertaPorInscricao,
                                                        ExigeJustificativaOferta = c.ExigeJustificativaOferta,
                                                        NumeroMaximoConvocacaoPorInscricao = c.NumeroMaximoConvocacaoPorInscricao,
                                                        PermiteNovaEntregaDocumentacao = c.PermiteNovaEntregaDocumentacao
                                                    })).ToList();
                                        ConfiguracaoEtapaDomainService.CopiarConfiguracoesEtapa(vo, grupoOfertaMapping);
                                    }
                                }
                            }
                        }
                    }

                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return retorno;
        }

        private long ExecutaCopia(Processo processo)
        {
            var gruposOferta = processo.GruposOferta;
            processo.GruposOferta = null;
            var hierarquiaOferta = processo.HierarquiasOferta;
            processo.HierarquiasOferta = null;


            this.SaveEntity(processo);

            processo.GruposOferta = gruposOferta;
            processo.HierarquiasOferta = hierarquiaOferta;


            return processo.Seq;
        }

        private Dictionary<long, long?> CopiaHierarquias(long seqProcesso, IEnumerable<HierarquiaOferta> hierarquias,
            Dictionary<long, long> grupoOfertaMapping,
            Dictionary<long, long?> hierarquiaOfertaMapping,
            IEnumerable<HierarquiaOferta> hierarquiaCompleta,
            DateTime? DataInicio, DateTime? DataFim,
            ISMCUnitOfWork unitOfWork)
        {
            foreach (var hierarquiaOferta in hierarquias)
            {
                long seq = hierarquiaOferta.Seq;
                hierarquiaOferta.Seq = 0;
                hierarquiaOferta.SeqProcesso = seqProcesso;
                if (hierarquiaOferta.EOferta)
                {
                    ((Oferta)hierarquiaOferta).Ativo = true;
                    ((Oferta)hierarquiaOferta).DataCancelamento = null;
                    ((Oferta)hierarquiaOferta).MotivoCancelamento = null;
                    ((Oferta)hierarquiaOferta).DataInicio = DataInicio;
                    ((Oferta)hierarquiaOferta).DataFim = DataFim;
                    ((Oferta)hierarquiaOferta).ExigePagamentoTaxa = false;
                    ((Oferta)hierarquiaOferta).NumeroVagasBolsa = 0;
                    ((Oferta)hierarquiaOferta).Taxas = new List<OfertaPeriodoTaxa>();
                    ((Oferta)hierarquiaOferta).DataInicioAtividade = null;
                    ((Oferta)hierarquiaOferta).DataFimAtividade = null;
                    ((Oferta)hierarquiaOferta).CargaHorariaAtividade = null;



                    var specOferta = new SMCSeqSpecification<Oferta>(seq);
                    var codigos = this.OfertaDomainService.SearchProjectionByKey(specOferta, x => x.CodigosAutorizacao);
                    foreach (var codigo in codigos)
                    {
                        codigo.Seq = 0;
                        codigo.SeqOferta = 0;
                    }
                    ((Oferta)hierarquiaOferta).CodigosAutorizacao = codigos;
                }

                if (hierarquiaOferta.EOferta)
                {
                    var oferta = (hierarquiaOferta as Oferta);
                    if (oferta.SeqGrupoOferta.HasValue && oferta.SeqGrupoOferta.Value != 0)
                    {
                        oferta.SeqGrupoOferta = grupoOfertaMapping[oferta.SeqGrupoOferta.Value];
                    }
                }

                if (hierarquiaOferta.SeqPai.HasValue)
                {
                    hierarquiaOferta.SeqPai = hierarquiaOfertaMapping[hierarquiaOferta.SeqPai.Value];
                }

                long? novoSeq = 0;
                if (!hierarquiaOferta.EOferta)
                {
                    novoSeq = HierarquiaOfertaDomainService.SalvarHierarquiaOferta(hierarquiaOferta);
                }
                else
                {
                    novoSeq = OfertaDomainService.SalvarOferta((Oferta)hierarquiaOferta);
                }
                hierarquiaOfertaMapping.Add(seq, novoSeq);

                var hierarquiasFilhas = hierarquiaCompleta.Where(x => x.SeqPai.HasValue && x.SeqPai.Value == seq);
                if (hierarquiasFilhas != null && hierarquiasFilhas.Count() > 0)
                {
                    CopiaHierarquias(seqProcesso, hierarquiasFilhas,
                        grupoOfertaMapping, hierarquiaOfertaMapping, hierarquiaCompleta, DataInicio, DataFim, unitOfWork);
                }
            }
            return hierarquiaOfertaMapping;
        }

        #endregion Copia de processo

        /// <summary>
        /// Verifica se é permitido cadastrar período taxa em lote para um processo
        /// </summary>
        public void VerificarConsistenciaCadastroPeriodoTaxaEmLote(long seqProcesso)
        {
            var spec = new SMCSeqSpecification<Processo>(seqProcesso);
            var processo = this.SearchByKey(spec,
                x => x.HierarquiasOferta, x => x.Taxas);
            if (!processo.SeqEvento.HasValue || !processo.Taxas.Any() || !processo.HierarquiasOferta.Any(x => x.EOferta))
            {
                throw new CadastroOfertaPeriodoTaxaNaoPermitidoException();
            }
        }

        /// <summary>
        /// Compara se dois processos possuem algum idioma em comum
        /// </summary>
        /// <returns>
        /// false: se os processos não tiverem NENHUM idioma em comum
        /// true : se os processos tiverem AO MENOS UM idioma em comum
        /// </returns>
        public bool CompararIdiomasProcesso(long seqProcessoDestino, long seqProcessoOrigem)
        {
            var spec = new SMCSeqSpecification<Processo>(seqProcessoDestino);
            var idiomasDestino = this.SearchProjectionByKey(spec, x => x.Idiomas.Select(i => i.Idioma));
            spec.Seq = seqProcessoOrigem;
            var idiomasOrigem = this.SearchProjectionByKey(spec, x => x.Idiomas.Select(i => i.Idioma));
            return idiomasDestino.Any(x => idiomasOrigem.Contains(x));
        }

        #region Integração GTI NOW

        public IEnumerable<TotalInscricoesProcessoVO> BuscarTotalInscricaoesProcessos(int rangeDias)
        {
            var spec = new ProcessoInscricaoVigenteSpecification(rangeDias);
            spec.SetOrderBy(x => x.Descricao);
            var ret = this.SearchProjectionBySpecification(spec, x => new TotalInscricoesProcessoVO
            {
                Descricao = x.Descricao,
                TotalInscricoes = x.Inscricoes.Count(),
                SeqProcesso = x.Seq
            });
            return ret;
        }

        public ResumoInscricoesProcessoVO BuscarSituacaoInscricoesProcessoSumarizada(long seqProcesso)
        {
            var spec = new SMCSeqSpecification<Processo>(seqProcesso);
            var ret = this.SearchProjectionByKey(spec,
                x => new ResumoInscricoesProcessoVO
                {
                    Descricao = x.Descricao,
                    Total = x.Inscricoes.Count(),
                    InscricoesIniciadas = x.Inscricoes.Count(i => i.HistoricosSituacao.Any(
                        h => h.AtualEtapa && h.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_INICIADA)),
                    InscricoesFinalizadas = x.Inscricoes.Count(i => i.HistoricosSituacao.Any(
                        h => h.AtualEtapa && h.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_FINALIZADA)),
                    InscricoesCanceladas = x.Inscricoes.Count(i => i.HistoricosSituacao.Any(
                        h => h.AtualEtapa && h.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA)),
                    InscricoesConfirmadas = x.Inscricoes.Count(i => i.HistoricosSituacao.Any(
                        h => h.AtualEtapa && h.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA)),
                    InscricoesDeferidas = x.Inscricoes.Count(i => i.HistoricosSituacao.Any(
                        h => h.AtualEtapa && h.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_DEFERIDA)),
                    InscricoesIndeferidas = x.Inscricoes.Count(i => i.HistoricosSituacao.Any(
                    h => h.AtualEtapa && h.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_INDEFERIDA))
                });
            return ret;
        }

        #endregion Integração GTI NOW

        public void IntegracaoProcesso(long seqProcesso, bool possuiIntegracao)
        {
            var processo = new Processo()
            {
                Seq = seqProcesso,
                PossuiIntegracao = possuiIntegracao
            };
            UpdateFields(processo, f => f.PossuiIntegracao);
        }

        public bool VerificarIntegracaoLegado(long seqProcesso)
        {
            //Resgatando a situação atual (ela será a mesma para todas as inscrições informadas)
            var dadosBusca = this.SearchProjectionByKey(seqProcesso, x => new
            {
                x.AnoReferencia,
                x.SemestreReferencia,
                x.TipoProcesso.IntegraSGALegado
            });

            // 1. Se o tipo de processo estiver configurado para integrar com o SGA legado e o semestre/ano de referência do processo for maior ou igual a 2º/2021
            var anoReferenciaConfig = Convert.ToInt32(ConfigurationManager.AppSettings["AnoReferenciaIntegracaoLegado"] ?? "2021");
            var semestreReferenciaConfig = Convert.ToInt32(ConfigurationManager.AppSettings["SemestreReferenciaIntegracaoLegado"] ?? "2");

            var anoSemestreProcesso = Convert.ToInt32($"{dadosBusca.AnoReferencia}{dadosBusca.SemestreReferencia}");
            var anoSemestreConfig = Convert.ToInt32($"{anoReferenciaConfig}{semestreReferenciaConfig}");
            if (anoSemestreProcesso >= anoSemestreConfig)
                return dadosBusca.IntegraSGALegado;

            return false;
        }

        public List<SMCDatasourceItem> BuscarProcessosIntegraGPC(bool integraGPC)
        {
            var retorno = new List<SMCDatasourceItem>();

            var spec = new ProcessoFilterSpecification { IntegraGPC = integraGPC };
            retorno = this.SearchProjectionBySpecification(spec, x => new SMCDatasourceItem
            {
                Seq = x.Seq,
                Descricao = x.Descricao
            }).OrderBy(o => o.Descricao).ToList();

            return retorno;
        }

        private void EspecificarTituloInscricoes(ProcessoHomeVO processo, int tipoProcesso)
        {
            switch (tipoProcesso)
            {
                case 1:
                    processo.TituloInscricoes = LABELSINSCRICAO.TITULO_INSCRICOES_INCRICAO;
                    break;
                case 2:
                    processo.TituloInscricoes = LABELSINSCRICAO.TITULO_INSCRICOES_UPLOAD;
                    break;
                case 3:
                    processo.TituloInscricoes = LABELSINSCRICAO.TITULO_INSCRICOES_AGENDAMENTO;
                    break;
                default:
                    break;
            }
        }

        //verficar se algum processo do tipo de processo em questão
        //possui inscrição com UID GED do processo preenchido.
        public bool ValidaTipoProcessoComUIDGed(long seqTipoProcesso)
        {
            bool retorno = false;

            var spec = new ProcessoFilterSpecification() { SeqTipoProcesso = seqTipoProcesso, UidGedPreenchido = true };

            var processo = this.SearchBySpecification(spec).ToList();

            if (processo != null && processo.Count() > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public List<ProcessoGestaoEventoQRCodeVO> BuscarProcessoHierarquiaLeituraQRCode()
        {
            //Busca a data atual do sistema e cria uma nova data com os segundos zero
            DateTime dataAtual = DateTime.Now;
            DateTime dataAtividadeAtual = new DateTime(dataAtual.Year, dataAtual.Month, dataAtual.Day, dataAtual.Hour, dataAtual.Minute, 0);
            var spec = new ProcessoFilterSpecification() { GestaoEventos = true, DataProcessoEventoCorrente = dataAtividadeAtual.Date };

            var processsos = this.SearchProjectionBySpecification(spec, p =>
            new
            {
                SeqProcesso = p.Seq,
                DescricaoProcesso = p.Descricao,
                p.ExibirPeriodoAtividadeOferta,
                p.SeqTemplateProcessoSGF,
                p.HoraAberturaCheckin,
                GrupoOferta = p.GruposOferta.Select(sg => new
                {
                    SeqGrupoOferta = sg.Seq,
                    DescricaoGrupoOferta = sg.Nome,
                    Ofertas = sg.Ofertas.Select(so => new
                    {
                        SeqOferta = so.Seq,
                        DescricaoCompleta = so.DescricaoCompleta,
                        DescricaoSimples = so.Nome,
                        so.DataInicioAtividade,
                        so.DataFimAtividade,
                        so.CargaHorariaAtividade,
                        so.SeqPai
                    })
                }).ToList()
            }).ToList();

            //var situacoesTemplateProcesso = TemplateProcessoService.BuscarSituacoesPorTemplateProcesso(inscricao.Processo.SeqTemplateProcessoSGF);

            List<ProcessoGestaoEventoQRCodeVO> retorno = new List<ProcessoGestaoEventoQRCodeVO>();

            foreach (var item in processsos)
            {
                var situacoesTemplateProcesso = TemplateProcessoService.BuscarSituacoesPorTemplateProcesso(item.SeqTemplateProcessoSGF);
                var processo = new ProcessoGestaoEventoQRCodeVO();
                processo.SeqProcesso = item.SeqProcesso;
                processo.DescricaoProcesso = item.DescricaoProcesso;
                processo.TokenHistoricoSituacao = situacoesTemplateProcesso.Contains(TOKENS.SITUACAO_INSCRICAO_DEFERIDA) ? TOKENS.SITUACAO_INSCRICAO_DEFERIDA : TOKENS.SITUACAO_INSCRICAO_CONFIRMADA;
                processo.GrupoOfertas = new List<ProcessoGrupoOfertaQRCodeVO>();
                processo.Hierarquias = new List<ProcessoHierarquiaQRcodeVO>();
                processo.Ofertas = new List<ProcessoOfertaQRCodeVO>();

                foreach (var grupo in item.GrupoOferta)
                {
                    var grupoOferta = new ProcessoGrupoOfertaQRCodeVO();
                    grupoOferta.SeqGrupoOferta = grupo.SeqGrupoOferta;
                    grupoOferta.DescricaoGrupoOferta = grupo.DescricaoGrupoOferta;
                    grupoOferta.SeqProcesso = item.SeqProcesso;
                    if (!processo.GrupoOfertas.Contains(grupoOferta))
                    {
                        processo.GrupoOfertas.Add(grupoOferta);
                    }

                    foreach (var oferta in grupo.Ofertas)
                    {
                        if (!oferta.DataInicioAtividade.HasValue || !oferta.DataFimAtividade.HasValue)
                        {
                            continue;
                        }

                        if (oferta.DataInicioAtividade.Value.AddHours(-item.HoraAberturaCheckin.Value.TotalHours) <= dataAtividadeAtual && oferta.DataFimAtividade >= dataAtividadeAtual)
                        {
                            var hierarquia = new ProcessoHierarquiaQRcodeVO();
                            if (oferta.SeqPai.HasValue)
                            {
                                hierarquia = BuscarNivelSuperMaximoHierarquiaOferta(oferta.SeqPai.Value);
                                hierarquia.SeqGrupoOferta = grupo.SeqGrupoOferta;
                                hierarquia.SeqProcesso = item.SeqProcesso;
                                if (!processo.Hierarquias.Contains(hierarquia))
                                {
                                    processo.Hierarquias.Add(hierarquia);
                                }
                            }
                            var ofertaQRCode = new ProcessoOfertaQRCodeVO();
                            ofertaQRCode.SeqGrupoOferta = grupo.SeqGrupoOferta;
                            if (oferta.SeqPai.HasValue)
                            {
                                ofertaQRCode.SeqNivelSuperior = hierarquia.SeqHierarquia;
                            }
                            ofertaQRCode.SeqProcesso = item.SeqProcesso;
                            ofertaQRCode.SeqOferta = oferta.SeqOferta;
                            var of = new Oferta()
                            {
                                Nome = oferta.DescricaoSimples,
                                DescricaoCompleta = oferta.DescricaoCompleta,
                                DataInicioAtividade = oferta.DataInicioAtividade,
                                DataFimAtividade = oferta.DataFimAtividade,
                                CargaHorariaAtividade = oferta.CargaHorariaAtividade,
                                Processo = new Processo()
                                {
                                    ExibirPeriodoAtividadeOferta = item.ExibirPeriodoAtividadeOferta
                                }
                            };
                            OfertaDomainService.AdicionarDescricaoCompleta(of, of.Processo.ExibirPeriodoAtividadeOferta);
                            ofertaQRCode.DescricaoCompleta = of.DescricaoCompleta;
                            ofertaQRCode.DescricaoSimples = oferta.DescricaoSimples;
                            ofertaQRCode.DataHoraInicioEvento = oferta.DataInicioAtividade.Value;
                            ofertaQRCode.DataHoraFimEvento = oferta.DataFimAtividade.Value;
                            processo.Ofertas.Add(ofertaQRCode);
                        }
                    }
                }
                if (processo.Ofertas.SMCAny())
                {
                    retorno.Add(processo);
                }
            }

            foreach (var item in retorno)
            {
                item.Ofertas = item.Ofertas.OrderBy(o => o.DescricaoCompleta).ToList();
            }

            return retorno;
        }

        private ProcessoHierarquiaQRcodeVO BuscarNivelSuperMaximoHierarquiaOferta(long? seqPai)
        {
            ProcessoHierarquiaQRcodeVO retorno = new ProcessoHierarquiaQRcodeVO();

            if (seqPai.HasValue)
            {
                var hierarquia = HierarquiaOfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<HierarquiaOferta>(seqPai.Value), x => new
                {
                    x.Seq,
                    x.Nome,
                    x.SeqPai
                });

                if (hierarquia.SeqPai.HasValue)
                {
                    return BuscarNivelSuperMaximoHierarquiaOferta(hierarquia.SeqPai);
                }

                retorno.SeqGrupoOferta = 0;
                retorno.SeqHierarquia = hierarquia.Seq;
                retorno.DescricaoHierarquia = hierarquia.Nome;

                return retorno;
            }

            return retorno;
        }

        public List<string> BuscarSituacoesProcesso(long seqProcesso)
        {
            // Busca as situacoes do processo
            var templateProcesso = this.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso),
                                                            x => new { x.SeqTemplateProcessoSGF, x.Descricao });
            if (templateProcesso == null) 
                return new List<string>();

            //Busca situação do template do processo
            var situacoesProcessoSGF = TemplateProcessoService.BuscarSituacoesPorTemplateProcesso(templateProcesso.SeqTemplateProcessoSGF);
            return situacoesProcessoSGF ?? new List<string>();

        }

        public Processo BuscarProcessoPorSeq(long seqProcesso)
        {

            return this.SearchByKey(seqProcesso, IncludesProcesso.ConfiguracoesNotificacao
                                                | IncludesProcesso.TipoProcesso
                                                | IncludesProcesso.ConfiguracoesNotificacao_TipoNotificacao
                                                | IncludesProcesso.ConfiguracoesFormulario
                                                | IncludesProcesso.GruposOferta
                                                | IncludesProcesso.GruposOferta_Ofertas);

        }

        public long BuscarSeqLayouMensagemEmailPorProcesso(long seqProcesso)
        {
            return this.SearchByKey(seqProcesso, IncludesProcesso.UnidadeResponsavelTipoProcessoIdVisual).UnidadeResponsavelTipoProcessoIdVisual.SeqLayoutMensagemEmail;
        }

        public bool ProcessoPossuiTaxa(long seqProcesso)
        {
            return this.SearchProjectionByKey(seqProcesso, x => x.Taxas.Any());
        }

        #region Emitir Documentação - Home Proceso Inscrito

        public bool ValidarPermissaoEmitirDocumentacao(long seqProcesso, long seqInscricao, long seqInscrito, long seqTipoDocumento)
        {
            bool usuarioLogadoCorrespondeAoInscrito = ValidarUsuarioInscritoAssociadoInscricao(seqInscricao, seqInscrito);
            if(!usuarioLogadoCorrespondeAoInscrito)
                return false;

            bool configuradoParaExibirNaHomeDataVigenteSemAssinaturaEletronica = ExisteConfiguracaoPraExibirNaHomeDataVigenteSemAssinatura(seqProcesso);
            if (!configuradoParaExibirNaHomeDataVigenteSemAssinaturaEletronica)
                return false;

            bool configuracaoRequerCheckinTipoEmissaoConsolidadaECheckinRealizado = ConfiguracaoRequerCheckinTipoEmissaoConsolidadaECheckinRealizado(seqInscricao, seqTipoDocumento, seqProcesso);
            if (!configuracaoRequerCheckinTipoEmissaoConsolidadaECheckinRealizado)
                return false;

            bool validaTokenTipoDocSituacaoInscDeferimento = ValidarTokenTipoDocSituacaoInscricaoTemplateProcesso(seqTipoDocumento, seqProcesso, seqInscricao);
            if (!validaTokenTipoDocSituacaoInscDeferimento)
                return false;

            return true;
        }

        private bool ExisteConfiguracaoPraExibirNaHomeDataVigenteSemAssinatura(long seqProcesso)
        {
            ConfiguracaoModeloDocumento configuracao = SearchProjectionByKey(new ProcessoFilterSpecification
            { SeqProcesso = seqProcesso }, x => x.ConfiguracoesModeloDocumento).FirstOrDefault();
            if (configuracao == null)
                return false;

            if (!configuracao.ExibeDocumentoHome)
                return false;

            return !configuracao.AssinaturaEletronica &&
                   ValidarPeriodoVigenciaDocumentoHome(configuracao.DataInicioDocumentoHome, configuracao.DataFimDocumentoHome);
        }
        private bool ConfiguracaoEstaAptaPraEmitirDocumento(ConfiguracaoModeloDocumento cfg)
        {
            if (!cfg.ExibeDocumentoHome)
                return false;

            return !cfg.AssinaturaEletronica &&
                   ValidarPeriodoVigenciaDocumentoHome(cfg.DataInicioDocumentoHome, cfg.DataFimDocumentoHome);
        }

        private bool ValidarPeriodoVigenciaDocumentoHome(DateTime? dataInicioDocumentoHome, DateTime? dataFimDocumentoHome)
        {
            var inicioDia = DateTime.Today;
            var fimDia = DateTime.Today.AddDays(1).AddTicks(-1);

            var inicioVigente = !dataInicioDocumentoHome.HasValue || dataInicioDocumentoHome.Value.Date <= inicioDia;
            var fimVigente = !dataFimDocumentoHome.HasValue || dataFimDocumentoHome.Value.Date.AddDays(1).AddTicks(-1) >= fimDia;

            return inicioVigente && fimVigente;
        }

        private bool ConfiguracaoRequerCheckinTipoEmissaoConsolidadaECheckinRealizado(long seqInscricao, long seqTipoDocumento, long seqProcesso)
        {
            bool configuracaoModeloDocumentoProcessoRequerCheckin = ConfiguracaoModeloDocumentoRequerCheckin(seqTipoDocumento, seqProcesso);

            bool tipoEmissaoTipoDocumentoConsolidada = false;
            string tipoEmissaoDocumento = BuscarTipoEmissaoTipoDocumento(seqTipoDocumento);
            if (!string.IsNullOrWhiteSpace(tipoEmissaoDocumento))
                tipoEmissaoTipoDocumentoConsolidada = tipoEmissaoDocumento.Equals(TipoEmissao.Consolidada.SMCGetDescription(), StringComparison.OrdinalIgnoreCase);

            bool checkinRealizadoNumaOfertaInscricao = InscricaoOfertaDomainService.VerificaPossuiCkeckin(seqInscricao);

            if (configuracaoModeloDocumentoProcessoRequerCheckin)
                return tipoEmissaoTipoDocumentoConsolidada && checkinRealizadoNumaOfertaInscricao;

            return true;
        }

        private bool ConfiguracaoModeloDocumentoRequerCheckin(long seqTipoDocumento, long seqProcesso)
        {
            ConfiguracaoModeloDocumentoFilterSpecification spec = new ConfiguracaoModeloDocumentoFilterSpecification { SeqTipoDocumento = seqTipoDocumento, SeqProcesso = seqProcesso };
            var configuracao = ConfiguracaoModeloDocumentoDomainService.SearchBySpecification(spec).FirstOrDefault();
            return configuracao.RequerCheckin;
        }

        private string BuscarTipoEmissaoTipoDocumento(long seqTipoDocumento)
        {
            var tipoDocumento = TipoDocumentoDomainService.SearchBySpecification(new SMCSeqSpecification<TipoDocumento>(seqTipoDocumento)).FirstOrDefault();

            if (tipoDocumento == null)
                return string.Empty;

            if (tipoDocumento.TipoEmissao == null)
                return string.Empty;

            string descricaoTipoDocumento = tipoDocumento.TipoEmissao.SMCGetDescription();

            return string.IsNullOrWhiteSpace(descricaoTipoDocumento) ? string.Empty : descricaoTipoDocumento;
        }

        private bool ValidarTokenTipoDocSituacaoInscricaoTemplateProcesso(long seqTipoDocumento, long seqProcesso, long seqInscricao)
        {
            bool isTokenTipoDocDeclaracaoOuCertificadoParticipacao = !string.IsNullOrEmpty(IsTokenTipoDocumentoDeclaracaoOuCertificadoParticipacao(seqTipoDocumento));
            bool situacaoAtualInscricaoConfirmada = IsTokenSituacaoAtualIgualInscricaoConfirmada(seqInscricao);
            bool situacaoAtualInscricaoDiferenteDeferida = IsTokenSituacaoAtualDiferenteInscricaoDeferida(seqInscricao);
            bool templateProcessoPossuiSituacaoDeferimentoNoFluxoSituacoes = BuscarSituacoesProcesso(seqProcesso)
                                                                                .Contains(TOKENS.SITUACAO_INSCRICAO_DEFERIDA);
            if (isTokenTipoDocDeclaracaoOuCertificadoParticipacao)
            {
                if (templateProcessoPossuiSituacaoDeferimentoNoFluxoSituacoes)
                    return !situacaoAtualInscricaoDiferenteDeferida;
                else
                    return situacaoAtualInscricaoConfirmada;
            }

            return true;
        }

        private bool IsTokenSituacaoAtualIgualInscricaoConfirmada(long seqInscricao)
        {
            return ValidarTokenTipoProcessoSituacaoAtual(seqInscricao, TOKENS.SITUACAO_INSCRICAO_CONFIRMADA);
        }

        private bool IsTokenSituacaoAtualDiferenteInscricaoDeferida(long seqInscricao)
        {
            return !ValidarTokenTipoProcessoSituacaoAtual(seqInscricao, TOKENS.SITUACAO_INSCRICAO_DEFERIDA);
        }

        private bool ValidarTokenTipoProcessoSituacaoAtual(long seqInscricao, string token)
        {
            string tokenTipoProcessoSituacaoAtual = InscricaoHistoricoSituacaoDomainService.BuscarTipoProcessoSituacaoAtual(seqInscricao);
            return !string.IsNullOrWhiteSpace(tokenTipoProcessoSituacaoAtual) &&
                   tokenTipoProcessoSituacaoAtual.Equals(token, StringComparison.OrdinalIgnoreCase);
        }

        private string IsTokenTipoDocumentoDeclaracaoOuCertificadoParticipacao(long seqTipoDocumneto)
        {
            var tipoDocumentoData = TipoDocumentoService.BuscarTipoDocumento(seqTipoDocumneto);
            var tokensValidosParaEmitirDocumentacao = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                TOKENS.TOKEN_TIPO_DOCUMENTO_CERTIFICADO_PARTICIPACAO,
                TOKENS.TOKEN_TIPO_DOCUMENTO_DECLARACAO_PARTICIPACAO
            };
            bool tokenValido = tipoDocumentoData != null && tokensValidosParaEmitirDocumentacao.Contains(tipoDocumentoData.Token);
            return tokenValido ? tipoDocumentoData.Token: string.Empty;
        }
        
        public bool ValidarUsuarioInscritoAssociadoInscricao(long seqInscricao, long seqInscrito)
        {
            InscricaoFilterSpecification spec = new InscricaoFilterSpecification()
            {
                SeqInscricao = seqInscricao,
                SeqInscrito = seqInscrito
            };

            Inscricao inscricaoAtual = InscricaoDomainService.SearchBySpecification(spec).FirstOrDefault();

            return inscricaoAtual != null;
        }

        public long BuscarSeqProcessoPorSeqInscricao(long seqInscricao)
        {
            long retorno = 0L;
            InscricaoFilterSpecification spec = new InscricaoFilterSpecification() { SeqInscricao = seqInscricao };
            Inscricao inscricao = InscricaoDomainService.SearchBySpecification(spec).FirstOrDefault();
            if (inscricao != null)
                retorno = inscricao.SeqProcesso;
            return retorno;
        }

        #endregion Emitir Documentação - Home Proceso Inscrito

    }
}