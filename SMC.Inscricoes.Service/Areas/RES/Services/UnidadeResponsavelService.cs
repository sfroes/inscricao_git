using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.RES;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.RES.DomainServices;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using SMC.Inscricoes.Domain.Areas.RES.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.RES.Data;
using SMC.Inscricoes.ServiceContract.Areas.RES.Data.UnidadeResponsavel;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;
using System.Collections.Generic;
using System.Linq;
using FormularioService = SMC.Formularios.ServiceContract.Areas.FRM.Interfaces.IFormularioService;
using IUnidadeResponsavelService = SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces.IUnidadeResponsavelService;

namespace SMC.Inscricoes.Service.Areas.RES.Services
{
    public class UnidadeResponsavelService : SMCServiceBase, IUnidadeResponsavelService
    {

        #region DomainService

        private UnidadeResponsavelDomainService UnidadeResponsavelDomainService
        {
            get { return this.Create<UnidadeResponsavelDomainService>(); }
        }

        private UnidadeResponsavelTipoFormularioDomainService UnidadeResponsavelTipoFormularioDomainService
        {
            get { return this.Create<UnidadeResponsavelTipoFormularioDomainService>(); }
        }

        private UnidadeResponsavelTipoProcessoDomainService UnidadeResponsavelTipoProcessoDomainService
        {
            get { return this.Create<UnidadeResponsavelTipoProcessoDomainService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        private FormularioService FormularioService
        {
            get { return this.Create<FormularioService>(); }
        }


        #endregion

        #region Consultas

        public List<SMCDatasourceItem> BuscarUnidadesResponsaveisKeyValue()
        {
            return UnidadeResponsavelDomainService.BuscarUnidadesResponsaveisKeyValue().ToList();
        }

        /// <summary>
        /// Retorna a lista de unidades responsáveis para exibição em listagem
        /// </summary>                
        public SMCPagerData<UnidadeResponsavelListaData> BuscarUnidadesResponsaveis(UnidadeResponsavelFiltroData filtro)
        {
            var spec = SMCMapperHelper.Create<UnidadeResponsavelFilterSpecification>(filtro);
            int total = 0;
            var itens = this.UnidadeResponsavelDomainService.SearchProjectionBySpecification(spec,
                x => new UnidadeResponsavelListaData
                {
                    Nome = x.Nome,
                    Seq = x.Seq,
                    Sigla = x.Sigla
                }, out total);
            return new SMCPagerData<UnidadeResponsavelListaData>(itens, total);
        }

        /// <summary>
        /// Retorna uma unidade responsável pelo sequencial
        /// </summary>                
        public UnidadeResponsavelData BuscarUnidadeResponsavel(long seqUnidadeResponsavel)
        {
            var a = UnidadeResponsavelDomainService.SearchByKey<UnidadeResponsavel, UnidadeResponsavelData>(
            seqUnidadeResponsavel, IncludesUnidadeResponsavel.Enderecos |
                                   IncludesUnidadeResponsavel.EnderecosEletronicos |
                                   IncludesUnidadeResponsavel.Telefones |
                                   IncludesUnidadeResponsavel.CentrosCusto);
            return a;
        }

        /// <summary>
        /// Retorna o nome e a sigla de uma unidades responsável
        /// </summary>
        public UnidadeResponsavelData BuscarUnidadeResponsavelSimplificado(long seqUnidadeResponsavel)
        {
            var spec = new UnidadeResponsavelSpecification() { SeqUnidadeResponsavel = seqUnidadeResponsavel };
            return this.UnidadeResponsavelDomainService.SearchProjectionByKey(spec,
                x => new UnidadeResponsavelData
                {
                    Seq = x.Seq,
                    Nome = x.Nome,
                    Sigla = x.Sigla,
                    CodigoUnidade = x.CodigoUnidade
                });
        }

        /// <summary>
        /// Busca a unidade responsável do SGF vinculada a uma unidade responsável GPI
        /// </summary>
        /// <param name="seqUnidadeResponsavel">Sequencial da unidade responável GPI</param>
        /// <returns>Sequencial da unidade responsável SGF vinculada a unidade responsável GPI</returns>
        public long? BuscarSeqUnidadeResponsavelSgf(long seqUnidadeResponsavel)
        {
            return this.UnidadeResponsavelDomainService.SearchProjectionByKey(seqUnidadeResponsavel, x => x.SeqUnidadeResponsavelSgf);
        }


        #endregion

        #region CRUD

        /// <summary>
        /// Salva a unidade responsável e retorna o sequencial da mesma
        /// </summary>                
        public long SalvarUnidadeResponsavel(UnidadeResponsavelData unidadeResponsavel)
        {
            return UnidadeResponsavelDomainService.SalvarUnidadeResponsavel(unidadeResponsavel);
        }

        /// <summary>
        /// Salva a unidade responsável e retorna o sequencial da mesma
        /// </summary>                
        public void ExcluirUnidadeResponsavel(long seqUnidadeResponsavel)
        {
            UnidadeResponsavelDomainService.DeleteEntity<UnidadeResponsavel>(seqUnidadeResponsavel);
        }

        #endregion

        #region Tipo Formulario
        public SMCPagerData<UnidadeResponsavelTipoFormularioListaData> BuscarUnidadeResponsavelTiposFormularios(long seqUnidadeResponsavel)
        {
            var data = UnidadeResponsavelTipoFormularioDomainService.BuscarUnidadeResponsavelTiposFormularios(seqUnidadeResponsavel)
                            .TransformList<UnidadeResponsavelTipoFormularioListaData>();
            return new SMCPagerData<UnidadeResponsavelTipoFormularioListaData>(data, data.Count);
        }

        public UnidadeResponsavelTipoFormularioData BuscarUnidadeResponsavelTipoFormulario(long seqFormulario)
        {
            var spec = new SMCSeqSpecification<UnidadeResponsavelTipoFormulario>(seqFormulario);
            return UnidadeResponsavelTipoFormularioDomainService.SearchProjectionByKey(spec,
                        x => new UnidadeResponsavelTipoFormularioData
                        {
                            Seq = x.Seq,
                            SeqTipoFormulario = x.SeqTipoFormularioSGF,
                            SeqUnidadeResponsavel = x.SeqUnidadeResponsavel,
                            Ativo = x.Ativo
                        });
        }

        public long SalvarTipoFormularioUnidadeResponsavel(UnidadeResponsavelTipoFormularioData tipoFormulario)
        {
            return UnidadeResponsavelTipoFormularioDomainService.Salvar(SMCMapperHelper.Create<UnidadeResponsavelTipoFormulario>(tipoFormulario));
        }

        public void ExcluirUnidadeResponsavelTipoFormulario(long seqConfiguracaoTipoFormulario)
        {
            UnidadeResponsavelTipoFormularioDomainService.Excluir(seqConfiguracaoTipoFormulario);
        }

        /// <summary>
        /// Busca a lista de tipos de formulário associados a unidade responsável
        /// </summary>
        public List<SMCDatasourceItem> BuscarTiposFormularioKeyValue(long seqUnidadeResponsavel)
        {
            return this.UnidadeResponsavelTipoFormularioDomainService
                .BuscarTiposFormularioKeyValue(seqUnidadeResponsavel);
        }

        #endregion

        #region[services]
        private INotificacaoService NotificacaoService
        {
            get { return this.Create<INotificacaoService>(); }
        }
        #endregion

        #region Tipo Processo

        public SMCPagerData<UnidadeResponsavelTipoProcessoListaData> BuscarUnidadeResponsavelTiposProcessos(long seqUnidadeResponsavel)
        {
            var spec = new SMCPropertySpecification<UnidadeResponsavelTipoProcesso>("SeqUnidadeResponsavel", seqUnidadeResponsavel);
            spec.SetOrderBy(o => o.TipoProcesso.Descricao);

            var data = UnidadeResponsavelTipoProcessoDomainService.SearchProjectionBySpecification(spec,
                                x => new UnidadeResponsavelTipoProcessoListaData
                                {
                                    Seq = x.Seq,
                                    SeqUnidadeResponsavel = seqUnidadeResponsavel,
                                    Descricao = x.TipoProcesso.Descricao,
                                    Ativo = x.Ativo
                                }).TransformList<UnidadeResponsavelTipoProcessoListaData>();

            return new SMCPagerData<UnidadeResponsavelTipoProcessoListaData>(data, data.Count);
        }

        public UnidadeResponsavelTipoProcessoData BuscarUnidadeResponsavelTipoProcesso(long seqUnidadeResponsavelTipoProcesso)
        {
            var spec = new SMCSeqSpecification<UnidadeResponsavelTipoProcesso>(seqUnidadeResponsavelTipoProcesso);
            var result = UnidadeResponsavelTipoProcessoDomainService.SearchProjectionByKey(spec,
                              x => new UnidadeResponsavelTipoProcessoData
                              {
                                  Seq = x.Seq,
                                  SeqTipoProcesso = x.SeqTipoProcesso,
                                  SeqUnidadeResponsavel = x.SeqUnidadeResponsavel,
                                  Ativo = x.Ativo,
                                  DetalhesTiposHierarquiaOferta = x.TiposHierarquiaOferta.Select(t => new UnidadeResponsavelDetalheTipoHierarquiaOfertaData
                                  {
                                      SeqUnidadeTipoHierarquia = t.Seq,
                                      SeqTipoHierarquiaOferta = t.SeqTipoHierarquiaOferta,
                                      TipoHierarquiaAtiva = t.Ativo
                                  }).ToList(),
                                  IdentidadesVisuais = x.IdentidadesVisuais.Select(s => new UnidadeResponsavelTipoProcessoIdentidadeVisualData()
                                  {
                                      Seq = s.Seq,
                                      Ativo = s.Ativo,
                                      CssAplicacao = s.CssAplicacao,
                                      Descricao = s.Descricao,
                                      SeqLayoutMensagemEmail = s.SeqLayoutMensagemEmail,
                                      SeqUnidadeResponsavelTipoProcesso = s.SeqUnidadeResponsavelTipoProcesso,
                                      TokenCssAlternativoSas = s.TokenCssAlternativoSas
                                  }).ToList(),
                              });
                        
            return result;
        }

        public long SalvarUnidadeResponsavelTipoProcessoTipoHierarquiaOferta(UnidadeResponsavelTipoProcessoData unidadeResponsavelTipoProcessoData)
        {
            return UnidadeResponsavelTipoProcessoDomainService.Salvar(SMCMapperHelper.Create<UnidadeResponsavelTipoProcesso>(unidadeResponsavelTipoProcessoData));
        }

        public void ExcluirUnidadeResponsavelTipoProcessoTipoHierarquiaOferta(long seqConfiguracaoTipoProcessoTipoHierarquiaOferta)
        {
            UnidadeResponsavelTipoProcessoDomainService.Excluir(seqConfiguracaoTipoProcessoTipoHierarquiaOferta);
        }

        public List<SMCDatasourceItem> BuscarTiposProcessoAssociados(long? seqUnidadeResponsavel)
        {
            var spec = new UnidadeResponsavelTipoProcessoFilterSpecification { SeqUnidadeResponsavel = seqUnidadeResponsavel };
           
             var tiposProcesso = this.UnidadeResponsavelTipoProcessoDomainService.SearchProjectionBySpecification(spec,
                x => new SMCDatasourceItem
                {
                    Seq = x.SeqTipoProcesso,
                    Descricao = x.TipoProcesso.Descricao
                }).ToList();


            var listaTiposProcesso = tiposProcesso.SMCDistinct(d=>d.Seq).ToList();
            return listaTiposProcesso;
        }

        public List<SMCDatasourceItem> BuscarTiposHieraquiaOfertaAssociados(long seqUnidadeResponsavel, long seqTipoProcesso)
        {
            var spec = new UnidadeResponsavelTipoProcessoFilterSpecification
            {
                SeqUnidadeResponsavel = seqUnidadeResponsavel,
                SeqTipoProcesso = seqTipoProcesso
            };

            var ret = this.UnidadeResponsavelTipoProcessoDomainService.SearchProjectionByKey(spec,
                x => x.TiposHierarquiaOferta.Select(h =>
                      new SMCDatasourceItem
                      {
                          Seq = h.SeqTipoHierarquiaOferta,
                          Descricao = h.TipoHierarquiaOferta.Descricao
                      }));
            return ret.OrderBy(x => x.Descricao).ToList();
        }

        public List<SMCDatasourceItem> BuscarLayoutNotificacaoEmailPorSiglaGrupoAplicacao()
        {
            return NotificacaoService.BuscarLayoutNotificacaoEmailPorSiglaGrupoAplicacao("GPI");
        }

        public List<SMCDatasourceItem> BuscarIdentidadesVisuais(long seqUnidadeResponsavel, long seqTipoProcesso)
        {
            return UnidadeResponsavelTipoProcessoDomainService.BuscarIdentidadesVisuais(seqUnidadeResponsavel, seqTipoProcesso);
        }
        #endregion

        #region Sistema/Origem

        public List<SMCDatasourceItem<string>> BuscarSistemaOrigemGADSelect(string sigla)
        {
            return UnidadeResponsavelDomainService.BuscarSistemaOrigemGADSelect(sigla);
        }

        public List<SMCDatasourceItem> BuscarUnidadesResponsaveisSelect(long? seqUnidadeResponsavel)
        {
            return UnidadeResponsavelDomainService.BuscarUnidadesResponsaveisKeyValue().ToList();
        }

        #endregion
        #region[Unidade Responsalvel Tipo Processo]

        public string BuscaTokenCssAlternativo(long seqUnidadeResponsavelTipoProcessoIdVisual)
        {
            return UnidadeResponsavelTipoProcessoDomainService.BuscaTokenCssAlternativo(seqUnidadeResponsavelTipoProcessoIdVisual);
        }
        #endregion

    }
}
