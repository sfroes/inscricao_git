using SMC.Framework.Service;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMC.Framework.Model;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class TipoItemHierarquiaOfertaService : SMCServiceBase, ITipoItemHierarquiaOfertaService
    {
        #region Domain Services
        private TipoItemHierarquiaOfertaDomainService TipoItemHierarquiaOfertaDomainService
        {
            get { return Create<TipoItemHierarquiaOfertaDomainService>(); }
        }       
        #endregion

        public List<SMCDatasourceItem> BuscarTiposItemHierarquiaOfertaSelect()
        {
            return this.TipoItemHierarquiaOfertaDomainService.SearchProjectionAll(x =>
                        new SMCDatasourceItem
                        {
                            Seq = x.Seq,
                            Descricao = x.Descricao
                        }, x => x.Descricao).ToList();
        }

        public List<SMCDatasourceItem> BuscarTiposItemHierarquiaOfertaPorProcessoSelect(long seqProcesso, bool? habilitaOferta = null)
        {
            return TipoItemHierarquiaOfertaDomainService.SearchProjectionBySpecification(new TipoItemHierarquiaOfertaPorProcessoSpecification(seqProcesso) { HabilitaOferta = habilitaOferta },
                                                                x => new SMCDatasourceItem
                                                                {
                                                                    Seq = x.Seq,
                                                                    Descricao = x.Descricao
                                                                }).ToList();
        }
    }
}
