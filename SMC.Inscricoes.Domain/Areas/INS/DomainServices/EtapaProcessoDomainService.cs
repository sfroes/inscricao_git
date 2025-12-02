using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using SMC.Financeiro.ServiceContract.BLT;
using SMC.Financeiro.ServiceContract.TXA.Data;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Exceptions;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Framework.Util;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Exceptions.ConfiguracaoEtapa;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Data;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class EtapaProcessoDomainService : InscricaoContextDomain<EtapaProcesso>
    {
        #region Services

        private IEtapaService EtapaService
        {
            get
            {
                return this.Create<IEtapaService>();
            }
        }

        private IIntegracaoFinanceiroService FinanceiroService
        {
            get { return this.Create<IIntegracaoFinanceiroService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get
            {
                return this.Create<ProcessoDomainService>();
            }
        }

        private GrupoOfertaDomainService GrupoOfertaDomainService
        {
            get
            {
                return this.Create<GrupoOfertaDomainService>();
            }
        }

        private OfertaDomainService OfertaDomainService
        {
            get
            {
                return this.Create<OfertaDomainService>();
            }
        }

        private OfertaPeriodoTaxaDomainService OfertaPeriodoTaxaDomainService
        {
            get
            {
                return this.Create<OfertaPeriodoTaxaDomainService>();
            }
        }

        private ConfiguracaoEtapaDomainService ConfiguracaoEtapaDomainService
        {
            get
            {
                return this.Create<ConfiguracaoEtapaDomainService>();
            }
        }

        private IPaginaService PaginaService
        {
            get
            {
                return this.Create<IPaginaService>();
            }
        }

        private INotificacaoService NotificacaoService
        {
            get
            {
                return this.Create<INotificacaoService>();
            }
        }

        private IProcessoCampoInscritoService ProcessoCampoInscritoService => this.Create<IProcessoCampoInscritoService>();

        private PermissaoInscricaoForaPrazoDomainService PermissaoInscricaoForaPrazoDomainService
        {
            get { return Create<PermissaoInscricaoForaPrazoDomainService>(); }
        }

        private TaxaDomainService TaxaDomainService
        {
            get { return Create<TaxaDomainService>(); }
        }


        private InscricaoBoletoTaxaDomainService InscricaoBoletoTaxaDomainService => Create<InscricaoBoletoTaxaDomainService>();
        private InscricaoOfertaDomainService InscricaoOfertaDomainService => Create<InscricaoOfertaDomainService>();


        #endregion Services

        public long SalvarEtapaProcesso(EtapaProcesso etapaProcesso)
        {
            var processo = this.ProcessoDomainService.BuscarProcessoPorSeq(etapaProcesso.SeqProcesso);

            //1º - Passo - Recuperar Token da Etapa
            etapaProcesso.Token = EtapaService.BuscarEtapa(etapaProcesso.SeqEtapaSGF).Token;

            if (etapaProcesso.SituacaoEtapa == SituacaoEtapa.Liberada)
            {
                var exigeCodigoOrigemOferta = ProcessoDomainService.SearchProjectionByKey(etapaProcesso.SeqProcesso, x => x.TipoProcesso.ExigeCodigoOrigemOferta);
                if (exigeCodigoOrigemOferta)
                {
                    var spec = new OfertaFilterSpecification { SeqProcesso = etapaProcesso.SeqProcesso, SemCodigoOrigem = true };
                    spec.SetOrderBy(x => x.Nome);
                    // Recupera as ofertas que estão sem codigo de origem
                    var ofertasSemCodigo = OfertaDomainService.SearchProjectionBySpecification(spec, x => x.DescricaoCompleta).ToList();
                    if (ofertasSemCodigo != null && ofertasSemCodigo.Any())
                        throw new LiberacaoEtapaOfertaSemCodigoOrigemException(ofertasSemCodigo);
                }
            }

            if (etapaProcesso.Seq != default(long))
            {
                var etapaOld = this.SearchByKey(new SMCSeqSpecification<EtapaProcesso>(etapaProcesso.Seq), IncludesEtapaProcesso.Configuracoes_DocumentosRequeridos | IncludesEtapaProcesso.Configuracoes_GruposDocumentoRequerido | IncludesEtapaProcesso.Processo);

                //2º - Passo validações
                if (etapaProcesso.Token == TOKENS.ETAPA_INSCRICAO && etapaOld.SituacaoEtapa != etapaProcesso.SituacaoEtapa)
                {
                    //Validações específicas para etapa de inscrição
                    ValidarEtapaInscricao(etapaProcesso, etapaOld);
                }

                if (etapaOld.Configuracoes.Any() && etapaOld.SeqEtapaSGF != etapaProcesso.SeqEtapaSGF)
                {
                    throw new AlteracaoEtapaNaoPermitidaException();
                }

                if ((etapaOld.DataInicioEtapa != etapaProcesso.DataInicioEtapa || etapaOld.DataFimEtapa != etapaProcesso.DataFimEtapa)
                    && etapaOld.Configuracoes.Any(
                        x => x.DataFim > etapaProcesso.DataFimEtapa || x.DataInicio < etapaProcesso.DataInicioEtapa))
                {
                    throw new AlteracaoEtapaDataInvalidaException();
                }

                if (etapaProcesso.SituacaoEtapa == SituacaoEtapa.Liberada)
                {
                    if (etapaOld.Configuracoes.Any
                        (
                            e =>
                            string.IsNullOrEmpty(e.DescricaoTermoEntregaDocumentacao) &&
                            (
                                (e.DocumentosRequeridos.Count > 0 && e.DocumentosRequeridos.Any(d => d.ExibeTermoResponsabilidadeEntrega)) ||
                                (e.GruposDocumentoRequerido.Count > 0 && e.GruposDocumentoRequerido.Any(g => g.ExibeTermoResponsabilidadeEntrega))
                            )
                        )
                       )
                        throw new LiberacaoEtapaSemDescricaoTermoResponsabilidadeEntregaException();

                    if (etapaOld.Configuracoes.Any
                        (
                            e =>
                            !string.IsNullOrEmpty(e.DescricaoTermoEntregaDocumentacao) &&
                            (
                             (e.DocumentosRequeridos.Count > 0 && e.DocumentosRequeridos.All(d => !d.ExibeTermoResponsabilidadeEntrega)) &&
                             (e.GruposDocumentoRequerido.Count > 0 && e.GruposDocumentoRequerido.All(g => !g.ExibeTermoResponsabilidadeEntrega))
                            )
                        )
                       )
                        throw new LiberacaoEtapaComDescricaoTermoResponsabilidadeEntregaException();

                    if (etapaOld.Configuracoes.Any
                        (
                            e =>
                            (
                                e.DocumentosRequeridos.Count > 0 && e.DocumentosRequeridos.Any(d => d.DataLimiteEntrega < etapaProcesso.DataFimEtapa)
                            )
                            ||
                            (
                                e.GruposDocumentoRequerido.Count > 0 && e.GruposDocumentoRequerido.Any(g => g.DataLimiteEntrega < etapaProcesso.DataFimEtapa)
                            )
                        )
                       )
                    {
                        throw new LiberacaoEtapaDataLimiteEntregaMenorDataFimEtapaException();
                    }
                }

                //Ao alterar a situação da etapa de inscrição de um processo de "Aguardando Liberação"/"Em Manutenção" para “Liberada”"
                //e se o Tipo de proceso do Processo em questão estiver habilitado para realizar a gestão de eventos
                if ((etapaProcesso.SituacaoEtapa == SituacaoEtapa.Liberada && etapaOld.SituacaoEtapa != SituacaoEtapa.Liberada) && processo.TipoProcesso.GestaoEventos)
            {
                    // Se houver uma Notificação configurada no Processo, cujo token do tipo de notificação é AVALIACAO_EVENTO,
                    // deverá existir um Formulário do evento configurado no Processo. Em caso de violação desta regra, abortar
                    // a operação e emitir a mensagem de erro
                    if (processo.ConfiguracoesNotificacao.Any(a => a.TipoNotificacao.Token == TOKENS.FORMULARIO_AVALIACAO))
                    {
                        if (!processo.ConfiguracoesFormulario.Any())
                        {
                            throw new ExisteNotificacaoAvaliacaoEventoNoProcessoException();
                        }
                    }

                    //A Data início do evento e a Data fim do evento deverão ter sido informadas no Processo. Em caso de violação
                    //desta regra, abortar a operação e emitir a mensagem de erro:
                    if (!(processo.DataInicioEvento.HasValue && processo.DataFimEvento.HasValue))
                    {
                        throw new PeriodoDoEventoNaoConfiguradoNoProcessoException();
                    }

                    //As datas de Início e Fim da Atividade deverão ter sido informadas em todas as Ofertas do Processo. Em caso de
                    //violação desta regra, abortar a operação e emitir a mensagem de erro
                    if (processo.GruposOferta.Any(a => a.Ofertas.Any(an => !(an.DataInicioAtividade.HasValue && an.DataFimAtividade.HasValue))))
                    {
                        throw new OfertaCadastradaSemPeriodoAtividadeConfiguradoException();
                    }
                }
            }
            this.SaveEntity(etapaProcesso);
            return etapaProcesso.Seq;
        }

        private void ValidarEtapaInscricao(EtapaProcesso etapaProcesso, EtapaProcesso etapaOld)
        {
            if ((etapaOld.SituacaoEtapa == SituacaoEtapa.AguardandoLiberacao
                        || etapaOld.SituacaoEtapa == SituacaoEtapa.EmManutencao)
                    && etapaProcesso.SituacaoEtapa == SituacaoEtapa.Liberada)
            {
                // RN_INS_084 
                // Busca os campos inscrito por processo
                var camposInscritoPorProcesso = ProcessoCampoInscritoService.BuscarCamposIncritosPorProcesso(etapaOld.Processo.Seq);

                // Verificar se o campo Sexo está marcado
                var campoSexoMarcadoPorProcesso = camposInscritoPorProcesso.Any(a => a.CampoInscrito == CampoInscrito.Sexo);

                // Verifica se existe algum documento requerido obrigatório para um sexo específico.
                var sexoObrigatorio = etapaOld.Configuracoes.Any(
                    e => ((e.DocumentosRequeridos.Count > 0 && e.DocumentosRequeridos.Any(d => d.Obrigatorio && d.Sexo != null))));

                // Se existir algum documento requerido obrigatório para um sexo específico e o campo "Sexo" não estiver na lista de campos do cadastro do inscrito,
                // abortar a operação e emitir a mensagem de erro:
                if (sexoObrigatorio && !campoSexoMarcadoPorProcesso)
                {
                    throw new DocumentoRequeridoParaSexoNaoConfiguradoException();
                }

                //Caso a etapa de inscrição não possua configuração cadastrada, abortar a operação e emitir a mensagem de erro:
                //"Não é permitida a liberação desta etapa de inscrição. Não existe configuração cadastrada."
                if (etapaOld.Configuracoes == null || etapaOld.Configuracoes.Count == 0)
                {
                    throw new LiberacaoEtapaSemConfiguracaoException();
                }

                // Verifica se existe um evento GRA e se pelo menos uma oferta tem taxa
                if (etapaOld.Processo.SeqEvento.HasValue)
                {
                    if (OfertaDomainService.Count(new ProcessoOfertaComTaxaSpecification { SeqProcesso = etapaOld.SeqProcesso }) == 0)
                    {
                        throw new LiberacaoEtapaProcessoComEventoOfertasSemTaxaException();
                    }
                }

                //1. Pelo menos um grupo de oferta associado com ofertas ativas e não canceladas.
                var spec = new SMCSeqSpecification<EtapaProcesso>(etapaProcesso.Seq);
                var existeOferta = this.SearchProjectionByKey(spec, x => x.Configuracoes.Any(
                    c => c.GruposOferta.Any(
                        g => g.GrupoOferta.Ofertas.Any(
                            o => !o.DataCancelamento.HasValue && o.Ativo))));
                if (!existeOferta)
                {
                    throw new LiberacaoEtapaSemOfertaAtivaException();
                }

                var existeOfertaComCodigoSemPagina = this.SearchProjectionByKey(spec, x => x.Configuracoes.Any(
                    c => c.GruposOferta.Any(
                        g => g.GrupoOferta.Ofertas.Any(o => o.CodigosAutorizacao.Any()))
                        && !c.Paginas.Any(p => p.Token == TOKENS.PAGINA_CODIGO_AUTORIZACAO)));
                if (existeOfertaComCodigoSemPagina)
                {
                    throw new LiberacaoEtapaCodigoSemPaginaException();
                }

                //2. Páginas de inscrição com as seguintes configurações:
                var processo = this.ProcessoDomainService.SearchByKey(
                    new SMCSeqSpecification<Processo>(etapaOld.SeqProcesso),
                    IncludesProcesso.Idiomas |
                    IncludesProcesso.ConfiguracoesNotificacao_ParametrosEnvioNotificacao |
                    IncludesProcesso.ConfiguracoesNotificacao_ConfiguracoesIdioma_ProcessoIdioma |
                    IncludesProcesso.ConfiguracoesNotificacao_TipoNotificacao);
                var etapaCompleta = this.SearchByKey(new SMCSeqSpecification<EtapaProcesso>(etapaProcesso.Seq),
                    x => x.Configuracoes,
                    x => x.Configuracoes[0].DocumentosRequeridos,
                    x => x.Configuracoes[0].GruposOferta,
                    x => x.Configuracoes[0].Paginas,
                    x => x.Configuracoes[0].Paginas[0].Idiomas,
                    x => x.Configuracoes[0].Paginas[0].Idiomas[0].Textos,
                    x => x.Configuracoes[0].Paginas[0].Idiomas[0].Arquivos);

                var idiomasProcesso = processo.Idiomas.Select(x => x.Idioma);
                foreach (var config in etapaCompleta.Configuracoes)
                {
                    foreach (var pagina in config.Paginas)
                    {
                        //a. Páginas cadastrados para todos os idiomas associados ao processo.
                        var idiomasPagina = pagina.Idiomas.Select(x => x.Idioma);
                        if (!idiomasProcesso.SMCContainsList(idiomasPagina, x => x))
                        {
                            throw new LiberacaoEtapaSemIdiomasConfiguradosException();
                        };
                        //b. Para cada seção de texto de uma página cadastrada para o idioma padrão, deve haver um texto
                        //cadastrado também, para a mesma página, nos demais idiomas associados ao processo.
                        var paginaIdiomaPadrao = pagina.Idiomas.FirstOrDefault(x => x.Idioma ==
                            processo.Idiomas.FirstOrDefault(p => p.Padrao).Idioma);
                        if (paginaIdiomaPadrao.Textos != null)
                        {
                            foreach (var secaoTexto in paginaIdiomaPadrao.Textos)
                            {
                                if (!String.IsNullOrEmpty(secaoTexto.Texto) &&
                                    pagina.Idiomas.Any(i => i.Textos.Any(t => String.IsNullOrEmpty(t.Texto) &&
                                    t.SeqSecaoPaginaSGF == secaoTexto.SeqSecaoPaginaSGF)))
                                {
                                    throw new SecaoTextoNaoCadastradaIdiomasException();
                                }
                            }
                        }

                        //c. Para cada seção de arquivo de uma página com arquivo(s) associado(s) no idioma padrão, deve
                        //haver pelo menos um arquivo associado também, para a mesma página, nos demais idiomas do
                        //processo.
                        if (paginaIdiomaPadrao.Arquivos != null && paginaIdiomaPadrao.Arquivos.Count > 0)
                        {
                            foreach (var secaoArquivo in paginaIdiomaPadrao.Arquivos)
                            {
                                if (pagina.Idiomas.Any(i => !i.Arquivos.Any()))
                                {
                                    throw new SecaoArquivoNaoCadastradaIdiomasException();
                                }
                            }
                        }

                        //d. Para página que foi configurada no SGF para exibir formulário, deve haver um formulário associado
                        //para todos os idiomas do processo.
                        var paginaSGF = this.PaginaService.BuscarPaginaPorPaginaEtapa(pagina.SeqPaginaEtapaSGF);
                        if (paginaSGF.ExibeFormulario)
                        {
                            if (pagina.Idiomas.Any(x => !x.SeqFormularioSGF.HasValue))
                            {
                                throw new PaginaSemFormularioException();
                            }
                        }

                        //e. As páginas configuradas no SGF para permitir exibição em outra página devem ter os seguintes
                        //itens preenchidos:
                        //Exibir dados na página de confirmação de inscrição?
                        //Exibir dados no comprovante?
                        if (paginaSGF.PermiteExibirEmOutraPagina
                            && (!pagina.ExibeComprovanteInscricao.HasValue
                                || !pagina.ExibeConfirmacaoInscricao.HasValue))
                        {
                            throw new ConfiguracaoPaginaExibicaoInvalidaException();
                        }
                    }

                    //6. Caso exista uma configuração de etapa que possua documento requerido que permite upload,
                    //a página UPLOAD_DOCUMENTOS deverá existir no fluxo de páginas desta configuração. Em caso de violação, abortar a operação e emitir a mensagem de erro:
                    //"Não é permitida a liberação desta etapa. Existe documento requerido que permite upload, porém, a página de Upload de Documentos não está no fluxo de páginas.
                    if (config.DocumentosRequeridos.Any(x => x.PermiteUploadArquivo)
                        && !config.Paginas.Any(x => x.Token == TOKENS.PAGINA_UPLOAD_DOCUMENTOS))
                    {
                        throw new UploadPermitidoSemPaginaException();
                    }
                }

                var ofertaSpec = new OfertaFilterSpecification { SeqProcesso = processo.Seq };
                ofertaSpec.SetOrderBy(x => x.Nome);
                var ofertas = this.OfertaDomainService.SearchBySpecification(ofertaSpec, IncludesOferta.CodigosAutorizacao | IncludesOferta.Taxas_Taxa_TipoTaxa).ToList();
                //5. Caso exista oferta no processo com código de autorização cadastrado, a página CODIGO_AUTORIZACAO
                //deverá existir no fluxo de páginas da configuração da etapa de inscrição ao qual o grupo desta
                //oferta está associado. Em caso de violação, abortar a operação e emitir a mensagem de erro:
                //"Não é permitida a liberação desta etapa. Existe oferta com código de autorização cadastrado, cujo fluxo de páginas da configuração da etapa de inscrição do grupo ao qual a oferta pertence não possui a página de Código de Autorização.
                foreach (var ofertaComCodigo in ofertas.Where(x => x.CodigosAutorizacao.Any()))
                {
                    var configuracaoOferta = etapaCompleta.Configuracoes.FirstOrDefault(
                        x => x.GruposOferta.Any(g => g.SeqGrupoOferta == ofertaComCodigo.SeqGrupoOferta));
                    if (configuracaoOferta != null
                        && !configuracaoOferta.Paginas.Any(x => x.Token == TOKENS.PAGINA_CODIGO_AUTORIZACAO))
                    {
                        throw new OfertaComCodigoSemPaginaException();
                    }
                }

                ValidarConfiguracaoOfertasLiberarInscricao(etapaCompleta.Configuracoes, ofertas);

                foreach (var configuracaoNotificacao in processo.ConfiguracoesNotificacao)
                {
                    //3. Caso o processo possua alguma configuração com tipo de notificação que permite agendamento,
                    //verificar se existe parâmetro de envio de notificação ativo configurado para ele.
                    //Caso não exista, abortar a operação e enviar a mensagem de erro:
                    //"Não é permitida a liberação desta etapa. Existe notificação que permite agendamento sem parametrização de envio de notificação."
                    if (configuracaoNotificacao.TipoNotificacao.PermiteAgendamento
                        && !configuracaoNotificacao.ParametrosEnvioNotificacao.Any(x => x.Ativo))
                    {
                        throw new NotificacaoAgendamentoSemParametroException();
                    }

                    //4. Caso o processo possua alguma configuração de notificação que não foi configurada para todos
                    //os idiomas, abortar a operação e emitir a mensagem de erro:
                    //"Não é permitida a liberação desta etapa. Existe notificação que não foi configurada para todos os idiomas do processo."
                    if (configuracaoNotificacao.ConfiguracoesIdioma.Any(x => !idiomasProcesso.Contains(x.ProcessoIdioma.Idioma)))
                    {
                        throw new NotifacaoSemConfiguracaoIdiomaException();
                    }
                }

                if (processo.ConfiguracoesNotificacao != null && processo.ConfiguracoesNotificacao.Count > 0)
                {
                    var seqConfiguracoesNotificacao = processo.ConfiguracoesNotificacao.SelectMany(x => x.ConfiguracoesIdioma.Select(i => i.SeqConfiguracaoTipoNotificacao)).ToArray();

                    var filtro = new ConfiguracaoNotificacaoEmailFiltroData
                    {
                        Assunto = Resources.MessagesResource.Assunto_Notificacao_Padrao,
                        Mensagem = Resources.MessagesResource.Mensagem_Notificacao_Padrao
                    };
                    if (this.NotificacaoService.VerificarExistenciaNotificacoesEmail(filtro, seqConfiguracoesNotificacao))
                    {
                        //Não é permitida a liberação desta etapa. Existe notificação que não foi configurada para todos os idiomas do processo.
                        throw new LiberacaoEtapaNotificacaoNaoConfiguradaException();
                    }

                    //7. Se existir notificação configurada para o processo utilizando a tag <DAT_ENTREGA_DOCUMENTACAO>,
                    //a data da entrega da documentação tem que estar preenchida em todas as configurações da etapa de inscrição. Em caso de violação, emitir a mensagem de erro abaixo e abortar a operação.
                    //“Não é permitida a liberação desta etapa de inscrição. Existe notificação configurada para este processo utilizando a data limite de entrega da documentação e esta informação não foi preenchida em todas as configurações desta etapa.
                    if (NotificacaoService.VerificarUsoTagConfiguracoes(
                        seqConfiguracoesNotificacao, TAGS_NOTIFICACAO.DATA_ENTREGA_DOCUMENTACAO)
                        && etapaCompleta.Configuracoes.Any(x => !x.DataLimiteEntregaDocumentacao.HasValue))
                    {
                        throw new DataEntregaDocumentacaoNotificacaoException();
                    }

                    //8. Se existir notificação configurada para o processo utilizando a tag <DSC_DOCUMENTACAO>,
                    //a descrição da documentação tem que estar preenchida em todas as configurações da etapa de inscrição. Em caso de violação, abortar a operação e emitir a mensagem de erro:
                    //“Não é permitida a liberação desta etapa de inscrição. Existe notificação configurada para este processo utilizando a descrição da entrega da documentação e esta informação não foi preenchida em todas as configurações desta etapa.
                    if (NotificacaoService.VerificarUsoTagConfiguracoes(
                            seqConfiguracoesNotificacao, TAGS_NOTIFICACAO.DESCRICAO_DOCUMENTACAO)
                        && etapaCompleta.Configuracoes.Any(x => string.IsNullOrEmpty(x.DescricaoEntregaDocumentacao)))
                    {
                        throw new DescricaoDocumentacaoNotificacaoException();
                    }

                    //9. Se existir notificação configurada para o processo utilizando a tag <NUM_EDITAL>, o número do edital tem
                    //que estar preenchido em todas as configurações da menor etapa cadastrada para o processo (verificar o sgf..etapa.num_ordem).
                    //Em caso de violação, abortar a operação e emitir a mensagem de erro:
                    //“Não é permitida a liberação desta etapa. Existe notificação configurada para este processo utilizando o número do edital e esta informação não foi preenchida em todas as configurações da etapa <descrição da menor etapa cadastrada para o processo>.”
                    if (NotificacaoService.VerificarUsoTagConfiguracoes(
                            seqConfiguracoesNotificacao, TAGS_NOTIFICACAO.NUMERO_EDITAL)
                        && etapaCompleta.Configuracoes.Any(x => string.IsNullOrEmpty(x.NumeroEdital)))
                    {
                        throw new NumeroEditalNotificacaoException();
                    }

                    // Verifica se todas as tags das notificações são válidas para liberar a etapa.
                    var validacaoTags = NotificacaoService.VerificarTagsInvalidas(seqConfiguracoesNotificacao);
                    if (validacaoTags.Erro)
                    {
                        throw new ProcessoTagInvalidaNotificacaoException(string.Join(",", validacaoTags.TagsInvalidas), validacaoTags.DescricaoTipoNotificacao);
                    }
                }
            }
        }

        /// <summary>
        ///     Valida se <see cref="ConfiguracaoEtapa.NumeroMaximoOfertaPorInscricao"/> é maior do que 1 ou ilimitado(null)
        ///     Quando a configuração permitir + de 1 oferta por inscrição, valida se o <see cref="TipoCobranca"></see>
        ///     das <see cref="Taxa"/>s associadas a <see cref="Oferta"/> é diferente de <see cref="TipoCobranca.PorOferta"/>
        ///     Se ambas as validações forem verdadeiras, verifica se a <see cref="OfertaPeriodoTaxa"/> associada a
        ///     uma <see cref="Oferta"/> está associada a todas as <see cref="Oferta"/>s deste <see cref="GrupoOferta"/>
        ///     Se houver alguma <see cref="OfertaPeriodoTaxa"/> associadas a algumas <see cref="Oferta"/>s, 
        ///     mas não a todas, é lançada a mensagem de exceção
        /// </summary>
        /// <param name="configuracoesEtapa"></param>
        /// <param name="ofertas"></param>
        /// <exception cref="LiberacaoEtapaTaxaPorOfertaException"></exception>
        private void ValidarConfiguracaoOfertasLiberarInscricao(IList<ConfiguracaoEtapa> configuracoesEtapa, List<Oferta> ofertas)
        {
            if (configuracoesEtapa.IsNullOrEmpty()) return;

            foreach (var config in configuracoesEtapa)
            {
                // Só continua validações se NumeroMaximoOfertaPorInscricao da Configuração da Etapa  == null ou > 1
                if (config.NumeroMaximoOfertaPorInscricao != null && config.NumeroMaximoOfertaPorInscricao <= 1)
                    continue;

                var ofertasPorGrupoOferta = ofertas
                    .Where(o => config.GruposOferta.Any(go => go.SeqGrupoOferta == o.SeqGrupoOferta))
                    .GroupBy(o => o.SeqGrupoOferta);
                
                foreach (var grupo in ofertasPorGrupoOferta)
                {
                    var ofertasDoGrupoOferta = grupo.ToList();

                    var seqTaxasNaoPorOferta = ofertasDoGrupoOferta
                        .SelectMany(o => o.Taxas.Where(f => f.Taxa.TipoCobranca != TipoCobranca.PorOferta)
                        .Select(t => t.SeqTaxa)).Distinct();

                    foreach (var seqTaxa in seqTaxasNaoPorOferta)
                    {
                        bool taxaPresenteEmTodas = ofertasDoGrupoOferta.All(o => o.Taxas.Any(t => t.SeqTaxa == seqTaxa));
                        if (taxaPresenteEmTodas) continue;

                        var ofertaNaoAssociadaAoGrupo = ofertasDoGrupoOferta
                                                            .First(o => o.Taxas.Any(t => t.SeqTaxa == seqTaxa));
                        var (descricaoTaxa, nomeGrupoOferta) =
                            BuscarDadosLiberacaoEtapaTaxaPorOfertaException(ofertaNaoAssociadaAoGrupo, seqTaxa);

                        throw new LiberacaoEtapaTaxaPorOfertaException(descricaoTaxa, nomeGrupoOferta);
                    }
                }
            }
        }
                
        /// <summary>
        /// Busca <see cref="TipoTaxa.Descricao"/>, e <see cref="GrupoOferta.Nome"/>
        /// </summary>
        /// <returns>Descrição do tipo da Taxa e do Grupo de Ofertas passados como oarâmetro</returns>
        private (string nomeTaxa, string nomeGrupoOferta) BuscarDadosLiberacaoEtapaTaxaPorOfertaException(Oferta oferta, long seqTaxa)
        {
            var taxa = oferta.Taxas.FirstOrDefault(f => f.SeqTaxa == seqTaxa);
            if (taxa == null) return (string.Empty, string.Empty);

            var grupoOfertaSpec = new GrupoOfertaFilterSpecification { Seq = oferta.SeqGrupoOferta };
            string nomeGrupoOfertaMsg = GrupoOfertaDomainService.SearchProjectionByKey(grupoOfertaSpec, o => o.Nome);
            string nomeTaxa = taxa.Taxa.TipoTaxa.Descricao;

            return (nomeTaxa, nomeGrupoOfertaMsg);

        }

        public List<SMCDatasourceItem> BuscarSituacoesPermitidas(long seqEtapaProcesso)
        {
            var ret = new List<SMCDatasourceItem>();

            if (seqEtapaProcesso == 0)
                return ret;

            var etapaOld = this.SearchByKey(new SMCSeqSpecification<EtapaProcesso>(seqEtapaProcesso));
            ret.Add(new SMCDatasourceItem
            {
                Seq = (short)etapaOld.SituacaoEtapa,
                Descricao = SMCEnumHelper.GetDescription(etapaOld.SituacaoEtapa)
            });
            if (etapaOld.SituacaoEtapa == SituacaoEtapa.Liberada && etapaOld.DataInicioEtapa <= DateTime.Now)
            {
                ret.Add(new SMCDatasourceItem
                {
                    Seq = (short)SituacaoEtapa.EmManutencao,
                    Descricao = SMCEnumHelper.GetDescription(SituacaoEtapa.EmManutencao)
                });
            }
            if (etapaOld.SituacaoEtapa == SituacaoEtapa.Liberada &&
                etapaOld.DataInicioEtapa > DateTime.Now)
            {
                ret.Add(new SMCDatasourceItem
                {
                    Seq = (short)SituacaoEtapa.AguardandoLiberacao,
                    Descricao = SMCEnumHelper.GetDescription(SituacaoEtapa.AguardandoLiberacao)
                });
            }
            if (etapaOld.SituacaoEtapa != SituacaoEtapa.Liberada)
            {
                ret.Add(new SMCDatasourceItem
                {
                    Seq = (short)SituacaoEtapa.Liberada,
                    Descricao = SMCEnumHelper.GetDescription(SituacaoEtapa.Liberada)
                });
            }
            return ret;
        }

        public void ExcluirEtapaProcesso(long seqEtapaProcesso)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    var etapa = this.SearchByKey(
                        new SMCSeqSpecification<EtapaProcesso>(seqEtapaProcesso),
                        x => x.Configuracoes,
                        x => x.Configuracoes[0].Paginas,
                        x => x.Configuracoes[0].Paginas[0].Idiomas,
                        x => x.Configuracoes[0].Paginas[0].Idiomas[0].Textos,
                        x => x.Configuracoes[0].Paginas[0].Idiomas[0].Arquivos,
                        x => x.Configuracoes[0].DocumentosRequeridos,
                        x => x.Configuracoes[0].GruposDocumentoRequerido);

                    if (etapa.SituacaoEtapa == SituacaoEtapa.Liberada)
                    {
                        throw new ExclusaoEtapaLiberadaException();
                    }

                    foreach (var configuracao in etapa.Configuracoes)
                    {
                        ConfiguracaoEtapaDomainService.ExcluirConfiguracaoEtapa(configuracao);
                    }
                    etapa.Configuracoes = null;
                    this.DeleteEntity(etapa);
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Verifica se a inclusão de etapas é permitida
        /// </summary>
        public void VerificarPermissaoCadastrarEtapa(long seqProcesso)
        {
            var specProcesso = new SMCSeqSpecification<Processo>(seqProcesso);
            var dadosProcesso = this.ProcessoDomainService.SearchProjectionByKey(specProcesso,
                x => new
                {
                    TipoProcessoDesativado = x.UnidadeResponsavel.TiposProcesso
                        .Any(t => t.TipoProcesso.Seq == x.SeqTipoProcesso && !t.Ativo),
                    TipoTemplateDesativado = x.TipoProcesso.Templates
                        .Any(t => t.SeqTemplateProcessoSGF == x.SeqTemplateProcessoSGF && !t.Ativo),
                    PossuiEtapas = x.EtapasProcesso.Any()
                });
            if (!dadosProcesso.PossuiEtapas)
            {
                if (dadosProcesso.TipoProcessoDesativado)
                {
                    //O tipo de processo informado para o processo foi desativado. Informe um tipo de processo ativo pra o processo antes de cadastrar a etapa.
                    throw new EtapaProcessoTipoProcessoDesativadoException();
                }

                if (dadosProcesso.TipoTemplateDesativado)
                {
                    //O template de processo informado para o processo foi desativado. Informe um template de processo ativo para o processo antes de cadastrar a etapa.
                    throw new EtapaProcessoTipoTemplateDesativadoException();
                }
            }
        }

        #region Prorrogação Etapa

        /// <summary>
        /// Busca a lista de confiugrações a partir das ofertas selecionas para a alteração de período de etapa
        /// </summary>
        public List<ProrrogacaoConfiguracaoVO> BuscarConfiguracoesProrrogacao(long seqEtapaProcesso
            , long[] seqOfertas)
        {
            var spec = new ConfiguracaoEtapaPorOfertasSpecification(seqOfertas);
            var configs = this.ConfiguracaoEtapaDomainService.SearchProjectionBySpecification(spec,
                x => new ProrrogacaoConfiguracaoVO
                {
                    SeqConfiguracaoEtapa = x.Seq,
                    Descricao = x.Nome,
                    DataFim = x.DataFim,
                    DataLimiteEntregaDocumentacao = x.DataLimiteEntregaDocumentacao,
                    DataFimAntiga = x.DataFim,
                    DataFimMinina = DateTime.Now > x.DataFim ? DateTime.Now : x.DataFim,
                    //DataLimiteEntregaDocumentacaoAntiga =
                    //    x.DataLimiteEntregaDocumentacao.HasValue ?
                    //    (DateTime.Now > x.DataLimiteEntregaDocumentacao.Value ? DateTime.Now : x.DataLimiteEntregaDocumentacao.Value )
                    //    : x.DataLimiteEntregaDocumentacao,
                    DataLimiteEntregaDocumentacaoAntiga = x.DataLimiteEntregaDocumentacao
                }
                , true).ToList();

            foreach (var config in configs)
            {
                if (DateTime.Now > config.DataFim)
                    config.DataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                            config.DataFim.Hour, config.DataFim.Minute, config.DataFim.Second);

                if (config.DataLimiteEntregaDocumentacao.HasValue && DateTime.Now > config.DataLimiteEntregaDocumentacao)
                    config.DataLimiteEntregaDocumentacao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                            config.DataLimiteEntregaDocumentacao.Value.Hour,
                                                            config.DataLimiteEntregaDocumentacao.Value.Minute,
                                                            config.DataLimiteEntregaDocumentacao.Value.Second);

                config.Ofertas = new List<OfertaProrrogacaoVO>();
                GrupoOfertaFilterSpecification specGrupo =
                    new GrupoOfertaFilterSpecification
                    {
                        SeqConfiguracaoEtapa = config.SeqConfiguracaoEtapa,
                        SeqEtapaProcesso = seqEtapaProcesso
                    };

                var listaOfertas = this.GrupoOfertaDomainService.SearchProjectionBySpecification(specGrupo,
                    x => x.Ofertas.Where(o => seqOfertas.Any(s => s == o.Seq)).Select(o => o.Seq));

                config.SeqOfertas = new List<long>();

                foreach (var lista in listaOfertas)
                {
                    config.SeqOfertas.AddRange(lista);
                }
                var taxasSpec = new SMCContainsSpecification<OfertaPeriodoTaxa, long>(x => x.SeqOferta, config.SeqOfertas.ToArray());
                config.Taxas = this.OfertaPeriodoTaxaDomainService.SearchProjectionBySpecification(taxasSpec,
                    x => new TaxaProrrogacaoVO
                    {
                        SeqTaxa = x.SeqTaxa,
                        Descricao = x.Taxa.TipoTaxa.Descricao,
                    }, true).ToList();
            }

            return configs;
        }

        /// <summary>
        /// Recupera o sumário de uma prorrogação de processo para ser exibido para o usuário
        /// </summary>
        public ProrrogacaoEtapaVO SumarioProrrogacao(ProrrogacaoEtapaVO etapaProrrogar)
        {
            var spectEtapa = new SMCSeqSpecification<EtapaProcesso>(etapaProrrogar.SeqEtapaProcesso);
            etapaProrrogar.DataFim = etapaProrrogar.Configuracoes.Max(x => x.DataFim);
            var dadosEtapa = this.SearchProjectionByKey(spectEtapa, x => new
            {
                SeqEtapaSGF = x.SeqEtapaSGF,
                DataFimAntiga = x.DataFimEtapa,
                DataFinsConfiguracoes = x.Configuracoes.Select(t => t.DataFim)
            });
            var dataMax = dadosEtapa.DataFinsConfiguracoes.Max();
            if (etapaProrrogar.Configuracoes.Max(x => x.DataFim) < dataMax)
            {
                etapaProrrogar.DataFim = dataMax;
            }
            else
            {
                etapaProrrogar.DataFim = etapaProrrogar.Configuracoes.Max(x => x.DataFim);
            }
            etapaProrrogar.Descricao = this.EtapaService.BuscarEtapasKeyValue(new long[] { dadosEtapa.SeqEtapaSGF }).First().Descricao;
            etapaProrrogar.DataFimAntiga = dadosEtapa.DataFimAntiga;
            foreach (var config in etapaProrrogar.Configuracoes)
            {
                if (config.DataFim < DateTime.Now || config.DataFim < config.DataFimAntiga)
                {
                    throw new ProrrogacaoEtapaDataInvalidaException();
                }
                var specOfertas = new SMCContainsSpecification<Oferta, long>(x => x.Seq, config.SeqOfertas.ToArray());
                var ofertas = this.OfertaDomainService.SearchByDepth(specOfertas, 10, x => x.HierarquiaOfertaPai, x => x.Processo);
                if (ofertas.Any(x => x.DataFim > config.DataFim))
                {
                    var specConfig = new SMCSeqSpecification<ConfiguracaoEtapa>(config.SeqConfiguracaoEtapa);
                    var nomeConfig = this.ConfiguracaoEtapaDomainService.SearchProjectionByKey(specConfig, x => x.Nome);
                    throw new ProrrogacaoEtapaDataOfertaMaiorConfiguracaoException(nomeConfig);
                }
                config.Ofertas = new List<OfertaProrrogacaoVO>();
                foreach (var oferta in ofertas)
                {
                    var seqSpec = new SMCSeqSpecification<Oferta>(oferta.Seq);
                    oferta.Taxas = this.OfertaDomainService.SearchByKey(seqSpec, x => x.Taxas,
                                                                x => x.Taxas[0].Taxa, x => x.Taxas[0].Taxa.TipoTaxa).Taxas;

                    OfertaDomainService.AdicionarDescricaoCompleta(oferta, oferta.Processo.ExibirPeriodoAtividadeOferta);

                    var vo = new OfertaProrrogacaoVO
                    {
                        SeqOferta = oferta.Seq,
                        Descricao = oferta.DescricaoCompleta,
                        DataFimAntiga = oferta.DataFim,
                        DataFim = config.DataFim,
                        Taxas = new List<TaxaProrrogacaoVO>(),
                    };
                    var taxasPorTipo = oferta.Taxas.GroupBy(x => x.SeqTaxa);
                    foreach (var taxa in taxasPorTipo)
                    {
                        var ultimaTaxa = taxa.OrderByDescending(x => x.DataFim).First();
                        var valorAntigo = FinanceiroService.BuscarEventosTaxa(
                                new EventoTaxaFiltroData { SeqEventoTaxa = ultimaTaxa.SeqEventoTaxa }).First().Valor;
                        vo.Taxas.Add(new TaxaProrrogacaoVO
                        {
                            DataVencimentoAntiga = FinanceiroService.BuscarParametrosCREI(
                                new ParametroCREIFiltroData { SeqParametroCREI = ultimaTaxa.SeqParametroCrei }).First().DataVencimentoTitulo,
                            Descricao = ultimaTaxa.Taxa.TipoTaxa.Descricao,
                            ValorAntigo = valorAntigo,
                            ValorNovo = (!config.Taxas.First(x => x.SeqTaxa == ultimaTaxa.SeqTaxa).SeqEventoTaxa.HasValue) ?
                                                                valorAntigo :
                                                                FinanceiroService.BuscarEventosTaxa(new EventoTaxaFiltroData
                                                                {
                                                                    SeqEventoTaxa = (int?)config.Taxas.First(x => x.SeqTaxa == ultimaTaxa.SeqTaxa).SeqEventoTaxa
                                                                }).First().Valor,
                            SeqTaxa = ultimaTaxa.SeqTaxa,
                            DataVencimento = config.Taxas.First(x => x.SeqTaxa == ultimaTaxa.SeqTaxa).DataVencimento
                        });
                        if (vo.Taxas.Any(x => x.DataVencimento.Date < config.DataFim.Date))
                        {
                            throw new ProrrogacaoEtapaDataVencimentoInvalidaException();
                        }
                    }
                    config.Ofertas.Add(vo);
                }
            }

            return etapaProrrogar;
        }

        public void ProrrogarEtapa(ProrrogacaoEtapaVO etapaProrrogar)
        {
            //Testar se a etapa está em manutenção
            var etapaSpec = new SMCSeqSpecification<EtapaProcesso>(etapaProrrogar.SeqEtapaProcesso);
            var etapa = this.SearchByKey(etapaSpec);

            if (etapa.SituacaoEtapa != SituacaoEtapa.EmManutencao)
            {
                //Para alterar o período da etapa do processo, é necessário que ela esteja "Em manutenção".
                throw new EtapaProrrogacaoSituacaoInvalidaException();
            }

            if (PermissaoInscricaoForaPrazoDomainService.Count(
                        new PermissaoInscricaoForaPrazoCoincidenteSpecification()
                        {
                            DataInicio = etapa.DataInicioEtapa,
                            DataFim = etapaProrrogar.DataFim,
                            SeqProcesso = etapaProrrogar.SeqProcesso
                        }) > 0)
            {
                throw new EtapaProrrogacaoCoincidePermissaoForaPrazoException();
            }

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    var specProcesso = new SMCSeqSpecification<Processo>(etapa.SeqProcesso);
                    var seqEvento = this.ProcessoDomainService.SearchProjectionByKey(specProcesso, x => x.SeqEvento);

                    //Prorrogação da etapa
                    etapa.DataFimEtapa = etapaProrrogar.DataFim;
                    this.UpdateEntity(etapa);

                    //Prorrogar configuração
                    foreach (var configuracao in etapaProrrogar.Configuracoes)
                    {
                        var specConfiguracao =
                            new SMCSeqSpecification<ConfiguracaoEtapa>(configuracao.SeqConfiguracaoEtapa);
                        var configBanco = this.ConfiguracaoEtapaDomainService.SearchByKey(specConfiguracao, x => x.ArquivoImagem);
                        configBanco.DataFim = configuracao.DataFim;
                        configBanco.DataLimiteEntregaDocumentacao = configuracao.DataLimiteEntregaDocumentacao;

                        /* Task 35023: TSK - Alterar implementação UC_INS_001_03_17 - Alterar Período Etapa Processo (Problemas na prorrogação)
                              10. Para cada oferta selecionada para prorrogação:
                                10.1. Verificar se sua data fim é igual à nova data fim informada para sua configuração (não considerar hora), ou seja, a prorrogação está sendo feita para o mesmo dia. Se for igual:
                                  10.1.1. Verificar se, para alguma das taxas listadas, o evento taxa ou o vencimento informado são diferentes do evento taxa ou vencimento da oferta período taxa, cuja data fim é a mesma data fim da oferta. Se for diferente:
                                    10.1.1.1. Verificar se existe inscrição para a oferta em questão, com título gerado durante o período desta oferta período taxa. Se existir:
                                      10.1.1.1.1. Abortar a operação e emitir a mensagem de erro: Existe inscrição com boleto gerado neste período. Não é permitido prorrogar a etapa de inscrição para o mesmo dia, utilizando um evento taxa e/ou vencimento diferentes.”
                                    10.1.1.2. Se não existir:
                                      10.1.1.2.1. Atualizar o evento taxa e/ou o parâmetro CREI desta oferta período taxa, com os valores informados.
                                10.2. Se for diferente, ou seja, a prorrogação está sendo feita para outro dia:
                                  10.2.1. Criar um novo período de oferta taxa para cada taxa associada à oferta, iniciando um dia após a data fim da última oferta período taxa cadastrada para a taxa e terminando na nova data fim informada para a configuração. Não preencher hora na data início e fim do novo período.
                                    10.2.1.1. Caso tenha sido informado um novo evento taxa para um determinado tipo de taxa, vincular à nova oferta período taxa o novo evento taxa informado.
                                    10.2.1.2. Caso não tenha sido informado um evento taxa novo, vincular à nova oferta período taxa o evento taxa da última oferta período taxa criada para o tipo de taxa nesta oferta.
                                    10.2.1.3. Setar na nova oferta período taxa criada, os mesmos valores de quantidade mínima e máxima da última oferta período taxa criada para o tipo de taxa nesta oferta e o parâmetro CREI relativo à data de vencimento informada para a taxa.
                                10.3. Alterar a data fim da oferta, conforme nova data fim informada para sua configuração.*/

                        //Prorrogar oferta
                        foreach (var oferta in configuracao.Ofertas)
                        {
                            var specOferta = new SMCSeqSpecification<Oferta>(oferta.SeqOferta);
                            var ofertaBanco = this.OfertaDomainService.SearchByKey(specOferta, IncludesOferta.Taxas_Taxa);

                            ofertaBanco.DataFim = oferta.DataFim;

                            var taxas = ofertaBanco.Taxas;
                            ofertaBanco.Taxas = null;

                            this.OfertaDomainService.UpdateEntity(ofertaBanco);

                            //Prorrogar oferta período taxa
                            foreach (var taxa in configuracao.Taxas)
                            {
                                var taxaOfertaAProrrogar = taxas.Where(x => x.SeqTaxa == taxa.SeqTaxa && !x.SeqPermissaoInscricaoForaPrazo.HasValue).OrderByDescending(x => x.DataFim)
                                    .FirstOrDefault();
                                var taxaRef = taxaOfertaAProrrogar?.Taxa;

                                if (taxaOfertaAProrrogar != null)
                                {
                                    /*
                                     10.1.1. Verificar se, para alguma das taxas listadas, o evento taxa ou o vencimento informado são
                                     diferentes do evento taxa ou vencimento da oferta período taxa, cuja data fim é a mesma data fim
                                     da oferta. Se for diferente:
                                    */

                                    /*
                                    10.2. Se for diferente, ou seja, a prorrogação está sendo feita para outro dia:
                                        10.2.1. Criar um novo período de oferta taxa para cada taxa associada à oferta, iniciando um dia após a data fim da última oferta período taxa cadastrada para a taxa e terminando na nova data fim informada para a configuração. Não preencher hora na data início e fim do novo período.
                                            10.2.1.1. Caso tenha sido informado um novo evento taxa para um determinado tipo de taxa, vincular à nova oferta período taxa o novo evento taxa informado.
                                            10.2.1.2. Caso não tenha sido informado um evento taxa novo, vincular à nova oferta período taxa o evento taxa da última oferta período taxa criada para o tipo de taxa nesta oferta.
                                            10.2.1.3. Setar na nova oferta período taxa criada, os mesmos valores de quantidade mínima e máxima da última oferta período taxa criada para o tipo de taxa nesta oferta e o parâmetro CREI relativo à data de vencimento informada para a taxa.
                                    */

                                    /*
                                     10.1. Verificar se a data fim da taxa é igual à nova data fim informada para sua configuração
                                     (não considerar hora), ou seja, a prorrogação está sendo feita para o mesmo dia. Se for igual:*/

                                    var seqParametroCrei = this.FinanceiroService.RecuperaOuGeraCrei(seqEvento.Value, taxa.DataVencimento.Date, SMCContext.User.Identity.Name);

                                    //Caso a prorrogação seja apenas de horas
                                    if (configuracao.DataFim.Date == taxaOfertaAProrrogar.DataFim.Date)
                                    {
                                        /*  10.1.1. Verificar se, para alguma das taxas listadas, o evento taxa ou o vencimento
                                         *  informado são diferentes do evento taxa ou vencimento da oferta período taxa,
                                         *  cuja data fim é a mesma data fim da oferta. Se for diferente:*/
                                        if ((taxa.SeqEventoTaxa.HasValue && taxa.SeqEventoTaxa != taxaOfertaAProrrogar.SeqEventoTaxa) ||
                                           taxaOfertaAProrrogar.SeqParametroCrei != seqParametroCrei)
                                        {
                                            var dataFimTitulo = taxaOfertaAProrrogar.DataFim.Date.AddDays(1).AddSeconds(-1);

                                            /*  10.1.1.1. Verificar se existe inscrição para a oferta em questão, com título gerado
                                             *  durante o período desta oferta período taxa. Se existir:*/
                                            var possuiTituloGerado = OfertaDomainService.SearchProjectionByKey(oferta.SeqOferta, x => x.InscricoesOferta.Any(i =>
                                                               i.Inscricao.Boletos.Any(b =>
                                                                   b.Titulos.Any(t => t.DataGeracao >= taxaOfertaAProrrogar.DataInicio && t.DataGeracao <= dataFimTitulo) &&
                                                                   b.Taxas.Any(t => t.SeqTaxa == taxa.SeqTaxa))));

                                            // 10.1.1.1.1. Abortar a operação e emitir a mensagem de erro: Existe inscrição com boleto gerado neste período.
                                            // Não é permitido prorrogar a etapa de inscrição para o mesmo dia, utilizando um evento taxa e/ou vencimento diferentes.”
                                            if (possuiTituloGerado)
                                                throw new SMCApplicationException("Existe inscrição com boleto gerado neste período. Não é permitido prorrogar a etapa de inscrição para o mesmo dia, utilizando um evento taxa e/ou vencimento diferentes.");
                                        }
                                    }
                                    /*10.2. Se for diferente, ou seja, a prorrogação está sendo feita para outro dia:
                                        10.2.1. Criar um novo período de oferta taxa para cada taxa associada à oferta, iniciando um dia após a data fim da última oferta período taxa cadastrada para a taxa e terminando na nova data fim informada para a configuração. Não preencher hora na data início e fim do novo período.
                                            10.2.1.1. Caso tenha sido informado um novo evento taxa para um determinado tipo de taxa, vincular à nova oferta período taxa o novo evento taxa informado.
                                            10.2.1.2. Caso não tenha sido informado um evento taxa novo, vincular à nova oferta período taxa o evento taxa da última oferta período taxa criada para o tipo de taxa nesta oferta.
                                            10.2.1.3. Setar na nova oferta período taxa criada, os mesmos valores de quantidade mínima e máxima da última oferta período taxa criada para o tipo de taxa nesta oferta e o parâmetro CREI relativo à data de vencimento informada para a taxa.
                                    */
                                    else
                                    {
                                        taxaOfertaAProrrogar.Seq = 0;
                                        taxaOfertaAProrrogar.DataInicio = taxaOfertaAProrrogar.DataFim.AddDays(1).Date;
                                    }
                                    taxaOfertaAProrrogar.DataFim = configuracao.DataFim.Date;

                                    /*10.1.1.2. Se não existir:
                                        10.1.1.2.1. Atualizar o evento taxa e/ou o parâmetro CREI desta oferta período taxa, com os valores informados.*/
                                    if (taxa.SeqEventoTaxa.HasValue)
                                        taxaOfertaAProrrogar.SeqEventoTaxa = (int)taxa.SeqEventoTaxa.Value;

                                    taxaOfertaAProrrogar.SeqParametroCrei = seqParametroCrei;

                                    this.OfertaPeriodoTaxaDomainService.SaveEntity(taxaOfertaAProrrogar);
                                }

                                // Busca todas as ofertas taxas com o mesmo tipo de taxa
                                if (taxaRef != null)
                                {
                                    var spec = new OfertaTipoTaxaSpecification() { SeqProcesso = etapa.SeqProcesso, SeqTipoTaxa = taxaRef.SeqTipoTaxa };
                                    var ofertaTaxas = OfertaPeriodoTaxaDomainService.SearchBySpecification(spec).ToArray();

                                    if (taxaRef.CobrarPorOferta.HasValue && taxaRef.CobrarPorOferta.Value
                                        && ProcessoDomainService.VerificarOfertaTaxaDeValoresDiferentesNaMesmaData(ofertaTaxas))
                                    {
                                        var taxaBanco = TaxaDomainService.SearchByKey(new SMCSeqSpecification<Taxa>(taxa.SeqTaxa), x => x.TipoTaxa);
                                        throw new ProrrogarEtapaValoresDistintosPeriodosCoincidentesException(taxaBanco.TipoTaxa.Descricao);
                                    }
                                }
                            }
                        }

                        this.ConfiguracaoEtapaDomainService.UpdateEntity(configBanco);
                    }

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Verifica se é possível prorrogar a etapa informada
        /// </summary>
        public void VerificarPossibilidadeProrrogacao(long seqEtapaProcesso)
        {
            var spec = new SMCSeqSpecification<EtapaProcesso>(seqEtapaProcesso);
            if (this.SearchProjectionByKey(spec, x => x.Token) != TOKENS.ETAPA_INSCRICAO)
            {
                throw new ProrrogacaoEtapaDiferenteInscricaoException();
            }
        }

        #endregion Prorrogação Etapa
    }
}