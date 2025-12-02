using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class TipoTaxaService : SMCServiceBase, ITipoTaxaService
    {
        private TipoTaxaDomainService TipoTaxaDomainService
        {
            get { return Create<TipoTaxaDomainService>(); }
        }

        public List<SMCDatasourceItem> BuscarTiposTaxaSelect()
        {
            return this.TipoTaxaDomainService.SearchProjectionAll(x =>
                new SMCDatasourceItem
                {
                    Seq = x.Seq,
                    Descricao = x.Descricao
                }, x => x.Descricao).ToList();
        }
    }
}
