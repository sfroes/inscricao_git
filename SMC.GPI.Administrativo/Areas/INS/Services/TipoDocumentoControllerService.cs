using SMC.Framework;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class TipoDocumentoControllerService : SMCControllerServiceBase
    {

        #region Services

        private ITipoDocumentoService TipoDocumentoService 
        {
            get { return this.Create<ITipoDocumentoService>(); }
        }

        private SMC.DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService TipoDocumentoDadosMestresService 
        {
            get { return this.Create<SMC.DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService>(); }
        }

        #endregion

        public SMCPagerModel<TipoDocumentoListaViewModel> BuscarTiposDocumento(TipoDocumentoFiltroViewModel filtros)
        {
            var datas = this.TipoDocumentoService.BuscarTiposDocumento(SMCMapperHelper.Create<TipoDocumentoFiltroData>(filtros));                        
            SMCPagerData<TipoDocumentoListaViewModel> model = SMCMapperHelper.Create<SMCPagerData<TipoDocumentoListaViewModel>>(datas);
            return new SMCPagerModel<TipoDocumentoListaViewModel>(model, filtros.PageSettings, filtros);
        }

        public TipoDocumentoViewModel BuscarTipoDocumento(long seqTipoDocumento)
        {
            return SMCMapperHelper.Create<TipoDocumentoViewModel>(this.TipoDocumentoService.BuscarTipoDocumento(seqTipoDocumento));
        }

        public long SalvarTipoDocumento(TipoDocumentoViewModel modelo)
        {
            return this.TipoDocumentoService.SalvarTipoDocumento(SMCMapperHelper.Create<TipoDocumentoData>(modelo));
        }

        public void ExcluirTipoDocumento(long seqTipoDocumento)
        {
            this.TipoDocumentoService.ExcluirTipoDocumento(seqTipoDocumento);
        }

        public List<SMCDatasourceItem> BuscarTiposDocumentoSelect()
        {
            return this.TipoDocumentoService.BuscarTiposDocumentoKeyValue().TransformList<SMCDatasourceItem>();
        }

        public List<SMCDatasourceItem> BuscarTiposDocumentoNaoUtilizadosSelect()
        {
            var docs = this.TipoDocumentoDadosMestresService.BuscarTiposDocumentos().SMCToDictionary(x => x.Seq, x => x);
            var usedDocs = BuscarTiposDocumento(new TipoDocumentoFiltroViewModel());

            foreach (var item in usedDocs)
            {
                docs.Remove(item.Seq);
            }

            return docs.Values.TransformList<SMCDatasourceItem>();
        }
    }
}