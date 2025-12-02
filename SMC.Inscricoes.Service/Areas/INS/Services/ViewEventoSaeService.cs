using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.RES.DomainServices;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class ViewEventoSaeService : SMCServiceBase, IViewEventoSaeService
    {

        private ViewEventoSaeDomainService ViewEventoSaeDomainService
        {
            get { return Create<ViewEventoSaeDomainService>(); }
        }

        private UnidadeResponsavelDomainService UnidadeResponsavelDomainService
        {
            get { return Create<UnidadeResponsavelDomainService>(); }
        }

        public List<SMCDatasourceItem> BuscarEventosSaeSelect(long seqUnidadeResponsavel, int? ano)
        {
            var spec = new ViewEventoSaeFilterSpecification()
            {
                CodigoUnidadePromotora = this.UnidadeResponsavelDomainService.BuscarCodigoUnidadePromotora(seqUnidadeResponsavel),
                Ano = ano
            };

            return this.ViewEventoSaeDomainService.BuscarEventosSaeSelect(spec);            
        }

    }
}
