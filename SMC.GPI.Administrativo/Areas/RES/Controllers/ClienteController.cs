using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.Util;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.RES.Models;
using SMC.GPI.Administrativo.Areas.RES.Services;
using SMC.Inscricoes.Common.Areas.RES.Constants;
using System;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.RES.Controllers
{
    public class ClienteController : SMCControllerBase
    {
        #region Serviços

        private ClienteControllerService ClienteControllerService
        {
            get
            {
                return this.Create<ClienteControllerService>();
            }
        }

        #endregion Serviços

        #region Listagem

        /// <summary>
        /// Exibe tela de listagem de clientes
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_RES_002_01_01.PESQUISAR_CLIENTE)]
        public ActionResult Index(ClienteFiltroViewModel filtros = null)
        {
            return View(filtros);
        }

        /// <summary>
        /// Action para listar os clientes
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_RES_002_01_01.PESQUISAR_CLIENTE)]
        public ActionResult ListarCliente(ClienteFiltroViewModel filtros)
        {
            SMCPagerModel<ClienteListaViewModel> pager = ClienteControllerService.BuscarClientes(filtros);
            return PartialView("_ListarCliente", pager);
        }

        #endregion Listagem

        #region Inclusão/Edição

        /// <summary>
        /// Exibe a tela de inclusão de um Cliente
        /// </summary>
        [HttpGet]
        [SMCAuthorize(UC_RES_002_01_02.MANTER_CLIENTE)]
        public ActionResult Incluir()
        {
            var modelo = new ClienteViewModel();
            PreencherModelo(modelo);
            return View(modelo);
        }

        /// <summary>
        /// Exibe tela de edição para um Cliente
        /// </summary>
        /// <param name="seqCliente">Sequencial do cliente a ser editado</param>
        [HttpGet]
        [SMCAuthorize(UC_RES_002_01_02.MANTER_CLIENTE)]
        public ActionResult Editar(SMCEncryptedLong seq)
        {
            ClienteViewModel modelo = ClienteControllerService.BuscarCliente(seq);
            PreencherModelo(modelo);
            return View(modelo);
        }

        /// <summary>
        /// Salva o Cliente
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_RES_002_01_02.MANTER_CLIENTE)]
        public ActionResult Salvar(ClienteViewModel modelo)
        {
            return this.SaveEdit(modelo, this.ClienteControllerService.SalvarCliente);
        }

        /// <summary>
        /// Salva um cliente e redireciona o usuário para a tela de novo registro
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_RES_002_01_02.MANTER_CLIENTE)]
        public ActionResult SalvarNovo(ClienteViewModel modelo)
        {
            return this.SaveNew(modelo, this.ClienteControllerService.SalvarCliente);
        }

        /// <summary>
        /// Salva um cliente e redireciona o usuário para a tela de listagem
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_RES_002_01_02.MANTER_CLIENTE)]
        public ActionResult SalvarSair(ClienteViewModel modelo)
        {
            return this.SaveQuit(modelo, this.ClienteControllerService.SalvarCliente);
        }

        #endregion Inclusão/Edição

        #region Excluir

        /// <summary>
        /// Action para exclusão de um cliente
        /// </summary>
        /// <param name="seqCliente">Sequencial do cliente a ser excluido</param>
        [HttpPost]
        [SMCAuthorize(UC_RES_002_01_02.MANTER_CLIENTE)]
        public ActionResult Excluir(SMCEncryptedLong seq)
        {
            ClienteControllerService.ExcluirCliente(seq);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                                            MessagesResource.Entidade_Cliente),
                                MessagesResource.Titulo_Sucesso,
                                SMCMessagePlaceholders.Centro);

            return RedirectToAction("ListarCliente");
        }

        #endregion Excluir

        #region Preencher Modelo

        [NonAction]
        private void PreencherModelo(ClienteViewModel modelo)
        {
            // Monta a lista de tipo de telefone deixando apenas tipo comercial e fax
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
        }

        #endregion Preencher Modelo
    }
}