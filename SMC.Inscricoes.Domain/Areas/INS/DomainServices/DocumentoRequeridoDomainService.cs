using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class DocumentoRequeridoDomainService : InscricaoContextDomain<DocumentoRequerido>
    {
        #region DomainServices

        private InscricaoDocumentoDomainService InscricaoDocumentoDomainService
        {
            get { return this.Create<InscricaoDocumentoDomainService>(); }
        }

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        private ConfiguracaoEtapaDomainService ConfiguracaoEtapaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaDomainService>(); }
        }

        private GrupoDocumentoRequeridoDomainService GrupoDocumentoRequeridoDomainService
        {
            get { return this.Create<GrupoDocumentoRequeridoDomainService>(); }
        }

        private GrupoDocumentoRequeridoItemDomainService GrupoDocumentoRequeridoItemDomainService
        {
            get { return this.Create<GrupoDocumentoRequeridoItemDomainService>(); }
        }
        


        private ISituacaoService SituacaoService
        {
            get { return this.Create<ISituacaoService>(); }
        }

        #endregion DomainServices

        /// <summary>
        /// Exclui um documento requerido e implementa a RN_INS_111
        /// </summary>
        /// <param name="seqDocumentoRequerido"></param>
        public void ExcluirDocumentoRequerido(long seqDocumentoRequerido)
        {
            var dados = this.SearchProjectionByKey(
                new SMCSeqSpecification<DocumentoRequerido>(seqDocumentoRequerido),
                x => new
                {
                    SeqConfiguracaoEtapa = x.SeqConfiguracaoEtapa,
                    Situacao = x.ConfiguracaoEtapa.EtapaProcesso.SituacaoEtapa,
                    Documento = x,
                    InscricaoDocumentos = x.InscricaoDocumentos,
                });

            //Não pode alterar a documentação entregue caso a etapa esteja liberaad
            if (dados.Situacao == SituacaoEtapa.Liberada)
            {
                throw new ExclusaoDocumentoRequeridoEtapaLiberadaException();
            }

            if (dados.Documento.UploadObrigatorio)
            {
                var motivosTeste = SituacaoService.BuscarSeqMotivosSituacaoPorToken(TOKENS.MOTIVO_INSCRICAO_CANCELADA_TESTE);
                //Não pode excluir documento requerido de upload obrigatório que já possui documentos entregues
                var totalDocumentosEntregues = InscricaoDomainService.Count(
                    new DocumentoRequeridoEmUsoSpecification
                    {
                        SeqConfiguracaoEtapa = dados.SeqConfiguracaoEtapa,
                        SeqDocumentoRequerido = seqDocumentoRequerido,
                        MotivosSituacaoCanceladaTesteSGF = motivosTeste
                    });
                if (totalDocumentosEntregues > 0)
                {
                    throw new ExclusaoDocumentoRequeridoComInscricaoException();
                }
            }

            if (dados.Documento.Obrigatorio)
            {
                if (!dados.Documento.PermiteUploadArquivo)
                {
                    //Não pode excluir documento requerido obrigatorio que não permite upload onde a inscrição já está com a documentação entregue
                    var spec = new InscricaoFilterSpecification
                    {
                        SeqConfiguracaoEtapa = dados.Documento.SeqConfiguracaoEtapa,
                        DocumentacaoEntregue = true
                    };
                    spec.SetOrderByDescending(x => x.DataInscricao);
                    var specInscricaoDocumento = new InscricaoDocumentoFilterSpecification
                    {
                        SeqDocumentoRequerido = seqDocumentoRequerido,
                        PossuiArquivo = true,
                    };
                    var specInscricaoDocumentoEntregues = new InscricaoDocumentoFilterSpecification
                    {
                        SeqDocumentoRequerido = seqDocumentoRequerido,
                        SituacaoEntregaDocumento = SituacaoEntregaDocumento.Deferido,
                    };
                    var totalDocumentComArquivo = this.InscricaoDocumentoDomainService.Count(specInscricaoDocumento
                         | specInscricaoDocumentoEntregues);
                    var totalInscricoes = InscricaoDomainService.Count(spec);
                    if (totalInscricoes > 0 || totalDocumentComArquivo > 0)
                    {
                        throw new ExclusaoDocumentoRequeridoComInscricaoException();
                    }
                }
            }
            foreach (var inscricao in dados.InscricaoDocumentos)
            {
                InscricaoDocumentoDomainService.DeleteEntity(inscricao);
            }
            this.DeleteEntity(dados.Documento);
        }

        /// <summary>
        /// Verifica se existe uma inscrição com a documentação já entregue.
        /// </summary>
        /// <returns>true se existir uma inscrição. Caso contrário retorna false.</returns>
        public bool VerificaInscricaoComDocumentoCadastrado(long seqDocumentoRequerido)
        {
            return InscricaoDocumentoDomainService.Count(new InscricaoDocumentoCadastradoSpecification() { SeqDocumentoRequerido = seqDocumentoRequerido }) > 0;
        }

        /// <summary>
        /// Salva um documento requerido e implementa a RN_INS_098
        /// </summary>
        public long SalvarDocumentoRequerido(DocumentoRequerido documento)
        {
            var situacaoEtapa = ConfiguracaoEtapaDomainService.SearchProjectionByKey(
                   new SMCSeqSpecification<ConfiguracaoEtapa>(documento.SeqConfiguracaoEtapa),
                    x => x.EtapaProcesso.SituacaoEtapa);
            if (documento.PermiteUploadArquivo == false) documento.UploadObrigatorio = false;
            if (documento.UploadObrigatorio && !documento.Obrigatorio)
            {
                //Um documento cujo upload é obrigatório deverá ter entrega obrigatória.
                throw new DocumentoRequeridoNaoObrigatorioUploadObrigatorioException();
            }
            if (documento.Seq == default(long))
            {
                //Se é uma inserção de novo documento
                if (situacaoEtapa == SituacaoEtapa.Liberada)
                {
                    throw new InclusaoDocumentoRequeridoEtapaLiberadaException();
                }

                if (documento.UploadObrigatorio)
                {
                    var motivosTeste = SituacaoService.BuscarSeqMotivosSituacaoPorToken(TOKENS.MOTIVO_INSCRICAO_CANCELADA_TESTE);
                    var totalInscricoesComDocumentos = InscricaoDomainService.Count(
                                                        new InscricaoComDocumentosSpecification(documento.SeqConfiguracaoEtapa) &
                                                        new InscricoesNaoCanceladasPorMotivoTesteSpecification(motivosTeste));

                    if (totalInscricoesComDocumentos > 0)
                    {
                        throw new InclusaoDocumentoRequeridoUploadObrigatorioException();
                    }
                }

                if (documento.Obrigatorio)
                {
                    var spec = new InscricaoFilterSpecification
                    {
                        SeqConfiguracaoEtapa = documento.SeqConfiguracaoEtapa,
                        DocumentacaoEntregue = true
                    };
                    spec.SetOrderByDescending(x => x.DataInscricao);
                    var totalInscricoesDocumentacaoEntregue = InscricaoDomainService.Count(spec);
                    if (totalInscricoesDocumentacaoEntregue > 0)
                    {
                        throw new InclusaoDocumentoRequeridoDocumentacaoEntregueException();
                    }
                }
                using (var unitOfWork = SMCUnitOfWork.Begin())
                {
                    documento = this.InsertEntity(documento);
                    //Implementação da RN_INS_122
                    var spec = new InscricaoComSituacaoSpecification { SeqConfiguracaoEtapa = documento.SeqConfiguracaoEtapa, ConsiderarGrupoOfertaDaConfiguracao = true };
                    var seqInscricoes = InscricaoDomainService.SearchProjectionBySpecification(spec, x => x.Seq).ToList();
                    foreach (var seqInscricao in seqInscricoes)
                    {
                        var inscricaoDocumento = new InscricaoDocumento
                        {
                            SeqInscricao = seqInscricao,
                            SeqDocumentoRequerido = documento.Seq,
                            SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoEntrega,
                            ExibirObservacaoParaInscrito = false
                        };
                        this.InscricaoDocumentoDomainService.InsertEntity(inscricaoDocumento);
                    }
                    unitOfWork.Commit();
                }
            }
            else
            {
                //Se é alteração de um documento existente

                /*Os campos “Tipo de documento”, "Permite mais de um arquivo?", “Permite upload?”, "Upload obrigatório?", “Obrigatório” e “Sexo”
                 * só podem ser alterados se a etapa do processo estiver em “Manutenção” ou “Aguardando Liberação”.*/
                var documentoBanco = this.SearchByKey(
                    new SMCSeqSpecification<DocumentoRequerido>(documento.Seq));

                if ((situacaoEtapa != SituacaoEtapa.EmManutencao && situacaoEtapa != SituacaoEtapa.AguardandoLiberacao) &&
                    (documentoBanco.Sexo != documento.Sexo
                    || documentoBanco.Obrigatorio != documento.Obrigatorio
                    || documentoBanco.SeqTipoDocumento != documento.SeqTipoDocumento
                    || documentoBanco.PermiteUploadArquivo != documento.PermiteUploadArquivo
                    || documentoBanco.UploadObrigatorio != documento.UploadObrigatorio
                    || documentoBanco.PermiteVarios != documento.PermiteVarios
                    || documentoBanco.ExibeTermoResponsabilidadeEntrega != documento.ExibeTermoResponsabilidadeEntrega
                    || documentoBanco.DataLimiteEntrega != documento.DataLimiteEntrega))
                {
                    throw new AlteracaoDocumentoRequeridoEtapaLiberadaException();
                }

                /* Não permitir alterar o campo "Permite entrega posterior?" de "Sim" para "Não" se o documento requerido fizer parte de um grupo de documentos
                 * que está configurado para exibir o termo de responsabilidade de entrega. Em caso de violação desta regra, abortar a operação e emitir a mensagem de erro:
                 * "O campo "Permite entrega posterior?" não pode ser alterado, pois este documento pertence a um grupo de documentos configurado para exibir o termo de
                 * responsabilidade de entrega."*/

                if (documentoBanco.PermiteEntregaPosterior && !documento.PermiteEntregaPosterior)
                {
                    var specGrupoDocumentoRequeridoItem = new GrupoDocumentoRequeridoItemFilterSpecification() { SeqDocumentoRequerido = documento.Seq };

                    var gruposDocumentosRequeridosItens = this.GrupoDocumentoRequeridoItemDomainService.SearchProjectionBySpecification(specGrupoDocumentoRequeridoItem, g => new
                    {
                        g.GrupoDocumentoRequerido.ExibeTermoResponsabilidadeEntrega
                    }).ToList();

                    if (gruposDocumentosRequeridosItens.Any(g => g.ExibeTermoResponsabilidadeEntrega))
                        throw new DocumentoRequeridoPermiteEntregaPosteriorAlteradoException();
                }

                //Busca todos os seqs dos motivos situações pelo token INSCRICAO_CANCELADA_TESTE
                var motivosTeste = SituacaoService.BuscarSeqMotivosSituacaoPorToken(TOKENS.MOTIVO_INSCRICAO_CANCELADA_TESTE);

                /*
                 Ao alterar o campo “Exibe termo de responsabilidade de entrega?” de “Sim” para “Não”, caso exista alguma inscrição com situação atual diferente de
                 INSCRICAO_CANCELADA ou com situação INSCRICAO_CANCELADA, mas com motivo diferente de INSCRICAO_CACELADA_TESTE, e o candidato marcou o documento em
                 questão para entregar posteriormente, abortar a operação e emitir a mensagem: O campo "Exibe termo de responsabilidade de entrega?” não pode ser alterado,
                 pois existe inscrito que informou que irá entregar esse documento posteriormente".
                 */
                if (documentoBanco.ExibeTermoResponsabilidadeEntrega && !documento.ExibeTermoResponsabilidadeEntrega)
                {
                    var specInscricao = new InscricaoFilterSpecification() { SeqConfiguracaoEtapa = documento.SeqConfiguracaoEtapa };
                    specInscricao.SetOrderByDescending(x => x.DataInscricao);
                    var inscricoes = InscricaoDomainService.SearchProjectionBySpecification(specInscricao, i => new
                    {
                        SeqMotivoSituacaoSGF = i.HistoricosSituacao.Where(h => h.Atual).FirstOrDefault().SeqMotivoSituacaoSGF,
                        TokenTipoProcessoSituacao = i.HistoricosSituacao.Where(h => h.Atual).FirstOrDefault().TipoProcessoSituacao.Token,
                        DocumentosRequeridos = i.Documentos.Where(d => d.SeqDocumentoRequerido == documento.Seq).Select(d => new { d.Seq, d.SeqInscricao, d.SeqDocumentoRequerido, d.EntregaPosterior }).ToList()
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

                // Upload obrigatorio
                if (!documentoBanco.UploadObrigatorio && documento.UploadObrigatorio)
                {
                    if (documentoBanco.UploadObrigatorio != documento.UploadObrigatorio)
                    {
                        var totalInscricoesComDocumentos = InscricaoDomainService.Count(
                                                                       new InscricaoComDocumentosSpecification(documento.SeqConfiguracaoEtapa) &
                                                                       new InscricoesNaoCanceladasPorMotivoTesteSpecification(motivosTeste));

                        if (totalInscricoesComDocumentos > 0)
                        {
                            throw new AlteracaoDocumentoRequeridoUploadObrigatorioException();
                        }
                    }

                    var totalGruposDoDocumento = this.GrupoDocumentoRequeridoDomainService.Count(
                        new DocumentoRequeridoNoGrupoSpecification(documento.Seq,
                            documento.SeqConfiguracaoEtapa));
                    if (totalGruposDoDocumento > 0)
                    {
                        throw new AlteracaoDocumentoRequeridoUploadEmGrupoException();
                    }
                }

                // Obrigatorio
                if (!documentoBanco.Obrigatorio && documento.Obrigatorio)
                {
                    var totalGruposDoDocumento = this.GrupoDocumentoRequeridoDomainService.Count(
                       new DocumentoRequeridoNoGrupoSpecification(documento.Seq,
                           documento.SeqConfiguracaoEtapa));
                    if (totalGruposDoDocumento > 0)
                    {
                        throw new AlteracaoDocumentoRequeridoObrigatorioGrupoException();
                    }
                }

                if (documentoBanco.Obrigatorio != documento.Obrigatorio)
                {
                    var totalInscricoesComDocumentacaoEntregue = InscricaoDomainService.Count(
                        new InscricaoFilterSpecification
                        {
                            SeqConfiguracaoEtapa = documento.SeqConfiguracaoEtapa,
                            DocumentacaoEntregue = true
                        }.SetOrderByDescending(x => x.DataInscricao));
                    if (totalInscricoesComDocumentacaoEntregue > 0)
                    {
                        throw new AlteracaoDocumentoRequeridoObrigatorioException();
                    }
                }

                // Tipo documento
                if (documentoBanco.SeqTipoDocumento != documento.SeqTipoDocumento)
                {
                    var totalInscricoesComDocumento = this.InscricaoDocumentoDomainService
                        .Count(new InscricaoDocumentoFilterSpecification
                        {
                            SeqDocumentoRequerido = documento.Seq,
                            MotivosSituacaoCanceladaTesteSGF = motivosTeste
                        });

                    if (totalInscricoesComDocumento > 0)
                    {
                        throw new AlteracaoDocumentoRequeridoTipoDocumentoException();
                    }
                }

                // Versao documento
                if (documentoBanco.VersaoDocumento == VersaoDocumento.CopiaSimples &&
                    documento.VersaoDocumento != VersaoDocumento.CopiaSimples)
                {
                    var totalInscricoesComDocumento = this.InscricaoDocumentoDomainService
                        .Count(new InscricaoDocumentoFilterSpecification
                        {
                            SeqDocumentoRequerido = documento.Seq,
                            VersaoDocumento = VersaoDocumento.CopiaSimples,
                            MotivosSituacaoCanceladaTesteSGF = motivosTeste
                        });
                    if (totalInscricoesComDocumento > 0)
                    {
                        throw new AlteracaoDocumentoRequeridoVersaoException();
                    }
                }

                if (documentoBanco.Sexo != documento.Sexo)
                {
                    var totalDocumentacoesEntregues = InscricaoDomainService.Count(
                        new InscricaoFilterSpecification
                        {
                            SeqConfiguracaoEtapa = documento.SeqConfiguracaoEtapa,
                            DocumentacaoEntregue = true
                        }.SetOrderByDescending(x => x.DataInscricao));
                    if (totalDocumentacoesEntregues > 0)
                    {
                        throw new AlteracoDocumentoRequeridoSexoException();
                    }
                }

                // Permite mais de um arquivo?
                ValidarPermitirVariosArquivos(documentoBanco, documento);

                ValidarPermitirEntregaPosterior(documentoBanco, documento);

                ValidarPermitirValidacaoOutroSetor(documentoBanco, documento);

                this.UpdateEntity(documento);
            }
            return documento.Seq;
        }

        /// <summary>
        /// Não é permitido alterar o parâmetro "Permite validação por outro setor" de "Sim" para "Não"
        /// se houver inscrição com este tipo de documento registrado como "Aguardando validação por outro setor"
        /// para a configuração da documentação requerida em questão.
        /// </summary>
        /// <param name="documentoBanco"></param>
        /// <param name="documento"></param>
        private void ValidarPermitirValidacaoOutroSetor(DocumentoRequerido documentoBanco, DocumentoRequerido documento)
        {
            if (!documento.ValidacaoOutroSetor && documentoBanco.ValidacaoOutroSetor)
            {
                var totalInscricoesQuePermiteValidacaoOutroSetor = InscricaoDomainService.Count(
                  new InscricaoFilterSpecification
                  {
                      SeqConfiguracaoEtapa = documento.SeqConfiguracaoEtapa,
                      SeqTipoDocumento = documento.SeqTipoDocumento,
                      SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoAnaliseSetorResponsavel
                        }.SetOrderByDescending(x => x.DataInscricao));

                if (totalInscricoesQuePermiteValidacaoOutroSetor > 0) { throw new AlteracaoDocumentoRequeridoValidacaoOutroSetorException(); }
            }
        }

        /// <summary>
        /// Não é permitido alterar o parâmetro "Permite entrega posterior" de "Sim" para "Não"
        /// se houver inscrição com este tipo de documento registrado como "Pendente", para a configuração da
        /// documentação requerida em questão.
        /// </summary>
        /// <param name="documentoBanco"></param>
        /// <param name="documento"></param>
        private void ValidarPermitirEntregaPosterior(DocumentoRequerido documentoBanco, DocumentoRequerido documento)
        {
            if (!documento.PermiteEntregaPosterior && documentoBanco.PermiteEntregaPosterior)
            {

                var specDocumentoRequerido = new DocumentoRequeridoFilterSpecification()
                {
                    SeqConfiguracaoEtapa = documento.SeqConfiguracaoEtapa,
                    SeqTipoDocumento = documento.SeqTipoDocumento,
                    SituacaoEntregaDocumento = SituacaoEntregaDocumento.Pendente
                };

                var totalDocumentosPententesMesmoTipo = this.Count(specDocumentoRequerido);
                               
                if (totalDocumentosPententesMesmoTipo > 0) { throw new AlteracaoDocumentoRequeridoPermiteEntregaPosteriorException(); }
            }
        }

        /// <summary>
        /// Não é permitido alterar o parâmetro "Permite mais de um arquivo?" de "Sim" para "Não"
        /// se houver inscrição para a configuração da documentação requerida em questão, com mais de um registro
        /// para o tipo de documento do documento alterado.
        /// </summary>
        /// <param name="documentoBanco"></param>
        /// <param name="documento"></param>
        private void ValidarPermitirVariosArquivos(DocumentoRequerido documentoBanco, DocumentoRequerido documento)
        {
            if (!documento.PermiteVarios && documentoBanco.PermiteVarios)
            {
                // Verifica se tem alguma inscrição que possui mais de um arquivo do tipo em questão.
                var dadosInscricoes = InscricaoDomainService.SearchProjectionBySpecification(new InscricaoFilterSpecification
                {
                    SeqConfiguracaoEtapa = documento.SeqConfiguracaoEtapa
                        }.SetOrderByDescending(x => x.DataInscricao), x => new { SeqInscricao = x.Seq, TotalDocumentos = x.Documentos.Count(d => d.DocumentoRequerido.SeqTipoDocumento == documento.SeqTipoDocumento) }).ToList();

                if (dadosInscricoes.Any(d => d.TotalDocumentos > 1)) { throw new AlteracaoDocumentoRequeridoVariosArquivosException(); }
            }
        }
    }
}