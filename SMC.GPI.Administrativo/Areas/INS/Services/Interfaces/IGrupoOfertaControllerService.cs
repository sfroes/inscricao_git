using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public interface IGrupoOfertaControllerService
    {
        List<SMCDatasourceItem> BuscarGruposOfertasSelect(long seqProcesso);

        SMCPagerModel<GrupoOfertaListaViewModel> BuscarGruposOferta(GrupoOfertaFiltroViewModel filtros);

        GrupoOfertaViewModel BuscarGrupoOferta(long seqGrupoOferta);

        void ExcluirGrupoOferta(long seqGrupoOferta);

        long SalvarGrupoOferta(GrupoOfertaViewModel modelo);
    }
}