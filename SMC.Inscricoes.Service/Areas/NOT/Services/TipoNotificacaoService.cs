using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.NOT.DomainServices;
using SMC.Inscricoes.Domain.Areas.NOT.Models;
using SMC.Inscricoes.Domain.Areas.NOT.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Data;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.NOT.Services
{
    public class TipoNotificacaoService : SMCServiceBase, ITipoNotificacaoService
    {
        #region Services
        private TipoNotificacaoDomainService TipoNotificacaoDomainService
        {
            get
            {
                return this.Create<TipoNotificacaoDomainService>();
            }
        }

        private ViewTipoNotificacaoDomainService ViewTipoNotificacaoDomainService
        {
            get
            {
                return this.Create<ViewTipoNotificacaoDomainService>();
            }
        }
        #endregion

        public SMCPagerData<TipoNotificacaoListaData> BuscarTipoNotificacoes(TipoNotificacaoFiltroData filtro)
        {
            var spec = filtro.Transform<ViewTipoNotificacaoSpecification>();            
            var data = this.ViewTipoNotificacaoDomainService.SearchProjectionBySpecification(spec,
                x => new TipoNotificacaoListaData
                {
                    Seq = x.Seq,
                    TipoNotificacao = x.Descricao,
                    PermiteAgendamento = x.PermiteAgendamento
                }, out int total);
            return new SMCPagerData<TipoNotificacaoListaData>(data, total);
        }

        public TipoNotificacaoData BuscarTipoNotificacao(long seqNotificacao)
        {
            var spec = new SMCSeqSpecification<TipoNotificacao>(seqNotificacao);
            var data = this.TipoNotificacaoDomainService.SearchProjectionBySpecification(spec,
                        x => new TipoNotificacaoData
                        {
                            Seq = x.Seq,
                            OldSeq = x.Seq,
                            PermiteAgendamento = x.PermiteAgendamento,
                            Atributos = x.AtributosAgendamento.Select(f => new TipoNotificacaoDetalheData
                            {
                                Seq = f.Seq,
                                Atributo = f.AtributoAgendamento
                            }).ToList()
                        }).FirstOrDefault();

            return data;
        }

        public long SalvarTipoNotificacao(TipoNotificacaoData tipoNotificacaoData)
        {
            return this.TipoNotificacaoDomainService.Salvar(tipoNotificacaoData.Transform<TipoNotificacao>(), tipoNotificacaoData.OldSeq);
        }

        public void Excluir(long seqTipoNotificacao)
        {
            this.TipoNotificacaoDomainService.DeleteEntity(seqTipoNotificacao);
        }
    }
}
