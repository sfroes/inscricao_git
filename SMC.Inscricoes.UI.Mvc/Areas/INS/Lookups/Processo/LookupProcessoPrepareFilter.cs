using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    class LookupProcessoPrepareFilter : ISMCFilter<LookupProcessoFiltroViewModel>
    {
        public LookupProcessoFiltroViewModel Filter(Framework.UI.Mvc.SMCControllerBase controllerBase, LookupProcessoFiltroViewModel filter)
        {
            var unidadeResponsavelService = controllerBase.Create<IUnidadeResponsavelService>();
            filter.UnidadesResponsaveis = unidadeResponsavelService.BuscarUnidadesResponsaveisKeyValue().TransformList<SMCDatasourceItem>();
            if (filter.UnidadesResponsaveis.Count == 1)
            {
                filter.UnidadeResponsavel = filter.UnidadesResponsaveis.First().Seq;
            }

            var tipoProcessoControllerService = controllerBase.Create<ITipoProcessoService>();
            filter.TiposProcesso = tipoProcessoControllerService.BuscarTiposProcessoKeyValue().TransformList<SMCDatasourceItem>();
            if (filter.TiposProcesso.Count == 1)
            {
                filter.TipoProcesso = filter.TiposProcesso.First().Seq;
            }
            return filter;
        }
    }
}
