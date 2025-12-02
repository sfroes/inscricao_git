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
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Framework.UI.Mvc.Html;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class GrupoOfertaControllerService : SMCControllerServiceBase
    {
        #region Services

        private IGrupoOfertaService GrupoOfertaService
        {
            get { return this.Create<IGrupoOfertaService>(); }
        }

        #endregion

        public List<SMCDatasourceItem> BuscarGruposOfertasSelect(long seqProcesso)
        {
            return this.GrupoOfertaService.BuscaGruposOfertaKeyValue(seqProcesso).TransformList<SMCDatasourceItem>();
        }
      
        public SMCPagerModel<GrupoOfertaListaViewModel> BuscarGruposOferta(GrupoOfertaFiltroViewModel filtros)
        {
            GrupoOfertaFiltroData filtroData = filtros.Transform<GrupoOfertaFiltroData>();
            var data = GrupoOfertaService.BuscarGruposOferta(filtroData);
            SMCPagerData<GrupoOfertaListaViewModel> model =
                    SMCMapperHelper.Create<SMCPagerData<GrupoOfertaListaViewModel>>(data);
            return new SMCPagerModel<GrupoOfertaListaViewModel>(model, filtros.PageSettings, filtros);
        }
        
        public void ExcluirGrupoOferta(long seqGrupoOferta)
        {
            GrupoOfertaService.ExcluirGrupoOferta(seqGrupoOferta);
        }

        public long SalvarGrupoOferta(GrupoOfertaViewModel modelo)
        {
            return GrupoOfertaService.SalvarGrupoOferta(modelo.Transform<GrupoOfertaData>());
        }

        public GrupoOfertaViewModel BuscarGrupoOferta(long seqGrupoOferta)
        {
             return GrupoOfertaService.BuscarGrupoOferta(seqGrupoOferta).Transform<GrupoOfertaViewModel>();
        }
    }
}