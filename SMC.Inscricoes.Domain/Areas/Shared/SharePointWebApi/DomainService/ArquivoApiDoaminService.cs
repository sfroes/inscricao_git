using SMC.DadosMestres.Common;
using SMC.DadosMestres.Common.Areas.GED.Enums;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.DadosMestres.Common.Areas.SHA.Enums;
using SMC.DadosMestres.ServiceContract.Areas.PES.Data;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Security;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Enums;
using SMC.Inscricoes.Common.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Const;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Models;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject;
using SMC.Inscricoes.Domain.Models;
using SMC.PDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Portfolio
{
    public class ArquivoApiDoaminService : InscricaoContextDomain<SharepointApi>
    {
        #region DOMAINSERVICE
        PortfolioApiDoaminService PortfolioApiDoaminService => Create<PortfolioApiDoaminService>();
        BibliotecaApiDoaminService BibliotecaApiDoaminService => Create<BibliotecaApiDoaminService>();
        InscricaoDomainService InscricaoDomainService => Create<InscricaoDomainService>();
        InscricaoDocumentoDomainService InscricaoDocumentoDomainService => Create<InscricaoDocumentoDomainService>();
        ArquivoAnexadoDomainService ArquivoAnexadoDomainService => Create<ArquivoAnexadoDomainService>();
        ViewTipoDocumentoDomainService ViewTipoDocumentoDomainService => Create<ViewTipoDocumentoDomainService>();
        #endregion

        public RetornoArquivoSharepointVO CriarArquivo(ParametrosArquivoVO modelo)
        {
            RetornoArquivoSharepointVO retorno = new RetornoArquivoSharepointVO();
            List<dynamic> arquivos = new List<dynamic>();

            if (modelo.SeqsDocumentos.Count == 0)
            {
                return retorno;
            }

            var inscricao = this.InscricaoDomainService.SearchProjectionByKey(modelo.SeqInscricao, p => new
            {
                p.Seq,
                p.SeqInscrito,
                p.Inscrito.Nome,
                p.UidProcessoGed,
                p.Processo.TipoProcesso.SeqHierarquiaClassificacaoGed,
                p.Processo.TipoProcesso.SeqContextoBibliotecaGed,
                p.Processo.SemestreReferencia,
                p.Processo.AnoReferencia,
                p.HabilitaGed,
                p.Inscrito.NumeroPassaporte,
                p.Inscrito.Cpf,
                p.Inscrito.NumeroIdentidade,
                NomeUnidadeResponsavel = p.Processo.UnidadeResponsavel.Nome,
                DataIncioInscrição = p.Processo.EtapasProcesso.FirstOrDefault(f => f.Token == TOKENS.ETAPA_INSCRICAO).DataInicioEtapa,
            });

            //Caso o ged não esteja habilitado, retorna vazio
            if (!inscricao.HabilitaGed)
            {
                return retorno;
            }

            if (modelo.OrigemInscricaoDocumento)
            {
                var spec = new InscricaoDocumentoFilterSpecification() { SeqInscricao = inscricao.Seq, Seqs = modelo.SeqsDocumentos };
                var result = this.InscricaoDocumentoDomainService.SearchProjectionBySpecification(spec,
                    p => new
                    {
                        p.Seq,
                        ArquivoAnexado = p.ArquivoAnexado,
                        DocumentoRequerido = p.DocumentoRequerido,
                        TipoDocumento = p.DocumentoRequerido.TipoDocumento
                    }).ToList();
                arquivos.AddRange(result);
            }

            if (modelo.OrigemComprovanteInscricao)
            {
                var spec = new SMCSeqSpecification<ArquivoAnexado>(modelo.SeqsDocumentos.FirstOrDefault());
                var result = ArquivoAnexadoDomainService.SearchByKey(spec);
                arquivos.Add(result);
            }

            if (inscricao.SeqContextoBibliotecaGed != null && inscricao.SeqContextoBibliotecaGed > 0)
            {
                var retornoApiBiblioteca = BibliotecaApiDoaminService.BuscarGuidBiblioteca(inscricao.SeqContextoBibliotecaGed.Value);

                if (retornoApiBiblioteca.StatusCode == HttpStatusCode.OK)
                {
                    var retornoApiPortfolio = PortfolioApiDoaminService.BuscarIdPortifolio(inscricao.SeqInscrito, retornoApiBiblioteca.GuidBiblioteca);

                    if (retornoApiPortfolio.ExistePortfolio && inscricao.UidProcessoGed != null)
                    {
                        foreach (var arquivo in arquivos)
                        {
                            DadosArquivoSharepointVO dadosArquivoSharepointVO = new DadosArquivoSharepointVO();
                            long seqArquivoAnexado;

                            if (modelo.OrigemInscricaoDocumento)
                            {
                                PrepararArquivoUpload(dadosArquivoSharepointVO, inscricao, modelo, arquivo, retornoApiBiblioteca, retornoApiPortfolio);
                                seqArquivoAnexado = arquivo.ArquivoAnexado.Seq;
                            }
                            else
                            {
                                PrepararArquivoComprovante(dadosArquivoSharepointVO, inscricao, modelo, arquivo, retornoApiBiblioteca, retornoApiPortfolio);
                                seqArquivoAnexado = arquivo.Seq;

                            }

                            retorno = RequisicoesSharePoint.EnviarPostApiGed<RetornoArquivoSharepointVO>(dadosArquivoSharepointVO, ACOES_WEBAPI_SHAREPOINT.SALVAR_ARQUIVO);

                            if (retorno.StatusCode == HttpStatusCode.BadRequest)
                            {
                                throw new Exception(retorno.ErroMessage);
                            }

                            if (retorno.StatusCode == HttpStatusCode.OK)
                            {
                                ArquivoAnexadoDomainService.UpdateFields(new ArquivoAnexado()
                                {
                                    Seq = seqArquivoAnexado,
                                    UidArquivoGed = Guid.Parse(retorno.IdGEDArquivo),
                                    UrlDownloadGed = retorno.URLDownloadArquivo,
                                    UrlPrivadaGed = retorno.URLPrivadaArquivo,
                                    UrlPublicaGed = retorno.URLPublicaArquivo
                                },
                                u => u.UidArquivoGed,
                                u => u.UrlDownloadGed,
                                u => u.UrlPrivadaGed,
                                u => u.UrlPublicaGed);
                            }
                        }
                    }
                }
            }
            return retorno;
        }

        public RetornoArquivoSharepointVO ExcluirArquivo(ParametrosArquivoVO modelo)
        {
            RetornoArquivoSharepointVO retorno = new RetornoArquivoSharepointVO();
            List<dynamic> arquivos = new List<dynamic>();

            if (modelo.SeqsGuidArquivo.Count == 0)
            {
                return retorno;
            }

            var inscricao = this.InscricaoDomainService.SearchProjectionByKey(modelo.SeqInscricao, p => new
            {
                p.Processo.TipoProcesso.SeqContextoBibliotecaGed,
                p.HabilitaGed
            });
            if (!inscricao.HabilitaGed)
            {
                return retorno;
            }

            if (inscricao.SeqContextoBibliotecaGed != null && inscricao.SeqContextoBibliotecaGed > 0)
            {
                var retornoApiBiblioteca = BibliotecaApiDoaminService.BuscarGuidBiblioteca(inscricao.SeqContextoBibliotecaGed.Value);

                if (retornoApiBiblioteca.StatusCode == HttpStatusCode.OK)
                {
                    foreach (var guidArquivo in modelo.SeqsGuidArquivo)
                    {
                        DadosArquivoSharepointVO dadosArquivoSharepointVO = new DadosArquivoSharepointVO();
                        dadosArquivoSharepointVO.GuidBiblioteca = retornoApiBiblioteca.GuidBiblioteca;
                        dadosArquivoSharepointVO.IdGEDArquivo = guidArquivo;

                        retorno = RequisicoesSharePoint.EnviarPostApiGed<RetornoArquivoSharepointVO>(dadosArquivoSharepointVO, ACOES_WEBAPI_SHAREPOINT.EXCLUIR_ARQUIVO);

                        if (retorno.StatusCode == HttpStatusCode.BadRequest)
                        {
                            throw new Exception(retorno.ErroMessage);
                        }
                    }
                }
            }
            return retorno;
        }

        public RetornoArquivoSharepointVO AtualizarArquivo(ParametrosArquivoVO modelo)
        {
            RetornoArquivoSharepointVO retorno = new RetornoArquivoSharepointVO();
            List<dynamic> arquivos = new List<dynamic>();

            if (modelo.SeqsDocumentos.Count == 0)
            {
                return retorno;
            }

            var inscricao = this.InscricaoDomainService.SearchProjectionByKey(modelo.SeqInscricao, p => new
            {
                p.Seq,
                p.Processo.TipoProcesso.SeqContextoBibliotecaGed,
                p.HabilitaGed,
                p.Inscrito.Nome
            });

            if (modelo.OrigemInscricaoDocumento)
            {
                var spec = new InscricaoDocumentoFilterSpecification() { SeqInscricao = inscricao.Seq, Seqs = modelo.SeqsDocumentos };
                var result = this.InscricaoDocumentoDomainService.SearchProjectionBySpecification(spec,
                    p => new
                    {
                        p.Seq,
                        p.ArquivoAnexado,
                    }).ToList();
                arquivos.AddRange(result);
            }

            if (!inscricao.HabilitaGed)
            {
                return retorno;
            }

            if (inscricao.SeqContextoBibliotecaGed != null && inscricao.SeqContextoBibliotecaGed > 0)
            {
                var retornoApiBiblioteca = BibliotecaApiDoaminService.BuscarGuidBiblioteca(inscricao.SeqContextoBibliotecaGed.Value);

                if (retornoApiBiblioteca.StatusCode == HttpStatusCode.OK)
                {
                    foreach (var arquivo in arquivos)
                    {
                        DadosArquivoSharepointVO dadosArquivoSharepointVO = new DadosArquivoSharepointVO();
                        dadosArquivoSharepointVO.GuidBiblioteca = retornoApiBiblioteca.GuidBiblioteca;
                        dadosArquivoSharepointVO.IdGEDArquivo = $"{arquivo.ArquivoAnexado.UidArquivoGed}";
                        dadosArquivoSharepointVO.NomeArquivo = arquivo.ArquivoAnexado.Nome;
                        dadosArquivoSharepointVO.Conteudo = arquivo.ArquivoAnexado.Conteudo;
                        dadosArquivoSharepointVO.Originador = MontarOriginador(inscricao, modelo.TipoSistema);
                        dadosArquivoSharepointVO.EventoAutenticacao = SMCPDFHelper.DocumentoPossuiAssinatura(arquivo.ArquivoAnexado.Conteudo) ? "Sim" : "Não";

                        retorno = RequisicoesSharePoint.EnviarPostApiGed<RetornoArquivoSharepointVO>(dadosArquivoSharepointVO, ACOES_WEBAPI_SHAREPOINT.ATUALIZAR_ARQVUIVO);

                        if (retorno.StatusCode == HttpStatusCode.BadRequest)
                        {
                            throw new Exception(retorno.ErroMessage);
                        }

                        if (retorno.StatusCode == HttpStatusCode.OK)
                        {
                            if (retorno.URLDownloadArquivo != arquivo.ArquivoAnexado.UrlDownloadGed ||
                               retorno.URLPrivadaArquivo != arquivo.ArquivoAnexado.UrlPrivadaGed ||
                               retorno.URLPublicaArquivo != arquivo.ArquivoAnexado.UrlPublicaGed)
                                ArquivoAnexadoDomainService.UpdateFields(new ArquivoAnexado()
                                {
                                    Seq = arquivo.ArquivoAnexado.Seq,
                                    UidArquivoGed = Guid.Parse(retorno.IdGEDArquivo),
                                    UrlDownloadGed = retorno.URLDownloadArquivo,
                                    UrlPrivadaGed = retorno.URLPrivadaArquivo,
                                    UrlPublicaGed = retorno.URLPublicaArquivo
                                },
                                u => u.UidArquivoGed,
                                u => u.UrlDownloadGed,
                                u => u.UrlPrivadaGed,
                                u => u.UrlPublicaGed);
                        }
                    }
                }
            }
            return retorno;
        }

        public RetornoArquivoSharepointVO AtualizarMetaDados(ParametrosArquivoVO modelo)
        {
            RetornoArquivoSharepointVO retorno = new RetornoArquivoSharepointVO();
            List<dynamic> arquivos = new List<dynamic>();

            if(modelo.SeqsDocumentos.Count == 0)
            {
                return retorno;
            }

            var inscricao = this.InscricaoDomainService.SearchProjectionByKey(modelo.SeqInscricao, p => new
            {
                p.Seq,
                p.Processo.TipoProcesso.SeqContextoBibliotecaGed,
                p.HabilitaGed,
                p.Inscrito.Nome
            });

            if (!inscricao.HabilitaGed)
            {
                return retorno;
            }

            if (modelo.OrigemInscricaoDocumento)
            {
                var spec = new InscricaoDocumentoFilterSpecification() { SeqInscricao = inscricao.Seq, Seqs = modelo.SeqsDocumentos };
                var result = this.InscricaoDocumentoDomainService.SearchProjectionBySpecification(spec,
                    p => new
                    {
                        p.Seq,
                        p.SituacaoEntregaDocumento,
                        p.ArquivoAnexado
                    }).ToList();
                arquivos.AddRange(result);
            }

            if (inscricao.SeqContextoBibliotecaGed != null && inscricao.SeqContextoBibliotecaGed > 0)
            {
                var retornoApiBiblioteca = BibliotecaApiDoaminService.BuscarGuidBiblioteca(inscricao.SeqContextoBibliotecaGed.Value);

                if (retornoApiBiblioteca.StatusCode == HttpStatusCode.OK)
                {
                    foreach (var arquivo in arquivos)
                    {
                        modelo.DocumentoDeferido = arquivo.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Deferido;

                        DadosArquivoSharepointVO dadosArquivoSharepointVO = new DadosArquivoSharepointVO();
                        dadosArquivoSharepointVO.GuidBiblioteca = retornoApiBiblioteca.GuidBiblioteca;
                        dadosArquivoSharepointVO.IdGEDArquivo = $"{arquivo.ArquivoAnexado.UidArquivoGed}";
                        dadosArquivoSharepointVO.AtualizarEventoValidacao = "sim";
                        dadosArquivoSharepointVO.EventoValidacao = MontarEventoValidacao(modelo, inscricao);
                        retorno = RequisicoesSharePoint.EnviarPostApiGed<RetornoArquivoSharepointVO>(dadosArquivoSharepointVO, ACOES_WEBAPI_SHAREPOINT.ATUALIZAR_METADADOS_ARQVUIVO);
                    }
                }
            }

            if (retorno.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new Exception(retorno.ErroMessage);
            }

            return retorno;
        }

        /// <summary>
        /// Prepara o arquivo de uplod para GED
        /// </summary>
        /// <param name="dadosArquivoSharepointVO">Modelo do arquivo que será enviado para o GED</param>
        /// <param name="inscricao">Dados da inscrição</param>
        /// <param name="modelo">Modelo de parametros</param>
        /// <param name="arquivo">Arquivo a ser upado</param>
        /// <param name="retornoApiBiblioteca">Dados da Biblioteca do GED</param>
        /// <param name="retornoApiPortfolio">Dados do portfolio do GED</param>
        private void PrepararArquivoUpload(DadosArquivoSharepointVO dadosArquivoSharepointVO,
                                           dynamic inscricao,
                                           ParametrosArquivoVO modelo,
                                           dynamic arquivo,
                                           RetornoBibliotecaSharepointVO retornoApiBiblioteca,
                                           RetornoPortfolioSharepointVO retornoApiPortfolio)
        {
            dadosArquivoSharepointVO.Autor = MontarAutor(inscricao, modelo.GeradoPeloSistema);
            dadosArquivoSharepointVO.Conteudo = arquivo.ArquivoAnexado.Conteudo;
            dadosArquivoSharepointVO.DataInicioFase = $"{inscricao.DataIncioInscrição.ToString("dd/MM/yyyy")}";
            dadosArquivoSharepointVO.Destinatario = MontarDestinatario(inscricao, modelo.GeradoPeloSistema);
            dadosArquivoSharepointVO.EstruturaPasta = "Inscrições";
            dadosArquivoSharepointVO.EventoAutenticacao = SMCPDFHelper.DocumentoPossuiAssinatura(arquivo.ArquivoAnexado.Conteudo) ? "Sim" : "Não";
            dadosArquivoSharepointVO.EventoValidacao = MontarEventoValidacao(modelo, inscricao);
            dadosArquivoSharepointVO.FasePrazoGuarda = FasePrazoGuarda.Corrente.ToString();
            dadosArquivoSharepointVO.GuidBiblioteca = retornoApiBiblioteca.GuidBiblioteca;
            dadosArquivoSharepointVO.IdentificadorArquivo = arquivo.DocumentoRequerido.PermiteVarios ? arquivo.Seq.ToString() : null;
            dadosArquivoSharepointVO.IdentificadorComponenteDigital = "Único";
            dadosArquivoSharepointVO.IdGEDPortfolio = retornoApiPortfolio.IdGedPortfolio;
            dadosArquivoSharepointVO.IdGEDProcesso = inscricao.UidProcessoGed.ToString();
            dadosArquivoSharepointVO.IndicacaoAnexos = "Não";
            dadosArquivoSharepointVO.NivelAcesso = NivelAcesso.Reservado.ToString();
            dadosArquivoSharepointVO.NomeArquivo = arquivo.ArquivoAnexado.Nome;
            dadosArquivoSharepointVO.NomePastaProcesso = $"GPI NOVO – Inscrição {inscricao.Seq}";
            dadosArquivoSharepointVO.NumeroDocumento = MontarNumeroDocumento(inscricao, arquivo.TipoDocumento.Token);
            dadosArquivoSharepointVO.NumeroProtocolo = $"GPI NOVO – {inscricao.SemestreReferencia}º/{inscricao.AnoReferencia} – Número de inscrição: {inscricao.Seq}";
            dadosArquivoSharepointVO.Originador = MontarOriginador(inscricao, modelo.TipoSistema);
            dadosArquivoSharepointVO.PrevisaoDesclassificacao = PrevisaoDesclassificacao.Permanente.ToString();
            dadosArquivoSharepointVO.SeqHierarquiaClassificacao = inscricao.SeqHierarquiaClassificacaoGed.ToString();
            dadosArquivoSharepointVO.SeqTipoDocumento = arquivo.DocumentoRequerido.SeqTipoDocumento.ToString();
            dadosArquivoSharepointVO.SiglaOrigem = "GPI NOVO";
            dadosArquivoSharepointVO.StatusDocumento = MontarStatusDocumento(inscricao, modelo.GeradoPeloSistema);
            dadosArquivoSharepointVO.TipoMeio = TipoMeio.Digital.ToString();
            dadosArquivoSharepointVO.TipoOperacao = MontarTipoOperacao(modelo.GeradoPeloSistema);
        }

        /// <summary>
        /// Prepara o comprovante para GED
        /// </summary>
        /// <param name="dadosArquivoSharepointVO">Modelo do arquivo que será enviado para o GED</param>
        /// <param name="inscricao">Dados da inscrição</param>
        /// <param name="modelo">Modelo de parametros</param>
        /// <param name="arquivo">Arquivo a ser upado</param>
        /// <param name="retornoApiBiblioteca">Dados da Biblioteca do GED</param>
        /// <param name="retornoApiPortfolio">Dados do portfolio do GED</param>
        private void PrepararArquivoComprovante(DadosArquivoSharepointVO dadosArquivoSharepointVO,
                                                dynamic inscricao,
                                                ParametrosArquivoVO modelo,
                                                dynamic arquivo,
                                                RetornoBibliotecaSharepointVO retornoApiBiblioteca,
                                                RetornoPortfolioSharepointVO retornoApiPortfolio)
        {
            dadosArquivoSharepointVO.Autor = MontarAutor(inscricao, modelo.GeradoPeloSistema);
            dadosArquivoSharepointVO.Conteudo = arquivo.Conteudo;
            dadosArquivoSharepointVO.DataInicioFase = $"{inscricao.DataIncioInscrição.ToString("dd/MM/yyyy")}";
            dadosArquivoSharepointVO.Destinatario = MontarDestinatario(inscricao, modelo.GeradoPeloSistema);
            dadosArquivoSharepointVO.EstruturaPasta = "Inscrições";
            dadosArquivoSharepointVO.EventoAutenticacao = SMCPDFHelper.DocumentoPossuiAssinatura(arquivo.Conteudo) ? "Sim" : "Não";
            dadosArquivoSharepointVO.EventoValidacao = MontarEventoValidacao(modelo, inscricao);
            dadosArquivoSharepointVO.FasePrazoGuarda = FasePrazoGuarda.Corrente.ToString();
            dadosArquivoSharepointVO.GuidBiblioteca = retornoApiBiblioteca.GuidBiblioteca;
            dadosArquivoSharepointVO.IdentificadorArquivo = null;
            dadosArquivoSharepointVO.IdentificadorComponenteDigital = "Único";
            dadosArquivoSharepointVO.IdGEDPortfolio = retornoApiPortfolio.IdGedPortfolio;
            dadosArquivoSharepointVO.IdGEDProcesso = inscricao.UidProcessoGed.ToString();
            dadosArquivoSharepointVO.IndicacaoAnexos = "Não";
            dadosArquivoSharepointVO.NivelAcesso = NivelAcesso.Reservado.ToString();
            dadosArquivoSharepointVO.NomeArquivo = arquivo.Nome;
            dadosArquivoSharepointVO.NomePastaProcesso = $"GPI NOVO – Inscrição {inscricao.Seq}";
            dadosArquivoSharepointVO.NumeroDocumento = "";
            dadosArquivoSharepointVO.NumeroProtocolo = $"GPI NOVO – {inscricao.SemestreReferencia}º/{inscricao.AnoReferencia} – Número de inscrição: {inscricao.Seq}";
            dadosArquivoSharepointVO.Originador = MontarOriginador(inscricao, modelo.TipoSistema);
            dadosArquivoSharepointVO.PrevisaoDesclassificacao = PrevisaoDesclassificacao.Permanente.ToString();
            dadosArquivoSharepointVO.SeqHierarquiaClassificacao = inscricao.SeqHierarquiaClassificacaoGed.ToString();

            var spec = new ViewTipoDocumentoFilterSpecification() { Token = TOKENS.TOKEN_TIPO_DOCUMENTO_COMPROVANTE_INSCRICAO };
            var seqTipoDocumentoGed = ViewTipoDocumentoDomainService.SearchProjectionBySpecification(spec, p => p.Seq).FirstOrDefault();
            dadosArquivoSharepointVO.SeqTipoDocumento = seqTipoDocumentoGed.ToString();

            dadosArquivoSharepointVO.SiglaOrigem = "GPI NOVO";
            dadosArquivoSharepointVO.StatusDocumento = MontarStatusDocumento(inscricao, modelo.GeradoPeloSistema);
            dadosArquivoSharepointVO.TipoMeio = TipoMeio.Digital.ToString();
            dadosArquivoSharepointVO.TipoOperacao = MontarTipoOperacao(modelo.GeradoPeloSistema);
        }

        /// <summary>
        ///Se o token do tipo de documento em questão for 
        ///“CPF_FRENTE_E_VERSO”, informar o número do CPF do inscrito formatado, se cadastrado.Se for 
        ///“DOCUMENTO_DE_IDENTIDADE_FRENTE_E_VERSO”, informar o número da identidade do inscrito, 
        ///se cadastrado.Se for “PASSAPORTE”, informar o número do passaporte do iniscrito se cadastrado.
        /// </summary>
        /// <param name="inscricao">Dados da inscricao</param>
        /// <param name="tipoSistema">Tipo de sistema</param>
        /// <returns>Descrição do autor</returns>
        private string MontarNumeroDocumento(dynamic inscricao, string tokenTipoDocumento)
        {
            switch (tokenTipoDocumento)
            {
                case TOKENS.TOKEN_TIPO_DOCUMENTO_CPF:
                    return inscricao.Cpf;
                case TOKENS.TOKEN_TIPO_DOCUMENTO_IDENTIDADE:
                    return inscricao.NumeroIdentidade;
                case TOKENS.TOKEN_TIPO_DOCUMENTO_PASSAPORTE:
                    return inscricao.NumeroPassaporte;
                default:
                    return "";
            }
        }

        /// <summary>
        ///Se o documento anexado se referir a um documento requerido no processo de inscrição,
        ///informar “Nome: {nome do inscrito } – Número de inscrição: {número da inscrição}.Se o
        ///documento tiver sido gerado pelo sistema, informar “PUC Minas - {nome da unidade responsável pelo processo em questão}.
        /// </summary>
        /// <param name="inscricao">Dados da inscricao</param>
        /// <param name="geradoPeloSistema">Se o arquivo for gerado pelo sistema</param>
        /// <returns>Descrição do autor</returns>
        private string MontarAutor(dynamic inscricao, bool geradoPeloSistema)
        {
            if (geradoPeloSistema)
            {
                return $"PUC Minas - { inscricao.NomeUnidadeResponsavel}";
            }
            else
            {
                return $"Nome: {inscricao.Nome} – Número de inscrição: {inscricao.Seq}";
            }
        }

        /// <summary>
        ///Se o documento anexado se referir a um documento requerido no processo de inscrição,
        ///informar o identificador do status “Cópia” do GDM.Se o documento tiver sido gerado pelo sistema, 
        ///informar o identificador do status “Original” do GDM.
        /// </summary>
        /// <param name="inscricao">Dados da inscricao</param>
        /// <param name="geradoPeloSistema">Se o arquivo for gerado pelo sistema</param>
        /// <returns>Descrição do status do documento</returns>
        private string MontarStatusDocumento(dynamic inscricao, bool geradoPeloSistema)
        {
            if (geradoPeloSistema)
            {
                return StatusDocumento.Original.ToString();
            }
            else
            {
                return StatusDocumento.Copia.ToString();
            }
        }

        /// <summary>
        ///Se o documento anexado se referir a um documento requerido no processo de
        ///inscrição, informar PUC Minas - {nome da unidade responsável pelo processo em questão}. 
        ///Se o documento tiver sido gerado pelo sistema, informar Nome: {nome do inscrito} – Número
        ///de inscrição: {número da inscrição}. 
        /// </summary>
        /// <param name="inscricao">Dados da inscricao</param>
        /// <param name="geradoPeloSistema">Se o arquivo for gerado pelo sistema</param>
        /// <returns>Descrição do destinátario</returns>
        private string MontarDestinatario(dynamic inscricao, bool geradoPeloSistema)
        {
            if (geradoPeloSistema)
            {
                return $"Nome: {inscricao.Nome} – Número de inscrição: {inscricao.Seq}";
            }
            else
            {
                return $"PUC Minas - { inscricao.NomeUnidadeResponsavel}";
            }
        }

        /// <summary>
        ///Se o documento tiver sido anexado no módulo de inscrição, informar Nome: {nome 
        ///do inscrito} – Número de inscrição: {número da inscrição}. Se tiver sido anexado no módulo
        ///administrativo, informar Nome: {nome do usuário} – Código de usuário: {código do usuário SAS}.
        /// </summary>
        /// <param name="inscricao">Dados da inscricao</param>
        /// <param name="geradoPeloSistema">Se o arquivo for gerado pelo sistema</param>
        /// <returns>Descrição do originador</returns>
        private string MontarOriginador(dynamic inscricao, TipoSistema tipoSistema)
        {
            switch (tipoSistema)
            {
                case TipoSistema.Inscricao:
                    return $"Nome: {inscricao.Nome} – Número de inscrição: {inscricao.Seq}";
                case TipoSistema.Administrativo:
                    return $"Nome: {SMCContext.User.SMCGetNome()} - Código de Usuario: {SMCContext.User.SMCGetSequencialUsuario()}";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Se o documento estiver sendo deferido juntamente com a postagem, informar a 
        ///descrição do evento de validação no seguinte formato: GPI NOVO – Nome: {nome do usuário
        ///responsável pelo deferimento} – Código de usuário: {Código do usuário SAS responsável pelo
        ///deferimento} – {data, hora e fuso atual} - IP: {IP da máquina logada}. Exemplo: GPI - 
        ///Nome: Beltrano Sabino - Código de usuário: 167557 - 2022-12-22 14:42:01.6479034 -03:00. Se o
        ///documento já tiver sido deferido previamente, informar a descrição do evento de validação no seguinte
        ///formato: “GPI NOVO – Unidade Responsável: {nome da unidade responsável pelo processo}. 
        ///Exemplo: “GPI NOVO – Unidade Responsável: Programa de Pós-graduação em Direito”. Se o
        ///documento não estiver defefrido, não informar valor para este metadado.
        /// </summary>
        /// <param name="modelo">Modelo de dados</param>
        /// <param name="inscricao">Dados da inscrição</param>
        /// <returns>Descrição do evento de validação</returns>
        private string MontarEventoValidacao(ParametrosArquivoVO modelo, dynamic inscricao)
        {
            if (modelo.DocumentoDeferido && !modelo.DocumentoDeferidoPreviamente)
            {
                return $"GPI NOVO - Nome: {SMCContext.User.SMCGetNome()} - Código do usuário: {SMCContext.User.SMCGetSequencialUsuario()} - {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK")} - IP: {SMCContext.ClientAddress.Ip}";
            }
            else if (modelo.DocumentoDeferido && modelo.DocumentoDeferidoPreviamente)
            {
                return $"GPI NOVO - Unidade Responsável: {inscricao.NomeUnidadeResponsavel}";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///Se o documento anexado se referir a um documento requerido no processo de
        ///inscrição, informar o identificador do tipo de operação "Postagem externa" do GDM.Se o documento
        ///tiver sido gerado pelo sistema, informar o identificador do tipo de operação "Emissão interna" do GDM.
        /// </summary>
        /// <param name="geradoPeloSistema">Se o arquivo for gerado pelo sistema</param>
        /// <returns>Descrição do autor</returns>
        private string MontarTipoOperacao(bool geradoPeloSistema)
        {
            if (geradoPeloSistema)
            {
                return TipoOperacao.EmissaoInterna.ToString();
            }
            else
            {
                return TipoOperacao.PostagemExterna.ToString();
            }
        }
    }
}
