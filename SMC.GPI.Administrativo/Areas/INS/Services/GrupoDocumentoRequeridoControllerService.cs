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
    public class GrupoDocumentoRequeridoControllerService : SMCControllerServiceBase
    {
        #region Services

        private IDocumentoRequeridoService DocumentoRequeridoService
        {
            get { return this.Create<IDocumentoRequeridoService>(); }
        }

        private IProcessoService ProcessoService
        {
            get { return Create<IProcessoService>(); }
        }

        private IInscricaoService InscricaoService
        {
            get { return Create<IInscricaoService>(); }
        }

        private IConfiguracaoEtapaService ConfiguracaoEtapaService
        {
            get { return Create<IConfiguracaoEtapaService>(); }
        }

        #endregion


        public List<GrupoDocumentosRequeridosListaViewModel> BuscarGruposDocumentacoesRequeridas(GrupoDocumentosRequeridosFiltroViewModel filtros)
        {
            return this.DocumentoRequeridoService.BuscarGruposDocumentosRequiridos(
                filtros.Transform<GrupoDocumentoRequeridoFiltroData>()).TransformList<GrupoDocumentosRequeridosListaViewModel>();            
        }

        public GrupoDocumentosRequeridosViewModel BuscarGrupoDocumentacaoRequerida(long seqGrupoDocumentacaoRequerida)
        {
            var grupo = this.DocumentoRequeridoService.BuscarGrupoDocumentoRequerido(seqGrupoDocumentacaoRequerida)
                .Transform<GrupoDocumentosRequeridosViewModel>();

            grupo.UploadObrigatorioOriginal = grupo.UploadObrigatorio;
            grupo.MinimoObrigatorioOriginal = grupo.MinimoObrigatorio;

            return grupo;
        }

        public long SalvarGrupoDocumentacaoRequerida(GrupoDocumentosRequeridosViewModel modelo)
        {
            return this.DocumentoRequeridoService.SalvarGrupoDocumentoRequerido(
                modelo.Transform<GrupoDocumentoRequeridoData>());
        }

        public void ExcluirGrupoDocumentacaoRequerida(long seqGrupoDocumentacaoRequerida)
        {
            this.DocumentoRequeridoService.ExcluirGrupoDocumentoRequerido(seqGrupoDocumentacaoRequerida);
        }

        public bool VerificaApenasInscricoesTeste(long seqConfiguracaoEtapa)
        {
            long[] seqInscricoes = ConfiguracaoEtapaService.BuscarInscricoesConfiguracaoEtapa(seqConfiguracaoEtapa);

            return InscricaoService.VerificaApenasInscricoesTeste(seqInscricoes);
        }
    }

}
