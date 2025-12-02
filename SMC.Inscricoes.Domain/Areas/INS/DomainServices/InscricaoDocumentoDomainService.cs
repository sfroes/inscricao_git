using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.DadosMestres.ServiceContract.Areas.GED.Data;
using SMC.DadosMestres.ServiceContract.Areas.GED.Interfaces;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Caching;
using SMC.Framework.Domain;
using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Security.Util;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Framework.Util;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Exceptions.InscricaoDocumento;
using SMC.Inscricoes.Common.Areas.RES;
using SMC.Inscricoes.Common.Enums;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.RES.DomainServices;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using SMC.Inscricoes.Domain.Areas.SEL.DomainServices;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Portfolio;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject;
using SMC.Inscricoes.Domain.Models;
using SMC.Inscricoes.Extensions;
using SMC.Localidades.Common.Areas.LOC.Enums;
using SMC.Notificacoes.Common.Areas.NTF.Enums;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Data;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;
using SMC.PDF;
using SMC.PDFData.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscricaoDocumentoDomainService : InscricaoContextDomain<InscricaoDocumento>
    {

        #region Variaveis do contexto
        private static string[] allowedExtensions = { ".doc", ".docx", ".xls", ".xlsx", ".jpg", ".jpeg", ".png", ".pdf", ".rar", ".zip", ".ps", ".xml" };
        private static string[] allowedExtensionsPDF = { ".png", ".jpg", ".jpeg", ".pdf", ".xml"};
        private List<long> arquivosCriadosGED = new List<long>();
        private List<string> arquivosDeletadosGED = new List<string>();
        private List<long> arquivosAtualizarGED = new List<long>();
        private List<long> arquivosAtualizarMetadadosGED = new List<long>();

        #endregion

        public const string ExtensoesConversaoPDF = ".doc, .docx, .xls, .xlsx, .jpg, .jpeg, .png, .pdf, .rar, .zip, .ps, .xml";
        public const string ExtensoesOffice = ".vnd.openxmlformats-officedocument.wordprocessingml.document, .msword, .vnd.ms-excel, .vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public const string ExtensoesSemConversaoPDF = ".pdf, .xml";

        #region Domain Service

        private TipoDocumentoDomainService TipoDocumentoDomainService => Create<TipoDocumentoDomainService>();
        private DocumentoRequeridoDomainService DocumentoRequeridoDomainService => this.Create<DocumentoRequeridoDomainService>();

        private ArquivoAnexadoDomainService ArquivoAnexadoDomainService => this.Create<ArquivoAnexadoDomainService>();

        private InscricaoDomainService InscricaoDomainService => this.Create<InscricaoDomainService>();

        private InscricaoHistoricoSituacaoDomainService InscricaoHistoricoSituacaoDomainService => this.Create<InscricaoHistoricoSituacaoDomainService>();

        private TipoProcessoSituacaoDomainService TipoProcessoSituacaoDomainService => this.Create<TipoProcessoSituacaoDomainService>();

        private GrupoDocumentoRequeridoDomainService GrupoDocumentoRequeridoDomainService => this.Create<GrupoDocumentoRequeridoDomainService>();

        private EtapaProcessoDomainService EtapaProcessoDomainService => Create<EtapaProcessoDomainService>();

        private InscricaoOfertaHistoricoSituacaoDomainService InscricaoOfertaHistoricoSituacaoDomainService => Create<InscricaoOfertaHistoricoSituacaoDomainService>();

        private ProcessoConfiguracaoNotificacaoIdiomaDomainService ProcessoConfiguracaoNotificacaoIdiomaDomainService => Create<ProcessoConfiguracaoNotificacaoIdiomaDomainService>();

        private InscritoDomainService InscritoDomainService => Create<InscritoDomainService>();

        private ConfiguracaoEtapaDomainService ConfiguracaoEtapaDomainService => Create<ConfiguracaoEtapaDomainService>();

        private UnidadeResponsavelDomainService UnidadeResponsavelDomainService => Create<UnidadeResponsavelDomainService>();

        private OfertaDomainService OfertaDomainService => Create<OfertaDomainService>();

        private InscricaoEnvioNotificacaoDomainService InscricaoEnvioNotificacaoDomainService => Create<InscricaoEnvioNotificacaoDomainService>();

        private ArquivoApiDoaminService ArquivoApiDoaminService => Create<ArquivoApiDoaminService>();
        private InscricaoOfertaDomainService InscricaoOfertaDomainService => Create<InscricaoOfertaDomainService>();
        private ProcessoDomainService ProcessoDomainService => Create<ProcessoDomainService>();

        #endregion Domain Service

        #region Services

        private ITipoDocumentoService TipoDocumentoService
        {
            get { return this.Create<ITipoDocumentoService>(); }
        }

        private ITemplateProcessoService TemplateProcessoService => Create<ITemplateProcessoService>();

        private INotificacaoService NotificacaoService => Create<INotificacaoService>();

        #endregion Services


        #region Registro de Entrega de Documentos

        /// <summary>
        /// Busca os documentos de uma inscrição
        /// </summary>
        /// <param name="filtro">Filtro de pesquisa</param>
        /// <returns>Lista de documentos de uma inscrição</returns>
        public SumarioDocumentosEntreguesVO BuscarSumarioDocumentosEntregue(InscricaoDocumentoFilterSpecification filtro)
        {
            long seqConfiguracaoEtapa = this.InscricaoDomainService.SearchProjectionByKey(filtro.SeqInscricao.Value, x => x.SeqConfiguracaoEtapa);

            var sexoInscrito = this.InscricaoDomainService.SearchProjectionByKey(filtro.SeqInscricao.Value, x => x.Inscrito.Sexo);

            var retorno = new SumarioDocumentosEntreguesVO();
            retorno.DocumentosObrigatorios = new List<DocumentoRequeridoVO>();
            retorno.GruposDocumentos = new List<GrupoDocumentoEntregueVO>();
            retorno.DocumentosOpcionais = new List<DocumentoRequeridoVO>();

            retorno.SeqInscricao = filtro.SeqInscricao.Value;
            var documentosRequeridos = BuscarDocumentosRequeridosInscricao(filtro);
            var obrigatoriosRequeridos = documentosRequeridos.Where(x => x.Obrigatorio && (!x.Sexo.HasValue || x.Sexo == sexoInscrito)).ToList();
            retorno.DocumentosObrigatorios.AddRange(obrigatoriosRequeridos);

            var naoObrigatorios = documentosRequeridos.Where(x => !x.Obrigatorio || (x.Obrigatorio && x.Sexo.HasValue && x.Sexo != sexoInscrito));

            //Recuperar todos os grupos e separar
            retorno.GruposDocumentos.AddRange(BuscarGruposDocumentosRequeridos(seqConfiguracaoEtapa, naoObrigatorios, filtro.SeqInscricao.Value));

            naoObrigatorios = naoObrigatorios.SMCRemove(x => retorno.GruposDocumentos.Any(g => g.DocumentosRequeridosGrupo.Any(d => d.Seq == x.Seq)));
            retorno.DocumentosOpcionais.AddRange(naoObrigatorios);

            retorno.DocumentosObrigatorios.ForEach(d =>
            {
                d.InscricaoDocumentos.ForEach(i =>
                {
                    i.ExibirExibirObservacaoParaInscrito = true;
                    i.ExibirInformacaoExibirObservacaoParaInscrito = true;
                });
            });

            retorno.GruposDocumentos.ForEach(g =>
            {
                g.DocumentosRequeridosGrupo.ForEach(d =>
                {
                    d.InscricaoDocumentos.ForEach(i =>
                    {
                        i.ExibirExibirObservacaoParaInscrito = true;
                        i.ExibirInformacaoExibirObservacaoParaInscrito = true;
                    });
                });
            });

            return retorno;
        }

        private IEnumerable<GrupoDocumentoEntregueVO> BuscarGruposDocumentosRequeridos(long seqConfiguracaoEtapa, IEnumerable<DocumentoRequeridoVO> documentosRequeridos, long seqInscricao)
        {
            var retornoGruposDocumentos = new List<GrupoDocumentoEntregueVO>();
            //Recuperar todos os grupos e separar
            var grupoSpec = new GrupoDocumentoRequeridoFilterSpecification
            {
                SeqConfiguracaoEtapa = seqConfiguracaoEtapa,
            };
            var gruposDocumentos = GrupoDocumentoRequeridoDomainService.SearchBySpecification(grupoSpec, x => x.Itens);
            foreach (var grupo in gruposDocumentos)
            {
                var grupoVO = new GrupoDocumentoEntregueVO();
                grupoVO.DocumentosRequeridosGrupo = new List<DocumentoRequeridoVO>();
                grupoVO.Descricao = grupo.Descricao;
                grupoVO.MinimoObrigatorio = grupo.MinimoObrigatorio;
                var seqDocsNoGrupo = grupo.Itens.Select(x => x.SeqDocumentoRequerido);
                foreach (long seqDocRequerido in seqDocsNoGrupo)
                {
                    var docsNoGrupo = documentosRequeridos.FirstOrDefault(x => x.Seq == seqDocRequerido);

                    if (docsNoGrupo == null)
                    {
                        var inscricaoDocumentoGrupo = NovaInscricaoOfertaGrupo(seqDocRequerido, seqInscricao);
                        var docRequerido = TransFormInscricaoDocumentoToDocumentoRequerido(inscricaoDocumentoGrupo);

                        // Preencher as solicitações de entrega dos documentos, conforme regras de tokens
                        docRequerido.SolicitacoesEntregaDocumento = BuscarSituacoesEntregaDocumento(docRequerido, seqInscricao);
                        docRequerido.InscricaoDocumentos.Add(inscricaoDocumentoGrupo);

                        grupoVO.DocumentosRequeridosGrupo.Add(docRequerido);
                    }
                    else if (!docsNoGrupo.InscricaoDocumentos.SMCAny())
                    {
                        var inscricaoDocumentoGrupo = NovaInscricaoOfertaGrupo(docsNoGrupo.Seq, seqInscricao);
                        docsNoGrupo.InscricaoDocumentos.Add(inscricaoDocumentoGrupo);

                        grupoVO.DocumentosRequeridosGrupo.Add(docsNoGrupo);
                    }
                    else
                    {
                        grupoVO.DocumentosRequeridosGrupo.Add(docsNoGrupo);
                    }
                }
                grupoVO.DocumentosRequeridosGrupo =
                        grupoVO.DocumentosRequeridosGrupo.OrderBy(x => x.DescricaoTipoDocumento).ToList();
                retornoGruposDocumentos.Add(grupoVO);
            }
            return retornoGruposDocumentos;
        }

        public DocumentoRequeridoVO TransFormInscricaoDocumentoToDocumentoRequerido(InscricaoDocumentoVO inscricaoDocumentoGrupo)
        {
            var documentoRequerido = inscricaoDocumentoGrupo.Transform<DocumentoRequeridoVO>();
            documentoRequerido.Seq = inscricaoDocumentoGrupo.SeqDocumentoRequerido;
            documentoRequerido.InscricaoDocumentos = new List<InscricaoDocumentoVO>()
            {
                inscricaoDocumentoGrupo.Transform<InscricaoDocumentoVO>()
            };
            return documentoRequerido;
        }

        private InscricaoDocumentoVO NovaInscricaoOfertaGrupo(long seqDocumentoRequerido, long seqInscricao)
        {
            var inscricaoDocumentoGrupo = DocumentoRequeridoDomainService.SearchProjectionByKey(
                                       new SMCSeqSpecification<DocumentoRequerido>(seqDocumentoRequerido),
                                       x => new InscricaoDocumentoVO
                                       {
                                           SeqConfiguracaoEtapa = x.SeqConfiguracaoEtapa,
                                           SeqInscricao = seqInscricao,
                                           SeqDocumentoRequerido = x.Seq,
                                           SeqTipoDocumento = x.SeqTipoDocumento,
                                           DataEntrega = DateTime.Today,
                                           VersaoDocumento = x.VersaoDocumento,
                                           VersaoDocumentoExigido = x.VersaoDocumento,
                                           FormaEntregaDocumento = FormaEntregaDocumento.Presencial,
                                           SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoEntrega,
                                           SituacaoEntregaDocumentoInicial = SituacaoEntregaDocumento.AguardandoEntrega,
                                           PermiteVarios = x.PermiteVarios,
                                           Obrigatorio = x.Obrigatorio,
                                           PermiteEntregaPosterior = x.PermiteEntregaPosterior,
                                           ValidacaoOutroSetor = x.ValidacaoOutroSetor,
                                           PermiteUploadArquivo = x.PermiteUploadArquivo,
                                           Sexo = x.Sexo,
                                           DescricaoTipoDocumento = x.TipoDocumento.Descricao,
                                           UploadObrigatorio = x.UploadObrigatorio,
                                           EntregaPosterior = false
                                       });
            inscricaoDocumentoGrupo.Seq = this.InsertEntity(inscricaoDocumentoGrupo.Transform<InscricaoDocumento>()).Seq;
            inscricaoDocumentoGrupo.DescricaoTipoDocumento = inscricaoDocumentoGrupo.DescricaoTipoDocumento;

            return inscricaoDocumentoGrupo;
        }

        private List<DocumentoRequeridoVO> BuscarDocumentosRequeridosInscricao(InscricaoDocumentoFilterSpecification filtro)
        {
            var documentosRequeridos = new List<DocumentoRequeridoVO>();
            var documentosVos = BuscarInscricaoDocumentos(filtro);
            var seqsDocumentosRequeridos = documentosVos.Select(x => x.SeqDocumentoRequerido).Distinct();

            foreach (var seq in seqsDocumentosRequeridos)
            {
                var documentoRequerido = TransFormInscricaoDocumentoToDocumentoRequerido(documentosVos.FirstOrDefault(x => x.SeqDocumentoRequerido == seq));
                documentoRequerido.InscricaoDocumentos = documentosVos.Where(x => x.SeqDocumentoRequerido == seq).ToList();
                documentosRequeridos.Add(documentoRequerido);
            }

            // Preencher as solicitações de entrega dos documentos, conforme regras de tokens
            // Não retirar pois nesse metodo que configura se é ou não pra bloquear os campos se for validação de outro setor.
            documentosRequeridos.SMCForEach(x => x.SolicitacoesEntregaDocumento = BuscarSituacoesEntregaDocumento(x, filtro.SeqInscricao.Value));

            return documentosRequeridos.OrderBy(x => x.DescricaoTipoDocumento).ToList();
        }

        public IEnumerable<InscricaoDocumentoVO> BuscarInscricaoDocumentos(InscricaoDocumentoFilterSpecification filtro)
        {
            var vos = this.SearchProjectionBySpecification(filtro, x => new InscricaoDocumentoVO
            {
                Seq = x.Seq,
                SeqInscricao = x.SeqInscricao,
                SeqDocumentoRequerido = x.SeqDocumentoRequerido,
                SeqTipoDocumento = x.DocumentoRequerido.SeqTipoDocumento,
                ExibeTermoResponsabilidadeEntrega = x.DocumentoRequerido.ExibeTermoResponsabilidadeEntrega,
                DataLimiteEntrega = x.DocumentoRequerido.DataLimiteEntrega,
                DataEntrega = x.DataEntrega,
                VersaoDocumento = x.VersaoDocumento,
                VersaoDocumentoExigido = x.DocumentoRequerido.VersaoDocumento,
                FormaEntregaDocumento = x.FormaEntregaDocumento,//.HasValue ? x.FormaEntregaDocumento.Value : FormaEntregaDocumento.Presencial,
                SeqArquivoAnexado = x.SeqArquivoAnexado,
                ArquivoAnexado = x.ArquivoAnexado != null ? new SMCUploadFile
                {
                    Size = x.ArquivoAnexado.Tamanho,
                    GuidFile = x.ArquivoAnexado.Seq.ToString(),
                    Name = x.ArquivoAnexado.Nome,
                    Type = x.ArquivoAnexado.Tipo
                } : null,
                DescricaoArquivoAnexado = x.DescricaoArquivoAnexado,
                Observacao = x.Observacao.ToString(),
                ExibirObservacaoParaInscrito = x.ExibirObservacaoParaInscrito,
                SituacaoEntregaDocumento = x.SituacaoEntregaDocumento,
                SituacaoEntregaDocumentoInicial = x.SituacaoEntregaDocumento,
                PermiteVarios = x.DocumentoRequerido.PermiteVarios,
                Obrigatorio = x.DocumentoRequerido.Obrigatorio,
                Sexo = x.DocumentoRequerido.Sexo,
                DocumentacaoEntregue = x.Inscricao.DocumentacaoEntregue,
                SituacaoInscricao = x.Inscricao.HistoricosSituacao.FirstOrDefault(s => s.Atual).TipoProcessoSituacao.Token,
                PermiteEntregaPosterior = x.DocumentoRequerido.PermiteEntregaPosterior,
                ValidacaoOutroSetor = x.DocumentoRequerido.ValidacaoOutroSetor,
                DescricaoTipoDocumento = x.DocumentoRequerido.TipoDocumento.Descricao,
                PermiteUploadArquivo = x.DocumentoRequerido.PermiteUploadArquivo,
                SeqConfiguracaoEtapa = x.DocumentoRequerido.SeqConfiguracaoEtapa,
                DataPrazoEntrega = x.DataPrazoEntrega,
                EntregaPosterior = x.EntregaPosterior,
            }).OrderBy(o => o.DescricaoTipoDocumento).ToList();

            foreach (var item in vos)
            {
                if (item.ArquivoAnexado != null) item.ArquivoAnexado.Description = item.DescricaoArquivoAnexado;
                //var tipoDocumento = this.TipoDocumentoService.BuscarTipoDocumento(item.SeqTipoDocumento);
                item.DescricaoTipoDocumento = !string.IsNullOrEmpty(item.DescricaoTipoDocumento) ? item.DescricaoTipoDocumento : $"Erro: Tipo do documento de id '({item.SeqTipoDocumento})' não existe";
                item.DataLimiteEntrega = item.DataLimiteEntrega.HasValue ? item.DataLimiteEntrega : item.DataPrazoEntrega;
            }

            return vos;
        }

        /// <summary>
        /// Salva um documento requerido de uma inscrição
        /// Realiza teste para validar a situação da inscrição para finalizar a entrega de documentos
        /// ou setar o flag como false, caso algum documento exigido tenha a entrega desfeita
        /// </summary>
        /// <param name="inscricaoDocumento">Documento requerido a ser salvo</param>
        /// <returns>O VO do documento requerido após as alterações</returns>
        public InscricaoDocumentoVO SalvarDocumento(InscricaoDocumento inscricaoDocumento)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    InscricaoDocumentoVO ret;
                    var docBanco = this.SearchByKey(new SMCSeqSpecification<InscricaoDocumento>(inscricaoDocumento.Seq)
                        , x => x.ArquivoAnexado);

                    if (inscricaoDocumento.ArquivoAnexado != null)
                    {
                        if (!VerifyExtension(inscricaoDocumento, inscricaoDocumento.SeqInscricao))
                        {
                            if (ValidarConverterPDF(inscricaoDocumento.SeqInscricao))
                            {
                                throw new ExtensaoDocumentoInvalidaPDFException(inscricaoDocumento.ArquivoAnexado.Nome);
                            }
                            else
                            {
                                throw new ExtensaoDocumentoInvalidaException(inscricaoDocumento.ArquivoAnexado.Nome);
                            }
                        }

                        if (inscricaoDocumento.ArquivoAnexado.Description != inscricaoDocumento.ArquivoAnexado.Nome)
                        {
                            inscricaoDocumento.DescricaoArquivoAnexado = inscricaoDocumento.ArquivoAnexado.Description;
                        }
                        if (inscricaoDocumento.ArquivoAnexado.State == SMCUploadFileState.Changed
                            && inscricaoDocumento.SeqArquivoAnexado.HasValue
                            && inscricaoDocumento.SeqArquivoAnexado.Value != default(long)
                            && docBanco.ArquivoAnexado.Conteudo == null)
                        {
                            inscricaoDocumento.ArquivoAnexado.Conteudo = docBanco.ArquivoAnexado.Conteudo;
                        }
                        else if (inscricaoDocumento.ArquivoAnexado.State == SMCUploadFileState.Unchanged)
                        {
                            inscricaoDocumento.ArquivoAnexado.Conteudo = docBanco.ArquivoAnexado.Conteudo;
                        }
                    }
                    else
                    {
                        inscricaoDocumento.SeqArquivoAnexado = null;
                    }

                    if (docBanco.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Deferido
                        && inscricaoDocumento.SituacaoEntregaDocumento != SituacaoEntregaDocumento.Deferido)
                    {
                        ret = DesfazerEntregaDocumento(inscricaoDocumento);
                    }
                    else if (docBanco.SituacaoEntregaDocumento != SituacaoEntregaDocumento.Deferido
                       && inscricaoDocumento.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Deferido)
                    {
                        ret = EntregarDocumento(inscricaoDocumento);
                    }
                    else
                    {
                        if (inscricaoDocumento.ArquivoAnexado == null) inscricaoDocumento.SeqArquivoAnexado = null;
                        this.UpdateEntity(inscricaoDocumento);
                        ret = this.BuscarInscricaoDocumentos(new InscricaoDocumentoFilterSpecification { Seq = inscricaoDocumento.Seq }).FirstOrDefault();
                    }
                    ret.DescricaoTipoDocumento = this.TipoDocumentoService.BuscarTipoDocumento(ret.SeqTipoDocumento).Descricao;
                    unitOfWork.Commit();
                    return ret;
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Salva todos os documentos entregues
        /// </summary>
        /// <param name="SumarioDocumentosEntreguesViewModel"></param>
        /// <returns>SeqProcesso</returns>
        public long SalvarSumarioDocumentosEntreguesInscricao(SumarioDocumentosEntreguesVO documentosEntregues)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    ValidarNotificacaoEntregaDocumentacao(documentosEntregues);

                    /*1*/
                    ValidarDocumentosObrigatoriosEGruposDocumentos(documentosEntregues);

                    /*2*/
                    var includes = IncludesInscricao.Ofertas | IncludesInscricao.Ofertas_Oferta | IncludesInscricao.Processo
                                 | IncludesInscricao.HistoricosSituacao_TipoProcessoSituacao
                                 | IncludesInscricao.Ofertas_HistoricosSituacao_TipoProcessoSituacao;
                    var inscricao = this.InscricaoDomainService.SearchByKey(new SMCSeqSpecification<Inscricao>(documentosEntregues.SeqInscricao), includes);

                    var inscricaoDocumentacaoEntregueBase = inscricao.DocumentacaoEntregue;
                    var tokenSituacaoInscricao = inscricao.HistoricosSituacao.FirstOrDefault(x => x.Atual)?.TipoProcessoSituacao.Token;

                    /*2. Caso a situação da documentação da inscrição seja “Entregue”, ou “Entregue com pendência”, atualizar o campo
                         “documentação entregue” da inscrição com o valor “Sim”, caso contrário, atualizar com o valor “Não”.*/
                    inscricao.DocumentacaoEntregue = (inscricao.SituacaoDocumentacao == SituacaoDocumentacao.Entregue ||
                                                      inscricao.SituacaoDocumentacao == SituacaoDocumentacao.EntregueComPendencia);

                    AtualizarInscricaoDocumentacaoEntrega(documentosEntregues.SeqInscricao, inscricao.DocumentacaoEntregue);

                    var ofertaExigePagamento = inscricao.Ofertas.Any(f => f.Oferta.ExigePagamentoTaxa);

                    /*2.1. Se a inscrição estiver na situação INSCRICAO_FINALIZADA e o campo "documentação entregue" estiver sendo alterado
                     * de “Não” para “Sim” e a oferta não exigir pagamento de taxa ou o campo "título pago" da inscrição também for "Sim":*/
                    if (tokenSituacaoInscricao == TOKENS.SITUACAO_INSCRICAO_FINALIZADA
                        && !inscricaoDocumentacaoEntregueBase && inscricao.DocumentacaoEntregue && (!ofertaExigePagamento || inscricao.TituloPago))
                    {
                        //2.1.1. Atualizar a situação da inscrição para INSCRICAO_CONFIRMADA.
                        AtualizarSituacaoInscricao(inscricao.Seq, inscricao.Processo.SeqTipoProcesso, TOKENS.SITUACAO_INSCRICAO_CONFIRMADA);

                        /* 2.1.2. Ao confirmar a inscrição, se não houver a situação INSCRICAO_DEFERIDA no template de processo
                           associado ao processo de inscrição em questão[...]*/
                        var situacoesTemplateProcesso = TemplateProcessoService.BuscarSituacoesPorTemplateProcesso(inscricao.Processo.SeqTemplateProcessoSGF);
                        // Verifica se o template de processo não possui inscrição deferida, e se a situação está trocando de finalizada para confirmada
                        if (!situacoesTemplateProcesso.Contains(TOKENS.SITUACAO_INSCRICAO_DEFERIDA))
                        {
                            //[...]Criar um novo registro no histórico da inscrição oferta para cada oferta da inscrição,
                            //com a situação CANDIDATO_CONFIRMADO, informando a data em que a alteração ocorreu,
                            //a etapa de SELECAO, o motivo e a justificativa nulos, os indicadores de atual e de atual etapa igual a “sim”.

                            var seqsInscricaoOferta = inscricao.Ofertas.Select(x => x.Seq).ToList();

                            InscricaoOfertaHistoricoSituacaoDomainService.AlterarHistoricoSituacao(new AlterarHistoricoSituacaoVO()
                            {
                                SeqProcesso = inscricao.SeqProcesso,
                                SeqInscricoesOferta = seqsInscricaoOferta,
                                TokenSituacaoDestino = TOKENS.SITUACAO_CANDIDATO_CONFIRMADO
                            });
                        }
                    }
                    /* 2.2. Se o campo “documentação entregue” estiver sendo alterado de “Sim” para “Não”:*/
                    if (inscricaoDocumentacaoEntregueBase && !inscricao.DocumentacaoEntregue)
                    {

                        /* 2.2.1. Verificar se o candidato está com a situação atual da inscrição como INSCRICAO_CONFIRMADA e, caso possua
                          histórico de situação da inscrição oferta, a situação atual de todas as inscrições-oferta seja CANDIDATO_CONFIRMADO. */
                        if (VerificarSituacaoAtualCandidatoOfertas(inscricao))
                        {
                            /* Em caso afirmativo, emitir a mensagem de confirmação abaixo:

                              "A inscrição deste candidato já foi confirmada. Ao realizar essa alteração, a entrega da documentação será desfeita
                              e o candidato voltará para a situação anterior. Caso tenha sido configurada alguma notificação de retorno de situação,
                              ele será comunicado deste fato. Deseja continuar?"

                              2.2.1.1. Caso o usuário clique em "Não", abortar a operação.
                              2.2.1.2. Caso o usuário clique em sim, voltar o candidato para a situação anterior e passar o indicador de atual e
                              de atual etapa para "Não" no histórico de situação da inscrição oferta.*/
                            this.InscricaoHistoricoSituacaoDomainService.AlterarSituacaoInscricoes(new AlteracaoSituacaoVO()
                            {
                                SeqTipoProcessoSituacaoDestino = -1,
                                SeqInscricoes = new List<long> { inscricao.Seq }
                            });
                        }
                        /*2.2.2. Se a situação atual da inscrição do candidato for INSCRICAO_DEFERIDA, ou for INSCRICAO_CONFIRMADA e
                                existir histórico de situação da inscrição oferta cuja situação atual não é CANIDATO_CONFIRMADO, emitir a mensagem de confirmação abaixo:

                        “Ao realizar essa alteração, a entrega da documentação será desfeita, gerando uma inconsistência em relação à
                        situação da inscrição, que só pôde ser confirmada, pelo fato de estar entregue. Deseja continuar?"

                        2.2.2.1. Caso o usuário clique em “Não”, abortar a operação.
                        2.2.2.2. Caso o usuário clique em “Sim”, prosseguir com a operação.*/
                    }

                    //InscricaoDomainService.AtualizarGED(inscricao);

                    SalvarDocumentosRequeridos(documentosEntregues);

                    CriarArquivosGED(inscricao.Seq, arquivosCriadosGED, TipoSistema.Administrativo);
                    AtualizarArquivoGED(inscricao.Seq, arquivosAtualizarGED, TipoSistema.Administrativo);
                    ExcluirArquivoGED(inscricao.Seq, arquivosDeletadosGED);
                    AtualizarMetadadosArquivoGED(inscricao.Seq, arquivosAtualizarMetadadosGED);

                    AtualizarDataPrazoNovaEntregaDocumentacao(documentosEntregues.SeqInscricao, documentosEntregues.DataPrazoNovaEntregaDocumentacao);

                    EnviarNotificacao(documentosEntregues);

                    unitOfWork.Commit();

                    return inscricao.SeqProcesso;
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public void EnviarNotificacao(SumarioDocumentosEntreguesVO documentosEntregues)
        {
            /*
             “RN_INS_179 - Envio de notificação de nova entrega da documentação”:

             Se a notificação de "Nova entrega da documentação" estiver configurada no processo com a tag {{DSC_LISTA_DOCUMENTACAO_INDEFERIDA}}
             e a inscrição possuir documento obrigatório ou grupo de documento com documento indeferido,

             ou

             Se a notificação de "Nova entrega da documentação" estiver configurada no processo com a tag {{DSC_LISTA_DOCUMENTACAO_PENDENTE}}
             e a inscrição possuir documento obrigatório ou grupo de documento com documento pendente:

             Enviar a notificação de nova entrega de documentação para o inscrito, utilizando a configuração de notificação do idioma escolhido na
             inscrição e convertendo as tags da seguinte forma:

             - NOM_SOCIAL_INSCRITO: Retornar o nome social do inscrito, caso esteja preenchido. Caso não esteja, retornar o nome do inscrito.
             - DSC_PROCESSO: Retornar a descrição do processo.
             - DSC_OFERTA: Retornar o nome completo da hierarquia de oferta da(s) oferta(s) selecionada(s) pelo inscrito. Caso exista mais de uma oferta, separar por vírgula.
             - DSC_LISTA_DOCUMENTACACO_INDEFERIDA: Retornar a lista de documentos obrigatórios e de grupos de documentos da inscrição que estão indeferidos.
                                                   Exibir os documentos em uma lista de tópicos e, para cada documento marcado para "Exibir a observação para o inscrito",
                                                   retornar, entre parênteses, na frente da descrição do documento, a "Observação".
             - DSC_LISTA_DOCUMENTACAO_PENDENTE: Retornar a lista de documentos obrigatórios e de grupos de documentos da inscrição que estão pendentes. Exibir os documentos
                                                em uma lista de tópicos e, para cada documento marcado para "Exibir a observação para o inscrito", retornar, entre parênteses,
                                                na frente da descrição do documento, a "Observação".
             - DAT_PRAZO_NOVA_ENTREGA_DOCUMENTACAO: Retornar o prazo para nova entrega da documentação.
             - NOM_UNIDADE_RESPONSAVEL: Retornar o nome da unidade responsável pelo processo.
             - END_UNIDADE_RESPONSAVEL: Retornar o endereço da unidade responsável pelo processo no seguinte formato: <logradouro> + ", " + <número> + " - " + <bairro> + " - "
                                       + <cidade> + "/" + <estado> + " - CEP: " <CEP>.
             - TEL_UNIDADE_RESPONSAVEL: Retornar os números de telefone da unidade responsável, separados por vírgula, no seguinte formato: "+" + <código de país> + " "
                                       + <DDD> + " " <número>.
             - END_ELETRONICO_UNIDADE_RESPONSAVEL: Retornar o e-mail da unidade responsável.

             */

            var specInscricao = new SMCSeqSpecification<Inscricao>(documentosEntregues.SeqInscricao);

            var existeNotificacaoNovaEntregaConfigurada = InscricaoDomainService.SearchProjectionByKey(specInscricao, i => i.Processo.ConfiguracoesNotificacao.Any(c => c.TipoNotificacao.Token == TOKENS.NOTIFICACAO_NOVA_ENTREGA_DOCUMENTACAO));

            if (existeNotificacaoNovaEntregaConfigurada)
            {
                var inscricao = this.InscricaoDomainService.SearchProjectionByKey(specInscricao, i => new
                {
                    SeqConfiguracaoTipoNotificacao = i.Processo.ConfiguracoesNotificacao.FirstOrDefault(c => c.TipoNotificacao.Token == TOKENS.NOTIFICACAO_NOVA_ENTREGA_DOCUMENTACAO).ConfiguracoesIdioma.FirstOrDefault(c => c.ProcessoIdioma.Idioma == i.Idioma).SeqConfiguracaoTipoNotificacao,
                    SeqProcesso = i.SeqProcesso,
                    Idioma = i.Idioma,
                    SeqInscrito = i.SeqInscrito,
                    SeqConfiguracaoEtapa = i.SeqConfiguracaoEtapa,
                    DescricaoProcesso = i.Processo.Descricao,
                    SeqInscricao = i.Seq,
                    DataPrazoNovaEntregaDocumentacao = i.DataPrazoNovaEntregaDocumentacao
                });

                //Busca a notificação no sistema de notificações
                var notificacao = NotificacaoService.BuscarConfiguracaoNotificacaoEmail(inscricao.SeqConfiguracaoTipoNotificacao);

                //Busca os documentos e grupos indeferidos ou pendentes
                var documentosObrigatoriosIndeferidos = documentosEntregues.DocumentosObrigatorios.SelectMany(d => d.InscricaoDocumentos.Where(i => i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Indeferido).Select(w => new { w.ExibirObservacaoParaInscrito, w.Observacao, d.DescricaoTipoDocumento })).ToList();
                var documentosObrigatoriosPendentes = documentosEntregues.DocumentosObrigatorios.SelectMany(d => d.InscricaoDocumentos.Where(i => i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente).Select(w => new { w.ExibirObservacaoParaInscrito, w.Observacao, d.DescricaoTipoDocumento })).ToList();
                var gruposDocumentosIndeferidos = documentosEntregues.GruposDocumentos.SelectMany(gd => gd.DocumentosRequeridosGrupo.SelectMany(dr => dr.InscricaoDocumentos.Where(i => i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Indeferido).Select(w => new { w.ExibirObservacaoParaInscrito, w.Observacao, dr.DescricaoTipoDocumento }))).ToList();
                var gruposDocumentosPendentes = documentosEntregues.GruposDocumentos.SelectMany(gd => gd.DocumentosRequeridosGrupo.SelectMany(dr => dr.InscricaoDocumentos.Where(i => i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente).Select(w => new { w.ExibirObservacaoParaInscrito, w.Observacao, dr.DescricaoTipoDocumento }))).ToList();

                //Caso existam as tags configuracas e existam documentos ou grupos indeferidos ou pendentes, envia a notificação
                if (
                    notificacao.Mensagem.Contains(TAGS_NOTIFICACAO.DSC_LISTA_DOCUMENTACAO_INDEFERIDA) && (documentosObrigatoriosIndeferidos.Any() || gruposDocumentosIndeferidos.Any())
                    ||
                    notificacao.Mensagem.Contains(TAGS_NOTIFICACAO.DSC_LISTA_DOCUMENTACAO_PENDENTE) && (documentosObrigatoriosPendentes.Any() || gruposDocumentosPendentes.Any())
                   )
                {
                    // Busca as informações para mesclagem de dados
                    Dictionary<string, string> dados = new Dictionary<string, string>();

                    //Busca os nomes dos documentos ou grupos indeferidos
                    if (documentosObrigatoriosIndeferidos.Any() || gruposDocumentosIndeferidos.Any())
                    {
                        var descricaoDocumentosObrigatoriosIndeferidos = "<ul>";

                        foreach (var item in documentosObrigatoriosIndeferidos)
                        {
                            descricaoDocumentosObrigatoriosIndeferidos += item.ExibirObservacaoParaInscrito ? $"<li>{item.DescricaoTipoDocumento} ({item.Observacao})</li>" : $"<li>{item.DescricaoTipoDocumento}</li>";
                        }

                        foreach (var item in gruposDocumentosIndeferidos)
                        {
                            descricaoDocumentosObrigatoriosIndeferidos += item.ExibirObservacaoParaInscrito ? $"<li>{item.DescricaoTipoDocumento} ({item.Observacao})</li>" : $"<li>{item.DescricaoTipoDocumento}</li>";
                        }

                        descricaoDocumentosObrigatoriosIndeferidos += "</ul>";

                        dados.Add(TAGS_NOTIFICACAO.DSC_LISTA_DOCUMENTACAO_INDEFERIDA, descricaoDocumentosObrigatoriosIndeferidos);
                    }
                    else
                    {
                        dados.Add(TAGS_NOTIFICACAO.DSC_LISTA_DOCUMENTACAO_INDEFERIDA, string.Empty);
                    }

                    //Busca os nomes dos documentos ou grupos Pendentes
                    if (documentosObrigatoriosPendentes.Any() || gruposDocumentosPendentes.Any())
                    {
                        var descricaoDocumentosObrigatoriosPendentes = "<ul>";

                        foreach (var item in documentosObrigatoriosPendentes)
                        {
                            descricaoDocumentosObrigatoriosPendentes += item.ExibirObservacaoParaInscrito ? $"<li>{item.DescricaoTipoDocumento} ({item.Observacao})</li>" : $"<li>{item.DescricaoTipoDocumento}</li>";
                        }

                        foreach (var item in gruposDocumentosPendentes)
                        {
                            descricaoDocumentosObrigatoriosPendentes += item.ExibirObservacaoParaInscrito ? $"<li>{item.DescricaoTipoDocumento} ({item.Observacao})</li>" : $"<li>{item.DescricaoTipoDocumento}</li>";
                        }

                        descricaoDocumentosObrigatoriosPendentes += "</ul>";

                        dados.Add(TAGS_NOTIFICACAO.DSC_LISTA_DOCUMENTACAO_PENDENTE, descricaoDocumentosObrigatoriosPendentes);
                    }
                    else
                    {
                        dados.Add(TAGS_NOTIFICACAO.DSC_LISTA_DOCUMENTACAO_PENDENTE, string.Empty);
                    }

                    // Busca a configuração de notificação para finalizar a inscrição no idioma da mesma
                    var specNotificacao = new ProcessoConfiguracaoNotificacaoIdiomaFilterSpecification()
                    {
                        SeqProcesso = inscricao.SeqProcesso,
                        Idioma = inscricao.Idioma,
                        TokenTipoNotificacao = TOKENS.NOTIFICACAO_NOVA_ENTREGA_DOCUMENTACAO
                    };
                    var configNotificacaoIdioma = ProcessoConfiguracaoNotificacaoIdiomaDomainService.SearchByKey(specNotificacao, IncludesProcessoConfiguracaoNotificacaoIdioma.ProcessoConfiguracaoNotificacao);

                    // Se não tem notificação para enviar, ou a configuração não está configurada com envio automático, retorna
                    if (configNotificacaoIdioma == null || !configNotificacaoIdioma.ProcessoConfiguracaoNotificacao.EnvioAutomatico)
                        return;

                    // Busca os dados do inscrito
                    var specInscrito = new SMCSeqSpecification<Inscrito>(inscricao.SeqInscrito);
                    var inscrito = InscritoDomainService.SearchByKey(specInscrito);

                    // Busca as informações da configuração de etapa
                    var specConfig = new SMCSeqSpecification<ConfiguracaoEtapa>(inscricao.SeqConfiguracaoEtapa);
                    var includesConfig = IncludesConfiguracaoEtapa.EtapaProcesso |
                                         IncludesConfiguracaoEtapa.EtapaProcesso_Processo;
                    var configEtapa = ConfiguracaoEtapaDomainService.SearchByKey(specConfig, includesConfig);

                    // Busca as informações da unidade responsável pelo processo
                    var specUnidade = new SMCSeqSpecification<UnidadeResponsavel>(configEtapa.EtapaProcesso.Processo.SeqUnidadeResponsavel);
                    var includesUnidade = IncludesUnidadeResponsavel.Enderecos |
                                          IncludesUnidadeResponsavel.EnderecosEletronicos |
                                          IncludesUnidadeResponsavel.Telefones;
                    var unidade = UnidadeResponsavelDomainService.SearchByKey(specUnidade, includesUnidade);

                    // Busca as informações para merge
                    dados.Add(TAGS_NOTIFICACAO.NOME_SOCIAL_INSCRITO, !string.IsNullOrEmpty(inscrito.NomeSocial) ? inscrito.NomeSocial.Trim() : inscrito.Nome.Trim());
                    dados.Add(TAGS_NOTIFICACAO.DSC_PROCESSO, inscricao.DescricaoProcesso);

                    // Busca as Ofertas
                    var ofertas = InscricaoDomainService.SearchProjectionByKey(inscricao.SeqInscricao, x => x.Ofertas.Select(o => new
                    {
                        o.NumeroOpcao,
                        o.SeqOferta
                    })).ToList();

                    // Oferta
                    if (ofertas != null && ofertas.Count > 0)
                    {
                        string dscOfertas = string.Empty;
                        bool virgula = false;
                        foreach (var oferta in ofertas.OrderBy(o => o.NumeroOpcao))
                        {
                            if (virgula)
                                dscOfertas += ", ";
                            dscOfertas += OfertaDomainService.BuscarHierarquiaOfertaCompleta(oferta.SeqOferta,false).DescricaoCompleta;
                            virgula = true;
                        }
                        dados.Add(TAGS_NOTIFICACAO.DESCRICAO_OFERTAS, dscOfertas);
                    }

                    // Data Prazo Nova Entrega Documentacao
                    dados.Add(TAGS_NOTIFICACAO.DAT_PRAZO_NOVA_ENTREGA_DOCUMENTACAO, inscricao.DataPrazoNovaEntregaDocumentacao.HasValue ? inscricao.DataPrazoNovaEntregaDocumentacao.Value.ToString("dd/MM/yyyy") : string.Empty);

                    // Unidade Responsavel
                    dados.Add(TAGS_NOTIFICACAO.NOME_UNIDADE_RESPONSAVEL, unidade.Nome.Trim());

                    Endereco endComercial = unidade.Enderecos.Where(e => e.TipoEndereco == TipoEndereco.Comercial).FirstOrDefault();

                    if (endComercial != null)
                        dados.Add(TAGS_NOTIFICACAO.ENDERECO_UNIDADE_RESPONSAVEL, endComercial.FormatarParaImpressao());

                    string telefones = "";
                    foreach (var tel in unidade.Telefones)
                    {
                        telefones += tel.FormatarParaImpressao() + ", ";
                    }

                    telefones = telefones.Substring(0, telefones.Length - 2);

                    if (!String.IsNullOrEmpty(telefones))
                        dados.Add(TAGS_NOTIFICACAO.TELEFONE_UNIDADE_RESPONSAVEL, telefones);

                    EnderecoEletronico email = unidade.EnderecosEletronicos.Where(e => e.TipoEnderecoEletronico == TipoEnderecoEletronico.Email).FirstOrDefault();

                    if (email != null)
                        dados.Add(TAGS_NOTIFICACAO.EMAIL_UNIDADE_RESPONSAVEL, email.Descricao.Trim());

                    // Envia a notificação para o email do inscrito
                    var destinatario = new NotificacaoEmailDestinatarioData() { EmailDestinatario = inscrito.Email };

                    var seqLayouMensagemEmail = ProcessoDomainService.BuscarSeqLayouMensagemEmailPorProcesso(inscricao.SeqProcesso);

                    this.InscricaoEnvioNotificacaoDomainService.SalvarInscricaoEnvioNotificacao(inscricao.SeqInscricao, configNotificacaoIdioma, dados, destinatario, seqLayouMensagemEmail);
                }
            }
        }

        private void ValidarNotificacaoEntregaDocumentacao(SumarioDocumentosEntreguesVO documentosEntregues)
        {
            /* Se a notificação de "Nova entrega da documentação" estiver configurada no processo, ou se a configuração da etapa associada à inscrição estiver
             * parametrizada para permitir nova entrega da documentação, e existir documento obrigatório, ou documento de grupo de documento, indeferido ou
             * pendente e o campo "Prazo para nova entrega da documentação" não estiver preenchido, abortar a operação e emitir a mensagem de erro:
             * "O prazo para nova entrega da documentação deve ser informado."*/

            var specInscricao = new SMCSeqSpecification<Inscricao>(documentosEntregues.SeqInscricao);

            var inscricao = InscricaoDomainService.SearchProjectionByKey(specInscricao, i => new
            {
                ExisteNotificacaoConfiguracaoProcesso = i.Processo.ConfiguracoesNotificacao.Any(c => c.TipoNotificacao.Token == TOKENS.NOTIFICACAO_NOVA_ENTREGA_DOCUMENTACAO),
                PermiteNovaEntregaDocumentacao = i.ConfiguracaoEtapa.PermiteNovaEntregaDocumentacao
            });

            var existeDocumentoObrigatorioIndeferidoOuPendente = documentosEntregues.DocumentosObrigatorios.Any(d => d.InscricaoDocumentos.Any(i => i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Indeferido || i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente));

            var existeGrupoDocumentoIndeferidoOuPendente = documentosEntregues.GruposDocumentos.Any(g => g.DocumentosRequeridosGrupo.Any(d => d.InscricaoDocumentos.Any(i => i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Indeferido || i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente)));

            if ((inscricao.ExisteNotificacaoConfiguracaoProcesso || inscricao.PermiteNovaEntregaDocumentacao) && (existeDocumentoObrigatorioIndeferidoOuPendente || existeGrupoDocumentoIndeferidoOuPendente) && !documentosEntregues.DataPrazoNovaEntregaDocumentacao.HasValue)
                throw new EntregaDocumentoNotificacaoSemDataPrazoNovaEntregaDocumentacaoException();
        }

        private void AtualizarInscricaoDocumentacaoEntrega(long seqInscricao, bool documentacaoEntregue)
        {
            var inscricao = this.InscricaoDomainService.SearchByKey(new SMCSeqSpecification<Inscricao>(seqInscricao));
            inscricao.DocumentacaoEntregue = documentacaoEntregue;
            InscricaoDomainService.UpdateEntity(inscricao);
        }

        private void AtualizarDataPrazoNovaEntregaDocumentacao(long seqInscricao, DateTime? dataPrazoNovaEntregaDocumentacao)
        {
            var inscricao = this.InscricaoDomainService.SearchByKey(new SMCSeqSpecification<Inscricao>(seqInscricao));
            inscricao.DataPrazoNovaEntregaDocumentacao = dataPrazoNovaEntregaDocumentacao;
            InscricaoDomainService.UpdateEntity(inscricao);
        }


        private void VerificaExtensoesDoArquivo(string nomeArquivo, string tipoArquivo, string ExtensoesAceita)
        {

            string[] splitTipoArquivo = tipoArquivo.Split('/');
           
            var extensao = Path.GetExtension(nomeArquivo.ToLower());

            if (!string.IsNullOrEmpty(extensao))
            {
                var tipagemArquivo = "." + splitTipoArquivo[1].ToLower();

                if (!ExtensoesAceita.Contains(extensao))
                {

                    throw new ExtensaoDocumentoNovaDocumentacaoException(nomeArquivo, ExtensoesAceita);

                }

                if ((extensao == "xlsx" || extensao == "docx") && !ExtensoesOffice.Contains(tipagemArquivo))
                {
                    throw new ExtensaoDocumentoNovaDocumentacaoException(nomeArquivo, ExtensoesAceita);
                }
            }
        }
        private void PercorreDocumentosParaValidarExtensoes(List<DocumentoRequeridoVO> documentosObrigatorios, List<DocumentoRequeridoVO> documentosOpcionais, string extensoesValidas)
        {
            if (documentosObrigatorios != null)
            {
                foreach (var documentoObrigatorio in documentosObrigatorios)
                {
                    foreach (var documento in documentoObrigatorio.InscricaoDocumentos)
                    {
                        if (documento.ArquivoAnexado != null)
                            VerificaExtensoesDoArquivo(documento.ArquivoAnexado.Name, documento.ArquivoAnexado.Type, extensoesValidas);
                    }
                }
            }

            if (documentosOpcionais != null)
            {
                foreach (var documentoOpcional in documentosOpcionais)
                {
                    foreach (var documento in documentoOpcional.InscricaoDocumentos)
                    {
                        if (documento.ArquivoAnexado != null)
                            VerificaExtensoesDoArquivo(documento.ArquivoAnexado.Name, documento.ArquivoAnexado.Type, extensoesValidas);
                    }
                }
            }
        }


        private void SalvarDocumentosRequeridos(SumarioDocumentosEntreguesVO documentosEntregues)
        {

            if (ValidarConverterPDF(documentosEntregues.SeqInscricao))
            {
                PercorreDocumentosParaValidarExtensoes(documentosEntregues.DocumentosObrigatorios, documentosEntregues.DocumentosOpcionais, ExtensoesSemConversaoPDF);
            }
            else
            {
                PercorreDocumentosParaValidarExtensoes(documentosEntregues.DocumentosObrigatorios, documentosEntregues.DocumentosOpcionais, ExtensoesConversaoPDF);
            }

            var todosDocumentos = documentosEntregues.DocumentosObrigatorios.ToList();
            todosDocumentos.AddRange(documentosEntregues.DocumentosOpcionais.ToList());
            todosDocumentos.AddRange(documentosEntregues.GruposDocumentos.SelectMany(x => x.DocumentosRequeridosGrupo).ToList());

            // Adiciona o SeqInscricao no documento novo
            todosDocumentos.SelectMany(t => t.InscricaoDocumentos).Where(i => i.Seq == 0 && i.SeqInscricao == 0).SMCForEach(d =>
            {
                d.SeqInscricao = documentosEntregues.SeqInscricao;
            });

            foreach (var documentoRequerido in todosDocumentos)
            {
                SalvarInscricaoDocumentoEntregue(documentoRequerido.Seq, documentosEntregues.SeqInscricao, documentoRequerido.InscricaoDocumentos.TransformList<InscricaoDocumento>());
            }
        }

        /// <summary>
        /// Salva o documento Inscrição entregue
        /// </summary>
        /// <param name="seqInscricao"></param>
        /// <param name="inscricaoDocumentos"></param>
        private void SalvarInscricaoDocumentoEntregue(long seqDocumentoRequerido, long seqInscricao, List<InscricaoDocumento> inscricaoDocumentos)
        {
            // Verifica se todas as InscricaoDocumento recebidas como parametro são de uma mesma inscrição
            ValidarInscricaoDocumentosMesmaInscricao(seqInscricao, inscricaoDocumentos);

            // Se existem documentos do mesmo tipo, verifica se o tipo do documento permite vários arquivos
            ValidarDocumentosMesmoTipoPermitemVarios(inscricaoDocumentos, true);

            // Busca os documentos da inscrição já cadastrados em banco de dados
            var specDoc = new InscricaoDocumentoFilterSpecification() { SeqDocumentoRequerido = seqDocumentoRequerido, SeqInscricao = seqInscricao };

            var listaDocBase = this.SearchBySpecification(specDoc, IncludesInscricaoDocumento.ArquivoAnexado).ToList();

            foreach (var docAlterado in inscricaoDocumentos)
            {
                // Verifica se o documento foi alterado ou não existe
                var docBase = listaDocBase.Where(o => o.Seq == docAlterado.Seq).FirstOrDefault();
                if (docBase != null)
                {
                    if (docAlterado.VersaoDocumento == VersaoDocumento.Nenhum) { docAlterado.VersaoDocumento = null; }
                    if (docBase.VersaoDocumento == VersaoDocumento.Nenhum) { docBase.VersaoDocumento = null; }
                    if (docAlterado.FormaEntregaDocumento == FormaEntregaDocumento.Nenhum) { docAlterado.FormaEntregaDocumento = null; }
                    if (docBase.FormaEntregaDocumento == FormaEntregaDocumento.Nenhum) { docBase.FormaEntregaDocumento = null; }

                    if (docAlterado.ArquivoAnexado == null && docBase.ArquivoAnexado != null
                        || (docAlterado.Observacao != docBase.Observacao)
                        || (docAlterado.ExibirObservacaoParaInscrito != docBase.ExibirObservacaoParaInscrito)
                        || (docAlterado.SituacaoEntregaDocumento != docBase.SituacaoEntregaDocumento)
                        || (docAlterado.VersaoDocumento != docBase.VersaoDocumento)
                        || (docAlterado.DataEntrega != docBase.DataEntrega)
                        || (docAlterado.DataPrazoEntrega != docBase.DataPrazoEntrega)
                        || (docAlterado.FormaEntregaDocumento != docBase.FormaEntregaDocumento)
                        || (docAlterado.ArquivoAnexado != null && docAlterado.ArquivoAnexado.State == SMCUploadFileState.Changed))
                    {
                        ValidarVinculoDocumentoAnexado(docAlterado);

                        if (docAlterado.ArquivoAnexado == null)
                        {
                            if (docAlterado.SeqArquivoAnexado.HasValue)
                            {
                                var seqArquivoAnexado = docAlterado.SeqArquivoAnexado.Value;
                                docAlterado.ArquivoAnexado = null;
                                docAlterado.SeqArquivoAnexado = null;
                                docAlterado.DescricaoArquivoAnexado = null;
                                this.UpdateEntity(docAlterado);
                                ArquivoAnexadoDomainService.DeleteEntity(seqArquivoAnexado);
                                var guidArquivoGED = docBase.ArquivoAnexado.UidArquivoGed.HasValue ? docBase.ArquivoAnexado.UidArquivoGed.ToString() : null;
                                if (guidArquivoGED != null)
                                {
                                    arquivosDeletadosGED.Add(guidArquivoGED);
                                }
                            }
                        }

                        //Caso o documento tenha sido tido sua situação alterada de pendente e ele tenha uma entrega posterior marcada passará a receber false
                        if (docAlterado.SituacaoEntregaDocumento != SituacaoEntregaDocumento.Pendente && docAlterado.EntregaPosterior)
                            docAlterado.EntregaPosterior = false;

                        if (!string.IsNullOrEmpty(docBase.ArquivoAnexado?.UrlDownloadGed) && docAlterado.ArquivoAnexado != null)
                        {
                            docAlterado.ArquivoAnexado.UidArquivoGed = docBase.ArquivoAnexado.UidArquivoGed;
                            docAlterado.ArquivoAnexado.UrlDownloadGed = docBase.ArquivoAnexado.UrlDownloadGed;
                            docAlterado.ArquivoAnexado.UrlPrivadaGed = docBase.ArquivoAnexado.UrlPrivadaGed;
                            docAlterado.ArquivoAnexado.UrlPublicaGed = docBase.ArquivoAnexado.UrlPublicaGed;
                            if (docAlterado.ArquivoAnexado.State == SMCUploadFileState.Changed)
                            {
                                arquivosAtualizarGED.Add(docAlterado.Seq);
                            }

                            arquivosAtualizarMetadadosGED.Add(docAlterado.Seq);

                        }
                        else if (string.IsNullOrEmpty(docBase.ArquivoAnexado?.UrlDownloadGed) && docAlterado.ArquivoAnexado != null)
                        {
                            arquivosCriadosGED.Add(docAlterado.Seq);
                        }

                        this.UpdateEntity(docAlterado);

                    }
                    listaDocBase.Remove(docBase);
                }
                else
                {
                    this.InsertEntity(docAlterado);
                    if (docAlterado.ArquivoAnexado != null)
                    {
                        arquivosCriadosGED.Add(docAlterado.Seq);
                    }
                }
            }
            // Exclusão dos documentos, que foram excluídos da tela
            foreach (var docExcluido in listaDocBase)
            {
                // Exclusão permitida apenas para Documentos requeridos com mais de
                // uma inscrição documento vinculada
                if (inscricaoDocumentos.SMCAny())
                {
                    var guidArquivoGED = docExcluido.ArquivoAnexado.UidArquivoGed.HasValue ? docExcluido.ArquivoAnexado.UidArquivoGed.ToString() : null;
                    this.DeleteEntity(docExcluido);
                    if (guidArquivoGED != null)
                    {
                        arquivosDeletadosGED.Add(guidArquivoGED);
                    }
                }
            }
        }

        /// <summary>
        /// Valida se existem documentos do mesmo tipo, verifica se o tipo do documento permite vários arquivos
        /// </summary>
        /// <param name="inscricaoDocumentos"></param>
        /// <param name="validarSeIncluirNovos">Valida apenas se incluir novos documentos</param>
        private void ValidarDocumentosMesmoTipoPermitemVarios(List<InscricaoDocumento> inscricaoDocumentos, bool validarSeIncluirNovos)
        {
            foreach (var doc in inscricaoDocumentos.GroupBy(d => d.SeqDocumentoRequerido).Where(g => g.Count() > 1).ToList())
            {
                SMCSeqSpecification<DocumentoRequerido> spec = new SMCSeqSpecification<DocumentoRequerido>(doc.Key);
                var tipoDoc = this.DocumentoRequeridoDomainService.SearchProjectionByKey(spec, d => new
                {
                    Seq = d.SeqTipoDocumento,
                    PermiteVariosArquivos = d.PermiteVarios,
                    Descricao = d.TipoDocumento.Descricao
                });

                if (!tipoDoc.PermiteVariosArquivos)
                    throw new DocumentoRequeridoNaoPermiteVariosArquivosException(tipoDoc.Descricao);
                else if ((!validarSeIncluirNovos || (validarSeIncluirNovos && doc.Any(d => d.Seq == 0))) &&
                         doc.Count() > 10)
                    throw new NumeroDeDocumentosExcedidoException(tipoDoc.Descricao);
            }
        }

        public bool ValidarSituacaoAtualCandidatoOfertasConfirmadas(SumarioDocumentosEntreguesVO documentosEntregues)
        {
            var includes = IncludesInscricao.Ofertas | IncludesInscricao.Ofertas_Oferta | IncludesInscricao.Processo
                         | IncludesInscricao.HistoricosSituacao_TipoProcessoSituacao
                         | IncludesInscricao.Ofertas_HistoricosSituacao_TipoProcessoSituacao;
            var inscricao = this.InscricaoDomainService.SearchByKey(new SMCSeqSpecification<Inscricao>(documentosEntregues.SeqInscricao), includes);

            var inscricaoDocumentacaoEntregueBase = inscricao.DocumentacaoEntregue;

            var situacaoDocumentacaoAssert = ValidacaoAssert_DocumentosObrigatoriosEGruposDocumentos(documentosEntregues);

            /*2. Caso a situação da documentação da inscrição seja “Entregue”, ou “Entregue com pendência”, atualizar o campo
                 “documentação entregue” da inscrição com o valor “Sim”, caso contrário, atualizar com o valor “Não”.*/
            inscricao.DocumentacaoEntregue = (situacaoDocumentacaoAssert == SituacaoDocumentacao.Entregue ||
                                              situacaoDocumentacaoAssert == SituacaoDocumentacao.EntregueComPendencia);

            /* 2.2. Se o campo “documentação entregue” estiver sendo alterado de “Sim” para “Não”:*/
            if (inscricaoDocumentacaoEntregueBase && !inscricao.DocumentacaoEntregue)
            {
                /* 2.2.1. Verificar se o candidato está com a situação atual da inscrição como INSCRICAO_CONFIRMADA e, caso possua
                             histórico de situação da inscrição oferta, a situação atual de todas as inscrições-oferta seja CANDIDATO_CONFIRMADO. */

                return VerificarSituacaoAtualCandidatoOfertas(inscricao);
                /* Em caso afirmativo, emitir a mensagem de confirmação abaixo:

                  "A inscrição deste candidato já foi confirmada. Ao realizar essa alteração, a entrega da documentação será desfeita
                  e o candidato voltará para a situação anterior. Caso tenha sido configurada alguma notificação de retorno de situação,
                  ele será comunicado deste fato. Deseja continuar?"

                  2.2.1.1. Caso o usuário clique em "Não", abortar a operação.
                  2.2.1.2. Caso o usuário clique em sim, voltar o candidato para a situação anterior e passar o indicador de atual e
                  de atual etapa para "Não" no histórico de situação da inscrição oferta.*/
            }
            return false;
        }

        public bool ValidarSituacaoAtualCandidatoOfertasDeferidas(SumarioDocumentosEntreguesVO documentosEntregues)
        {
            var includes = IncludesInscricao.Ofertas | IncludesInscricao.Ofertas_Oferta | IncludesInscricao.Processo
                         | IncludesInscricao.HistoricosSituacao_TipoProcessoSituacao
                         | IncludesInscricao.Ofertas_HistoricosSituacao_TipoProcessoSituacao;
            var inscricao = this.InscricaoDomainService.SearchByKey(new SMCSeqSpecification<Inscricao>(documentosEntregues.SeqInscricao), includes);

            var inscricaoDocumentacaoEntregueBase = inscricao.DocumentacaoEntregue;

            var situacaoDocumentacaoAssert = ValidacaoAssert_DocumentosObrigatoriosEGruposDocumentos(documentosEntregues);
            /*2. Caso a situação da documentação da inscrição seja “Entregue”, ou “Entregue com pendência”, atualizar o campo
                 “documentação entregue” da inscrição com o valor “Sim”, caso contrário, atualizar com o valor “Não”.*/
            var documentacaoEntregueAlterada = (situacaoDocumentacaoAssert == SituacaoDocumentacao.Entregue ||
                                              situacaoDocumentacaoAssert == SituacaoDocumentacao.EntregueComPendencia);

            /* 2.2. Se o campo “documentação entregue” estiver sendo alterado de “Sim” para “Não”:*/
            if (inscricaoDocumentacaoEntregueBase && !documentacaoEntregueAlterada)
            {
                /*2.2.2. Se a situação atual da inscrição do candidato for INSCRICAO_DEFERIDA, ou for INSCRICAO_CONFIRMADA e
                                 existir histórico de situação da inscrição oferta cuja situação atual não é CANIDATO_CONFIRMADO, emitir a mensagem de confirmação abaixo:

                         “Ao realizar essa alteração, a entrega da documentação será desfeita, gerando uma inconsistência em relação à
                         situação da inscrição, que só pôde ser confirmada, pelo fato de estar entregue. Deseja continuar?"

                         2.2.2.1. Caso o usuário clique em “Não”, abortar a operação.
                         2.2.2.2. Caso o usuário clique em “Sim”, prosseguir com a operação.*/

                return VerificarSituacaoAltualCandidatoOfertaNaoConfirmado(inscricao);
            }
            return false;
        }

        private void ValidarDocumentosObrigatoriosEGruposDocumentos(SumarioDocumentosEntreguesVO documentosEntregues)
        {
            // Task 38008
            // Verificar se enviou mais de 10 arquivos por tipo de documento que permite vários
            var tiposPermiteVarios = new List<(long SeqTipoDocumento, string DescricaoDocumento)>();
            var permiteVariosObrigatorio = documentosEntregues.DocumentosObrigatorios.Where(d => d.PermiteVarios).Select(d => (d.SeqTipoDocumento, d.DescricaoTipoDocumento));
            var permiteVariosOpcionais = documentosEntregues.DocumentosOpcionais.Where(d => d.PermiteVarios).Select(d => (d.SeqTipoDocumento, d.DescricaoTipoDocumento));
            var permiteVariosGrupos = documentosEntregues.GruposDocumentos.SelectMany(g => g.DocumentosRequeridosGrupo).Where(d => d.PermiteVarios).Select(d => (d.SeqTipoDocumento, d.DescricaoTipoDocumento));

            if (permiteVariosObrigatorio != null && permiteVariosObrigatorio.Any())
                tiposPermiteVarios.AddRange(permiteVariosObrigatorio);

            if (permiteVariosOpcionais != null && permiteVariosOpcionais.Any())
                tiposPermiteVarios.AddRange(permiteVariosOpcionais);

            if (permiteVariosGrupos != null && permiteVariosGrupos.Any())
                tiposPermiteVarios.AddRange(permiteVariosGrupos);

            tiposPermiteVarios = tiposPermiteVarios.Distinct().ToList();
            if (tiposPermiteVarios.Any())
            {
                foreach (var item in tiposPermiteVarios)
                {
                    // Verifica o numero de arquivos enviados para cada tipo
                    int qtdEnviado = documentosEntregues.DocumentosObrigatorios.Where(d => d.SeqTipoDocumento == item.SeqTipoDocumento).SelectMany(d => d.InscricaoDocumentos).Count();
                    qtdEnviado += documentosEntregues.DocumentosOpcionais.Where(d => d.SeqTipoDocumento == item.SeqTipoDocumento).SelectMany(d => d.InscricaoDocumentos).Count();
                    qtdEnviado += documentosEntregues.GruposDocumentos.SelectMany(g => g.DocumentosRequeridosGrupo).Where(d => d.SeqTipoDocumento == item.SeqTipoDocumento).SelectMany(d => d.InscricaoDocumentos).Count();

                    bool algumNovoAdicionado = false;
                    algumNovoAdicionado = documentosEntregues.DocumentosObrigatorios.Where(d => d.SeqTipoDocumento == item.SeqTipoDocumento).SelectMany(d => d.InscricaoDocumentos).Any(d => d.Seq == 0);
                    algumNovoAdicionado = algumNovoAdicionado || documentosEntregues.DocumentosOpcionais.Where(d => d.SeqTipoDocumento == item.SeqTipoDocumento).SelectMany(d => d.InscricaoDocumentos).Any(d => d.Seq == 0);
                    algumNovoAdicionado = algumNovoAdicionado || documentosEntregues.GruposDocumentos.SelectMany(g => g.DocumentosRequeridosGrupo).Where(d => d.SeqTipoDocumento == item.SeqTipoDocumento).SelectMany(d => d.InscricaoDocumentos).Any(d => d.Seq == 0);

                    if (algumNovoAdicionado && qtdEnviado > 10)
                        throw new NumeroDeDocumentosExcedidoException(item.DescricaoDocumento);
                }
            }

            // Task 37625
            /*
                Ao clicar em  Salvar, executar a regra "RN_INS_161 - Consistência da quantidade de arquivos anexados em um grupo", que diz:
                Se a situação atual da inscrição for "Inscrição iniciada", verificar se existe algum grupo de documentos com mais arquivos anexados que a quantidade mínima.
                Em caso afirmativo, abortar a operação e emitir a mensagem de erro abaixo:
                "Não é permitido anexar mais de <quantidade mínima de itens> para o grupo <descrição do grupo>, enquanto a inscrição do candidato não for finalizada."
            */

            var tokenSituacaoAtual = InscricaoDomainService.SearchProjectionByKey(documentosEntregues.SeqInscricao, x => x.HistoricosSituacao.FirstOrDefault(h => h.Atual).TipoProcessoSituacao.Token);
            if (tokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_INICIADA && documentosEntregues.GruposDocumentos != null)
            {
                // Verificar se existe mais arquivos enviados para um grupo do que o mínimo necessário.
                foreach (var item in documentosEntregues.GruposDocumentos)
                {
                    if (item.MinimoObrigatorio > 0)
                    {
                        var totalEnviadoGrupo = item.DocumentosRequeridosGrupo.SelectMany(d => d.InscricaoDocumentos).Count(d => d.ArquivoAnexado != null);
                        if (totalEnviadoGrupo > item.MinimoObrigatorio)
                            throw new NumeroAnexosGrupoMinimoUltrapassadoInscricaoIniciadaException(item.MinimoObrigatorio, item.Descricao);
                    }
                }
            }

            // Caso não tenha documentação requerida, retorna sempre não requerido
            var seqConfiguracaoEtapa = InscricaoDomainService.SearchProjectionByKey(documentosEntregues.SeqInscricao, x => x.SeqConfiguracaoEtapa);
            var possuiDocumentacaoRequerida = InscricaoDomainService.PossuiDocumentoRequerido(seqConfiguracaoEtapa);

            // 1.2.2.2.2.Se não estiver, caso não exista lista de documentos requeridos para a configuração do grupo de ofertas da
            //   inscrição, salvar a situação da documentação com o valor "Não requerida".
            if (!possuiDocumentacaoRequerida)
            {
                AtualizarInscricaoSituacaoDocumentacao(documentosEntregues.SeqInscricao, SituacaoDocumentacao.NaoRequerida);
            }
            else
            {
                /*Observação: documentos que são obrigatórios para um sexo específico, deverão ser verificados somente para este sexo.*/
                //1.Verificar se todos os documentos obrigatórios e se os grupos de documentos obrigatórios estão com o
                //    número mínimo de documentos em uma das situações: "Deferido" ou "Aguardando análise do setor responsável".
                if (ValidarDocumentosDeferidoOuAguardandoAnaliseSetorResponsavel(documentosEntregues.DocumentosObrigatorios, documentosEntregues.GruposDocumentos))
                {
                    //  1.1.Se estiverem, salvar a situação da documentação da inscrição com o valor "Entregue";
                    AtualizarInscricaoSituacaoDocumentacao(documentosEntregues.SeqInscricao, SituacaoDocumentacao.Entregue);
                }
                else
                {
                    // 1.2.Se não estiver, verificar se estão com as situações "Deferido", "Aguardando análise do setor responsável" e "Pendente";
                    // (Tem que ter uma das três, mas pelo menos um pendente)
                    if (ValidarDocumentosDeferidoOuAguardandoAnaliseSetorResponsavelPendente(documentosEntregues.DocumentosObrigatorios, documentosEntregues.GruposDocumentos))
                    {
                        //  1.2.1.Se estiverem, salvar a situação da documentação da inscrição com valor "Entregue com pendência";
                        AtualizarInscricaoSituacaoDocumentacao(documentosEntregues.SeqInscricao, SituacaoDocumentacao.EntregueComPendencia);
                    }
                    else
                    {
                        // 1.2.2.Se não estiverem, verificar se pelo menos um está com a situação "Aguardando entrega" ou "Indeferido".
                        //      No caso do grupo, além de verificar as situações citadas, não poderá existir nenhum documento com a situação "Em validação";
                        if (ValidarDocumentosPossuiAlgumAguardandoEntregaOuIndeferido(documentosEntregues.DocumentosObrigatorios, documentosEntregues.GruposDocumentos))
                        {
                            // 1.2.2.1.Se estiver, salvar a situação da documentação da inscrição com valor "Aguardando (nova) entrega";
                            AtualizarInscricaoSituacaoDocumentacao(documentosEntregues.SeqInscricao, SituacaoDocumentacao.AguardandoEntrega);
                        }
                        else
                        {
                            // 1.2.2.2.Se não estiverem, verificar se pelo menos um está com a situação "Aguardando Validação";
                            if (ValidarDocumentosPossuiAlgumAguardandoValidacao(documentosEntregues.DocumentosObrigatorios, documentosEntregues.GruposDocumentos))
                            {
                                // 1.2.2.2.1.Se estiver, salvar a situação da documentação da inscrição com valor "Aguardando validação";
                                AtualizarInscricaoSituacaoDocumentacao(documentosEntregues.SeqInscricao, SituacaoDocumentacao.AguardandoValidacao);
                            }
                            else
                            {
                                // 1.2.2.2.2.Se não estiver, caso não exista lista de documentos requeridos para a configuração do grupo de ofertas da
                                //   inscrição, salvar a situação da documentação com o valor "Não requerida".
                                var existe = this.InscricaoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(documentosEntregues.SeqInscricao), x => x.GrupoOferta.ConfiguracoesEtapa.Any(g => g.ConfiguracaoEtapa.DocumentosRequeridos.Any()));
                                if (!existe)
                                {
                                    AtualizarInscricaoSituacaoDocumentacao(documentosEntregues.SeqInscricao, SituacaoDocumentacao.NaoRequerida);
                                }
                            }
                        }
                    }
                }
            }
        }

        private SituacaoDocumentacao ValidacaoAssert_DocumentosObrigatoriosEGruposDocumentos(SumarioDocumentosEntreguesVO documentosEntregues)
        {
            /*Observação: documentos que são obrigatórios para um sexo específico, deverão ser verificados somente para este sexo.*/
            //1.Verificar se todos os documentos obrigatórios e se os grupos de documentos obrigatórios estão com o
            //    número mínimo de documentos em uma das situações: "Deferido" ou "Aguardando análise do setor responsável".
            if (ValidarDocumentosDeferidoOuAguardandoAnaliseSetorResponsavel(documentosEntregues.DocumentosObrigatorios, documentosEntregues.GruposDocumentos))
            {
                //  1.1.Se estiverem, salvar a situação da documentação da inscrição com o valor "Entregue";
                return SituacaoDocumentacao.Entregue;
            }
            else
            {
                // 1.2.Se não estiver, verificar se estão com as situações "Deferido", "Aguardando análise do setor responsável" e "Pendente";
                // (Tem que ter uma das três, mas pelo menos um pendente)
                if (ValidarDocumentosDeferidoOuAguardandoAnaliseSetorResponsavelPendente(documentosEntregues.DocumentosObrigatorios, documentosEntregues.GruposDocumentos))
                {
                    //  1.2.1.Se estiverem, salvar a situação da documentação da inscrição com valor "Entregue com pendência";
                    return SituacaoDocumentacao.EntregueComPendencia;
                }
                else
                {
                    // 1.2.2.Se não estiverem, verificar se pelo menos um está com a situação "Aguardando entrega" ou "Indeferido".
                    //      No caso do grupo, além de verificar as situações citadas, não poderá existir nenhum documento com a situação "Em validação";
                    if (ValidarDocumentosPossuiAlgumAguardandoEntregaOuIndeferido(documentosEntregues.DocumentosObrigatorios, documentosEntregues.GruposDocumentos))
                    {
                        // 1.2.2.1.Se estiver, salvar a situação da documentação da inscrição com valor "Aguardando (nova) entrega";
                        return SituacaoDocumentacao.AguardandoEntrega;
                    }
                    else
                    {
                        // 1.2.2.2.Se não estiverem, verificar se pelo menos um está com a situação "Aguardando Validação";
                        if (ValidarDocumentosPossuiAlgumAguardandoValidacao(documentosEntregues.DocumentosObrigatorios, documentosEntregues.GruposDocumentos))
                        {
                            // 1.2.2.2.1.Se estiver, salvar a situação da documentação da inscrição com valor "Aguardando validação";
                            return SituacaoDocumentacao.AguardandoValidacao;
                        }
                        else
                        {
                            // 1.2.2.2.2.Se não estiver, caso não exista lista de documentos requeridos para a configuração do grupo de ofertas da
                            //   inscrição, salvar a situação da documentação com o valor "Não requerida".
                            var existe = this.InscricaoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(documentosEntregues.SeqInscricao), x => x.GrupoOferta.ConfiguracoesEtapa.Any(g => g.ConfiguracaoEtapa.DocumentosRequeridos.Any()));
                            if (!existe)
                            {
                                return SituacaoDocumentacao.NaoRequerida;
                            }
                        }
                    }
                }
            }
            return SituacaoDocumentacao.Nenhum;
        }

        private void AtualizarSituacaoInscricao(long seqInscricao, long seqTipoProcesso, string tokenSituacaoInscricao)
        {
            if (HistoricoSituacaoInscricaoAtualIgual(seqInscricao, tokenSituacaoInscricao)) { return; }

            var seqTipoProcessoSituacao =
                          TipoProcessoSituacaoDomainService.SearchProjectionByKey(new TipoProcessoSituacaoFilterSpecification
                          {
                              SeqTipoProcesso = seqTipoProcesso,
                              Token = tokenSituacaoInscricao
                          }, x => x.Seq);

            // Cria um novo item de InscricaoHistoricoSituacao
            InscricaoHistoricoSituacaoDomainService.AlterarSituacaoInscricoes(new AlteracaoSituacaoVO()
            {
                SeqTipoProcessoSituacaoDestino = seqTipoProcessoSituacao,
                SeqInscricoes = new List<long> { seqInscricao }
            });
        }

        private bool HistoricoSituacaoInscricaoAtualIgual(long seqInscricao, string tokenSituacaoInscricao)
        {
            var include = IncludesInscricaoHistoricoSituacao.TipoProcessoSituacao;

            var situacaoAtualInscricao = InscricaoHistoricoSituacaoDomainService.BuscarSituacaoAtualInscricao(seqInscricao, include);

            if (situacaoAtualInscricao == null) { throw new InscricaoHistoricoSituacaoSemHistoricoAtualException(); }

            return situacaoAtualInscricao.TipoProcessoSituacao.Token == tokenSituacaoInscricao;
        }

        private bool ValidarDocumentosPossuiAlgumAguardandoValidacao(List<DocumentoRequeridoVO> documentosObrigatorios, List<GrupoDocumentoEntregueVO> gruposDocumentos)
        {
            var algumAguardandoValidacao = gruposDocumentos.Where(g => !GrupoDocumentosObrigatoriosDeferido(g)).SMCAny(g =>
                                               g.DocumentosRequeridosGrupo.SMCAny(d => d.InscricaoDocumentos.SMCAny(doc =>
                                                   doc.SituacaoEntregaDocumento == SituacaoEntregaDocumento.AguardandoValidacao)));

            if (algumAguardandoValidacao) { return true; }

            //verificar se pelo menos um documento está com a situação "Aguardando validação"
            var algumDocumentoAguardandoValidacao = documentosObrigatorios.SMCAny(x => x.InscricaoDocumentos
                                                     .SMCAny(d => d.SituacaoEntregaDocumento == SituacaoEntregaDocumento.AguardandoValidacao));

            if (algumDocumentoAguardandoValidacao) { return true; }

            return false;
        }

        /// <summary>
        /// verificar se pelo menos um documento está com a situação "Aguardando entrega" ou "Indeferido".
        /// No caso do grupo, além de verificar as situações citadas, não poderá existir nenhum documento com a situação "Em validação"
        /// </summary>
        /// <param name="documentosObrigatorios"></param>
        /// <param name="gruposDocumentos"></param>
        /// <returns></returns>
        private bool ValidarDocumentosPossuiAlgumAguardandoEntregaOuIndeferido(List<DocumentoRequeridoVO> documentosObrigatorios, List<GrupoDocumentoEntregueVO> gruposDocumentos)
        {
            // 1.2.2.Se não estiverem, verificar se pelo menos um documento está com a situação "Aguardando entrega" ou "Indeferido".
            //      No caso do grupo, além de verificar as situações citadas, não poderá existir nenhum documento com a situação "Em validação";
            var algumAguardandoValidacao = gruposDocumentos.Where(g => !GrupoDocumentosObrigatoriosDeferido(g)).SMCAny(g =>
                                                g.DocumentosRequeridosGrupo.SMCAny(d => d.InscricaoDocumentos.SMCAny(doc =>
                                                    doc.SituacaoEntregaDocumento == SituacaoEntregaDocumento.AguardandoValidacao)));

            ///não poderá existir nenhum documento com a situação "Em validação"
            if (algumAguardandoValidacao) { return false; }

            var situacoesEntregaDocumentos = new List<SituacaoEntregaDocumento>() {
                SituacaoEntregaDocumento.AguardandoEntrega,
                SituacaoEntregaDocumento.Indeferido
            };

            //verificar se pelo menos um documento está com a situação "Aguardando entrega" ou "Indeferido".
            var algumAguardandoEntregaOuIndeferido = documentosObrigatorios.SMCAny(x => x.InscricaoDocumentos
                                                     .SMCAny(d => situacoesEntregaDocumentos.Contains(d.SituacaoEntregaDocumento)));

            if (algumAguardandoEntregaOuIndeferido) { return true; }

            // verificar se pelo menos um documento está com a situação "Aguardando entrega" ou "Indeferido".
            var algumGrupoAguardandoEntregaOuIndeferido = gruposDocumentos.Where(g => !GrupoDocumentosObrigatoriosDeferido(g)).SMCAny(g =>
                                                g.DocumentosRequeridosGrupo.SMCAny(d => d.InscricaoDocumentos.TrueForAll(doc =>
                                                   situacoesEntregaDocumentos.Contains(doc.SituacaoEntregaDocumento))));

            if (algumGrupoAguardandoEntregaOuIndeferido) { return true; }

            return false;
        }

        private bool ValidarDocumentosDeferidoOuAguardandoAnaliseSetorResponsavelPendente(List<DocumentoRequeridoVO> documentosObrigatorios, List<GrupoDocumentoEntregueVO> gruposDocumentos)
        {
            //Verificar se todos os documentos obrigatórios estão com uma das situações:
            //"Deferido", "Aguardando análise do setor responsável" e "Pendente".
            // Tem que ter uma das três, mas pelo menos um pendente
            var algumGrupoPendente = gruposDocumentos.SMCAny(g =>
                                               g.DocumentosRequeridosGrupo.SMCAny(d => d.InscricaoDocumentos.SMCAny(doc =>
                                                   doc.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente)));

            //verificar se pelo menos um documento está com a situação "Pendente"
            var algumDocumentoPendente = documentosObrigatorios.SMCAny(x => x.InscricaoDocumentos
                                                     .SMCAny(d => d.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente));

            var situacoesEntregaDocumentos = new List<SituacaoEntregaDocumento>() {
                SituacaoEntregaDocumento.Deferido,
                SituacaoEntregaDocumento.AguardandoAnaliseSetorResponsavel,
                SituacaoEntregaDocumento.Pendente
            };
            if (algumDocumentoPendente || algumGrupoPendente)
            {
                return ValidarDocumentosSituacaoEntrega(situacoesEntregaDocumentos, documentosObrigatorios, gruposDocumentos, false);
            }
            return false;
        }

        private bool ValidarDocumentosDeferidoOuAguardandoAnaliseSetorResponsavel(List<DocumentoRequeridoVO> documentosObrigatorios, List<GrupoDocumentoEntregueVO> gruposDocumentos)
        {
            //Verificar se todos os documentos obrigatórios estão com uma das situações:
            //"Deferido" ou "Aguardando análise do setor responsável".
            var situacoesEntregaDocumentos = new List<SituacaoEntregaDocumento>() { SituacaoEntregaDocumento.Deferido, SituacaoEntregaDocumento.AguardandoAnaliseSetorResponsavel };
            return ValidarDocumentosSituacaoEntrega(situacoesEntregaDocumentos, documentosObrigatorios, gruposDocumentos, false);
        }

        /// <summary>
        /// Validadar se todos os documentos possuem alguma das situações passadas por parâmetro
        /// </summary>
        /// <param name="documentosObrigatorios"></param>
        /// <param name="gruposDocumentos"></param>
        /// <returns></returns>
        private bool ValidarDocumentosSituacaoEntrega(List<SituacaoEntregaDocumento> situacoesEntregaDocumentos, List<DocumentoRequeridoVO> documentosObrigatorios, List<GrupoDocumentoEntregueVO> gruposDocumentos, bool desconsiderarGruposDeferidos)
        {
            //Verificar se todos os documentos obrigatórios estão com uma das situações:
            foreach (var docRequerido in documentosObrigatorios)
            {
                foreach (var doc in docRequerido.InscricaoDocumentos)
                {
                    if (!situacoesEntregaDocumentos.Contains(doc.SituacaoEntregaDocumento))
                    {
                        return false;
                    }
                }
            }

            //Verificar se os grupos de documentos obrigatórios estão com o número mínimo de documentos em uma das situações
            foreach (var grupo in gruposDocumentos)
            {
                // Desconsidera os grupos que já estiverem deferidos
                if (!desconsiderarGruposDeferidos)
                {
                    if (GrupoDocumentosObrigatoriosDeferido(grupo))
                        continue;
                }

                var existeAlgumSituacao = grupo.DocumentosRequeridosGrupo.SelectMany(x => x.InscricaoDocumentos).Any(x => situacoesEntregaDocumentos.Contains(x.SituacaoEntregaDocumento));
                if (!existeAlgumSituacao)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Verifica se um grupo de documentos está com a situação deferida ou aguardando validação do setor responsável
        /// </summary>
        /// <param name="gruposDocumentos">Grupo de documentos a ser validado</param>
        /// <returns>Deferido ou não</returns>
        private bool GrupoDocumentosObrigatoriosDeferido(GrupoDocumentoEntregueVO grupoDocumentos)
        {
            // Situações de deferimento
            var situacoesEntregaDocumentos = new List<SituacaoEntregaDocumento>() { SituacaoEntregaDocumento.Deferido, SituacaoEntregaDocumento.AguardandoAnaliseSetorResponsavel };

            //Verificar se os grupos de documentos obrigatórios estão com o número mínimo de documentos em uma das situações
            var qtd = grupoDocumentos.DocumentosRequeridosGrupo.SelectMany(x => x.InscricaoDocumentos.Where(d => situacoesEntregaDocumentos.Contains(d.SituacaoEntregaDocumento))).LongCount();

            // Caso tenha atingido o número mínimo, considera como deferido
            if (grupoDocumentos.MinimoObrigatorio > qtd)
                return false;

            return true;
        }

        /// <summary>
        /// Verifica se um grupo de documentos está em alguma situação de entrega de documento, dada uma lista de situações
        /// </summary>
        /// <param name="grupoDocumentos">Grupos de documentos a ser validado</param>
        /// <param name="situacoesEntregaDocumentos">Lista de situações para verificação</param>
        /// <returns></returns>
        private bool GrupoDocumentoEmSituacaoEntregaDocumento(GrupoDocumentoEntregueVO grupoDocumentos, SituacaoEntregaDocumento[] situacoesEntregaDocumentos)
        {
            //Verificar se os grupos de documentos obrigatórios estão com o número mínimo de documentos em uma das situações
            var qtd = grupoDocumentos.DocumentosRequeridosGrupo.SelectMany(x => x.InscricaoDocumentos.Where(d => situacoesEntregaDocumentos.Contains(d.SituacaoEntregaDocumento))).LongCount();

            // Caso tenha atingido o número mínimo, considera como deferido
            if (grupoDocumentos.MinimoObrigatorio > qtd)
                return false;

            return true;
        }

        /// <summary>
        /// Realiza a entrega de um documento
        /// </summary>
        private InscricaoDocumentoVO EntregarDocumento(InscricaoDocumento inscricaoDocumento)
        {
            if (!inscricaoDocumento.DataEntrega.HasValue || inscricaoDocumento.DataEntrega == DateTime.MinValue)
            {
                throw new EntregaDocumentoSemDataException();
            }

            inscricaoDocumento.SituacaoEntregaDocumento = SituacaoEntregaDocumento.Deferido;
            //Regras de negócio
            var inscricaoAux = this.SearchByKey(new SMCSeqSpecification<InscricaoDocumento>(inscricaoDocumento.Seq),
                x => x.DocumentoRequerido, x => x.DocumentoRequerido.TipoDocumento, x => x.Inscricao);
            if (inscricaoAux.DocumentoRequerido.VersaoDocumento != VersaoDocumento.CopiaSimples
                && inscricaoDocumento.VersaoDocumento == VersaoDocumento.CopiaSimples
                && String.IsNullOrEmpty(inscricaoDocumento.Observacao))
            {
                throw new VersaoEntregueDiferenteExigidaSemObservacaoException(
                    this.TipoDocumentoService.BuscarTipoDocumento(inscricaoAux.DocumentoRequerido.SeqTipoDocumento).Descricao);
            }

            if (inscricaoDocumento.ArquivoAnexado != null)
            {
                if (inscricaoDocumento.SeqArquivoAnexado.HasValue && inscricaoDocumento.ArquivoAnexado.State == SMCUploadFileState.Unchanged)
                    inscricaoDocumento.ArquivoAnexado =
                        ArquivoAnexadoDomainService.SearchByKey(
                        new SMCSeqSpecification<ArquivoAnexado>(inscricaoDocumento.SeqArquivoAnexado.Value));
                else if (inscricaoDocumento.SeqArquivoAnexado.HasValue && inscricaoDocumento.ArquivoAnexado.State == SMCUploadFileState.Changed)
                    inscricaoDocumento.ArquivoAnexado.Seq = inscricaoDocumento.SeqArquivoAnexado.Value;
            }
            else
            {
                inscricaoDocumento.SeqArquivoAnexado = null;
            }

            var seq = this.UpdateEntity(inscricaoDocumento).Seq;
            var entregaFinalizada = ValidarEntregaDocumentos(
                       this.SearchBySpecification(new InscricaoDocumentoFilterSpecification
                       {
                           SeqInscricao = inscricaoAux.SeqInscricao,
                           SituacaoEntregaDocumento = SituacaoEntregaDocumento.Deferido
                       }), inscricaoAux.SeqInscricao);

            // Confirma a inscrição, caso todos os documento obrigatorios tenham sido entregues
            ConfirmarInscricao(inscricaoAux, entregaFinalizada);

            var ret = this.BuscarInscricaoDocumentos(new InscricaoDocumentoFilterSpecification { Seq = seq }).FirstOrDefault();

            return ret;
        }

        private void ConfirmarInscricao(InscricaoDocumento inscricaoAux, bool entregaFinalizada)
        {
            // Verifica se todos os documentos obrigatórios foram entregues e confirma a inscrição.
            if (entregaFinalizada && !inscricaoAux.Inscricao.DocumentacaoEntregue)
            {
                var inscricao = this.InscricaoDomainService.SearchByKey(
                    new SMCSeqSpecification<Inscricao>(inscricaoAux.SeqInscricao),
                                              IncludesInscricao.ArquivoComprovante | IncludesInscricao.HistoricosSituacao
                                            | IncludesInscricao.HistoricosSituacao_TipoProcessoSituacao
                                            | IncludesInscricao.Ofertas | IncludesInscricao.Ofertas_Oferta | IncludesInscricao.Processo
                    );

                // Armazena os historicos para não perder o valor após salvar
                var historicosSituacao = inscricao.HistoricosSituacao;
                inscricao.HistoricosSituacao = null;

                // Define a documentação como entregue
                inscricao.DocumentacaoEntregue = true;
                this.InscricaoDomainService.UpdateEntity(inscricao);

                inscricao.HistoricosSituacao = historicosSituacao;

                var ofertaExigePagamento = inscricao.Ofertas.Any(f => f.Oferta.ExigePagamentoTaxa);
                if (!ofertaExigePagamento ||
                    (ofertaExigePagamento && inscricao.TituloPago))
                {
                    //Verificar e alterar a situação da inscrição
                    if (inscricao.HistoricosSituacao.Any(x => x.Atual &&
                            x.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_FINALIZADA))
                    {
                        var seqTipoProcessoSituacao =
                            TipoProcessoSituacaoDomainService.SearchProjectionByKey(new TipoProcessoSituacaoFilterSpecification
                            {
                                SeqTipoProcesso = inscricao.Processo.SeqTipoProcesso,
                                Token = TOKENS.SITUACAO_INSCRICAO_CONFIRMADA
                            }, x => x.Seq);

                        // Cria um novo item de InscricaoHistoricoSituacao
                        InscricaoHistoricoSituacaoDomainService.AlterarSituacaoInscricoes(new AlteracaoSituacaoVO()
                        {
                            SeqTipoProcessoSituacaoDestino = seqTipoProcessoSituacao,
                            SeqInscricoes = new List<long> { inscricao.Seq }
                        });
                    }
                }
            }
        }

        private InscricaoDocumentoVO DesfazerEntregaDocumento(InscricaoDocumento inscricaoDocumento)
        {
            this.UpdateEntity(inscricaoDocumento);

            var entregaFinalizada = ValidarEntregaDocumentos(
                this.SearchBySpecification(new InscricaoDocumentoFilterSpecification
                {
                    SeqInscricao = inscricaoDocumento.SeqInscricao,
                    SituacaoEntregaDocumento = SituacaoEntregaDocumento.Deferido
                })
                , inscricaoDocumento.SeqInscricao);

            var inscricao = this.InscricaoDomainService.SearchByKey(
                new SMCSeqSpecification<Inscricao>(inscricaoDocumento.SeqInscricao),
                                          IncludesInscricao.HistoricosSituacao_TipoProcessoSituacao | IncludesInscricao.Ofertas_HistoricosSituacao_TipoProcessoSituacao);

            if (!entregaFinalizada && inscricao.DocumentacaoEntregue)
            {
                // Testar a situação da inscrição
                if (VerificarSituacaoAtualCandidatoOfertas(inscricao))
                {
                    this.InscricaoHistoricoSituacaoDomainService.AlterarSituacaoInscricoes(new AlteracaoSituacaoVO()
                    {
                        SeqTipoProcessoSituacaoDestino = -1,
                        SeqInscricoes = new List<long> { inscricao.Seq }
                    });
                }
                // Limpa a referência para o histórico de situação para não apagar as modificações feitas nos passos anteriores.
                // Ao desfazer uma entrega, a nova situação criada estava sendo apagada pois a lista de historico já estava carregada no objeto 'inscricao'.
                inscricao.HistoricosSituacao = null;
                inscricao.Ofertas = null;

                inscricao.DocumentacaoEntregue = false;
                this.InscricaoDomainService.UpdateEntity(inscricao);
            }

            var ret = this.BuscarInscricaoDocumentos(new InscricaoDocumentoFilterSpecification { Seq = inscricaoDocumento.Seq }).FirstOrDefault();
            return ret;
        }

        /// <summary>
        /// Verificar se o candidato está com a situação atual da inscrição como INSCRICAO_CONFIRMADA e, caso possua
        /// histórico de situação da inscrição oferta, a situação atual de todas as inscrições-oferta seja CANDIDATO_CONFIRMADO.
        /// </summary>
        /// <param name="inscricao"></param>
        /// <returns></returns>
        private bool VerificarSituacaoAtualCandidatoOfertas(Inscricao inscricao)
        {
            var historicoAtual = inscricao.HistoricosSituacao.FirstOrDefault(f => f.Atual);

            if (historicoAtual == null) { throw new InscricaoHistoricoSituacaoSemHistoricoAtualException(); }

            return historicoAtual.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA &&
                        // Verifica se todas as situações da inscrição oferta (que são atuais) estão na situação CANDIDATO_CONFIRMADO.
                        inscricao.Ofertas.All(f => f.HistoricosSituacao.FirstOrDefault(g => g.Atual) == null || f.HistoricosSituacao.FirstOrDefault(g => g.Atual).TipoProcessoSituacao.Token == TOKENS.SITUACAO_CANDIDATO_CONFIRMADO);
        }

        /// <summary>
        /// Se a situação atual da inscrição do candidato for INSCRICAO_DEFERIDA, ou for INSCRICAO_CONFIRMADA e
        /// existir histórico de situação da inscrição oferta cuja situação atual não é CANIDATO_CONFIRMADO
        /// </summary>
        /// <param name="inscricao"></param>
        /// <returns></returns>
        private bool VerificarSituacaoAltualCandidatoOfertaNaoConfirmado(Inscricao inscricao)
        {
            var historicoAtual = inscricao.HistoricosSituacao.FirstOrDefault(f => f.Atual);

            return historicoAtual.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_DEFERIDA ||
                (historicoAtual.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA
                     // Verifica se EXISTE a situação da inscrição oferta (que são atuais) na situação CANDIDATO_CONFIRMADO.
                     && inscricao.Ofertas.SMCAny(f => f.HistoricosSituacao.FirstOrDefault(g => g.Atual) != null && f.HistoricosSituacao.FirstOrDefault(g => g.Atual).TipoProcessoSituacao.Token != TOKENS.SITUACAO_CANDIDATO_CONFIRMADO));
        }

        public void DuplicarEntregaDocumento(long seqInscricaoDocumento)
        {
            var inscricaoDocumento = this.SearchByKey(new SMCSeqSpecification<InscricaoDocumento>(seqInscricaoDocumento));
            var novaInscricaoDocumento = new InscricaoDocumento
            {
                SeqInscricao = inscricaoDocumento.SeqInscricao,
                SeqDocumentoRequerido = inscricaoDocumento.SeqDocumentoRequerido,
                SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoEntrega
            };
            this.InsertEntity(novaInscricaoDocumento);
        }

        /// <summary>
        /// Exclui um documento para uma inscrição
        /// A operação só deve ser possível se o documento em questão permirtir a entrega de mais de um documento
        /// e se houver outro documento do mesmo tipo já entregue, acs contrário deve lançar uma exceção com
        /// a mensagem
        /// "Para o documento {0} é necessário existir ao menos um registroPara o documento <descrição do tipo de documento excluído>" é necessário existir ao menos um registro"
        /// </summary>
        /// <param name="seqInscricaoDocumento"></param>
        public void ExcluirInscricaoDocumento(long seqInscricaoDocumento)
        {
            var inscricaoDocumento = this.SearchByKey(
                new SMCSeqSpecification<InscricaoDocumento>(seqInscricaoDocumento),
                x => x.DocumentoRequerido);
            var documentoRequerido = inscricaoDocumento.DocumentoRequerido;
            var numeroItensTipoDocumento = this.Count(new InscricaoDocumentoFilterSpecification
            {
                SeqDocumentoRequerido = documentoRequerido.Seq,
                SeqInscricao = inscricaoDocumento.SeqInscricao
            });

            if (numeroItensTipoDocumento <= 1)
            {
                //Recuperar o tipo de Documeno
                var tipoDocumento = this.TipoDocumentoService.BuscarTipoDocumento(documentoRequerido.SeqTipoDocumento);
                throw new ExclusaoInscricaoDocumentoInvalidaException(tipoDocumento.Descricao);
            }
            else
            {
                this.DeleteEntity(inscricaoDocumento);
            }
        }

        #endregion Registro de Entrega de Documentos

        /// <summary>
        /// Salvar as novas entregas de documentação de uma inscrição
        /// </summary>
        /// <param name="novaEntregaDocumentacao">Documentos enviados pelo usuário</param>
        /// <returns>Sequencial da inscrição</returns>
        public long SalvarNovaEntregaDocumentacao(NovaEntregaDocumentacaoVO novaEntregaDocumentacao)
        {
            bool validaPdf = ValidarConverterPDF(novaEntregaDocumentacao.SeqInscricao);
            using (var unityOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    foreach (var documento in novaEntregaDocumentacao.Documentos)
                    {

                        if (documento.ArquivoAnexado != null)
                        {
                            if (validaPdf)
                            {
                                VerificaExtensoesDoArquivo(documento.ArquivoAnexado.Name, documento.ArquivoAnexado.Type, ExtensoesSemConversaoPDF);
                            }
                            else
                            {
                                VerificaExtensoesDoArquivo(documento.ArquivoAnexado.Name, documento.ArquivoAnexado.Type, ExtensoesConversaoPDF);
                            }
                        }



                        if (documento.ArquivoAnexado != null)
                        {
                            var specDoc = new InscricaoDocumentoFilterSpecification() { SeqDocumentoRequerido = documento.SeqDocumentoRequerido, SeqInscricao = novaEntregaDocumentacao.SeqInscricao };

                            var listaDocBase = this.SearchBySpecification(specDoc, IncludesInscricaoDocumento.ArquivoAnexado).ToList();

                            var docBase = listaDocBase.FirstOrDefault(l => l.Seq == documento.Seq);

                            if (docBase != null)
                            {
                                if (documento.SeqArquivoAnexadoAnterior.HasValue)
                                {
                                    if (docBase.ArquivoAnexado.UidArquivoGed.HasValue)
                                    {
                                        arquivosDeletadosGED.Add(docBase.ArquivoAnexado.UidArquivoGed.ToString());
                                    }

                                    this.DeleteEntity(docBase);

                                    var novoInscricaoDocumento = new InscricaoDocumento()
                                    {
                                        SeqInscricao = novaEntregaDocumentacao.SeqInscricao,
                                        SeqDocumentoRequerido = documento.SeqDocumentoRequerido,
                                        SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoValidacao,
                                        ArquivoAnexado = documento.ArquivoAnexado.Transform<ArquivoAnexado>(),
                                        DescricaoArquivoAnexado = documento.DescricaoArquivoAnexado,
                                        DataEntrega = DateTime.Now,
                                        FormaEntregaDocumento = FormaEntregaDocumento.Upload,
                                        VersaoDocumento = VersaoDocumento.CopiaSimples,
                                        DataPrazoEntrega = null,
                                        EntregaPosterior = false
                                    };

                                    this.SaveEntity(novoInscricaoDocumento);
                                    arquivosCriadosGED.Add(novoInscricaoDocumento.Seq);
                                }
                                else
                                {
                                    docBase.SeqInscricao = novaEntregaDocumentacao.SeqInscricao;
                                    docBase.SeqDocumentoRequerido = documento.SeqDocumentoRequerido;
                                    docBase.SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoValidacao;
                                    docBase.ArquivoAnexado = documento.ArquivoAnexado.Transform<ArquivoAnexado>();
                                    docBase.DescricaoArquivoAnexado = documento.DescricaoArquivoAnexado;
                                    docBase.DataEntrega = DateTime.Now;
                                    docBase.FormaEntregaDocumento = FormaEntregaDocumento.Upload;
                                    docBase.VersaoDocumento = VersaoDocumento.CopiaSimples;
                                    docBase.DataPrazoEntrega = null;
                                    docBase.EntregaPosterior = false;

                                    this.UpdateEntity(docBase);
                                    arquivosAtualizarGED.Add(docBase.Seq);
                                }
                            }
                        }
                    }

                    var sumarioInscricao = this.BuscarSumarioDocumentosEntregue(new InscricaoDocumentoFilterSpecification() { SeqInscricao = novaEntregaDocumentacao.SeqInscricao });

                    if (ValidarDocumentosNovaEntregaPossuiAlgumAguardandoEntregaOuIndeferido(sumarioInscricao.DocumentosObrigatorios, sumarioInscricao.GruposDocumentos) &&
                         ValidarDocumentosNovaEntregaPossuiAlgumAguardandoValidacao(sumarioInscricao.DocumentosObrigatorios, sumarioInscricao.GruposDocumentos))
                    {
                        AtualizarInscricaoSituacaoDocumentacao(sumarioInscricao.SeqInscricao, SituacaoDocumentacao.AguardandoValidacao);
                        AtualizarIndicadorDocumentacaoEntregue(sumarioInscricao.SeqInscricao, false);
                    }

                    CriarArquivosGED(novaEntregaDocumentacao.SeqInscricao, arquivosCriadosGED, TipoSistema.Inscricao);
                    AtualizarArquivoGED(novaEntregaDocumentacao.SeqInscricao, arquivosAtualizarGED, TipoSistema.Inscricao);
                    ExcluirArquivoGED(novaEntregaDocumentacao.SeqInscricao, arquivosDeletadosGED);

                    //if (!ValidarDocumentosNovaEntregaPossuiAlgumAguardandoEntregaOuIndeferido(sumarioInscricao.DocumentosObrigatorios, sumarioInscricao.GruposDocumentos) &&
                    //    ValidarDocumentosNovaEntregaPossuiAlgumAguardandoValidacao(sumarioInscricao.DocumentosObrigatorios, sumarioInscricao.GruposDocumentos))
                    //{
                    //    AtualizarInscricaoSituacaoDocumentacao(sumarioInscricao.SeqInscricao, SituacaoDocumentacao.AguardandoValidacao);
                    //    AtualizarIndicadorDocumentacaoEntregue(sumarioInscricao.SeqInscricao, false);
                    //}

                    unityOfWork.Commit();

                    return novaEntregaDocumentacao.SeqInscricao;
                }
                catch (Exception ex)
                {
                    unityOfWork.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Salva os documentos anexados em uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="param">Documentos para salvar</param>
        public void SalvarInscricaoDocumentoUpload(long seqInscricao, List<InscricaoDocumento> param)
        {
            //Valida a criação e processo
            //var inscricao = this.InscricaoDomainService.SearchByKey(seqInscricao);
            //InscricaoDomainService.AtualizarGED(inscricao);

            using (var trans = SMCUnitOfWork.Begin())
            {
                // valida se algum documento novo esta com o conteudo vazio que foi anexado esta com o conteudo vazio
                ValidarDocumentosSemConteudo(param);

                // Verifica se todas as InscricaoDocumento recebidas como parametro são de uma mesma inscrição
                ValidarInscricaoDocumentosMesmaInscricao(seqInscricao, param);

                // Valida Documentos Obrigatórios
                ValidarEntregaDocumentos(param.ToArray(), seqInscricao, true);

                // Se existem documentos do mesmo tipo, verifica se o tipo do documento permite vários arquivos
                ValidarDocumentosMesmoTipoPermitemVarios(param, false);

                // Verifica as regras para avançar na inscrição
                VerificarRegrasAvancarInscricao(seqInscricao);

                //Recuperação da configuração da etapa para ser usada para busca de documentos
                var specInscricaoKey = new SMCSeqSpecification<Inscricao>(seqInscricao);
                var dadosInscricao = this.InscricaoDomainService.SearchProjectionByKey(specInscricaoKey, x => new
                {
                    x.Processo.TipoProcesso.OrientacaoAceiteConversaoPDF,
                    x.Processo.TipoProcesso.TermoAceiteConversaoPDF
                });

                //Somente converte para PDF se o mesmo tiver orientação para Conversao e termo de aceite
                if (!string.IsNullOrEmpty(dadosInscricao.OrientacaoAceiteConversaoPDF) &&
                    !string.IsNullOrEmpty(dadosInscricao.TermoAceiteConversaoPDF))
                {
                    ConverterArquivosPDF(param);
                }

                List<InscricaoDocumento> documentosTela = new List<InscricaoDocumento>();
                documentosTela.AddRange(param);

                // Atualiza os arquivos
                var docsAlterados = AtualizarInscricaoDocumentosArquivosAnexados(seqInscricao, param);

                // Se algum documento obrigatório ou pertencente a um grupo de documentos tiver sua situação alterada para “Aguardando validação”,
                // alterar a situação da documentação da inscrição para “Aguardando validação”.
                // alterar indicador de documentação entregue da inscrição para "Não".

                var seqsDocumentosAguardandoValidacao = docsAlterados.Where(d => (d.SituacaoAnterior != SituacaoEntregaDocumento.AguardandoValidacao || (d.SituacaoAnterior == SituacaoEntregaDocumento.AguardandoValidacao && d.NovaSituacao == SituacaoEntregaDocumento.Pendente)) &&
                                                                                 (d.NovaSituacao == SituacaoEntregaDocumento.AguardandoValidacao || d.NovaSituacao == SituacaoEntregaDocumento.Pendente)
                                                                            ).Select(d => d.SeqDocumentoRequerido).ToList();

                var flagTermoEntregaPosterior = docsAlterados.Any(d => d.NovaSituacao == SituacaoEntregaDocumento.Pendente) || documentosTela.Any(a => a.EntregaPosterior);

                if (seqsDocumentosAguardandoValidacao.Any())
                {
                    var seqConfiguracaoEtapa = InscricaoDomainService.SearchProjectionByKey(seqInscricao, x => x.SeqConfiguracaoEtapa);
                    var seqsDocumentosRequeridosObrgatorios = InscricaoDomainService.ObterSeqsDocumentosRequeridos(seqConfiguracaoEtapa);
                    var documentosObrigatoriosAlterados = seqsDocumentosAguardandoValidacao.Where(s => seqsDocumentosRequeridosObrgatorios.Contains(s));

                    if (documentosObrigatoriosAlterados.Any())
                    {
                        var inscricaoAtual = InscricaoDomainService.SearchByKey(seqInscricao);
                        if (inscricaoAtual != null)
                        {
                            inscricaoAtual.SituacaoDocumentacao = SituacaoDocumentacao.AguardandoValidacao;
                            inscricaoAtual.DocumentacaoEntregue = false;
                            inscricaoAtual.CompromissoEntregaDocumentacao = flagTermoEntregaPosterior;
                            InscricaoDomainService.SaveEntity(inscricaoAtual);
                        }
                    }
                }

                //2. Atualizar a situação da documentação da inscrição para "Aguardando validação".
                //var possuiDocumentoRequerido = InscricaoDomainService.PossuiDocumentoRequerido(seqConfiguracaoEtapa);
                //var seqConfiguracaoEtapa = InscricaoDomainService.SearchProjectionByKey(seqInscricao, x => x.SeqConfiguracaoEtapa);
                //if (possuiDocumentoRequerido)
                //    AtualizarInscricaoSituacaoDocumentacao(seqInscricao, SituacaoDocumentacao.AguardandoValidacao);

                trans.Commit();
            }
        }

        /// <summary>
        /// Valida se um documento foi postado está sem conteudo
        /// </summary>
        /// <param name="docuemntos"></param>
        private void ValidarDocumentosSemConteudo(List<InscricaoDocumento> docuemntos)
        {
            string mensagemErro = "O(s) documento(s) listado(s) abaixo apresentaram problema no conteúdo por favor reanexa-lo(s): <br /> {0}";
            string documentosComErro = string.Empty;
            bool existeDocumentoComErro = false;
            foreach (var documento in docuemntos)
            {
                if (documento.SeqArquivoAnexado == null && documento.ArquivoAnexado != null && documento.ArquivoAnexado.Conteudo == null)
                {
                    existeDocumentoComErro = true;
                    documentosComErro += $"{documento.ArquivoAnexado.Nome}<br />";
                }
            }

            if (existeDocumentoComErro)
            {
                throw new Exception(string.Format(mensagemErro, documentosComErro));
            }
        }

        private bool ValidarDocumentosNovaEntregaPossuiAlgumAguardandoEntregaOuIndeferido(List<DocumentoRequeridoVO> documentosObrigatorios, List<GrupoDocumentoEntregueVO> gruposDocumentos)
        {
            /*Se não existir nenhum documento obrigatório ou grupo de documento [1] com a situação “Aguardando entrega” ou “Indeferido”
             *
             * 1 - Para que um grupo de documento seja considerado “Aguardando entrega” ou “Indeferido”, ele deve possuir ao menos um documento em uma destas situações,
             * não pode possuir nenhum documento na situação “Em validação”, e a quantidade de documentos na situação “Deferido”, “Pendente” ou “Aguardando análise do setor responsável”
             * tem que ser menor que a quantidade mínima exigida pelo grupo.
             */


            var situacoesEntregaDocumentos = new List<SituacaoEntregaDocumento>() {
                SituacaoEntregaDocumento.AguardandoEntrega,
                SituacaoEntregaDocumento.Indeferido
            };

            //verificar se pelo menos um documento obrigatório está com a situação "Aguardando entrega" ou "Indeferido".
            var algumDocumentoObrigatorioAguardandoEntregaOuIndeferido = documentosObrigatorios.Any(x => x.InscricaoDocumentos.Any(d => situacoesEntregaDocumentos.Contains(d.SituacaoEntregaDocumento)));

            if (algumDocumentoObrigatorioAguardandoEntregaOuIndeferido) return false;


            //verificar se algum documento de grupo está com a situação "Aguardando entrega" ou "Indeferido".
            var algumDocumentoGrupoAguardandoEntregaOuIndeferido = gruposDocumentos.Any(g => g.DocumentosRequeridosGrupo.Any(d => d.InscricaoDocumentos.Any(i => situacoesEntregaDocumentos.Contains(i.SituacaoEntregaDocumento))));

            //verificar se algum documento de grupo está com a situação "Em validação".
            var algumDocumentoGrupoEmValidacao = gruposDocumentos.Any(g => g.DocumentosRequeridosGrupo.Any(d => d.InscricaoDocumentos.Any(i => i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.AguardandoValidacao)));


            //validar se a quantidade de documentos na situação “Deferido”, “Pendente” ou “Aguardando análise do setor responsável” é menor que a quantidade mínima exigida pelo grupo
            var situacoesEntregaDocumentoGrupo = new List<SituacaoEntregaDocumento>
            {
                SituacaoEntregaDocumento.Deferido,
                SituacaoEntregaDocumento.Pendente,
                SituacaoEntregaDocumento.AguardandoAnaliseSetorResponsavel
            };

            var algumDocumentoGrupoQuantidadeMinimaRequerida = false;

            foreach (var grupoDocumento in gruposDocumentos)
            {
                var qtd = grupoDocumento.DocumentosRequeridosGrupo.SelectMany(x => x.InscricaoDocumentos.Where(d => situacoesEntregaDocumentoGrupo.Contains(d.SituacaoEntregaDocumento))).LongCount();

                if (qtd < grupoDocumento.MinimoObrigatorio)
                {
                    algumDocumentoGrupoQuantidadeMinimaRequerida = true;
                    break;
                };

            }

            if (algumDocumentoGrupoAguardandoEntregaOuIndeferido && !algumDocumentoGrupoEmValidacao && algumDocumentoGrupoQuantidadeMinimaRequerida) return false;

            return true;


        }

        private bool ValidarDocumentosNovaEntregaPossuiAlgumAguardandoValidacao(List<DocumentoRequeridoVO> documentosObrigatorios, List<GrupoDocumentoEntregueVO> gruposDocumentos)
        {
            /* Se existir documento obrigatório ou grupo de documento com [2] a situação “Aguardando validação”
             *
             *2 - Para que um grupo de documentos seja considerado “Aguardando validação”, ele deve possuir ao menos um documento nesta situação
             *e a quantidade de documentos na situação “Deferido”, “Pendente” ou “Aguardando análise do setor responsável” tem que ser menor que a
             *quantidade mínima exigida pelo grupo.
             */

            //verificar se algum documento obrigatorio está com a situação "Em validação".
            var algumDocumentoObrigatorioAguardandoValidacao = documentosObrigatorios.Any(d => d.InscricaoDocumentos.Any(i => i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.AguardandoValidacao));

            if (algumDocumentoObrigatorioAguardandoValidacao) return true;

            //verificar se algum documento de grupo está com a situação "Em validação".
            var algumDocumentoGrupoAguardandoValidacao = gruposDocumentos.Any(g => g.DocumentosRequeridosGrupo.Any(d => d.InscricaoDocumentos.Any(i => i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.AguardandoValidacao)));

            //validar se a quantidade de documentos na situação “Deferido”, “Pendente” ou “Aguardando análise do setor responsável” é menor que a quantidade mínima exigida pelo grupo
            var situacoesEntregaDocumentoGrupo = new List<SituacaoEntregaDocumento>
            {
                SituacaoEntregaDocumento.Deferido,
                SituacaoEntregaDocumento.Pendente,
                SituacaoEntregaDocumento.AguardandoAnaliseSetorResponsavel
            };

            var algumDocumentoGrupoQuantidadeMinimaRequerida = false;

            foreach (var grupoDocumento in gruposDocumentos)
            {
                var qtd = grupoDocumento.DocumentosRequeridosGrupo.SelectMany(x => x.InscricaoDocumentos.Where(d => situacoesEntregaDocumentoGrupo.Contains(d.SituacaoEntregaDocumento))).LongCount();

                if (qtd < grupoDocumento.MinimoObrigatorio)
                {
                    algumDocumentoGrupoQuantidadeMinimaRequerida = true;
                    break;
                };

            }

            if (algumDocumentoGrupoAguardandoValidacao && algumDocumentoGrupoQuantidadeMinimaRequerida) return true;

            return false;


        }

        /// <summary>
        /// Percorre os documentos encontrados no banco atualizando os que foram recebidas como parâmetro
        /// e excluindo os que estão apenas no banco
        /// </summary>
        /// <param name="seqInscricao"></param>
        /// <param name="inscricaoDocumentos"></param>
        private List<(long SeqDocumentoRequerido, SituacaoEntregaDocumento SituacaoAnterior, SituacaoEntregaDocumento NovaSituacao)> AtualizarInscricaoDocumentosArquivosAnexados(long seqInscricao, List<InscricaoDocumento> inscricaoDocumentos)
        {
            // Cria o retorno
            var ret = new List<(long SeqDocumentoRequerido, SituacaoEntregaDocumento SituacaoAnterior, SituacaoEntregaDocumento NovaSituacao)>();

            // Busca os documentos da inscrição já cadastrados em banco de dados
            var specDoc = new InscricaoDocumentoFilterSpecification() { SeqInscricao = seqInscricao };

            var listaDoc = this.SearchBySpecification(specDoc, IncludesInscricaoDocumento.ArquivoAnexado | IncludesInscricaoDocumento.DocumentoRequerido).ToList();

            List<long> seqsArquivosRemover = new List<long>();

            // Percorre os documentos encontrados no banco atualizando os que foram recebidas como parâmetro
            // e excluindo os que estão apenas no banco
            DateTime dataEntrega = DateTime.Now;
            foreach (var doc in listaDoc)
            {
                // Verifica se o documento do banco foi alterada ou excluída
                InscricaoDocumento itemParam = null;

                // Caso tenha sido salvo mais de um documento requerido deste mesmo tipo no banco
                if (listaDoc.Count(l => l.SeqDocumentoRequerido == doc.SeqDocumentoRequerido) > 1)
                    itemParam = inscricaoDocumentos.FirstOrDefault(l => l.SeqDocumentoRequerido == doc.SeqDocumentoRequerido && (l.Seq == 0 || l.Seq == doc.Seq));
                else
                    itemParam = inscricaoDocumentos.FirstOrDefault(l => l.SeqDocumentoRequerido == doc.SeqDocumentoRequerido);

                //InscricaoDocumento itemParam = inscricaoDocumentos.Where(o => o.Seq == doc.Seq).FirstOrDefault();
                if (itemParam != null)
                {
                    if (doc.DescricaoArquivoAnexado != itemParam.DescricaoArquivoAnexado
                        || doc.SeqDocumentoRequerido != itemParam.SeqDocumentoRequerido
                        || (doc.ArquivoAnexado != null && itemParam.ArquivoAnexado == null)
                        || (!doc.EntregaPosterior && itemParam.EntregaPosterior)
                        || (doc.EntregaPosterior && !itemParam.EntregaPosterior)
                        || (doc.ArquivoAnexado == null && itemParam.ArquivoAnexado != null)
                        || (itemParam.ArquivoAnexado != null && (itemParam.ArquivoAnexado.State == SMCUploadFileState.Changed || itemParam.Seq != doc.Seq)))
                    {
                        var situacaoAntiga = doc.SituacaoEntregaDocumento;

                        // Chama a alteração dos dados do documento
                        AlterarDadosDocumento(doc, itemParam, dataEntrega);

                        // Registra que alterou este item
                        ret.Add((doc.SeqDocumentoRequerido, situacaoAntiga, doc.SituacaoEntregaDocumento));

                        if (situacaoAntiga == SituacaoEntregaDocumento.Deferido && doc.SituacaoEntregaDocumento != SituacaoEntregaDocumento.Deferido)
                        {
                            arquivosAtualizarMetadadosGED.Add(doc.Seq);
                        }

                        // Bug 37522: Problema ao salvar relacionamento de arquivo anexado. Salva a entidade mas não atualiza o sequencial no objeto pai.
                        // Debugamos todo o código do entity do nosso framework e não descobrimos nada que pudesse estar causando isso.
                        // Forcei a inclusão manual e atualização do seq do arquivo. Não remover.
                        if (doc.ArquivoAnexado?.Seq == 0 && itemParam.EntregaPosterior == false)
                        {

                            // Salva o documento
                            ArquivoAnexadoDomainService.SaveEntity(doc.ArquivoAnexado);
                            // Update no seq
                            doc.SeqArquivoAnexado = doc.ArquivoAnexado.Seq;
                            arquivosCriadosGED.Add(doc.Seq);
                        }

                        if (doc.ArquivoAnexado?.Seq == 0 && itemParam.EntregaPosterior == true)
                        {
                            // Salva o documento
                            ArquivoAnexadoDomainService.DeleteEntity(doc.ArquivoAnexado);
                            // Update no seq
                            doc.SeqArquivoAnexado = (long?)null;
                        }

                        if (doc.ConvertidoParaPDF != itemParam.ConvertidoParaPDF)
                        {
                            doc.ConvertidoParaPDF = itemParam.ConvertidoParaPDF;
                        }

                        this.SaveEntity(doc);
                    }
                    inscricaoDocumentos.Remove(itemParam);
                }
                else
                {
                    // Bug 37913
                    // Não achou n post o registro para este documento requerido
                    // Caso ele tenha arquivo, remove pois significa que o usuário voltou na tela e alterou o tipo de documento requerido enviado anteriormente
                    if (doc.SeqArquivoAnexado.HasValue)
                    {
                        // Armazena o seq do arquivo anexado para remover
                        var seqArquivoAnexadoRemover = doc.SeqArquivoAnexado.Value;

                        // Atualiza as informações do documento requerido
                        doc.SeqArquivoAnexado = null;
                        doc.ArquivoAnexado = null;
                        doc.DataEntrega = null;
                        doc.EntregaPosterior = false;
                        doc.SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoEntrega;
                        doc.FormaEntregaDocumento = null;
                        doc.VersaoDocumento = null;
                        doc.DescricaoArquivoAnexado = null;
                        doc.DataPrazoEntrega = null;

                        this.SaveEntity(doc);

                        // Remove o arquivo anexado
                        seqsArquivosRemover.Add(seqArquivoAnexadoRemover);
                    }
                    else if (doc.EntregaPosterior)
                    {
                        // Atualiza as informações do documento requerido
                        doc.SeqArquivoAnexado = null;
                        doc.ArquivoAnexado = null;
                        doc.DataEntrega = null;
                        doc.EntregaPosterior = false;
                        doc.SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoEntrega;
                        doc.FormaEntregaDocumento = null;
                        doc.VersaoDocumento = null;
                        doc.DescricaoArquivoAnexado = null;
                        doc.DataPrazoEntrega = null;

                        this.SaveEntity(doc);
                    }
                }
            }

            CriarArquivosGED(seqInscricao, arquivosCriadosGED, TipoSistema.Inscricao);
            ExcluirArquivoGED(seqInscricao, arquivosDeletadosGED);
            AtualizarArquivoGED(seqInscricao, arquivosAtualizarGED, TipoSistema.Inscricao);
            AtualizarMetadadosArquivoGED(seqInscricao, arquivosAtualizarMetadadosGED);

            // Percorre a lista de documentos salvos e procura se tem mais de um que tenha o mesmo seq de documento requerido porém não tem anexo
            listaDoc.GroupBy(d => d.SeqDocumentoRequerido).Where(g => g.Count() > 1).ToList().ForEach(g =>
            {
                // Verifica se algum item possui arquivo
                var itemManter = g.FirstOrDefault(i => i.SeqArquivoAnexado.HasValue) ?? g.FirstOrDefault();
                g.ToList().ForEach(i =>
                {
                    if (!i.SeqArquivoAnexado.HasValue && i != itemManter)
                        DeleteEntity(i);
                });
            });

            // Se sobrou algum documento no parametro que não foi atualizado, inclui em banco
            foreach (var doc in inscricaoDocumentos)
            {
                if (doc.EntregaPosterior)
                {
                    // Registra que alterou este item
                    ret.Add((doc.SeqDocumentoRequerido, doc.SituacaoEntregaDocumento, SituacaoEntregaDocumento.Pendente));
                    continue;
                }

                long? seqArquivoAnexadoAtual = null;
                ValidarVinculoDocumentoAnexado(doc);

                // Caso tenha arquivo, recupera o seq do arquivo para ver se criou outro ou se usou o mesmo
                if (doc.ArquivoAnexado != null)
                    seqArquivoAnexadoAtual = doc.ArquivoAnexado.Seq;

                // Registra que alterou este item
                ret.Add((doc.SeqDocumentoRequerido, doc.SituacaoEntregaDocumento, SituacaoEntregaDocumento.AguardandoValidacao));

                // Altera a situação
                AlterarDadosDocumento(doc, dataEntrega);

                doc.Seq = 0;
                InsertEntity(doc);

                // Caso tenha seq de arquivo e seja igual o seq armazenado anteriormente, significa que usou o mesmo arquivo.
                // Neste caso, remove da lista de arquivos a serem removidos
                // Caso tenha criado outro arquivo, mantém o id antigo na lista para que possa ser removido
                if (doc.ArquivoAnexado != null && doc.ArquivoAnexado.Seq == seqArquivoAnexadoAtual)
                    if (seqsArquivosRemover.Contains(seqArquivoAnexadoAtual.Value))
                        seqsArquivosRemover.Remove(seqArquivoAnexadoAtual.Value);
            }

            foreach (var item in seqsArquivosRemover)
                ArquivoAnexadoDomainService.DeleteEntity(item);

            return ret;
        }

        private void CriarArquivosGED(long seqIncricao, List<long> documentos, TipoSistema tipoSistema)
        {
            ParametrosArquivoVO modelo = new ParametrosArquivoVO();
            modelo.SeqInscricao = seqIncricao;
            modelo.OrigemInscricaoDocumento = true;
            modelo.SeqsDocumentos = documentos;
            modelo.TipoSistema = tipoSistema;

            ArquivoApiDoaminService.CriarArquivo(modelo);
        }

        private void ExcluirArquivoGED(long seqIncricao, List<string> documentos)
        {
            ParametrosArquivoVO modelo = new ParametrosArquivoVO();
            modelo.SeqInscricao = seqIncricao;
            modelo.SeqsGuidArquivo = documentos;

            ArquivoApiDoaminService.ExcluirArquivo(modelo);
        }

        private void AtualizarArquivoGED(long seqInscricao, List<long> documentos, TipoSistema tipoSistema)
        {
            ParametrosArquivoVO modelo = new ParametrosArquivoVO();
            modelo.SeqsDocumentos = documentos;
            modelo.SeqInscricao = seqInscricao;
            modelo.TipoSistema = tipoSistema;
            modelo.OrigemInscricaoDocumento = true;

            ArquivoApiDoaminService.AtualizarArquivo(modelo);
        }

        private void AtualizarMetadadosArquivoGED(long seqInscricao, List<long> documentos)
        {
            ParametrosArquivoVO modelo = new ParametrosArquivoVO();
            modelo.SeqsDocumentos = documentos;
            modelo.SeqInscricao = seqInscricao;
            modelo.OrigemInscricaoDocumento = true;

            ArquivoApiDoaminService.AtualizarMetaDados(modelo);
        }

        private void ValidarVinculoDocumentoAnexado(InscricaoDocumento doc)
        {
            if (doc.SeqArquivoAnexado.HasValue
                && doc.ArquivoAnexado != null
                && doc.ArquivoAnexado.Conteudo == null
                && doc.ArquivoAnexado.State == SMCUploadFileState.Unchanged)
            {
                doc.ArquivoAnexado = ArquivoAnexadoDomainService.SearchByKey(doc.SeqArquivoAnexado.Value);
            }
        }

        /// <summary>
        /// Verifica se todas as InscricaoDocumento recebidas como parametro são de uma mesma inscrição
        /// </summary>
        /// <param name="inscricaoDocumentos"></param>
        private void ValidarInscricaoDocumentosMesmaInscricao(long seqInscricao, List<InscricaoDocumento> inscricaoDocumentos)
        {
            if (inscricaoDocumentos.Count > 1 && inscricaoDocumentos.Select(o => o.SeqInscricao).Distinct().Count() > 1)
                throw new InscricaoInvalidaException();

            // Se o sequencial da inscrição recebido como parametro for diferente da lista de documentos, erro
            if (inscricaoDocumentos.Count > 1 && seqInscricao != inscricaoDocumentos.Select(o => o.SeqInscricao).Distinct().SingleOrDefault())
                throw new InscricaoInvalidaException();
        }

        private void VerificarRegrasAvancarInscricao(long seqInscricao)
        {
            // Busca a inscrição
            IncludesInscricao includesInscricao = IncludesInscricao.ConfiguracaoEtapa |
                                                  IncludesInscricao.ConfiguracaoEtapa_EtapaProcesso;
            var specInscricao = new SMCSeqSpecification<Inscricao>(seqInscricao);
            Inscricao inscricao = InscricaoDomainService.SearchByKey(specInscricao, includesInscricao);

            // Verifica as regras para avançar na inscrição
            InscricaoDomainService.VerificarRegrasAvancarInscricao(inscricao);
        }

        /// <summary>
        ///  Atualizar a situação da documentação da inscrição
        /// </summary>
        /// <param name="seqInscricao"></param>
        /// <param name="situacaoDocumentacao"></param>
        private void AtualizarInscricaoSituacaoDocumentacao(long seqInscricao, SituacaoDocumentacao situacaoDocumentacao)
        {
            var inscricao = InscricaoDomainService.SearchByKey(seqInscricao);
            if (inscricao != null && inscricao.SituacaoDocumentacao != situacaoDocumentacao)
            {
                inscricao.SituacaoDocumentacao = situacaoDocumentacao;
                InscricaoDomainService.SaveEntity(inscricao);
            }
        }

        /// <summary>
        ///  Atualizar o indicador de documentação entregue
        /// </summary>
        /// <param name="seqInscricao"></param>
        /// <param name="documentacaoEntregue"></param>
        private void AtualizarIndicadorDocumentacaoEntregue(long seqInscricao, bool documentacaoEntregue)
        {
            var inscricao = InscricaoDomainService.SearchByKey(seqInscricao);
            if (inscricao != null && inscricao.DocumentacaoEntregue != documentacaoEntregue)
            {
                inscricao.DocumentacaoEntregue = documentacaoEntregue;
                InscricaoDomainService.SaveEntity(inscricao);
            }
        }

        /// <summary>
        /// •	O registro de documento incluído/alterado deverá conter os seguintes dados:
        /// - Tipo de documento: tipo do documento informado.
        /// - Situação da entrega: "Aguardando Validação"
        /// - Arquivo anexado: referência do arquivo carregado na base de dados.
        /// - Descrição do arquivo: descrição informada.
        /// - Data de entrega: data do upload.
        /// - Forma de entrega: "Upload".
        /// - Versão: "Cópia simples".
        /// - Observação: nula.
        /// - Data de devolução: nula.
        /// - Prazo de entrega: nulo.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="itemParam"></param>
        /// <param name="dataEntrega"></param>
        private void AlterarDadosDocumento(InscricaoDocumento doc, InscricaoDocumento itemParam, DateTime dataEntrega)
        {
            AlterarDadosDocumento(doc, dataEntrega);
            doc.SeqDocumentoRequerido = itemParam.SeqDocumentoRequerido;
            doc.DocumentoRequerido = null;
            doc.DescricaoArquivoAnexado = itemParam.DescricaoArquivoAnexado;
            doc.EntregaPosterior = itemParam.EntregaPosterior;
            if (itemParam.ArquivoAnexado == null)
            {
                string guidArquivoGED = null;
                if (doc.ArquivoAnexado != null)
                {
                    guidArquivoGED = doc.ArquivoAnexado.UidArquivoGed.HasValue ? doc.ArquivoAnexado.UidArquivoGed.ToString() : null;
                }
                doc.ArquivoAnexado = null;
                doc.SeqArquivoAnexado = null;
                if (itemParam.EntregaPosterior)
                {
                    doc.SituacaoEntregaDocumento = SituacaoEntregaDocumento.Pendente;
                    doc.DataPrazoEntrega = itemParam.DataPrazoEntrega;
                }
                else
                {
                    doc.SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoEntrega;
                    doc.DataPrazoEntrega = null;
                }
                doc.DataEntrega = null;
                doc.FormaEntregaDocumento = null;
                doc.VersaoDocumento = null;
                doc.Observacao = null;
                doc.DataDevolucaoInscrito = null;

                if (!string.IsNullOrEmpty(guidArquivoGED))
                {
                    arquivosDeletadosGED.Add(guidArquivoGED);
                }

                this.UpdateEntity(doc);

                if (doc.SeqArquivoAnexado.HasValue)
                {
                    var seqArquivoAnexado = doc.SeqArquivoAnexado.Value;
                    ArquivoAnexadoDomainService.DeleteEntity(seqArquivoAnexado);
                }
            }
            else if ((itemParam.ArquivoAnexado.State == SMCUploadFileState.Changed) || (itemParam.ArquivoAnexado.State == SMCUploadFileState.Unchanged && doc.ArquivoAnexado == null))
            {
                if (doc.ArquivoAnexado != null && doc.ArquivoAnexado.Seq > 0 && doc.ArquivoAnexado.UidArquivoGed != null)
                {
                    arquivosAtualizarGED.Add(doc.Seq);
                }
                else if (doc.ArquivoAnexado != null && doc.ArquivoAnexado.Seq > 0 && doc.ArquivoAnexado.UidArquivoGed == null)
                {
                    arquivosCriadosGED.Add(doc.Seq);
                }

                if (doc.ArquivoAnexado == null) { doc.ArquivoAnexado = new ArquivoAnexado(); }
                doc.ArquivoAnexado.Nome = itemParam.ArquivoAnexado.Nome;
                doc.ArquivoAnexado.Tipo = itemParam.ArquivoAnexado.Tipo;
                doc.ArquivoAnexado.Tamanho = itemParam.ArquivoAnexado.Tamanho;
                doc.ArquivoAnexado.Conteudo = itemParam.ArquivoAnexado.Conteudo;

                if (doc.ArquivoAnexado.Conteudo == null && itemParam.ArquivoAnexado.Seq > 0)
                    doc.ArquivoAnexado.Conteudo = ArquivoAnexadoDomainService.SearchProjectionByKey(itemParam.ArquivoAnexado.Seq, x => x.Conteudo);
            }
            else if (itemParam.ArquivoAnexado.State == SMCUploadFileState.Unchanged && itemParam.SeqArquivoAnexado != doc.SeqArquivoAnexado)
            {
                doc.ArquivoAnexado = itemParam.ArquivoAnexado;
                doc.SeqArquivoAnexado = itemParam.SeqArquivoAnexado;
            }
        }

        /// <summary>
        /// •	O registro de documento incluído/alterado deverá conter os seguintes dados:
        /// - Tipo de documento: tipo do documento informado.
        /// - Situação da entrega: "Aguardando Validação"
        /// - Arquivo anexado: referência do arquivo carregado na base de dados.
        /// - Descrição do arquivo: descrição informada.
        /// - Data de entrega: data do upload.
        /// - Forma de entrega: "Upload".
        /// - Versão: "Cópia simples".
        /// - Observação: nula.
        /// - Data de devolução: nula.
        /// - Prazo de entrega: nulo.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="dataEntrega"></param>
        private void AlterarDadosDocumento(InscricaoDocumento doc, DateTime dataEntrega)
        {
            doc.DataEntrega = dataEntrega;
            doc.SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoValidacao;
            doc.FormaEntregaDocumento = FormaEntregaDocumento.Upload;
            doc.VersaoDocumento = VersaoDocumento.CopiaSimples;
            doc.Observacao = null;
            doc.DataDevolucaoInscrito = null;
            doc.DataPrazoEntrega = null;
        }

        private bool ValidarEntregaDocumentos(IEnumerable<InscricaoDocumento> param, long seqInscricao, bool isUpload = false)
        {
            bool isValid = true;
            string messageErro = "";
            var sexoInscrito = this.InscricaoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(seqInscricao),
                x => x.Inscrito.Sexo);
            var seqDocs = param.Select(x => x.SeqDocumentoRequerido);
            var seqConfiguracaoEtapa = InscricaoDomainService.SearchProjectionByKey(
                new SMCSeqSpecification<Inscricao>(seqInscricao), x => x.SeqConfiguracaoEtapa);
            var docSpec = new DocumentoRequeridoFilterSpecification { SeqConfiguracaoEtapa = seqConfiguracaoEtapa };
            if (isUpload) docSpec.UploadObrigatorio = true;
            else docSpec.Obrigatorio = true;

            var listaDocumentos = DocumentoRequeridoDomainService.SearchBySpecification(docSpec, x => x.TipoDocumento).ToList();

            // Valida as extensões permitidas
            foreach (var file in param.Where(x => x.ArquivoAnexado != null))
            {
                if (!VerifyExtension(file, seqInscricao))
                {
                    isValid = false;
                    if (ValidarConverterPDF(seqInscricao))
                    {
                        messageErro += string.Format(Common.Areas.INS.Resources.ExceptionsResource.ExtensaoDocumentoInvalidaPDFException,
                          file.ArquivoAnexado.Description);
                    }
                    else
                    {
                        messageErro += string.Format(Common.Areas.INS.Resources.ExceptionsResource.ExtensaoDocumentoInvalidaException,
                            file.ArquivoAnexado.Description);
                    }
                }
            }

            foreach (var doc in listaDocumentos.Where(x => !x.Sexo.HasValue || x.Sexo.Value == sexoInscrito))
            {
                //Verifica se todos os documentos foram entregues para setar o flag de entregue ou gerar erro de upload
                if (!seqDocs.Contains(doc.Seq)
                   || (isUpload
                       && !param.Where(x => x.SeqDocumentoRequerido == doc.Seq).FirstOrDefault().SeqArquivoAnexado.HasValue
                       && param.Where(x => x.SeqDocumentoRequerido == doc.Seq).FirstOrDefault().ArquivoAnexado == null
                       && param.Where(x => x.SeqDocumentoRequerido == doc.Seq).FirstOrDefault().EntregaPosterior == false
                       )
                   )
                {
                    isValid = false;
                    var tipo = this.TipoDocumentoService.BuscarTipoDocumento(doc.SeqTipoDocumento);
                    messageErro += string.Format(Resources.MessagesResource.DocumentoRequeridoObrigatorioNaoSubmetidoException,
                        tipo.Descricao);
                }

                //Verifica se todos os documentos possuem arquivo e flag entrega posterior marcada
                if (!seqDocs.Contains(doc.Seq)
                   || (isUpload
                       && param.Where(x => x.SeqDocumentoRequerido == doc.Seq).FirstOrDefault().ArquivoAnexado != null
                       && param.Where(x => x.SeqDocumentoRequerido == doc.Seq).FirstOrDefault().EntregaPosterior == true
                       )
                   )
                {
                    isValid = false;
                    var tipo = this.TipoDocumentoService.BuscarTipoDocumento(doc.SeqTipoDocumento);
                    messageErro += string.Format(Resources.MessagesResource.DocumentoRequeridoArquivoEntregaPosterior,
                        tipo.Descricao);
                }
            }

            //Validação de grupo de Documentos obrigatório
            var specGrupo = new GrupoDocumentoRequeridoFilterSpecification
            {
                SeqConfiguracaoEtapa = seqConfiguracaoEtapa,
            };
            if (isUpload) specGrupo.UploadObrigatorio = true;
            var gruposObrigatorios = GrupoDocumentoRequeridoDomainService.SearchBySpecification(specGrupo, x => x.Itens,
                x => x.Itens[0].DocumentoRequerido);
            foreach (var grupo in gruposObrigatorios)
            {
                var seqDocumentosRequeridosGrupo = grupo.Itens.Select(x => x.SeqDocumentoRequerido);
                int numeroDocumentosGrupoSubmetidos = 0;
                if (isUpload)
                {
                    numeroDocumentosGrupoSubmetidos =
                        param.Count(x => seqDocumentosRequeridosGrupo.Contains(x.SeqDocumentoRequerido) &&
                            isUpload && (x.ArquivoAnexado != null || x.SeqArquivoAnexado.HasValue || x.EntregaPosterior));

                    if (param.Any(x => seqDocumentosRequeridosGrupo.Contains(x.SeqDocumentoRequerido) &&
                             isUpload && x.ArquivoAnexado != null && x.EntregaPosterior))
                    {
                        isValid = false;
                        var seqTipo = param.First(x => seqDocumentosRequeridosGrupo.Contains(x.SeqDocumentoRequerido) && isUpload && x.ArquivoAnexado != null && x.EntregaPosterior).DocumentoRequerido.SeqTipoDocumento;
                        var tipo = this.TipoDocumentoService.BuscarTipoDocumento(seqTipo);
                        messageErro += string.Format(Resources.MessagesResource.DocumentoRequeridoArquivoEntregaPosterior,
                            tipo.Descricao);
                    }
                }
                else
                {
                    numeroDocumentosGrupoSubmetidos = param.Count(x => seqDocumentosRequeridosGrupo.Contains(x.SeqDocumentoRequerido));
                }
                if (numeroDocumentosGrupoSubmetidos < grupo.MinimoObrigatorio)
                {
                    isValid = false;
                    string listaDocs = "";
                    foreach (var item in grupo.Itens)
                    {
                        listaDocs += TipoDocumentoService
                            .BuscarTipoDocumento(item.DocumentoRequerido.SeqTipoDocumento).Descricao + ",";
                    }
                    listaDocs = listaDocs.Substring(0, listaDocs.Length - 1);
                    messageErro += String.Format(Resources.MessagesResource.GrupoDocumentoRequeridoObrigatorioSubmetidoInsuficienteException
                        , grupo.MinimoObrigatorio, listaDocs);
                }
            }

            if (!isValid)
            {
                //Lançar exceção de obrigatoriedade de documenos se for teste no upload
                //Se for teste de entrega retornar falso
                if (isUpload) throw new SMCApplicationException(messageErro);
                else return isValid;
            }
            return isValid;
        }

        private bool VerifyExtension(InscricaoDocumento file, long seqInscricao)
        {
            //Se converter ele valida as extensões de um PDF
            if (ValidarConverterPDF(seqInscricao))
            {
                return allowedExtensionsPDF.Contains(file.ArquivoAnexado.Extension.ToLower());
            }
            else
            {
                return allowedExtensions.Contains(file.ArquivoAnexado.Extension.ToLower());
            }
        }

        private bool ValidarConverterPDF(long seqInscricao)
        {
            //Valida se é para converter para PDF
            var dadosIncricao = InscricaoDomainService.SearchProjectionByKey(
                new SMCSeqSpecification<Inscricao>(seqInscricao), x => new
                {
                    x.Processo.TipoProcesso.TermoAceiteConversaoPDF,
                    x.Processo.TipoProcesso.OrientacaoAceiteConversaoPDF
                });

            return !string.IsNullOrEmpty(dadosIncricao.OrientacaoAceiteConversaoPDF) &&
                   !string.IsNullOrEmpty(dadosIncricao.TermoAceiteConversaoPDF);
        }

        /// <summary>
        /// Retorna um objeto contendo:
        /// A lista de documentos obrigatórios (se tiverem sido carregados, contendo os arquivos, caso contrário não)
        /// A lista de documentos que estão em um grupo com upload obrigatório
        /// A lista de documentos opicionais cujo upload já foi realizado
        /// A lista de tipos de documentos cujo upload pode ser realizado mais de uma vez ou onde o upload não é obrigatório
        /// </summary>
        public InscricaoDocumentosUploadVO BuscarDocumentosInscricaoUpload(long seqInscricao)
        {
            var tiposDocumentos = TipoDocumentoService.BuscarTiposDocumentos();

            // Passo 1: Buscar documentos da Inscrição
            var documentosCarregados = BuscarDocumentosInscricao(seqInscricao);
            documentosCarregados = documentosCarregados.OrderBy(x => x.Seq).ToList();

            //Recuperação da configuração da etapa para ser usada para busca de documentos
            var specInscricaoKey = new SMCSeqSpecification<Inscricao>(seqInscricao);
            var dadosInscricao = this.InscricaoDomainService.SearchProjectionByKey(specInscricaoKey, x => new
            {
                x.SeqConfiguracaoEtapa,
                x.Inscrito.Sexo,
                x.ConfiguracaoEtapa.DescricaoTermoEntregaDocumentacao,
                x.Processo.TipoProcesso.OrientacaoAceiteConversaoPDF,
                x.Processo.TipoProcesso.TermoAceiteConversaoPDF
            });

            long seqConfiguracaoEtapa = dadosInscricao.SeqConfiguracaoEtapa;
            var sexoInscrito = dadosInscricao.Sexo;

            var documentosObrigatorios = documentosCarregados.Where(x => x.UploadObrigatorio && (!x.Sexo.HasValue || x.Sexo.Value == sexoInscrito)).ToList();
            var documentosOpcionaisCarregados = documentosCarregados.Where(x => !x.UploadObrigatorio).GroupBy(x => x.SeqDocumentoRequerido).Select(g => g.OrderBy(d => (d.SeqArquivoAnexado.HasValue ? 1 : 2)).FirstOrDefault()).ToList();
            var documentosAdicionaisCarregados = documentosCarregados.Where(x => !x.UploadObrigatorio || x.PermiteVarios).ToList();

            //Caso haja documento obrigatório com mais de um item, exibir apenas o primeiro como obrigatório
            ValidarDocumentosObrigatoriosComMaisDeUmItem(documentosObrigatorios, documentosOpcionaisCarregados);

            // Passo 2: Busca de documentos para inscrição que não fez upload algum
            //caso não haja documento registrados, buscar os documentos obrigatórios para o processo
            documentosObrigatorios = documentosObrigatorios.SMCAny()
               ? documentosObrigatorios
               : BuscarDocumentosObrigatorios(seqInscricao, seqConfiguracaoEtapa, sexoInscrito);

            documentosObrigatorios = BuscarDocumentosInscricaoSemUpload(documentosObrigatorios, tiposDocumentos);

            // Passo 3: Busca de documentos em grupos obrigatórios
            var documentosGruposObrigatorios = BuscarDocumentosEmGruposObrigatorios(seqConfiguracaoEtapa, documentosOpcionaisCarregados, tiposDocumentos);

            //documentosOpcionaisCarregados = documentosOpcionaisCarregados.SMCRemove(
            //    x => documentosGruposObrigatorios.Any(d => d.Seq == x.Seq)
            //    // Task 32258:TSK Documentos adicionais, serão todos que permitem vários
            //    // removo os itens adicionais da lista de documentos opcionais
            //    || documentosAdicionaisCarregados.Any(d => d.Seq == x.Seq)
            //    ).ToList();

            documentosOpcionaisCarregados = documentosOpcionaisCarregados.SMCRemove(
                x => documentosGruposObrigatorios.Any(d => d.Seq == x.Seq) ||
                documentosGruposObrigatorios.SelectMany(d => d.DocumentosRequeridosGrupo).Select(d => d.Seq).Contains(x.SeqDocumentoRequerido) ||
                documentosObrigatorios.Select(d => d.SeqDocumentoRequerido).Contains(x.SeqDocumentoRequerido)
            ).ToList();

            documentosAdicionaisCarregados = documentosAdicionaisCarregados.SMCRemove(
                x => documentosObrigatorios.Any(d => d.Seq == x.Seq)
                || documentosOpcionaisCarregados.Any(d => d.Seq == x.Seq)
                || documentosGruposObrigatorios.Any(d => d.Seq == x.Seq)
                || documentosGruposObrigatorios.SelectMany(d => d.DocumentosRequeridosGrupo).Select(d => d.Seq).Contains(x.SeqDocumentoRequerido)
                ).ToList();

            return new InscricaoDocumentosUploadVO
            {
                DocumentosEmGruposObrigatorios = documentosGruposObrigatorios,
                DocumentosOpcionais = documentosOpcionaisCarregados != null ? documentosOpcionaisCarregados.ToList() : null,
                DocumentosObrigatorios = documentosObrigatorios.ToList(),
                DescricaoTermoEntregaDocumentacao = dadosInscricao.DescricaoTermoEntregaDocumentacao,
                // Passo 4 : Buscar a lista de documentos opcionais e documentos obrigatórios que permitim multiplos documentos
                TiposDocumentosOpcionais = BuscarDocumentosOpcionais(seqConfiguracaoEtapa, tiposDocumentos, sexoInscrito),
                DocumentosAdicionais = documentosAdicionaisCarregados,
                TiposDocumentosAdicionais = BuscarTipoDocumentosAdicionais(seqConfiguracaoEtapa),
                ExibirMensagemInformativaConversaoPDF = (!string.IsNullOrEmpty(dadosInscricao.OrientacaoAceiteConversaoPDF) &&
                                                         !string.IsNullOrEmpty(dadosInscricao.TermoAceiteConversaoPDF))
            };
        }

        #region [ Métodos privados para Buscar Documentos Inscrição Upload ]

        /// <summary>
        /// Caso haja documento obrigatório com mais de um item, exibir apenas o primeiro como obrigatório
        /// </summary>
        /// <param name="documentosObrigatorios"></param>
        /// <param name="documentosOpcionaisCarregados"></param>
        private void ValidarDocumentosObrigatoriosComMaisDeUmItem(List<InscricaoDocumentoVO> documentosObrigatorios, List<InscricaoDocumentoVO> documentosOpcionaisCarregados)
        {
            List<long> seqsDocumentosObrigatoriosExistentes = new List<long>();
            for (int i = 0; i < documentosObrigatorios.Count; i++)
            {
                if (seqsDocumentosObrigatoriosExistentes.Contains(documentosObrigatorios[i].SeqDocumentoRequerido))
                {
                    documentosOpcionaisCarregados.Add(documentosObrigatorios[i]);
                    documentosObrigatorios.RemoveAt(i);
                    i--;
                }
                else
                {
                    seqsDocumentosObrigatoriosExistentes.Add(documentosObrigatorios[i].SeqDocumentoRequerido);
                }
            }
        }

        /// <summary>
        /// Busca de documentos para inscrição que não fez upload algum
        /// </summary>
        /// <param name="documentosObrigatorios"></param>
        /// <param name="tiposDocumentos"></param>
        /// <returns></returns>
        private List<InscricaoDocumentoVO> BuscarDocumentosInscricaoSemUpload(List<InscricaoDocumentoVO> documentosObrigatorios, TipoDocumentoData[] tiposDocumentos)
        {
            documentosObrigatorios = documentosObrigatorios.SMCRemove(x => !x.UploadObrigatorio).ToList();

            foreach (var item in documentosObrigatorios)
            {
                if (item.ArquivoAnexado != null) item.ArquivoAnexado.Description = item.DescricaoArquivoAnexado;
                item.DescricaoTipoDocumento = tiposDocumentos.FirstOrDefault(x => x.Seq == item.SeqTipoDocumento).Descricao;
            }
            //Objeto de retorno
            return documentosObrigatorios.OrderBy(x => x.DescricaoTipoDocumento).ToList();
        }

        private List<InscricaoDocumentoVO> BuscarDocumentosObrigatorios(long seqInscricao, long seqConfiguracaoEtapa, Sexo? sexoInscrito)
        {
            DocumentoRequeridoFilterSpecification docSpec = new DocumentoRequeridoFilterSpecification
            {
                SeqConfiguracaoEtapa = seqConfiguracaoEtapa,
                UploadObrigatorio = true,
            };

            return this.DocumentoRequeridoDomainService.SearchProjectionBySpecification(docSpec,
                x => new InscricaoDocumentoVO
                {
                    SeqInscricao = seqInscricao,
                    SeqDocumentoRequerido = x.Seq,
                    SeqTipoDocumento = x.SeqTipoDocumento,
                    UploadObrigatorio = x.UploadObrigatorio && (!x.Sexo.HasValue || (sexoInscrito.HasValue && x.Sexo.Value == sexoInscrito.Value)),
                }).ToList();
        }

        private IEnumerable<InscricaoDocumentoVO> BuscarDocumentosInscricao(long seqInscricao)
        {
            InscricaoDocumentoFilterSpecification spec = new InscricaoDocumentoFilterSpecification
            {
                SeqInscricao = seqInscricao,
                PermiteUploadArquivo = true,
            };
            return this.SearchProjectionBySpecification(spec, x => new InscricaoDocumentoVO
            {
                Seq = x.Seq,
                SeqInscricao = x.SeqInscricao,
                SeqDocumentoRequerido = x.SeqDocumentoRequerido,
                SeqTipoDocumento = x.DocumentoRequerido.SeqTipoDocumento,
                SeqArquivoAnexado = x.SeqArquivoAnexado,
                DescricaoArquivoAnexado = x.DescricaoArquivoAnexado,
                ArquivoAnexado = x.ArquivoAnexado != null ? new SMCUploadFile
                {
                    GuidFile = x.ArquivoAnexado.Seq.ToString(),
                    Type = x.ArquivoAnexado.Tipo,
                    Name = x.ArquivoAnexado.Nome,
                    Size = x.ArquivoAnexado.Tamanho,
                } : null,
                UploadObrigatorio = x.DocumentoRequerido.UploadObrigatorio,
                PermiteVarios = x.DocumentoRequerido.PermiteVarios,
                DescricaoTipoDocumento = x.DocumentoRequerido.TipoDocumento.Descricao,
                Sexo = x.DocumentoRequerido.Sexo,
                EntregaPosterior = x.EntregaPosterior,
                ExibeTermoResponsabilidadeEntrega = x.DocumentoRequerido.ExibeTermoResponsabilidadeEntrega,
                DataLimiteEntrega = x.DocumentoRequerido.DataLimiteEntrega
            }).ToList();
        }

        private List<InscricaoDocumentoGruposVO> BuscarDocumentosEmGruposObrigatorios(long seqConfiguracaoEtapa, List<InscricaoDocumentoVO> documentosOpcionaisCarregados, TipoDocumentoData[] tiposDocumentos)
        {
            List<InscricaoDocumentoGruposVO> documentosGruposObrigatorios = new List<InscricaoDocumentoGruposVO>();
            var grupoSpec = new GrupoDocumentoRequeridoFilterSpecification
            {
                SeqConfiguracaoEtapa = seqConfiguracaoEtapa,
                UploadObrigatorio = true,
            };
            var gruposDocumentos = GrupoDocumentoRequeridoDomainService.SearchBySpecification(grupoSpec,
                x => x.Itens, x => x.Itens[0].DocumentoRequerido);
            foreach (var grupo in gruposDocumentos)
            {
                //Gera o vo do grupo
                var listaItemsGrupo = new List<InscricaoDocumentoGruposVO>();
                foreach (var item in grupo.Itens)
                {
                    if (listaItemsGrupo.Count < grupo.MinimoObrigatorio)
                    {
                        InscricaoDocumentoVO documentoGrupoCarregado =
                            documentosOpcionaisCarregados.FirstOrDefault(x => x.SeqDocumentoRequerido == item.SeqDocumentoRequerido && (x.EntregaPosterior || x.SeqArquivoAnexado.HasValue));
                        if (documentoGrupoCarregado != null)
                        {
                            InscricaoDocumentoGruposVO itemGrupoVO = new InscricaoDocumentoGruposVO(documentoGrupoCarregado);
                            itemGrupoVO.DescricaoGrupoDocumentos = grupo.Descricao;
                            itemGrupoVO.SeqGrupoDocumentoRequerido = grupo.Seq;
                            listaItemsGrupo.Add(itemGrupoVO);
                            //adiciona um item já carregado do grupo
                        }
                    }
                }
                //Verifica se o número mínimo foi satisfeito, se não for adiciona o número de itens necessários
                int minimoCriar = grupo.MinimoObrigatorio - listaItemsGrupo.Count;
                for (int i = 0; i < minimoCriar; i++)
                {
                    InscricaoDocumentoGruposVO grupoVO = new InscricaoDocumentoGruposVO
                    {
                        DescricaoGrupoDocumentos = grupo.Descricao,
                        SeqGrupoDocumentoRequerido = grupo.Seq
                    };
                    listaItemsGrupo.Add(grupoVO);
                }
                //Preenche os itens possíveis para seleção no grupo
                var itensPermitidosGrupo = grupo.Itens.Select(x => new SMCDatasourceItem
                {
                    Seq = x.SeqDocumentoRequerido,
                    Descricao = tiposDocumentos.FirstOrDefault(t => t.Seq == x.DocumentoRequerido.SeqTipoDocumento).Descricao
                }).ToList();
                foreach (var item in listaItemsGrupo)
                {
                    item.DocumentosRequeridosGrupo = itensPermitidosGrupo;
                    item.ExibeTermoResponsabilidadeEntrega = grupo.ExibeTermoResponsabilidadeEntrega;
                    item.DataLimiteEntrega = grupo.DataLimiteEntrega;
                }
                documentosGruposObrigatorios.AddRange(listaItemsGrupo);
                //Adiciona os itens do grupo a lista de retorno
            }

            return documentosGruposObrigatorios.OrderBy(x => x.DescricaoGrupoDocumentos).ToList();
        }

        /// <summary>
        /// Busca os documentos requeridos com as descrições dos tipos documentos que permitem adicionar mais de um documento (Permitem vários)
        /// </summary>
        /// <param name="seqConfiguracaoEtapa"></param>
        /// <returns></returns>
        private List<SMCDatasourceItem> BuscarTipoDocumentosAdicionais(long seqConfiguracaoEtapa)
        {
            var specTipoDocumentosAdicionais = new DocumentoRequeridoFilterSpecification() { SeqConfiguracaoEtapa = seqConfiguracaoEtapa, PermiteUploadArquivo = true, PermiteVarios = true };
            var listaItensAdicionais = DocumentoRequeridoDomainService.SearchProjectionBySpecification(specTipoDocumentosAdicionais,
                x => new SMCDatasourceItem()
                {
                    Seq = x.Seq,
                    Descricao = x.TipoDocumento.Descricao
                }).ToList();

            return listaItensAdicionais;
        }

        #endregion [ Métodos privados para Buscar Documentos Inscrição Upload ]

        public List<SMCDatasourceItem> BuscarDocumentosOpcionais(long seqConfiguracaoEtapa, TipoDocumentoData[] tiposDocumentos = null, Sexo? sexoInscrito = null)
        {
            DocumentoRequeridoOpcionalOuMultiploSpecification opcionaisSpec =
                new DocumentoRequeridoOpcionalOuMultiploSpecification(seqConfiguracaoEtapa);
            if (sexoInscrito.HasValue) opcionaisSpec.Sexo = sexoInscrito;
            GrupoDocumentoRequeridoFilterSpecification grupoSpec =
                new GrupoDocumentoRequeridoFilterSpecification
                {
                    SeqConfiguracaoEtapa = seqConfiguracaoEtapa,
                    UploadObrigatorio = true
                };

            var grupos = GrupoDocumentoRequeridoDomainService.SearchProjectionBySpecification(grupoSpec,
               x => x.Itens);
            List<long> seqDocsRequeridosEmGrupo = new List<long>();
            foreach (var item in grupos)
            {
                seqDocsRequeridosEmGrupo.AddRange(item.Select(x => x.SeqDocumentoRequerido));
            }
            var containsSpec = new SMCContainsSpecification<DocumentoRequerido, long>(x => x.Seq, seqDocsRequeridosEmGrupo.ToArray());
            var docsOpcionais = DocumentoRequeridoDomainService.SearchProjectionBySpecification(opcionaisSpec & !containsSpec,
                x => new
                {
                    SeqDocumentoRequerido = x.Seq,
                    SeqTipoDocumento = x.SeqTipoDocumento
                });
            if (tiposDocumentos == null) tiposDocumentos = TipoDocumentoService.BuscarTiposDocumentos();
            //Objeto de retorno
            var listaItemsOpcionais = docsOpcionais.Select(x => new SMCDatasourceItem
            {
                Seq = x.SeqDocumentoRequerido,
                Descricao = tiposDocumentos.FirstOrDefault(t => t.Seq == x.SeqTipoDocumento).Descricao
            }).ToList();
            return listaItemsOpcionais;
        }

        /// <summary>
        /// NV05
        /// Listar as situações de acordo com as regras dos tokens:
        /// - Validação outro setor
        /// - Validação setor responsável
        /// </summary>
        /// <param name="documentoRequerido"></param>
        /// <returns></returns>
        public List<SMCDatasourceItem<string>> BuscarSituacoesEntregaDocumento(DocumentoRequeridoVO documentoRequerido, long seqInscricao)
        {
            /* Task 37625:
             * No campo Validação: se a situação atual da inscrição for "Inscrição iniciada", as situações "Aguardando análise do setor responsável", "Deferido", "Indeferido e "Pendente"
             * não deverão ser listadas. */
            var keyInscricaoSituacaoCache = $"__SMC_CACHE_SITUACAO_INSCRICAO_{seqInscricao}";
            var keyInscricaoSituacaoFinalizadaCache = $"__SMC_CACHE_SITUACAO_INSCRICAO_FINALIZADA_{seqInscricao}";

            var tokenSituacaoAtual = SMCCacheManager.Get(keyInscricaoSituacaoCache)?.ToString();
            bool possuiSituacaoFinalizada = (bool?)SMCCacheManager.Get(keyInscricaoSituacaoFinalizadaCache) ?? false;

            if (string.IsNullOrEmpty(tokenSituacaoAtual))
            {
                var dadosInscricao = InscricaoDomainService.SearchProjectionByKey(seqInscricao, x => new
                {
                    SeqInscrito = x.SeqInscrito,
                    x.HistoricosSituacao.FirstOrDefault(h => h.Atual).TipoProcessoSituacao.Token,
                    PossuiFinlizada = x.HistoricosSituacao.Any(h => h.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_FINALIZADA)
                });

                tokenSituacaoAtual = dadosInscricao.Token;
                possuiSituacaoFinalizada = dadosInscricao.PossuiFinlizada;

                SMCCacheManager.Add(keyInscricaoSituacaoCache, tokenSituacaoAtual, new TimeSpan(0, 1, 0));
                SMCCacheManager.Add(keyInscricaoSituacaoFinalizadaCache, possuiSituacaoFinalizada, new TimeSpan(0, 1, 0));
            }
            bool situacaoInscricaoIniciada = tokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_INICIADA && !possuiSituacaoFinalizada;

            var lista = new List<SMCDatasourceItem<string>>();

            if (!SMCSecurityHelper.Authorize(UC_INS_003_01_05.VALIDACAO_OUTRO_SETOR) && !SMCSecurityHelper.Authorize(UC_INS_003_01_05.VALIDACAO_SETOR_RESPONSAVEL))
                return lista;

            /* Se o usuário logado possuir os dois tokens, exibir;
                * Aguardando análise do setor responsável;
                • Aguardando entrega;
                • Aguardando validação;
                • Deferido;
                • Indeferido;
                • Verificar se o documento pode ser entregue posteriormente. Se sim, exibir também a situação: Pendente*/
            if (SMCSecurityHelper.Authorize(UC_INS_003_01_05.VALIDACAO_OUTRO_SETOR) && SMCSecurityHelper.Authorize(UC_INS_003_01_05.VALIDACAO_SETOR_RESPONSAVEL))
            {
                if (!situacaoInscricaoIniciada)
                {
                    if (documentoRequerido.ValidacaoOutroSetor)
                        lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.AguardandoAnaliseSetorResponsavel));

                    lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.Deferido));
                    lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.Indeferido));
                    if (documentoRequerido.PermiteEntregaPosterior)
                        lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.Pendente));
                }

                lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.AguardandoEntrega));
                lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.AguardandoValidacao));

                return lista.OrderBy(a => a.Descricao).ToList();
            }

            /* Validação outro setor
                UC_INS_003_01_05.Validaca o_Outro_Setor
                Se a pessoa logada tiver permissão nesse token, verificar se o documento requerido em questão
                foi parametrizado para ser validado por outro setor.*/
            else if (SMCSecurityHelper.Authorize(UC_INS_003_01_05.VALIDACAO_OUTRO_SETOR))
            {
                if (documentoRequerido.ValidacaoOutroSetor)
                {
                    /* 1. Se foi, exibir as situações:
                        - Aguardando entrega;
                        - Aguardando validação;
                        - Deferido;
                        - Indeferido;
                        - Verificar se o documento pode ser entregue posteriormente. Se sim, exibir também a situação: Pendente.*/

                    if (!situacaoInscricaoIniciada)
                    {
                        lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.AguardandoAnaliseSetorResponsavel));
                        lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.Deferido));
                        lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.Indeferido));
                        if (documentoRequerido.PermiteEntregaPosterior)
                            lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.Pendente));
                    }
                    lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.AguardandoEntrega));
                    lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.AguardandoValidacao));
                }
                else
                {
                    /* 2. Se não foi, todos os campos deverão ser exibidos desabilitados e
                     * preenchidos com os valores atuais da situação do documento em questão.*/
                    documentoRequerido.InscricaoDocumentos.ForEach(d => d.BloquearTodosOsCampos = true);
                }
            }

            /*  Validação setor responsável
                UC_INS_003_01_05.Validaca o_Setor_Responsavel
                Se a pessoa logada tiver permissão nesse token, verificar se o documento requerido em questão foi
                parametrizado para ser validado por outro setor.*/
            else if (SMCSecurityHelper.Authorize(UC_INS_003_01_05.VALIDACAO_SETOR_RESPONSAVEL))
            {
                if (documentoRequerido.ValidacaoOutroSetor)
                {
                    var deferidoOuIndeferido = false;
                    documentoRequerido?.InscricaoDocumentos?.SMCForEach(d =>
                    {
                        /* 1. Se foi, verificar se a situação atual do documento é Deferido ou Indeferido
                        * Se sim, exibir todos os campos referentes ao documento em questão desabilitados e preenchidos com os valores
                        * atuais da situação do documento;*/
                        if (d.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Deferido ||
                            d.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Indeferido)
                        {
                            d.BloquearTodosOsCampos = true;
                            deferidoOuIndeferido = true;
                        }
                    });

                    if (deferidoOuIndeferido)
                    {
                        if (!situacaoInscricaoIniciada)
                        {
                            lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.Deferido));
                            lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.Indeferido));
                        }
                    }
                    else
                    {
                        /* Se não, exibir as situações:
                         * - Aguardando entrega;
                         * - Aguardando análise do setor responsável;
                         * - Verificar se o documento pode ser entregue posteriormente. Se sim, exibir também a situação: Pendente.*/
                        lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.AguardandoEntrega));
                        lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.AguardandoValidacao));
                        if (!situacaoInscricaoIniciada)
                        {
                            lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.AguardandoAnaliseSetorResponsavel));
                            if (documentoRequerido.PermiteEntregaPosterior)
                                lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.Pendente));
                        }
                    }
                }
                else
                {
                    /* 2. Se não foi, exibir as situações:
                        - Aguardando entrega;
                        - Aguardando validação;
                        - Deferido;
                        - Indeferido;
                        - Verificar se o documento pode ser entregue posteriormente. Se sim, exibir também a situação: Pendente.*/

                    if (!situacaoInscricaoIniciada)
                    {
                        lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.Deferido));
                        lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.Indeferido));
                        if (documentoRequerido.PermiteEntregaPosterior)
                            lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.Pendente));
                    }
                    lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.AguardandoEntrega));
                    lista.Add(BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento.AguardandoValidacao));
                }
            }

            return lista.OrderBy(l => l.Descricao).ToList();
        }

        private SMCDatasourceItem<string> BuscarSituacaoEntregaDocumento(SituacaoEntregaDocumento situacaoEntregaDocumento)
        {
            return new SMCDatasourceItem<string> { Seq = situacaoEntregaDocumento.ToString(), Descricao = SMCEnumHelper.GetDescription(situacaoEntregaDocumento) };
        }

        /// <summary>
        /// Converte lista de documentos em PDF 
        /// </summary>
        /// <param name="documentos">Lista de documentos as serem convertidos</param>
        private void ConverterArquivosPDF(List<InscricaoDocumento> documentos)
        {

            foreach (var documento in documentos)
            {
                if (documento.ArquivoAnexado != null && documento.ArquivoAnexado.Conteudo != null)
                {
                    //verifica se ja não é um PDF
                    var isPDF = documento.ArquivoAnexado.Tipo.ToLower().Contains("pdf") || documento.ArquivoAnexado.Tipo.ToLower().Contains("xml");
                    if (!isPDF)
                    {
                        var options = new SMCPdfSaveOptions()
                        {
                            File = new SMCPdfFile()
                            {
                                FileData = documento.ArquivoAnexado.Conteudo,
                                Type = documento.ArquivoAnexado.Extension,
                                Name = documento.ArquivoAnexado.Nome
                            }
                        };
                        //Faz alteração do dados do arquivo que foi anexado
                        documento.ArquivoAnexado.Nome = NovoNomeArquivo(documento.ArquivoAnexado.Nome);
                        documento.ArquivoAnexado.Extension = ".pdf";
                        documento.ArquivoAnexado.Description = documento.ArquivoAnexado.Nome;
                        documento.ArquivoAnexado.Tipo = "application/pdf";
                        documento.ArquivoAnexado.Conteudo = SMCPDFHelper.ConvertFilePDF(options);
                        documento.ArquivoAnexado.Tamanho = documento.ArquivoAnexado.Conteudo.Length;
                        documento.ConvertidoParaPDF = true;
                    }
                }
            }
        }

        /// <summary>
        /// Altera o nome do arquivo
        /// </summary>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <returns>Novo nome do arquivo</returns>
        private string NovoNomeArquivo(string nomeArquivo)
        {
            string[] splitNomeArquivo = nomeArquivo.Split('.');
            string extensaoArquivo = splitNomeArquivo.LastOrDefault();
            string retorno = nomeArquivo.Replace(extensaoArquivo, "pdf");
            return retorno;
        }
    }
}