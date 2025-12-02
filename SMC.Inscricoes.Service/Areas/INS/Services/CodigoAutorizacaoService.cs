using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class CodigoAutorizacaoService : SMCServiceBase, ICodigoAutorizacaoService
    {
        #region Domain Services
        private CodigoAutorizacaoDomainService CodigoAutorizacaoDomainService
        {
            get { return Create<CodigoAutorizacaoDomainService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return Create<ProcessoDomainService>(); }
        }
        #endregion

        public long SalvarCodigoAutorizacao(CodigoAutorizacaoData codigoAutorizacao)
        {
            return this.CodigoAutorizacaoDomainService.SalvarCodigoAutorizacao(
                            codigoAutorizacao.Transform<CodigoAutorizacao>());
        }

        public List<SMCDatasourceItem> BuscarCodigosAutorizacaoKeyValue(long seqUnidadeResponsavel)
        {
            return this.CodigoAutorizacaoDomainService.SearchProjectionBySpecification(
                new CodigoAutorizacaoFilterSpecification
                {
                    SeqUnidadeResponsavel = seqUnidadeResponsavel
                },
                 x => new SMCDatasourceItem
                 {
                     Seq = x.Seq,
                     Descricao = x.Descricao + " - " + x.Codigo +
                        (x.Cliente == null ? "" : (" - " + x.Cliente.Nome))
                 }).ToList();
        }

        public List<SMCDatasourceItem> BuscarCodigosAutorizacaoPorProcessoSelect(long seqProcesso)
        {
            var seqUnidade = this.ProcessoDomainService
                .SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso),
                    x => x.SeqUnidadeResponsavel);
            return this.BuscarCodigosAutorizacaoKeyValue(seqUnidade);
        }
    }
}
