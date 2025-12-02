using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Extensions;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Financeiro.ServiceContract.BLT;
using SMC.Financeiro.ServiceContract.TXA.Data;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class DocumentoRequeridoControllerService : SMCControllerServiceBase
    {
        #region Services

        private IDocumentoRequeridoService DocumentoRequeridoService
        {
            get { return this.Create<IDocumentoRequeridoService>(); }
        }

        private IInscricaoService InscricaoService
        {
            get { return this.Create<InscricaoService>(); }
        }

        private IConfiguracaoEtapaService ConfiguracaoEtapaService
        {
            get { return Create<IConfiguracaoEtapaService>(); }
        }

        #endregion

        public SMCPagerModel<DocumentoRequeridoListaViewModel> BuscarDocumentosRequeridos(DocumentoRequeridoFiltroViewModel filtros)
        {
            SMCPagerData<DocumentoRequeridoListaData> data = this.DocumentoRequeridoService.BuscarDocumentosRequeridos(
                filtros.Transform<DocumentoRequeridoFiltroData>());
            var listaDocumentos = SMCMapperHelper.Create<SMCPagerData<DocumentoRequeridoListaViewModel>>(data);
            return new SMCPagerModel<DocumentoRequeridoListaViewModel>(listaDocumentos, filtros.PageSettings, filtros);
        }

        public DocumentoRequeridoViewModel BuscarDocumentoRequerido(long seqDocumentoRequerido)
        {
            var documento = this.DocumentoRequeridoService.
                BuscarDocumentoRequerido(seqDocumentoRequerido).Transform<DocumentoRequeridoViewModel>();

            documento.UploadObrigatorioOriginal = documento.UploadObrigatorio;
            documento.TipoDocumentoOriginal = documento.SeqTipoDocumento;
            documento.VersaoDocumentoOriginal = (short)documento.VersaoDocumento;

            return documento;
        }

        public List<SMCDatasourceItem> BuscarDocumentosRequeridosSelect(long seqConfiguracaoEtapa,
            bool? obrigatorio = null, bool? uploadObrigatorio = null)
        {
            var filtro = new DocumentoRequeridoFiltroData
            {
                SeqConfiguracaoEtapa = seqConfiguracaoEtapa,
                Obrigatorio = obrigatorio,
                UploadObrigatorio = uploadObrigatorio
            };
            return this.DocumentoRequeridoService.BuscarDocumentosRequeridosKeyValue(filtro)
                .TransformList<SMCDatasourceItem>();
        }

        public long SalvarDocumentoRequerido(DocumentoRequeridoViewModel modelo)
        {
            return this.DocumentoRequeridoService.SalvarDocumentoRequerido(
                modelo.Transform<DocumentoRequeridoData>());
        }

        public void ExcluirDocumentoRequerido(long seqDocumentoRequerido)
        {
            this.DocumentoRequeridoService.ExcluirDocumentoRequerido(seqDocumentoRequerido);
        }

        public bool VerificaApenasInscricoesTeste(long seqConfiguracaoEtapa)
        {            
            long[] seqInscricoes = ConfiguracaoEtapaService.BuscarInscricoesConfiguracaoEtapa(seqConfiguracaoEtapa);

            return InscricaoService.VerificaApenasInscricoesTeste(seqInscricoes);
        }

        public bool VerificaInscricaoComDocumentoCadastrado(long seqDocumentoRequerido)
        {
            return DocumentoRequeridoService.VerificaInscricaoComDocumentoCadastrado(seqDocumentoRequerido);
        }
    }

}