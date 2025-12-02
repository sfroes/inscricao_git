using SMC.DadosMestres.ServiceContract.Areas.GED.Interfaces;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class GrupoDocumentoRequeridoDomainService : InscricaoContextDomain<GrupoDocumentoRequerido>
    {
        #region Services

        private ITipoDocumentoService TipoDocumentoService
        {
            get { return this.Create<ITipoDocumentoService>(); }
        }

        private ConfiguracaoEtapaDomainService ConfiguracaoEtapaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaDomainService>(); }
        }

        private InscricaoDocumentoDomainService InscricaoDocumentoDomainService
        {
            get { return this.Create<InscricaoDocumentoDomainService>(); }
        }

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        private DocumentoRequeridoDomainService DocumentoRequeridoDomainService
        {
            get { return this.Create<DocumentoRequeridoDomainService>(); }
        }

        private GrupoDocumentoRequeridoItemDomainService GrupoDocumentoRequeridoItemDomainService
        {
            get { return this.Create<GrupoDocumentoRequeridoItemDomainService>(); }
        }

        private ISituacaoService SituacaoService
        {
            get { return Create<ISituacaoService>(); }
        }

        #endregion Services

        public List<SMCDatasourceItem> BuscarTiposDocumentoGrupo(long seqGrupoDocumentos)
        {
            var spec = new SMCSeqSpecification<GrupoDocumentoRequerido>(seqGrupoDocumentos);
            var grupo = SearchByKey(spec, x => x.Itens, x => x.Itens[0].DocumentoRequerido);
            return grupo.Itens.Select(x => new SMCDatasourceItem
            {
                Seq = x.SeqDocumentoRequerido,
                Descricao = TipoDocumentoService.BuscarTipoDocumento(x.DocumentoRequerido.SeqTipoDocumento).Descricao
            }).ToList();
        }

        /// <summary>
        /// Exclui um grupo de documentos requerido e implementa as validações da regra RN_INS_112
        /// </summary>
        /// <param name="seqGrupoDocumentoRequerido"></param>
        public void ExcluirGrupoDocumentoRequerido(long seqGrupoDocumentoRequerido)
        {
            var dados = this.SearchProjectionByKey(
                new SMCSeqSpecification<GrupoDocumentoRequerido>(seqGrupoDocumentoRequerido),
                x => new
                {
                    Situacao = x.ConfiguracaoEtapa.EtapaProcesso.SituacaoEtapa,
                    Grupo = x,
                });
            if (dados.Situacao == SituacaoEtapa.Liberada)
            {
                throw new MudancaGrupoDocumentoEtapaLiberadaException("Exclusão");
            }

            if (dados.Grupo.UploadObrigatorio)
            {
                var seqDocumentosNoGrupo = this.SearchProjectionByKey(
                    new SMCSeqSpecification<GrupoDocumentoRequerido>(seqGrupoDocumentoRequerido),
                    x => x.Itens.Select(i => i.SeqDocumentoRequerido));

                var motivosTeste = SituacaoService.BuscarSeqMotivosSituacaoPorToken(TOKENS.MOTIVO_INSCRICAO_CANCELADA_TESTE);
                var totalDocumentos = InscricaoDocumentoDomainService.Count(new
                    InscricaoDocumentoFilterSpecification
                { SeqConfiguracaoEtapa = dados.Grupo.SeqConfiguracaoEtapa, MotivosSituacaoCanceladaTesteSGF = motivosTeste });
                if (totalDocumentos > 0)
                {
                    throw new ExclusaoGrupoDocumentoInscricaoComDocumentosException();
                }
            }
            else
            {
                var spec = new InscricaoFilterSpecification
                {
                    SeqConfiguracaoEtapa = dados.Grupo.SeqConfiguracaoEtapa,
                    DocumentacaoEntregue = true
                };
                spec.SetOrderByDescending(x => x.DataInscricao);             
                var totalInscricoes = InscricaoDomainService.Count(spec);
                if (totalInscricoes > 0)
                {
                    throw new ExclusaoGrupoDocumentoComInscricaoException();
                }
            }
            this.DeleteEntity(dados.Grupo);
        }

        /// <summary>
        /// Salva um grupo de documentos requeridos e implementa as regras de validação da RN_INS_097
        /// </summary>
        /// <param name="grupo"></param>
        /// <returns></returns>
        public long SalvarGrupoDocumentoRequerido(GrupoDocumentoRequerido grupo)
        {
            #region Validações

            var situacaoEtapa = ConfiguracaoEtapaDomainService.SearchProjectionByKey(
                  new SMCSeqSpecification<ConfiguracaoEtapa>(grupo.SeqConfiguracaoEtapa),
                   x => x.EtapaProcesso.SituacaoEtapa);

            var motivosTeste = SituacaoService.BuscarSeqMotivosSituacaoPorToken(TOKENS.MOTIVO_INSCRICAO_CANCELADA_TESTE);
            var inscricaoDocumentoEntregueSpec = new InscricaoFilterSpecification { SeqConfiguracaoEtapa = grupo.SeqConfiguracaoEtapa, DocumentacaoEntregue = true }.SetOrderByDescending(x => x.DataInscricao) &
                                                 new InscricoesNaoCanceladasPorMotivoTesteSpecification(motivosTeste);

            /*
            Se o campo "Exibe termo de responsabilidade de entrega?" for igual a "Sim", todos os documentos associados ao grupo devem estar configurados com o campo
            "Permite entrega posterior?" igual a "Sim". Em caso de violação desta regra, abortar a operação e exibir a mensagem:
            “Não é permitido criar um grupo que exibe o termo de responsabilidade de entrega se algum de seus documentos não estiver configurado para permitir entrega posterior."
            */
            if (grupo.ExibeTermoResponsabilidadeEntrega)
            {
                var seqsDocumentosRequeridos = grupo.Itens.Select(i => i.SeqDocumentoRequerido).ToList();

                var specDocumentoRequerido = new DocumentoRequeridoFilterSpecification() { Seqs = seqsDocumentosRequeridos };

                var documentosRequeridos = this.DocumentoRequeridoDomainService.SearchProjectionBySpecification(specDocumentoRequerido, g => new 
                {
                    g.PermiteEntregaPosterior
                }).ToList();

                if (documentosRequeridos.Any(d => !d.PermiteEntregaPosterior))
                    throw new GrupoDocumentoTermoResponsabilidadePermiteEntregaPosteriorException();
            }

            if (situacaoEtapa == SituacaoEtapa.Liberada)
            {
                throw new MudancaGrupoDocumentoEtapaLiberadaException(grupo.Seq == default(long) ? "Inclusão" : "Alteração");
            }
            if (grupo.Seq == default(long))
            {
                //Validaõe exclusivas para inserção
                if (grupo.UploadObrigatorio)
                {
                    ValidarUploadGrupoSalvar(grupo.SeqConfiguracaoEtapa, "Inclusão", motivosTeste);
                }

                var totalInscricoes = InscricaoDomainService.Count(inscricaoDocumentoEntregueSpec);
                if (totalInscricoes > 0)
                {
                    throw new InsclusaoGrupoDocumentoDocumentacaoEntregueException();
                }
            }
            else
            {
                var grupoBanco = this.SearchByKey(
                    new SMCSeqSpecification<GrupoDocumentoRequerido>(grupo.Seq),
                                            x => x.Itens, x => x.Itens[0].DocumentoRequerido,
                                            x => x.Itens[0].DocumentoRequerido.TipoDocumento,
                                            x => x.ConfiguracaoEtapa.Inscricoes);
                //Validações para alteração

               /* Ao alterar o campo “Exibe termo de responsabilidade de entrega?” de “Sim” para “Não”, caso exista alguma inscrição com situação atual diferente de
                * INSCRICAO_CANCELADA ou com situação INSCRICAO_CANCELADA, mas com motivo diferente de INSCRICAO_CACELADA_TESTE, e o candidato marcou algum dos 
                * documentos do grupo em questão para entregar posteriormente, abortar a operação e emitir a mensagem:
                * “O campo “Exibe termo de responsabilidade de entrega?” não pode ser alterado, pois existe inscrito que informou que irá entregar o documento relativo 
                * a este grupo posteriormente.”*/

                if(grupoBanco.ExibeTermoResponsabilidadeEntrega && !grupo.ExibeTermoResponsabilidadeEntrega)
                {

                    var seqsDocumentosRequeridos = grupo.Itens.Select(i => i.SeqDocumentoRequerido).ToList();

                    var specInscricao = new InscricaoFilterSpecification() { SeqConfiguracaoEtapa = grupo.SeqConfiguracaoEtapa };

                    specInscricao.SetOrderByDescending(x => x.DataInscricao);
                    var inscricoes = InscricaoDomainService.SearchProjectionBySpecification(specInscricao, i => new
                    {
                        SeqMotivoSituacaoSGF = i.HistoricosSituacao.Where(h => h.Atual).FirstOrDefault().SeqMotivoSituacaoSGF,
                        TokenTipoProcessoSituacao = i.HistoricosSituacao.Where(h => h.Atual).FirstOrDefault().TipoProcessoSituacao.Token,
                        DocumentosRequeridos = i.Documentos.Where(d => seqsDocumentosRequeridos.Contains(d.SeqDocumentoRequerido)).Select(d => new { d.Seq, d.SeqInscricao, d.SeqDocumentoRequerido, d.EntregaPosterior }).ToList()
                    }).ToList();

                    foreach (var inscricao in inscricoes)
                    {
                        if (
                            (inscricao.TokenTipoProcessoSituacao != TOKENS.SITUACAO_INSCRICAO_CANCELADA &&
                             inscricao.DocumentosRequeridos.Any(d => d.EntregaPosterior))
                             ||
                            (inscricao.TokenTipoProcessoSituacao == TOKENS.SITUACAO_INSCRICAO_CANCELADA &&
                             !motivosTeste.Contains(inscricao.SeqMotivoSituacaoSGF.Value) &&
                             inscricao.DocumentosRequeridos.Any(d => d.EntregaPosterior))
                          )
                        {
                            throw new AlteracaoDocumentoRequeridoExibirTermoResponsabilidadeEntregaException();
                        }
                    }                    
                }

                if (!grupoBanco.UploadObrigatorio && grupo.UploadObrigatorio)
                {
                    ValidarUploadGrupoSalvar(grupo.SeqConfiguracaoEtapa, "Alteração", motivosTeste);
                }

                if (!grupo.UploadObrigatorio && grupo.MinimoObrigatorio != grupoBanco.MinimoObrigatorio)
                {
                    var totalInscricoes = InscricaoDomainService.Count(inscricaoDocumentoEntregueSpec);
                    if (totalInscricoes > 0)
                    {
                        throw new GrupoDocumentoAumentoMinimoException();
                    }
                }

                IEnumerable<long> faltando;
                var documentosNoGrupoSpec = new SMCContainsSpecification<DocumentoRequerido, long>(
                 x => x.Seq, grupo.Itens.Select(x => x.SeqDocumentoRequerido).ToArray());
                var documentosNoGrupo = DocumentoRequeridoDomainService.SearchBySpecification(
                    documentosNoGrupoSpec);
                var documentosNoBanco = grupoBanco.Itens.Select(x => x.DocumentoRequerido);
                var listasIguais = documentosNoGrupo.SMCContainsList(documentosNoBanco, x => x.Seq, out faltando);

                if (grupo.UploadObrigatorio)
                {
                    if (grupo.MinimoObrigatorio != grupoBanco.MinimoObrigatorio)
                    {
                        var totalDocumentos = InscricaoDocumentoDomainService.Count(new
                            InscricaoDocumentoFilterSpecification
                        { SeqConfiguracaoEtapa = grupo.SeqConfiguracaoEtapa, MotivosSituacaoCanceladaTesteSGF = motivosTeste });
                        if (totalDocumentos > 0)
                        {
                            throw new GrupoDocumentoAlteracaoMinimoException();
                        }
                    }

                    var containsSpec = new SMCContainsSpecification<InscricaoDocumento, long>(
                        x => x.SeqDocumentoRequerido, documentosNoBanco.Select(x => x.Seq).ToArray());

                    if (!listasIguais && this.InscricaoDocumentoDomainService.Count(containsSpec) > 0 &&
                            // Verifica se todas as inscrições sao de teste
                            !InscricaoDomainService.VerificaApenasInscricoesTeste(grupoBanco.ConfiguracaoEtapa.Inscricoes.Select(f => f.Seq).ToArray()))
                    {
                        throw new GrupoDocumentoUploadItensAlteradosException();
                    }
                }
                else
                {
                    if (!listasIguais)
                    {
                        foreach (var item in faltando)
                        {
                            var spec = new InscricaoDocumentoGrupoAlteradoSpecification
                            {
                                SeqDocumentoRequerido = item,
                                DocumentacaoEntregue = true,
                                MotivosSituacaoCanceladaTesteSGF = motivosTeste
                            };
                            if (InscricaoDocumentoDomainService.Count(spec) > 0)
                            {
                                throw new GrupoDocumentoAlteradoItemAlteracoException(
                                 documentosNoBanco.FirstOrDefault(x => x.Seq == item).TipoDocumento.Descricao);
                            }
                        }
                    }
                }
            }

            if (grupo.UploadObrigatorio)
            {
                var containSpec = new SMCContainsSpecification<DocumentoRequerido, long>(x => x.Seq,
                    grupo.Itens.Select(i => i.SeqDocumentoRequerido).ToArray());
                var docSpec = new DocumentoRequeridoFilterSpecification { PermiteUploadArquivo = false };
                var totalDocumentosSemUpload = DocumentoRequeridoDomainService.Count(docSpec & containSpec);
                if (totalDocumentosSemUpload > 0)
                {
                    throw new GrupoDocumentoDocumentoSemUploadException();
                }
            }

            if (grupo.MinimoObrigatorio >= grupo.Itens.Count)
            {
                throw new GrupoDocumentoMinimoItensInvalidoException();
            }

            #endregion Validações

            this.SaveEntity(grupo);
            return grupo.Seq;
        }

        private void ValidarUploadGrupoSalvar(long seqConfiguracaoEtapa, string operacao, IEnumerable<long> motivosTeste)
        {
            var totalDocumentos = InscricaoDocumentoDomainService.Count(new
                   InscricaoDocumentoFilterSpecification
            { SeqConfiguracaoEtapa = seqConfiguracaoEtapa, MotivosSituacaoCanceladaTesteSGF = motivosTeste });
            if (totalDocumentos > 0)
            {
                throw new GrupoDocumentoUploadObrigatorioInvalidoException(operacao);
            }
        }
    }
}