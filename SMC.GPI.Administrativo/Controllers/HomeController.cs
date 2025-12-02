using SMC.DadosMestres.ServiceContract.Areas.SHA.Data;
using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.UI.Mvc.Util;
using SMC.Framework.Util;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SMC.GPI.Administrativo.Areas.INS.Views.AcompanhamentoProcesso.App_LocalResources;
using SMC.Seguranca.ServiceContract.Areas.APL.Interfaces;
using SMC.Framework.Security;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Results;
using SMC.Framework.UI.Mvc.Binders;
using SMC.Framework.Rest;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;

namespace SMC.GPI.Administrativo.Controllers
{
    public class HomeController : SMCControllerBase
    {
        #region Services

        private IArquivoAnexadoService ArquivoAnexadoService => Create<IArquivoAnexadoService>();
        private IInscricaoService InscricaoService => Create<IInscricaoService>();
        private IAplicacaoService AplicacaoService => Create<IAplicacaoService>();
        #endregion


        /// <summary>
        /// Página Home
        /// </summary>
        [SMCAuthorize(UC_INS_001_01_01.PESQUISAR_PROCESSO,
                      UC_INS_001_04_01.PESQUISAR_TIPO_HIERARQUIA_OFERTA,
                      UC_INS_001_05_01.PESQUISAR_TIPO_DOCUMENTO,
                      UC_INS_001_06_01.MANTER_TIPO_ITEM_HIERARQUIA_OFERTA,
                      UC_INS_001_07_01.MANTER_CODIGO_AUTORIZACAO,
                      UC_INS_001_09_01.PESQUISAR_TIPO_PROCESSO,
                      UC_INS_001_10_01.MANTER_TIPO_TAXA,
                      UC_INS_003_01_01.CONSULTA_POSICAO_CONSOLIDADA_POR_PROCESSO,
                      UC_INS_005_01_02.REALIZAR_CHECKIN_POR_QRCODE)]
        public ActionResult Index()
        {
            //Validar se o usuario é somente atendente
            var redirecionamento = AplicacaoService.VerificarUsuarioApenasUmPapelAplicacao(User.SMCGetSequencialUsuario().Value,
                                                                               SIGLA_APLICACAO.GPI_ADMIN,
                                                                                  "OPERACIONAL_CHECKIN_EVENTOS");
            if (redirecionamento)
            {
                return RedirectToAction("Index", "Checkin", new { area = "INS", somenteAtendente = true });
            }

            return View();
        }

        /// <summary>
        /// Logout da aplicação
        /// </summary>
        [SMCAllowAnonymous]
        public ActionResult Logout()
        {
            // Desconecta o usuário
            SMCFederationHelper.SignOut("Index", "Home");

            return new EmptyResult();
        }




        #region Download Arquivo

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
            return CarregarDocumento(new SMCEncryptedLong(guidFile));
        }

        [SMCAllowAnonymous]
        public ActionResult DownloadDocumentos(long seqInscricao)
        {
            if (seqInscricao > 0)
            {
                SMCFile zip = DownloadZip(seqInscricao);
                if (zip != null)
                {
                    return File(zip.Conteudo, System.Net.Mime.MediaTypeNames.Application.Zip, zip.Nome);
                }
                else
                {
                    SetErrorMessage(UIResource.Mensagem_Documentacao_Vazia, target: SMCMessagePlaceholders.Centro);
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            return null;
        }

        [HttpGet]
        [SMCAuthorize(UC_INS_003_01_06.CONSULTA_INSCRITO)]
        public ActionResult CarregarDocumento(SMCEncryptedLong seqArquivo)
        {
            SMCUploadFileGED arquivo = ArquivoAnexadoService.BuscarArquivoAnexado(seqArquivo);

            if (arquivo.UrlPublicaGed != null)
            {
                return Redirect(arquivo.UrlPublicaGed);
            }

            if (arquivo.FileData == null)
            {
                Response.AppendHeader("Content-Disposition", "inline; filename=" + "\"" + arquivo.Name + "_expurgo.pdf\"");
                return File(CONSTANTS.CONTEUDO_ARQUIVO_EXPURGO_PDF, "application/pdf");
            }

            Response.AppendHeader("Content-Disposition", "inline; filename=" + "\"" + arquivo.Name + "\"");

            //o mime para download de arquivos xml tem que ser especificadamente este. 
            if (arquivo.Type.ToLower().Contains("xml"))
            {
                return File(arquivo.FileData, System.Net.Mime.MediaTypeNames.Application.Octet);
            }

            return File(arquivo.FileData, arquivo.Type);


        }

        public SMCFile DownloadZip(long seqInscricao)
        {
            var registroDocumentos = InscricaoService.BuscarDocumentosInscricao(seqInscricao);

            List<SMCFile> documentos = new List<SMCFile>();
            foreach (var item in registroDocumentos)
            {
                if (item.SeqArquivoAnexado.HasValue)
                {
                    var arquivo = ArquivoAnexadoService.BuscarArquivoAnexado(item.SeqArquivoAnexado.Value);

                    if (arquivo.FileData == null)
                    {
                        documentos.Add(new SMCFile
                        {
                            Nome = arquivo.Name + "_expurgo.pdf",
                            Conteudo = CONSTANTS.CONTEUDO_ARQUIVO_EXPURGO_PDF,
                            Description = arquivo.Description,
                            Tipo = "application/pdf",
                            Tamanho = CONSTANTS.CONTEUDO_ARQUIVO_EXPURGO_PDF.Length
                        });
                    }
                    else
                    {
                        documentos.Add(new SMCFile
                        {
                            Nome = arquivo.Name,
                            Conteudo = arquivo.FileData,
                            Description = arquivo.Description,
                            Tipo = arquivo.Type,
                            Tamanho = (int)arquivo.Size
                        });
                    }
                }
            }

            if (documentos.Count > 0)
            {
                byte[] zip = SMCFileHelper.ZipFiles(documentos.ToArray());
                return new SMCFile() { Conteudo = zip, Nome = "ArquivosAnexados.zip" };
            }
            return null;
        }

        #endregion
    }
}