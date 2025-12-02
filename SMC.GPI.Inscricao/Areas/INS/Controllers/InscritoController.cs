using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.Util;
using SMC.GPI.Inscricao.Areas.INS.Models;
using SMC.GPI.Inscricao.Areas.INS.Services;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.UI.Mvc.Attributes;
using SMC.Localidades.ServiceContract.Areas.LOC.Data;
using SMC.Localidades.ServiceContract.Areas.LOC.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMC.GPI.Inscricao.Areas.INS.Controllers
{
    public class InscritoController : SMCControllerBase
    {
        private static readonly string ProcessoAtualSeq = "ProcessoAtualSeqFromLink";

        #region Serviços

        private InscritoControllerService InscritoControllerService
        {
            get { return Create<InscritoControllerService>(); }
        }

        private ProcessoControllerService ProcessoControllerService
        {
            get { return Create<ProcessoControllerService>(); }
        }

        private ILocalidadeService LocalidadeService
        {
            get { return Create<ILocalidadeService>(); }
        }

        private IInscritoService InscritoService
        {
            get { return Create<IInscritoService>(); }
        }

        private IProcessoCampoInscritoService ProcessoCampoInscritoService => Create<IProcessoCampoInscritoService>();

        #endregion Serviços

        #region Index

        /// <summary>
        /// Redireciona para a página Home
        /// </summary>
        [SMCAuthorize(UC_INS_002_01_01.MANTER_INSCRITO)]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }

        #endregion Index

        #region Cadastrar

        /// <summary>
		/// Exibe a tela de cadastro do inscrito
        /// Obs.: O modelo só é criado efetivamente no passo 1 do wizard.
		/// </summary>
        [GoogleTagManagerFilter]
        [HttpGet]
        [SMCAuthorize(UC_INS_002_01_01.MANTER_INSCRITO)]
        public ActionResult Cadastrar(SMCEncryptedLong origem = null, Guid? uidProcesso = null)
        {


            if (Session["GuidProcesso"] != null)
            {
                uidProcesso = Guid.Parse(Session["GuidProcesso"].ToString());
            }

            // Busca os dados do inscrito
            Guid UidProcesso = new Guid();
            long? seqProcesso = null;
            InscritoLGPDData dadosLGPD = new InscritoLGPDData();
            InscritoViewModel modelo = new InscritoViewModel();
            modelo = InscritoControllerService.BuscarInscrito();
            if (uidProcesso != null)
            {
                UidProcesso = (Guid)uidProcesso;
                SMCLanguage idioma = GetCurrentLanguage().HasValue ? this.GetCurrentLanguage().Value : SMCLanguage.Portuguese;
                ProcessoHomeViewModel modeloProcesso = new ProcessoHomeViewModel();
                modeloProcesso = ProcessoControllerService.BuscarProcessoHome(UidProcesso, idioma, null);
                seqProcesso = modeloProcesso.SeqProcesso;
                if (seqProcesso != null)
                {
                    dadosLGPD = InscritoService.BuscarInscritoLGPD(modelo.Seq, seqProcesso);
                    modelo.TermoLGPD = dadosLGPD.TermoLGPD;
                    modelo.ConsentimentoLGPD = dadosLGPD.ConsentimentoLGPD ?? false;
                }

                if (!string.IsNullOrEmpty(modeloProcesso.OrientacaoCadastroInscrito))
                {
                    modelo.EXibirOrientacaoInscrito = true;
                    modelo.OrientacaoCadastroInscrito = modeloProcesso.OrientacaoCadastroInscrito;
                }

                Session["__CSSLayout__"] = modeloProcesso.UrlCss;
            }

            modelo.Origem = origem;
            modelo.UidProcesso = uidProcesso;

            TempData["___seqProcessoAtual___"] = uidProcesso;

            if (Request.UrlReferrer != null)
            {
                modelo.BackUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                modelo.BackUrl = Request.Url.ToString();
            }


            return View(modelo);
        }

        ///<summary>
        /// Salva o cadastro do Inscrito
        /// Depois de salvar, chama a página Index, que redireciona para a Home
        ///</summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_002_01_01.MANTER_INSCRITO)]
        [GoogleTagManagerFilter]
        public ActionResult Cadastrar(InscritoViewModel modelo, string backUrl)
        {

            if (string.IsNullOrEmpty(modelo.TermoLGPD) && modelo.Seq == 0)
            {
                modelo.ConsentimentoLGPD = false;
            }

            // Verifica se a confirmação de email é igual ao email
            if (modelo.Email != modelo.EmailConfirmacao)
            {
                SetErrorMessage(Views.Inscrito.App_LocalResources.UIResource.Mensagem_Validacao_Email, "", SMCMessagePlaceholders.Centro);
                CancelClearErrorMessages = true;

                return InscritoStep2(modelo);
            }

            ValidarDadosVisiveisInscrito(modelo);

            // Salva o inscrito
            //if (Save(modelo, InscritoControllerService.SalvarInscrito))
            if (InscritoControllerService.SalvarInscrito(modelo) > 0)
            {
                // modelo.Origem está chegando 0 quando deveria estar null. Verifica por valores maiores que 0.
                if (modelo.Origem.GetValueOrDefault() > 0)
                {
                    return WizardRedirect("ContinuarInscricao", "Inscricao", new { area = "INS", seqInscricao = modelo.Origem.Value });
                }

                if (modelo.UidProcesso != null)
                {
                    TempData["___seqProcessoAtual___"] = modelo.UidProcesso;

                    if (string.IsNullOrEmpty(backUrl))
                        return WizardRedirect("IndexProcesso", "Home", new { area = "", uidProcesso = modelo.UidProcesso });



                    return WizardRedirectUrl(backUrl);
                }
                else
                {
                    return WizardRedirect("Index", "Home", new { area = "" });
                }
            }
            else
            {
                CancelClearErrorMessages = true;
                return InscritoStep2(modelo);
            }
        }

        [SMCAuthorize(UC_INS_002_01_01.MANTER_INSCRITO)]
        public ActionResult CancelarCadastro(SMCEncryptedLong origem, string uidProcesso, string backUrl)
        {
            TempData["___seqProcessoAtual___"] = uidProcesso;
            if (origem != null)
            {
                return RedirectToAction("ContinuarInscricao", "Inscricao", new { area = "INS", seqInscricao = origem.Value, uidProcesso = uidProcesso });
            }

            return Redirect(backUrl);

            //return RedirectToAction("Index", "Home", new { area = "", uidProcesso = uidProcesso });
        }

        #endregion Cadastrar

        #region Passos do Wizard

        /// <summary>
        /// Passo 1 do Wizard de Cadastro da Inscrição
        /// </summary>
        /// <param name="modelo">Modelo do wizard</param>
        [SMCAuthorize(UC_INS_002_01_01.MANTER_INSCRITO)]
        public ActionResult InscritoStep1(InscritoViewModel modelo)
        {
            this.PreencherModelo(modelo);
            if (modelo.CodigoPaisNacionalidade != null && modelo.CodigoPaisNacionalidade != 800)
            {
                modelo.HabilitaDesricaoNaturalidade = true;
            }
            modelo.Step = 0;
            return PartialView("_InscritoStep1", modelo);
        }
        [SMCAllowAnonymous]
        public JsonResult HabilitarDesricaoNaturalidade(int? CodigoPaisNacionalidade)
        {
            bool habilitaCampo = false;

            if (CodigoPaisNacionalidade != null && CodigoPaisNacionalidade != 800)
            {
                habilitaCampo = true;
            }

            return Json(habilitaCampo);
        }
        /// <summary>
        /// Passo 2 do Wizard de Cadastro da Inscrição
        /// </summary>
        /// <param name="modelo">Modelo do wizard</param>
        [SMCAuthorize(UC_INS_002_01_01.MANTER_INSCRITO)]
        public ActionResult InscritoStep2(InscritoViewModel modelo)
        {
            try
            {
                if(!modelo.ExibirNaturalidade)
                {
                    modelo.DescricaoNaturalidadeEstrangeira = null;
                }

                InscritoControllerService.ValidaInscritoPrimeiroPasso(modelo);
                if (!String.IsNullOrEmpty(modelo.NomeMae)) modelo.NomeMae = modelo.NomeMae.TrimEnd();
                if (!String.IsNullOrEmpty(modelo.NomePai)) modelo.NomePai = modelo.NomePai.TrimEnd();
                modelo.Nome = modelo.Nome.TrimEnd();
                this.PreencherModelo(modelo);
                modelo.Step = 1;

                if (!CancelClearErrorMessages)
                    this.ClearMessages(SMCMessagePlaceholders.Centro);

                return PartialView("_InscritoStep2", modelo);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message.Replace("\r\n", "<br />"), "", target: SMCMessagePlaceholders.Centro);
                return InscritoStep1(modelo);
            }
        }

        #endregion Passos do Wizard

        #region Propriedades de apoio

        private bool CancelClearErrorMessages { get; set; }

        #endregion Propriedades de apoio

        #region Métodos de Apoio

        /// <summary>
        /// Preenche as listas de select para o modelo.
        /// </summary>
        /// <param name="modelo">Modelo a ser preenchido</param>
		[NonAction]
        private void PreencherModelo(InscritoViewModel modelo)
        {
            // Busca a lista de estados para UF do RG
            modelo.EstadosIdentidade = BuscarEstadosSelect();

            // Busca a lista de paises para nacionalidade
            modelo.Paises = BuscarPaisesSelect();

            ConfigurarVisibilidadeCamposPorProcesso(modelo);

            // Monta a lista de tipo de endereço eletrônico excluindo o tipo email
            if (modelo.TiposEnderecoEletronico.SMCIsNullOrEmpty())
            {
                foreach (TipoEnderecoEletronico item in Enum.GetValues(typeof(TipoEnderecoEletronico)))
                {
                    if (item != TipoEnderecoEletronico.Email && item != TipoEnderecoEletronico.Nenhum)
                    {
                        modelo.TiposEnderecoEletronico.Add(new SMCDatasourceItem()
                        {
                            Descricao = SMCEnumHelper.GetDescription(item),
                            Seq = Convert.ToInt64(item)
                        });
                    }
                }
            }
        }

        #endregion Métodos de Apoio

        /// <summary>
        /// Busca uma lista de paises
        /// </summary>
        /// <returns>Lista de países</returns>
        [NonAction]
        private List<SMCDatasourceItem> BuscarPaisesSelect()
        {
            PaisData[] paises = LocalidadeService.BuscarPaisesValidosCorreios();
            List<SMCDatasourceItem> lista = new List<SMCDatasourceItem>();
            foreach (var pais in paises)
            {
                lista.Add(new SMCDatasourceItem(pais.Codigo, pais.Nome));
            }
            return lista;
        }

        /// <summary>
        /// Busca uma lista de estados
        /// </summary>
        /// <returns>Lista de estados</returns>
        [NonAction]
        private List<SMCSelectListItem> BuscarEstadosSelect()
        {
            UFData[] ufs = LocalidadeService.BuscarUfs();
            List<SMCSelectListItem> lista = new List<SMCSelectListItem>();
            foreach (var uf in ufs)
            {
                lista.Add(new SMCSelectListItem(uf.Codigo, uf.Codigo));
            }
            return lista;
        }

        /// <summary>
        /// Configura a visibilidade dos campos por processo
        /// </summary>
        /// <param name="modelo">Modelo do inscrito</param>
        private void ConfigurarVisibilidadeCamposPorProcesso(InscritoViewModel modelo)
        {

            if (modelo.UidProcesso.HasValue)
            {
                List<ProcessoCampoInscritoData> camposInscritoProcesso = ProcessoCampoInscritoService.BuscarCamposInscritosPorUIIDProcesso(modelo.UidProcesso.Value);

                foreach (var campo in camposInscritoProcesso)
                {
                    if (campo.CampoInscrito == CampoInscrito.CPF)
                    {
                        modelo.ExibirCPF = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.DataNascimento)
                    {
                        modelo.ExibirDataNascimento = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Email)
                    {
                        modelo.ExibirEmail = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Endereco)
                    {
                        modelo.ExibirEndereco = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.EstadoCivil)
                    {
                        modelo.ExibirEstadoCivil = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Filiacao)
                    {
                        modelo.ExibirFiliacao = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Nacionalidade)
                    {
                        modelo.ExibirNacionalidade = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Naturalidade)
                    {
                        modelo.ExibirNaturalidade = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Nome)
                    {
                        modelo.ExibirNome = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.NumeroIdentidade)
                    {
                        modelo.ExibirNumeroIdentidade = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.OrgaoEmissorIdentidade)
                    {
                        modelo.ExibirOrgaoEmissorIdentidade = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.OutrosEndereçosEletronicos)
                    {
                        modelo.ExibirOutrosEnderecosEletronicos = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.PaisOrigem)
                    {
                        modelo.ExibirPaisOrigem = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Passaporte)
                    {
                        modelo.ExibirPassaporte = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Sexo)
                    {
                        modelo.ExibirSexo = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Telefone)
                    {
                        modelo.ExibirTelefone = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.UfIdentidade)
                    {
                        modelo.ExibirUfIdentidade = true;
                    }
                }
            }
            else
            {
                modelo.ExibirCPF = true;
                modelo.ExibirDataNascimento = true;
                modelo.ExibirEmail = true;
                modelo.ExibirEndereco = true;
                modelo.ExibirEstadoCivil = true;
                modelo.ExibirFiliacao = true;
                modelo.ExibirNacionalidade = true;
                modelo.ExibirNaturalidade = true;
                modelo.ExibirNome = true;
                modelo.ExibirNumeroIdentidade = true;
                modelo.ExibirOrgaoEmissorIdentidade = true;
                modelo.ExibirOutrosEnderecosEletronicos = true;
                modelo.ExibirPaisOrigem = true;
                modelo.ExibirPassaporte = true;
                modelo.ExibirSexo = true;
                modelo.ExibirTelefone = true;
                modelo.ExibirUfIdentidade = true;
            }
        }

        private void ValidarDadosVisiveisInscrito(InscritoViewModel modelo)
        {
            ConfigurarVisibilidadeCamposPorProcesso(modelo);

            //valida se o inscrito é a primeira vez significa que todos os campos estão na tela ou
            //se não tem processo associado significa que todos os campos estão na tela
            if (modelo.Seq == 0 || !modelo.UidProcesso.HasValue)
            {
                if (!modelo.ExibirEndereco && modelo.Enderecos.SMCAny())
                {
                    modelo.Enderecos = null;
                }

                if (!modelo.ExibirTelefone && modelo.Telefones.SMCAny())
                {
                    modelo.Telefones = null;
                }

                return;
            }

            var inscritoInDB = InscritoControllerService.BuscarInscrito();
            //Mapeio o que se encontra na tela com o que está no banco
            modelo.CodigoPaisEmissaoPassaporte = modelo.ExibirPassaporte ? modelo.CodigoPaisEmissaoPassaporte : inscritoInDB.CodigoPaisEmissaoPassaporte;
            modelo.CodigoPaisNacionalidade = modelo.ExibirPaisOrigem ? modelo.CodigoPaisNacionalidade : inscritoInDB.CodigoPaisNacionalidade;
            modelo.Cpf = modelo.ExibirCPF ? modelo.Cpf : inscritoInDB.Cpf;
            modelo.DataNascimento = modelo.ExibirDataNascimento ? modelo.DataNascimento : inscritoInDB.DataNascimento;
            modelo.DataValidadePassaporte = modelo.ExibirPassaporte ? modelo.DataValidadePassaporte : inscritoInDB.DataValidadePassaporte;
            modelo.Email = modelo.ExibirEmail ? modelo.Email : inscritoInDB.Email;
            modelo.Enderecos = modelo.ExibirEndereco ? modelo.Enderecos : null;
            modelo.EnderecosEletronicos = modelo.ExibirOutrosEnderecosEletronicos ? modelo.EnderecosEletronicos : inscritoInDB.EnderecosEletronicos;

            //Para nacionalidades que sejam diferentes de Brasileira, o sistema deve limpar os campos UF e Cidade
            if (modelo.Nacionalidade != TipoNacionalidade.Brasileira)
            {
                modelo.EstadoCidade.Estado = null;
                modelo.EstadoCidade.SeqCidade = null;
            }
            else
            {
                modelo.EstadoCidade = modelo.ExibirNaturalidade ? modelo.EstadoCidade : inscritoInDB.EstadoCidade;
            }

            modelo.EstadoCivil = modelo.ExibirEstadoCivil ? modelo.EstadoCivil : inscritoInDB.EstadoCivil;
            modelo.EstadosIdentidade = modelo.ExibirUfIdentidade ? modelo.EstadosIdentidade : inscritoInDB.EstadosIdentidade;
            modelo.Nacionalidade = modelo.ExibirNacionalidade ? modelo.Nacionalidade : inscritoInDB.Nacionalidade;
            modelo.Nome = modelo.ExibirNome ? modelo.Nome : inscritoInDB.Nome;
            modelo.NomeMae = modelo.ExibirFiliacao ? modelo.NomeMae : inscritoInDB.NomeMae;
            modelo.NomePai = modelo.ExibirFiliacao ? modelo.NomePai : inscritoInDB.NomePai;
            modelo.NumeroIdentidade = modelo.ExibirNumeroIdentidade ? modelo.NumeroIdentidade : inscritoInDB.NumeroIdentidade;
            modelo.NumeroPassaporte = modelo.ExibirPassaporte ? modelo.NumeroPassaporte : inscritoInDB.NumeroPassaporte;
            modelo.OrgaoEmissorIdentidade = modelo.ExibirOrgaoEmissorIdentidade ? modelo.OrgaoEmissorIdentidade : inscritoInDB.OrgaoEmissorIdentidade;
            modelo.Paises = modelo.ExibirPaisOrigem ? modelo.Paises : inscritoInDB.Paises;
            modelo.Sexo = modelo.ExibirSexo ? modelo.Sexo : inscritoInDB.Sexo;
            modelo.Telefones = modelo.ExibirTelefone ? modelo.Telefones : null;
            modelo.TiposEnderecoEletronico = modelo.ExibirOutrosEnderecosEletronicos ? modelo.TiposEnderecoEletronico : inscritoInDB.TiposEnderecoEletronico;
            modelo.UfIdentidade = modelo.ExibirUfIdentidade ? modelo.UfIdentidade : inscritoInDB.UfIdentidade;
            modelo.ConsentimentoLGPD = !string.IsNullOrEmpty(modelo.TermoLGPD) ? modelo.ConsentimentoLGPD : inscritoInDB.ConsentimentoLGPD;
        }
    }
}