using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.UI.Mvc.Util;
using SMC.Framework.Util;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.GPI.Administrativo.Areas.RES.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class ProcessoController : SMCControllerBase
    {
        #region Serviços

        private ProcessoControllerService ProcessoControllerService
        {
            get { return this.Create<ProcessoControllerService>(); }
        }

        private UnidadeResponsavelControllerService UnidadeResponsavelControllerService
        {
            get { return this.Create<UnidadeResponsavelControllerService>(); }
        }

        private ClienteControllerService ClienteControllerService
        {
            get { return this.Create<ClienteControllerService>(); }
        }

        private TipoProcessoControllerService TipoProcessoControllerService
        {
            get { return this.Create<TipoProcessoControllerService>(); }
        }

        private ConfiguracaoEtapaControllerService ConfiguracaoEtapaControllerService
        {
            get { return this.Create<ConfiguracaoEtapaControllerService>(); }
        }

        private ConfiguracaoEtapaPaginaControllerService ConfiguracaoEtapaPaginaControllerService
        {
            get { return this.Create<ConfiguracaoEtapaPaginaControllerService>(); }
        }

        private IHierarquiaOfertaService HierarquiaOfertaService
        {
            get { return Create<IHierarquiaOfertaService>(); }
        }

        private IProcessoCampoInscritoService ProcessoCampoInscritoService
        {
            get { return Create<IProcessoCampoInscritoService>(); }
        }

        private ITipoProcessoCampoInscritoService TipoProcessoCampoInscritoService
        {
            get { return Create<ITipoProcessoCampoInscritoService>(); }
        }
        private IProcessoService ProcessoService
        {
            get { return Create<IProcessoService>(); }
        }
        private IUnidadeResponsavelService UnidadeResponsavelService
        {
            get { return Create<IUnidadeResponsavelService>(); }
        }

        private TipoProcessoService TipoProcessoService
        {
            get { return this.Create<TipoProcessoService>(); }
        }

        private IViewEventoSaeService ViewEnventoSaeService
        {
            get { return this.Create<IViewEventoSaeService>(); }
        }

        #endregion Serviços

        #region Listagem

        /// <summary>
        /// Exibe tela de listagem de processo
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_001_01_01.PESQUISAR_PROCESSO)]
        public ActionResult Index(ProcessoFiltroViewModel filtros = null)
        {
            if (filtros == null)
            {
                filtros = new ProcessoFiltroViewModel();
            }
            PreencherFiltros(filtros);
            return View(filtros);
        }

        /// <summary>
        /// Action para listar os processo
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_001_01_01.PESQUISAR_PROCESSO)]
        public ActionResult ListarProcesso(ProcessoFiltroViewModel filtros)
        {
            // Caso tenha sido clicado no botão limpar do filtro, retorna a mensagem padrão de selecione o filtro
            if (CheckPostClearSubmit())
                return DisplayFilterMessage();

            SMCPagerModel<ProcessoListaViewModel> pager = this.ProcessoControllerService.BuscarProcessos(filtros);
            return PartialView("_ListarProcesso", pager);
        }

        /// <summary>
        /// Exibe o cabecalho contendo informações sobre o processo
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <param name="action">Action que será chamada conforme a opção do menu</param>
        /// <param name="exibirInformacoesCabecalho">Condicional para exibição das informações do processo no cabeçalho</param>
        /// <param name="exibirBotaoConfiguracaoCabecalho">Condicional para exibição do botão de configuração no cabeçalho</param>
        /// <returns></returns>
        [SMCAllowAnonymous]
        public ActionResult CabecalhoProcesso(SMCEncryptedLong seqProcesso, ProcessoActionsEnum action = ProcessoActionsEnum.Nenhum, bool exibirInformacoesCabecalho = false, bool exibirBotaoConfiguracaoCabecalho = false)
        {
            CabecalhoProcessoViewModel modelo = this.ProcessoControllerService.BuscarCabecalhoProcesso(seqProcesso);

            modelo.Seq = seqProcesso;
            modelo.Action = action;
            modelo.ExibirInformacoesCabecalho = exibirInformacoesCabecalho;
            modelo.ExibirBotaoConfiguracaoCabecalho = exibirBotaoConfiguracaoCabecalho;

            return PartialView("_CabecalhoProcesso", modelo);
        }

        #endregion Listagem

        #region Edição/Inclusão

        /// <summary>
        /// Exibe a tela de inclusão de um processo
        /// </summary>
        [HttpGet]
        [SMCAuthorize(UC_INS_001_01_02.MANTER_PROCESSO)]
        public ActionResult Incluir()
        {
            ProcessoViewModel modelo = new ProcessoViewModel();
            PreencherModeloInserir(modelo);
            return View(modelo);
        }

        /// <summary>
        /// Exibe tela de edição para um processo
        /// </summary>
        /// <param name="seq">Sequencial do processo a ser editado</param>
        [HttpGet]
        [SMCAuthorize(UC_INS_001_01_02.MANTER_PROCESSO)]
        public ActionResult Editar(SMCEncryptedLong seq)
        {
            ProcessoViewModel modelo = this.ProcessoControllerService.BuscarProcesso(seq);
            modelo.TokenCssAlternativoSas = ProcessoControllerService.BuscaTokenCssAlternativo(modelo.SeqUnidadeResponsavelTipoProcessoIdVisual);
            modelo.IdentidadesVisuais = this.UnidadeResponsavelService.BuscarIdentidadesVisuais(modelo.SeqUnidadeResponsavel, modelo.SeqTipoProcesso);
            PreencherModeloEditar(modelo);
            return View(modelo);
        }

        [SMCAllowAnonymous]
        public JsonResult BuscarIdentidadesVisuais(long seqUnidadeResponsavel, long seqTipoProcesso)
        {

            var identidades = this.UnidadeResponsavelService.BuscarIdentidadesVisuais(seqUnidadeResponsavel, seqTipoProcesso);

            return Json(identidades);
        }


        /// <summary>
        /// Salva um processo e redireciona o usuário para a tela de alteração
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_01_02.MANTER_PROCESSO)]
        public ActionResult Salvar(ProcessoViewModel modelo)
        {
            SalvarProcesso(modelo);
            return this.SaveEdit(modelo, this.ProcessoControllerService.SalvarProcesso, PreencherModeloEditar);
        }

        /// <summary>
        /// Salva um processo e redireciona o usuário para a tela de novo registro
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_01_02.MANTER_PROCESSO)]
        public ActionResult SalvarNovo(ProcessoViewModel modelo)
        {
            SalvarProcesso(modelo);
            return this.SaveNew(modelo, this.ProcessoControllerService.SalvarProcesso, PreencherModeloInserir);
        }

        /// <summary>
        /// Salva um processo e redireciona o usuário para a tela de listagem
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_01_02.MANTER_PROCESSO)]
        public ActionResult SalvarSair(ProcessoViewModel modelo)
        {
            SalvarProcesso(modelo);
            return this.SaveQuit(modelo, this.ProcessoControllerService.SalvarProcesso);
        }

        private void SalvarProcesso(ProcessoViewModel modelo)
        {
            // Executa apenas na edição.
            if (modelo.Seq.GetValueOrDefault() == 0)
                return;


            //// Verifica se taxas foram alteradas de "Por quantidade de ofertas" para outro tipo
            //ValidarAlteracaoCobranca(
            //    modelo.Taxas.Where(f => f.TipoCobrancaOriginal == TipoCobranca.PorQuantidadeOfertas && f.TipoCobranca != TipoCobranca.PorQuantidadeOfertas), modelo,
            //    Views.Processo.App_LocalResources.UIResource.Assert_AlteracaoCobrancaOfertaQuantidadeOfertaParaOutroTipo
            //);

            // Verifica se taxas foram alteradas de outro tipo para "Por quantidade de ofertas"
            //ValidarAlteracaoCobranca(
            //    modelo.Taxas.Where(taxa => taxa.TipoCobrancaOriginal != TipoCobranca.PorQuantidadeOfertas && taxa.TipoCobranca == TipoCobranca.PorQuantidadeOfertas),
            //    modelo,
            //    Views.Processo.App_LocalResources.UIResource.Assert_AlteracaoCobrancaOfertaOutroTipoParaQuantidadeOferta
            //);

            // UC_INS_001_01 - Processo - Inserir consistência alteração config. GAD documento -- comentado temporariamente
            // Verifica os arquivos sendo alterados
            //var existemDocumentosEmitidos = this.ProcessoControllerService.ValidarAssertDocumentoEmitido(modelo);
            //if (existemDocumentosEmitidos)
            //    Assert(modelo, Views.Processo.App_LocalResources.UIResource.Assert_DocumentosEmitidosAssociados);

        }

        // Método que realiza a validação de alterações de tipo de cobrança
        private void ValidarAlteracaoCobranca(IEnumerable<TaxaProcessoViewModel> taxas, ProcessoViewModel modelo, string mensagemAssert)
        {
            if (taxas.Any())
            {
                var descricaoTaxas = string.Join(", ", taxas.Select(taxa => $"\"{taxa.Descricao}\""));
                Assert(modelo, string.Format(mensagemAssert, descricaoTaxas));

            }
        }

        /// <summary>
        /// Preenche o modelo do processo para um novo processo
        /// </summary>
        /// <param name="modelo">Modelo a ser preenchido</param>
        [NonAction]
        private void PreencherModeloInserir(ProcessoViewModel modelo)
        {
            modelo.UnidadesResponsaveis = this.UnidadeResponsavelControllerService.BuscarUnidadesResponsaveisSelect();
            modelo.Clientes = this.ClienteControllerService.BuscarClientesSelect();
            // Monta a lista de tipo de telefone deixando todos os tipos
            if (modelo.TiposTelefone.SMCIsNullOrEmpty())
            {
                foreach (TipoTelefone item in Enum.GetValues(typeof(TipoTelefone)))
                {
                    if (item == TipoTelefone.Comercial || item == TipoTelefone.Fax)
                    {
                        modelo.TiposTelefone.Add(new SMCDatasourceItem()
                        {
                            Descricao = SMCEnumHelper.GetDescription(item),
                            Seq = Convert.ToInt64(item)
                        });
                    }
                }
            }

            //preencher a lista de campos do inscrito do Enum CampoInscrito
            if (modelo.ListaCamposInscrito.SMCIsNullOrEmpty())
            {
                modelo.ListaCamposInscrito = new List<SMCSelectListItem>();
                foreach (CampoInscrito item in Enum.GetValues(typeof(CampoInscrito)))
                {
                    if (item != CampoInscrito.Nenhum)
                    {
                        modelo.ListaCamposInscrito.Add(new SMCSelectListItem()
                        {
                            Text = string.IsNullOrEmpty(SMCEnumHelper.GetDescription(item)) ? item.ToString() : SMCEnumHelper.GetDescription(item),
                            Value = ((long)item).ToString(),
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Preenche o modelo do processo para um processo existente
        /// </summary>
        /// <param name="modelo">Modelo a ser preenchido</param>
        [NonAction]
        private void PreencherModeloEditar(ProcessoViewModel modelo)
        {
            modelo.UnidadesResponsaveis = this.UnidadeResponsavelControllerService.BuscarUnidadesResponsaveisSelect();
            modelo.TiposProcesso = this.UnidadeResponsavelControllerService.BuscarTiposProcessoAssociados(modelo.SeqUnidadeResponsavel);
            modelo.TiposHierarquiaOferta = this.UnidadeResponsavelControllerService.BuscarTiposHierarquiaOfertaAssociados(modelo.SeqUnidadeResponsavel, modelo.SeqTipoProcesso);
            modelo.TemplatesProcesso = this.TipoProcessoControllerService.BuscarTemplatesAssociados(modelo.SeqTipoProcesso);
            modelo.Clientes = this.ClienteControllerService.BuscarClientesSelect();
            modelo.TiposTaxa = this.TipoProcessoControllerService.BuscarTiposTaxaAssociados(modelo.SeqTipoProcesso);
            modelo.EventosGRA = this.ProcessoControllerService.BuscarEventosGRASelect(modelo.SeqUnidadeResponsavel);
            modelo.HabilitaDatasEvento = TipoProcessoControllerService.ConferirHabilitaDatasEvento(modelo.SeqTipoProcesso);
            modelo.EventosSae = ViewEnventoSaeService.BuscarEventosSaeSelect(modelo.SeqUnidadeResponsavel, modelo.AnoReferencia);
            // Documentos e Formulários
            modelo.TiposDocumento = this.TipoProcessoControllerService.BuscarTiposDocumentoSelect(modelo.SeqTipoProcesso);//Buscar descrição no GDM/TipoDocumento
            modelo.ConfiguracoesAssinaturaGad = this.ProcessoControllerService.BuscarConfiguracoesAssinaturaGadSelect(modelo.SeqUnidadeResponsavel);//Buscar descrição no GAD/ConfiguracoesDocumento
            modelo.TiposFormulario = UnidadeResponsavelControllerService.BuscarTiposFormularioAssociadosSelect(modelo.SeqUnidadeResponsavel);

            if (modelo.TiposFormulario != null)
            {
                foreach (var item in modelo.ConfiguracoesFormulario)
                {
                    modelo.Formularios = UnidadeResponsavelControllerService.BuscarFormulariosSelect(item.SeqTipoFormularioSgf);
                }
                foreach (var item in modelo.ConfiguracoesFormulario)
                {
                    modelo.VisoesFormulario = UnidadeResponsavelControllerService.BuscarVisoesSelect(item.SeqTipoFormularioSgf);
                }
            }


            // Monta a lista de tipo de telefone deixando todos os tipos
            if (modelo.TiposTelefone.SMCIsNullOrEmpty())
            {
                foreach (TipoTelefone item in Enum.GetValues(typeof(TipoTelefone)))
                {
                    if (item == TipoTelefone.Comercial || item == TipoTelefone.Fax)
                    {
                        modelo.TiposTelefone.Add(new SMCDatasourceItem()
                        {
                            Descricao = SMCEnumHelper.GetDescription(item),
                            Seq = Convert.ToInt64(item)
                        });
                    }
                }
            }

            var camposInscritoPorTipoProcesso = TipoProcessoCampoInscritoService.BuscarTiposProcessoCamposInscritoPorTipoProcesso(modelo.SeqTipoProcesso);
            //preencher a lista de campos do inscrito do Enum CampoInscrito
            if (modelo.ListaCamposInscrito.SMCIsNullOrEmpty())
            {
                modelo.ListaCamposInscrito = new List<SMCSelectListItem>();
                foreach (CampoInscrito item in Enum.GetValues(typeof(CampoInscrito)))
                {
                    if (item != CampoInscrito.Nenhum)
                    {
                        modelo.ListaCamposInscrito.Add(new SMCSelectListItem()
                        {
                            Text = SMCEnumHelper.GetDescription(item),
                            Value = Convert.ToInt64(item).ToString(),
                            IsDisabled = camposInscritoPorTipoProcesso.Any(a => a.CampoInscrito == item) ? true : false
                        });
                    }
                }
            }

            foreach (var item in camposInscritoPorTipoProcesso)
            {
                if (!modelo.CamposInscrito.Any(a => a == (long)item.CampoInscrito))
                {
                    modelo.CamposInscrito.Add((long)item.CampoInscrito);
                }
            }
        }

        /// <summary>
        /// Preenche o filtro dos processos
        /// </summary>
        /// <param name="modelo">Modelo a ser preenchido</param>
        [NonAction]
        private void PreencherFiltros(ProcessoFiltroViewModel modelo)
        {
            modelo.UnidadesResponsaveis = this.UnidadeResponsavelControllerService.BuscarUnidadesResponsaveisSelect();
            modelo.TiposProcesso = this.UnidadeResponsavelControllerService.BuscarTiposProcessoAssociados(modelo.SeqUnidadeResponsavel);
        }

        /// <summary>
        /// Recarrega os contatos do processo.
        /// </summary>
        /// <param name="seqUnidadeResponsavel">O sequencial da unidade responsável.</param>
        /// <returns>A partial view com os dados de contato do processo.</returns>
        [SMCAllowAnonymous]
        public ActionResult RecarregarContatosProcesso(long seqUnidadeResponsavel)
        {
            var modelo = this.UnidadeResponsavelControllerService.BuscarUnidadeResponsavel(seqUnidadeResponsavel);
            var modeloProcesso = new ProcessoViewModel
            {
                EnderecosEletronicos = modelo.EnderecosEletronicos,
                NomeContato = modelo.NomeContato,
                Telefones = modelo.Telefones
            };

            // Monta a lista de tipo de telefone deixando todos os tipos
            if (modeloProcesso.TiposTelefone.SMCIsNullOrEmpty())
            {
                foreach (TipoTelefone item in Enum.GetValues(typeof(TipoTelefone)))
                {
                    if (item == TipoTelefone.Comercial || item == TipoTelefone.Fax)
                    {
                        modeloProcesso.TiposTelefone.Add(new SMCDatasourceItem()
                        {
                            Descricao = SMCEnumHelper.GetDescription(item),
                            Seq = Convert.ToInt64(item)
                        });
                    }
                }
            }
            return PartialView("_DadosContatoProcesso", modeloProcesso);
        }

        [SMCAllowAnonymous]
        public JsonResult BuscarCamposInscritoTipoProcesso(long? seqTipoProcesso)
        {
            if (!seqTipoProcesso.HasValue)
                return Json(new List<long>());

            var camposInscritoTipoProcesso = TipoProcessoCampoInscritoService.BuscarTiposProcessoCamposInscritoPorTipoProcesso(seqTipoProcesso.Value);
            List<long> listaCamposInscrito = new List<long>();
            foreach (var item in camposInscritoTipoProcesso)
            {
                listaCamposInscrito.Add(Convert.ToInt64(item.CampoInscrito));
            }

            return Json(listaCamposInscrito);
        }

        [SMCAllowAnonymous]
        public ActionResult DownloadDocumento(string guidFile, string name, string type)
        {
            if (Guid.TryParse(guidFile, out Guid guid))
            {
                var data = SMCUploadHelper.GetFileData(new SMCUploadFile { GuidFile = guidFile });
                if (data != null)
                {
                    return File(data, type, name);
                }
            }

            var arquivo = this.ProcessoService.BuscarArquivoAnexadoConfigurancaoEmissaoDocumento(new SMCEncryptedLong(guidFile));

            Response.AppendHeader("Content-Disposition", "inline; filename=" + "\"" + arquivo.Name + "\"");

            return File(arquivo.FileData, arquivo.Type);
        }




        [SMCAllowAnonymous]
        public JsonResult BuscarEventosSae(long? seqUnidadeResponsavel, string AnoReferencia)
        {
            if (!seqUnidadeResponsavel.HasValue)
                return Json(new List<long>());

            var eventosSae = ViewEnventoSaeService.BuscarEventosSaeSelect(seqUnidadeResponsavel.Value, int.Parse(AnoReferencia)).TransformList<SMCSelectListItem>();

            return Json(eventosSae);
        }

        #endregion Edição/Inclusão

        #region Excluir

        /// <summary>
        /// Action para excluir um processo
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo a ser excluído</param>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_01_02.MANTER_PROCESSO)]
        public ActionResult Excluir(SMCEncryptedLong seqProcesso)
        {
            ProcessoControllerService.ExcluirProcesso(seqProcesso);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                                            MessagesResource.Entidade_Processo),
                                MessagesResource.Titulo_Sucesso,
                                SMCMessagePlaceholders.Centro);

            return RedirectToAction("ListarProcesso");
        }

        #endregion Excluir

        #region Selects

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarTiposProcesso(long? SeqUnidadeResponsavel)
        {
            var listaTiposProcesso = this.UnidadeResponsavelControllerService
                .BuscarTiposProcessoAssociados(SeqUnidadeResponsavel);

            return Json(listaTiposProcesso);
        }

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarEventos(long SeqUnidadeResponsavel)
        {
            var listaEventos = this.ProcessoControllerService.BuscarEventosGRASelect(SeqUnidadeResponsavel);

            return Json(listaEventos);
        }

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarTiposHierarquiaOferta(long SeqTipoProcesso, long SeqUnidadeResponsavel)
        {
            var listaTiposHierarquiaOferta = this.UnidadeResponsavelControllerService.
                BuscarTiposHierarquiaOfertaAssociados(SeqUnidadeResponsavel, SeqTipoProcesso);
            return Json(listaTiposHierarquiaOferta);
        }

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarTiposTaxa(long SeqTipoProcesso)
        {
            var listaTiposTaxa = this.TipoProcessoControllerService.BuscarTiposTaxaAssociados(SeqTipoProcesso);
            return Json(listaTiposTaxa);
        }

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarTemplatesProcesso(long SeqTipoProcesso)
        {
            var listaTemplatesProcesso = this.TipoProcessoControllerService.BuscarTemplatesAssociados(SeqTipoProcesso);
            return Json(listaTemplatesProcesso);
        }

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarTiposDocumentoSelect(long SeqTipoProcesso)
        {
            var retorno = TipoProcessoControllerService.BuscarTiposDocumentoSelect(SeqTipoProcesso);
            return Json(retorno);
        }

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarConfiguracoesAssinaturaGadSelect(long SeqUnidadeResponsavel)
        {
            var retorno = ProcessoControllerService.BuscarConfiguracoesAssinaturaGadSelect(SeqUnidadeResponsavel);
            return Json(retorno);
        }

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarTiposFormularioAssociadosSelect(long SeqUnidadeResponsavel)
        {
            var retorno = UnidadeResponsavelControllerService.BuscarTiposFormularioAssociadosSelect(SeqUnidadeResponsavel);
            return Json(retorno);
        }

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarFormulariosSelect(long SeqTipoFormularioSgf)
        {
            var retorno = UnidadeResponsavelControllerService.BuscarFormulariosSelect(SeqTipoFormularioSgf);
            return Json(retorno);
        }

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarVisoesSelect(long SeqTipoFormularioSgf)
        {
            var retorno = UnidadeResponsavelControllerService.BuscarVisoesSelect(SeqTipoFormularioSgf);
            return Json(retorno);
        }

        #endregion Selects

        #region Copia Processo

        [SMCAuthorize(UC_INS_001_01_06.COPIAR_PROCESSO)]
        [HttpGet]
        public ActionResult CopiarProcesso(SMCEncryptedLong seqProcesso)
        {
            CopiaProcessoViewModel model = ProcessoControllerService.BuscarProcessoCopia(seqProcesso);
            model.MensagemInformativa = "";
            if (model.TemplateProcessoDesativado)
            {
                model.MensagemInformativa += SMC.GPI.Administrativo.Areas.INS.Views
                    .Processo.App_LocalResources.UIResource.Mensagem_Copia_Processo_Template_Desativado;
            }
            if (model.TipoHierarquiaOfertaDesativado)
            {
                model.CopiarItens = false;
                model.MensagemInformativa += SMC.GPI.Administrativo.Areas.INS.Views
                    .Processo.App_LocalResources.UIResource.Mensagem_Copia_Processo_Tipo_Hierarquia_Desativada;
            }
            if (model.TipoProcessoDesativado)
            {
                model.CopiarItens = false;
                model.MensagemInformativa += SMC.GPI.Administrativo.Areas.INS.Views
                    .Processo.App_LocalResources.UIResource.Mensagem_Copia_Processo_Tipo_Desativado;
            }
            return PartialView(model);
        }

        [SMCAuthorize(UC_INS_001_01_06.COPIAR_PROCESSO)]
        [HttpPost]
        public ActionResult CopiarProcesso(CopiaProcessoViewModel modelo)
        {
            ProcessoControllerService.CopiarProcesso(modelo);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Copia_Registro,
                MessagesResource.Entidade_Processo),
                MessagesResource.Titulo_Sucesso,
                SMCMessagePlaceholders.Centro);

            return SMCRedirectToAction("Index");
        }

        #endregion Copia Processo

        #region Dependencies

        [SMCAllowAnonymous]
        [HttpPost]
        public ActionResult BuscarItensHierarquiaProcesso(long seqProcesso, long seqTipoItem)
        {
            return Json(HierarquiaOfertaService.BuscarHierarquiaOfertasPorTipoKeyValue(seqProcesso, seqTipoItem));
        }

        [SMCAllowAnonymous]
        [HttpPost]
        public string BuscarIdsTagManager(long? seqTipoProcesso)
        {
            if (seqTipoProcesso.HasValue)
            {
                var tipoProcesso = TipoProcessoControllerService.BuscarTipoProcesso(seqTipoProcesso.Value);
                return tipoProcesso.IdsTagManager;
            }
            return string.Empty;
        }

        [SMCAllowAnonymous]
        [HttpPost]
        public ActionResult BuscarLinkProcesso(Guid uidProcesso, string idsTagManager)
        {
            var modelLink = new ProcessoViewModel
            {
                UidProcesso = uidProcesso,
                IdsTagManager = idsTagManager
            };
            return PartialView("_LinkProcesso", modelLink);
        }

        [SMCAllowAnonymous]
        [HttpPost]
        public JsonResult ConferirHabilitaDatasEvento(long SeqTipoProcesso)
        {
            var exibirDocumentosEFormulários = TipoProcessoControllerService.ConferirHabilitaDatasEvento(SeqTipoProcesso);
            return Json(exibirDocumentosEFormulários);
        }

        [SMCAllowAnonymous]
        [HttpPost]
        public JsonResult ConferirHabilitaGestaoEvento(long SeqTipoProcesso)
        {
            var exibirGestaoEvento = this.TipoProcessoService.ConferirHabilitaGestaoEvento(SeqTipoProcesso);
            return Json(exibirGestaoEvento);
        }

        #endregion Dependencies
    }
}