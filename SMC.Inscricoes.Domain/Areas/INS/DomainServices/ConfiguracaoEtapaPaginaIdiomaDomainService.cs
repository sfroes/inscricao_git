using SMC.Formularios.Common.Areas.FRM.Includes;
using SMC.Formularios.ServiceContract.Areas.FRM.Interfaces;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Exceptions.Processo;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class ConfiguracaoEtapaPaginaIdiomaDomainService : InscricaoContextDomain<ConfiguracaoEtapaPaginaIdioma>
    {
        #region Services
        private TextoSecaoPaginaDomainService TextoSecaoPaginaDomainService
        {
            get { return this.Create<TextoSecaoPaginaDomainService>(); }
        }

        private IPaginaService PaginaService
        {
            get { return this.Create<IPaginaService>(); }
        }

        private IFormularioService FormularioService
        {
            get { return this.Create<IFormularioService>(); }
        }

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        private ISituacaoService SituacaoService
        {
            get { return this.Create<ISituacaoService>(); }
        }
        #endregion

        /// <summary>
        /// Busca as informações de uma página
        /// </summary>
        /// <param name="filtro">Filtro para pesquisa</param>
        /// <returns>Informações da página</returns>
        public PaginaVO BuscarPagina(ConfiguracaoEtapaPaginaIdiomaFilterSpecification filtro)
        {
            // Verifica se infomrou a configuração da etapa x página no filtro. Idioma já é obrigatório
            if (!filtro.SeqConfiguracaoEtapaPagina.HasValue)
                throw new ConfiguracaoEtapaInvalidaException();

            //FIX: Cache
            //return GetCacheResult(() =>
            //{
            IncludesConfiguracaoEtapaPaginaIdioma includes = IncludesConfiguracaoEtapaPaginaIdioma.Arquivos |
                                                             IncludesConfiguracaoEtapaPaginaIdioma.Arquivos_Arquivo |
                                                             IncludesConfiguracaoEtapaPaginaIdioma.Textos |
                                                             IncludesConfiguracaoEtapaPaginaIdioma.ConfiguracaoEtapaPagina;
            ConfiguracaoEtapaPaginaIdioma pagina = this.SearchByKey(filtro, includes);
            if (pagina == null)
                throw new ConfiguracaoEtapaPaginaIdiomaInvalidaException();

            // Cria o VO da página
            PaginaVO vo = new PaginaVO()
            {
                SeqConfiguracaoEtapaPagina = pagina.SeqConfiguracaoEtapaPagina,
                SeqConfiguracaoEtapa = pagina.ConfiguracaoEtapaPagina.SeqConfiguracaoEtapa,
                Titulo = pagina.Titulo,
                Ordem = pagina.ConfiguracaoEtapaPagina.Ordem,
                Secoes = new List<SecaoPaginaVO>()
            };

            // Inclui as seções de texto
            foreach (var texto in pagina.Textos)
            {
                SecaoPaginaTextoVO voTxt = new SecaoPaginaTextoVO()
                {
                    Token = texto.Token,
                    Texto = texto.Texto
                };
                vo.Secoes.Add(voTxt);
            }

            // Inclui as seções de arquivo
            foreach (var arquivo in pagina.Arquivos.OrderBy(x => x.Ordem))
            {
                // Verifica se na lista já existe uma seção de arquivo com o token
                SecaoPaginaArquivoVO voArq = (SecaoPaginaArquivoVO)vo.Secoes.SingleOrDefault(s => s.Token.Equals(arquivo.Token));
                if (voArq == null)
                {
                    voArq = new SecaoPaginaArquivoVO()
                    {
                        Token = arquivo.Token,
                        Arquivos = new List<ArquivoSecaoVO>()
                    };
                    vo.Secoes.Add(voArq);
                }
                ArquivoSecaoVO arq = new ArquivoSecaoVO()
                {
                    SeqArquivo = arquivo.SeqArquivo,
                    NomeLink = arquivo.NomeLink,
                    Descricao = arquivo.Descricao,
                    Arquivo = SMCMapperHelper.Create<SMCUploadFile>(arquivo.Arquivo)
                };
                voArq.Arquivos.Add(arq);
            }

            return vo;
            // }, string.Format("{0}.{1}.Pagina", filtro.SeqConfiguracaoEtapaPagina.Value, filtro.Idioma));
        }

        public void ExcluirConfiguracaoEtapaPaginaIdioma(long seqConfiguracaoEtapaPaginaIdioma)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    var item = this.SearchByKey(
                        new SMCSeqSpecification<ConfiguracaoEtapaPaginaIdioma>(seqConfiguracaoEtapaPaginaIdioma),
                        x => x.Textos);

                    foreach (var texto in item.Textos)
                    {
                        this.TextoSecaoPaginaDomainService.DeleteEntity(texto);
                    }
                    item.Textos = null;
                    this.DeleteEntity(item);
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
        /// Exclui todas as páginas para os idiomas informados na configuração de etapa informada
        /// </summary>
        public void ExcluirPaginasIdiomas(long seqConfiguracaoEtapa, SMCLanguage[] idiomas)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    var spec = new ConfiguracaoEtapaPaginaIdiomaFilterSpecification
                    {
                        SeqConfiguracaoEtapa = seqConfiguracaoEtapa
                    };
                    var containsSpec = new SMCContainsSpecification<ConfiguracaoEtapaPaginaIdioma, SMCLanguage>(x => x.Idioma, idiomas);
                    var paginasApagar = this.SearchProjectionBySpecification(spec & containsSpec, x => x.Seq);

                    foreach (var seqPag in paginasApagar)
                    {
                        this.ExcluirConfiguracaoEtapaPaginaIdioma(seqPag);
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

        /// <summary>
        /// Busca uma configuração de página por idioma para edição e/ou exibição
        /// </summary>        
        public ConfiguracaoPaginaIdiomaVO BuscarConfiguracaoEtapaPaginaIdioma(long seqConfiguracaoEtapaPaginaIdioma)
        {
            var ret = this.SearchProjectionByKey(
                new SMCSeqSpecification<ConfiguracaoEtapaPaginaIdioma>(seqConfiguracaoEtapaPaginaIdioma),
                x => new ConfiguracaoPaginaIdiomaVO
                {
                    SeqConfiguracaoEtapaPaginaIdioma = seqConfiguracaoEtapaPaginaIdioma,
                    SeqPaginaEtapaSGF = x.ConfiguracaoEtapaPagina.SeqPaginaEtapaSGF,
                    SeqUnidadeResponsavel = x.ConfiguracaoEtapaPagina.ConfiguracaoEtapa.EtapaProcesso.Processo.SeqUnidadeResponsavel,
                    SeqFormulario = x.SeqFormularioSGF,
                    Titulo = x.Titulo,
                    SeqVisao = x.SeqVisaoSGF,
                    SeqVisaoGestao = x.SeqVisaoGestaoSGF
                });
            if (ret.SeqFormulario.HasValue)
            {
                ret.SeqTipoFormulario = FormularioService.BuscarFormulario(ret.SeqFormulario.Value, IncludesFormulario.Nenhum).SeqTipoFormulario;
            }
            ret.ExibeFormulario = PaginaService.BuscarPaginasEtapaCompletas(new long[] { ret.SeqPaginaEtapaSGF })[0]
                .Pagina.ExibeFormulario;
            return ret;
        }

        /// <summary>
        /// Salva a configuração da página no idioma
        /// </summary>        
        public void SalvarConfiguracaoEtapaPaginaIdioma(ConfiguracaoPaginaIdiomaVO configuracaoPaginaIdioma)
        {
            var processo = ProcessoDomainService.SearchByKey(configuracaoPaginaIdioma.SeqProcesso, IncludesProcesso.ConfiguracoesFormulario);

            if (!string.IsNullOrEmpty(configuracaoPaginaIdioma.PaginaToken) && configuracaoPaginaIdioma.PaginaToken == TOKENS.PAGINA_FORMULARIO_INSCRICAO)
            {
                if (processo.ConfiguracoesFormulario.Any())
                {
                    var possuiFormularioInscricao = processo.ConfiguracoesFormulario.Any(cf => cf.SeqFormularioSgf == configuracaoPaginaIdioma.SeqFormulario);

                    if (possuiFormularioInscricao)
                    {
                        throw new UsarMesmoFormularioInscricaoEventoProcessoException();
                    }
                }
            }


            // A visão de inscrição não pode ser igual à visão de gestão.
            if (configuracaoPaginaIdioma.SeqVisao.HasValue && configuracaoPaginaIdioma.SeqVisao == configuracaoPaginaIdioma.SeqVisaoGestao)
            {
                throw new VisaoGestaoInvalidaException();
            }

            var spec = new SMCSeqSpecification<ConfiguracaoEtapaPaginaIdioma>(
                configuracaoPaginaIdioma.SeqConfiguracaoEtapaPaginaIdioma);
            //Aplicar a RN_INS_114
            var dados = this.SearchProjectionByKey(
                spec,
                x => new
                {
                    Config = x,
                    Situacao = x.ConfiguracaoEtapaPagina.ConfiguracaoEtapa.EtapaProcesso.SituacaoEtapa,
                    SeqConfiguracoesEtapa = x.ConfiguracaoEtapaPagina.SeqConfiguracaoEtapa
                });
            if (dados.Situacao == SituacaoEtapa.Liberada)
            {
                throw new AlteracaoPaginaEtapaLiberadaException();
            }

            // Verifica se houve alteração no formulário ou em alguma visão.
            if ((dados.Config.SeqFormularioSGF.HasValue && dados.Config.SeqFormularioSGF != configuracaoPaginaIdioma.SeqFormulario)
             || (dados.Config.SeqVisaoSGF.HasValue && dados.Config.SeqVisaoSGF != configuracaoPaginaIdioma.SeqVisao)
             || (dados.Config.SeqVisaoGestaoSGF.HasValue && dados.Config.SeqVisaoGestaoSGF != configuracaoPaginaIdioma.SeqVisaoGestao))
            {
                var motivosTeste = SituacaoService.BuscarSeqMotivosSituacaoPorToken(TOKENS.MOTIVO_INSCRICAO_CANCELADA_TESTE);
                if (this.InscricaoDomainService.Count(
                            new InscricaoComFormularioPreenchidoSpecification(dados.SeqConfiguracoesEtapa, dados.Config.SeqFormularioSGF.Value) &
                            new InscricoesNaoCanceladasPorMotivoTesteSpecification(motivosTeste)
                    ) > 0)
                {
                    throw new AlteracaoPaginaEtapaComInscricaoException();
                }
            }

            if (configuracaoPaginaIdioma.SeqFormulario.HasValue)
            {
                if (configuracaoPaginaIdioma.SeqFormulario != dados.Config.SeqFormularioSGF)
                {
                    var seqTipoFormulario = this.FormularioService.
                        BuscarFormulario(configuracaoPaginaIdioma.SeqFormulario.Value, IncludesFormulario.Nenhum).SeqTipoFormulario;

                    var tiposFormulario = this.SearchProjectionByKey(spec, x => x.ConfiguracaoEtapaPagina.ConfiguracaoEtapa.
                        EtapaProcesso.Processo.UnidadeResponsavel.TiposFormulario);
                    if (tiposFormulario.Any(x => x.SeqTipoFormularioSGF == seqTipoFormulario && !x.Ativo))
                    {
                        //O tipo de formulário do formulário informado foi desativado. Ele não pode mais ser utilizado.
                        throw new ConfiguracaoPaginaIdiomaTipoFormularioDesativadoException();

                    }
                }
            }

            dados.Config.SeqFormularioSGF = configuracaoPaginaIdioma.SeqFormulario;
            dados.Config.SeqVisaoSGF = configuracaoPaginaIdioma.SeqVisao;
            dados.Config.SeqVisaoGestaoSGF = configuracaoPaginaIdioma.SeqVisaoGestao;
            dados.Config.Titulo = configuracaoPaginaIdioma.Titulo;
            this.UpdateEntity(dados.Config);
        }
    }
}
