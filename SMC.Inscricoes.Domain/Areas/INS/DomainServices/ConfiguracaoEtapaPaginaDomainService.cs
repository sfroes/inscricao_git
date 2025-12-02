using SMC.Formularios.Common.Areas.TMP.Enums;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Formularios.ServiceContract.TMP.Data;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class ConfiguracaoEtapaPaginaDomainService : InscricaoContextDomain<ConfiguracaoEtapaPagina>
    {
        #region DomainServices

        private ConfiguracaoEtapaPaginaIdiomaDomainService ConfiguracaoEtapaPaginaIdiomaDomainService
        {
            get
            {
                return this.Create<ConfiguracaoEtapaPaginaIdiomaDomainService>();
            }
        }

        private TextoSecaoPaginaDomainService TextoSecaoPaginaDomainService
        {
            get
            {
                return this.Create<TextoSecaoPaginaDomainService>();
            }
        }

        private ArquivoSecaoPaginaDomainService ArquivoSecaoPaginaDomainService
        {
            get
            {
                return this.Create<ArquivoSecaoPaginaDomainService>();
            }
        }

        private IPaginaService PaginaService
        {
            get
            {
                return this.Create<IPaginaService>();
            }
        }

        private ISituacaoService SituacaoService
        {
            get { return this.Create<ISituacaoService>(); }
        }

        private InscricaoDadoFormularioDomainService InscricaoDadoFormularioDomainService
        {
            get { return this.Create<InscricaoDadoFormularioDomainService>(); }
        }

        private InscricaoDadoFormularioCampoDomainService InscricaoDadoFormularioCampoDomainService
        {
            get { return this.Create<InscricaoDadoFormularioCampoDomainService>(); }
        }

        private InscricaoHistoricoPaginaDomainService InscricaoHistoricoPaginaDomainService
        {
            get { return this.Create<InscricaoHistoricoPaginaDomainService>(); }
        }

        #endregion

        /// <summary>
        /// Exclui uma página de uma configuração de etapa e suas seções filhas para todos os idiomas
        /// </summary>        
        public void ExcluirConfiguracaoEtapaPagina(long seqConfiguracaoEtapaPagina)
        {

            var paginaCompleta = this.SearchByKey(
                       new SMCSeqSpecification<ConfiguracaoEtapaPagina>(seqConfiguracaoEtapaPagina),
                                    x => x.Idiomas,
                                    x => x.Idiomas[0].InscricaoDadoFormulario[0].DadosCampos,
                                    x => x.Idiomas[0].Textos, 
                                    x => x.Idiomas[0].Arquivos, 
                                    x => x.InscricaoHistoricoPaginas[0].Inscricao.HistoricosSituacao[0].TipoProcessoSituacao);
            ExcluirConfiguracaoEtapaPagina(paginaCompleta);
        }

        internal void ExcluirConfiguracaoEtapaPagina(ConfiguracaoEtapaPagina paginaCompleta)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    var configuracaoEtapaPagina = this.SearchByKey(
                       new SMCSeqSpecification<ConfiguracaoEtapaPagina>(paginaCompleta.Seq),
                            IncludesConfiguracaoEtapaPagina.ConfiguracaoEtapa_EtapaProcesso);
                    if (configuracaoEtapaPagina.ConfiguracaoEtapa.EtapaProcesso.SituacaoEtapa == SituacaoEtapa.Liberada)
                    {
                        throw new ExclusaoPaginaEtapaLiberadaException();
                    }

                    var motivosTeste = SituacaoService.BuscarSeqMotivosSituacaoPorToken(TOKENS.MOTIVO_INSCRICAO_CANCELADA_TESTE);
                    if (paginaCompleta.InscricaoHistoricoPaginas != null)
                    {
                        // Verifica se todas as inscrições são de teste
                        if (paginaCompleta.InscricaoHistoricoPaginas.Any(
                            s => s.Inscricao.HistoricosSituacao.Any(
                                                            f => f.Atual &&
                                                            (f.TipoProcessoSituacao.Token != TOKENS.SITUACAO_INSCRICAO_CANCELADA ||
                                                            f.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA &&
                                                                (!f.SeqMotivoSituacaoSGF.HasValue || !motivosTeste.Contains(f.SeqMotivoSituacaoSGF.Value)))
                                                        )
                            ))
                        {
                            throw new ExclusaoPaginaEtapaComInscricaoException();
                        }

                        foreach (var historico in paginaCompleta.InscricaoHistoricoPaginas)
                        {
                            InscricaoHistoricoPaginaDomainService.DeleteEntity(historico);
                        }
                    }      

                    foreach (var idioma in paginaCompleta.Idiomas)
                    {                        
                        foreach (var texto in idioma.Textos)
                        {
                            this.TextoSecaoPaginaDomainService.DeleteEntity(texto);
                        }
                        idioma.Textos = null;

                        foreach (var arquivo in idioma.Arquivos)
                        {
                            this.ArquivoSecaoPaginaDomainService.DeleteEntity(arquivo);
                        }

                        if (idioma.InscricaoDadoFormulario != null)
                        {
                            foreach (var inscricaDadoFormulario in idioma.InscricaoDadoFormulario)
                            {
                                foreach (var campo in inscricaDadoFormulario.DadosCampos)
                                {
                                    InscricaoDadoFormularioCampoDomainService.DeleteEntity(campo);
                                }
                                InscricaoDadoFormularioDomainService.DeleteEntity(inscricaDadoFormulario);
                            }
                        }

                        idioma.Arquivos = null;
                        this.ConfiguracaoEtapaPaginaIdiomaDomainService.DeleteEntity(idioma);
                    }

                    paginaCompleta.Idiomas = null;
                    this.DeleteEntity(paginaCompleta);
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
        /// Duplica uma página copiando os valores padrão do SGF se necessário
        /// </summary>        
        public void DuplicarConfiguracaoEtapaPagina(long seqConfiguracaoEtapaPagina)
        {
            var etapaLiberada = this.SearchProjectionByKey(
                        new SMCSeqSpecification<ConfiguracaoEtapaPagina>(seqConfiguracaoEtapaPagina),
                        x => x.ConfiguracaoEtapa.EtapaProcesso.SituacaoEtapa == SituacaoEtapa.Liberada);
            if (etapaLiberada)
            {
                throw new DuplicacaoPaginaEtapaLiberadaException();
            }
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    var dadosPagina = this.SearchProjectionByKey(
                        new SMCSeqSpecification<ConfiguracaoEtapaPagina>(seqConfiguracaoEtapaPagina),
                        x => new
                        {
                            SeqPaginaEtapaSGF = x.SeqPaginaEtapaSGF,
                            SeqConfiguracaoEtapa = x.SeqConfiguracaoEtapa,
                            Idiomas = x.Idiomas.Select(i => i.Idioma),
                            Ordem = x.Ordem
                        });
                    short ordem = dadosPagina.Ordem;
                    ordem++;
                    var paginaEtapaSGF = this.PaginaService.BuscarPaginaEtapa(dadosPagina.SeqPaginaEtapaSGF);
                    CriarPaginaPadraoSGF(dadosPagina.SeqConfiguracaoEtapa,
                        dadosPagina.Idiomas,
                        paginaEtapaSGF, ordem);
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
        /// Busca uma configuração etapa página para ser exibida em edição
        /// </summary>        
        public ConfiguracaoPaginaVO BuscarConfiguracaoEtapaPaginaEdicao(long seqConfiguracaoEtapaPagina) 
        {
           var ret =  this.SearchProjectionByKey(
                new SMCSeqSpecification<ConfiguracaoEtapaPagina>(seqConfiguracaoEtapaPagina),
                x => new ConfiguracaoPaginaVO
                {
                    SeqConfiguracaoEtapaPagina = seqConfiguracaoEtapaPagina,
                    SeqPaginaEtapaSGF = x.SeqPaginaEtapaSGF,
                    ExibirComprovante = x.ExibeComprovanteInscricao,
                    ExibirConfirmacao = x.ExibeConfirmacaoInscricao,
                    ExibeDadosPessoais = x.ExibeDadosPessoais,
                    PaginaToken = x.Token
                });
           ret.DescricaoPagina = this.PaginaService.BuscarPaginasEtapaCompletas(
               new long[]{ret.SeqPaginaEtapaSGF})[0].Pagina.Descricao;
           return ret;
        }

        /// <summary>
        /// Salva as alterações numa configuração etapa página
        /// </summary>        
        public void SalvarConfiguracaoPagina(ConfiguracaoPaginaVO configuracaoPagina) 
        {
            //Aplicar a RN_INS_113
            var dados = this.SearchProjectionByKey(
                new SMCSeqSpecification<ConfiguracaoEtapaPagina>(configuracaoPagina.SeqConfiguracaoEtapaPagina),
                x => new
                {
                    config = x,
                    Situacao = x.ConfiguracaoEtapa.EtapaProcesso.SituacaoEtapa
                });
            if ((dados.config.ExibeComprovanteInscricao != configuracaoPagina.ExibirComprovante
                || dados.config.ExibeConfirmacaoInscricao != configuracaoPagina.ExibirConfirmacao)
                && dados.Situacao==SituacaoEtapa.Liberada) 
            {
                throw new AlteracaoPaginaEtapaLiberadaException();                
            }
            dados.config.ExibeConfirmacaoInscricao = configuracaoPagina.ExibirConfirmacao;
            dados.config.ExibeComprovanteInscricao = configuracaoPagina.ExibirComprovante;
            dados.config.ExibeDadosPessoais = configuracaoPagina.ExibeDadosPessoais;
            this.UpdateEntity(dados.config);
        }

        /// <summary>
        /// Criar a coniguração etapa página no GPI com base na página do SGF
        /// Realiza controle de transação se necessário
        /// </summary>        
        internal void CriarPaginaPadraoSGF(long seqConfiguracaoEtapa, IEnumerable<SMCLanguage> idiomas,
            PaginaEtapaData paginaEtapaSGF, short? ordem = null)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    ConfiguracaoEtapaPagina pagina = new ConfiguracaoEtapaPagina
                    {
                        SeqPaginaEtapaSGF = paginaEtapaSGF.Seq,
                        Ordem = ordem.HasValue ? ordem.Value : (Int16)(paginaEtapaSGF.Ordem * 10),
                        Token = paginaEtapaSGF.Pagina.Token,
                        SeqConfiguracaoEtapa = seqConfiguracaoEtapa
                    };

                    if (paginaEtapaSGF.Pagina.Token == TOKENS.PAGINA_FORMULARIO_INSCRICAO ||
                        paginaEtapaSGF.Pagina.Token == TOKENS.PAGINA_CODIGO_AUTORIZACAO ||
                        paginaEtapaSGF.Pagina.Token == TOKENS.PAGINA_UPLOAD_DOCUMENTOS)
                    {
                        pagina.ExibeComprovanteInscricao = true;
                        pagina.ExibeConfirmacaoInscricao = true;
                    }

                    // RN_INS_088 
                    if (paginaEtapaSGF.Pagina.Token == TOKENS.PAGINA_COMPROVANTE_INSCRICAO)
                    {
                        pagina.ExibeDadosPessoais = true;
                    }

                    pagina = this.InsertEntity(pagina);
                    CriarPaginaIdioma(idiomas, paginaEtapaSGF, pagina.Seq);
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        internal void CriarPaginaIdioma(IEnumerable<SMCLanguage> idiomas, PaginaEtapaData paginaEtapaSGF, long SeqConfiguracaoEtapaPagina)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    foreach (var idioma in idiomas)
                    {
                        ConfiguracaoEtapaPaginaIdioma paginaIdioma = new ConfiguracaoEtapaPaginaIdioma
                        {
                            SeqConfiguracaoEtapaPagina = SeqConfiguracaoEtapaPagina,
                            Idioma = idioma,
                            Titulo = paginaEtapaSGF.Pagina.Titulo,
                        };
                        paginaIdioma = ConfiguracaoEtapaPaginaIdiomaDomainService.InsertEntity(paginaIdioma);

                        foreach (var secao in
                            paginaEtapaSGF.Pagina.Secoes.Where(x => x.TipoSecaoPagina == TipoSecaoPagina.Texto))
                        {
                            TextoSecaoPagina secaoTexto = new TextoSecaoPagina
                            {
                                SeqConfiguracaoEtapaPaginaIdioma = paginaIdioma.Seq,
                                SeqSecaoPaginaSGF = secao.Seq,
                                Texto = secao.TextoPadrao,
                                Token = secao.Token
                            };
                            TextoSecaoPaginaDomainService.InsertEntity(secaoTexto);
                        }
                    }
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

       
    }
}
