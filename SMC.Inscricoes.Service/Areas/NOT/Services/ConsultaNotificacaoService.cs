using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.NOT.DomainServices;
using SMC.Inscricoes.Domain.Areas.NOT.Models;
using SMC.Inscricoes.Domain.Areas.NOT.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Data;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.NOT.Services
{
    public class ConsultaNotificacaoService : SMCServiceBase, IConsultaNotificacaoService
    {
        #region DomainServices
        private ViewInscricaoEnvioNotificacaoDomainService ViewInscricaoEnvioNotificacaoDomainService
        {
            get { return this.Create<ViewInscricaoEnvioNotificacaoDomainService>(); }
        }

        private INotificacaoService NotificacaoService
        {
            get { return this.Create<INotificacaoService>(); }
        }
        #endregion

        public SMCPagerData<ConsultaNotificacaoListaData> BuscarNotificacoes(ConsultaNotificacaoFiltroData consultaNotificacaoFiltroData)
        {

            var spec = consultaNotificacaoFiltroData.Transform<ViewInscricaoEnvioNotificacaoSpecification>();

            spec.SetOrderByDescending(p => p.DataEnvio)
                .SetOrderBy(p => p.Inscricao.GrupoOferta.Nome)
                .SetOrderBy(p => p.Inscricao.Inscrito.Nome)
                .SetOrderBy(p => p.DescricaoTipoNotificacao);

            //    .SetOrderBy(p => p.Inscricao.Ofertas[0].Oferta.Nome)
            int total;
            var data = this.ViewInscricaoEnvioNotificacaoDomainService.SearchProjectionBySpecification(spec,
                            x => new ConsultaNotificacaoListaData
                            {
                                Seq = x.Seq,
                                GrupoOferta = x.Inscricao.GrupoOferta.Nome,
                                Oferta = x.Inscricao.Ofertas.Select(f => f.Oferta.Nome).ToList(),
                                Inscrito = (x.Inscricao.Inscrito.NomeSocial != null) ? x.Inscricao.Inscrito.NomeSocial + " (" + x.Inscricao.Inscrito.Nome + ")" : x.Inscricao.Inscrito.Nome,
                                TipoNotificacao = x.DescricaoTipoNotificacao,
                                Assunto = x.AssuntoNotificacao,
                                Sucesso = x.SucessoEnvio,
                                DataEnvio = x.DataEnvio,
                                SeqUnidadeResponsavel = x.Inscricao.Processo.SeqUnidadeResponsavel
                            }, out total).ToList();

            return new SMCPagerData<ConsultaNotificacaoListaData>(data, total);
        }

        public ConsultaNotificacaoData BuscarNotificacao(long seqNotificacao)
        {
            var spec = new SMCSeqSpecification<ViewInscricaoEnvioNotificacao>(seqNotificacao);

            var notificacao = ViewInscricaoEnvioNotificacaoDomainService.SearchProjectionBySpecification(spec,
                             x => new ConsultaNotificacaoData
                             {
                                 Seq = x.Inscricao.SeqProcesso,
                                 Processo = x.Inscricao.Processo.Descricao,
                                 GrupoOferta = x.Inscricao.GrupoOferta.Nome,
                                 Oferta = x.Inscricao.Ofertas.Select(f => f.Oferta.Nome).ToList(),
                                 Inscrito = (x.Inscricao.Inscrito.NomeSocial != null) ? x.Inscricao.Inscrito.NomeSocial + " (" + x.Inscricao.Inscrito.Nome + ")" : x.Inscricao.Inscrito.Nome,
                                 DescricaoTipoNotificacao = x.DescricaoTipoNotificacao,
                                 AssuntoNotificacao = x.AssuntoNotificacao,
                                 SucessoEnvio = x.SucessoEnvio,
                                 DataEnvio = x.DataEnvio,
                                 DescricaoErroEnvio = x.DescricaoErroEnvio,
                                 SeqNotificacaoEmailDestinatario = x.SeqNotificacaoEmailDestinatario
                             }).FirstOrDefault();

            var notComp = NotificacaoService.ConsultaNotificacao(notificacao.SeqNotificacaoEmailDestinatario);

            if (notComp != null)
            {
                notificacao.NomeRementente = notComp.NomeOrigem;
                notificacao.EmailRemetente = notComp.EmailOrigem;
                notificacao.EmailResposta = notComp.EmailResposta;
                notificacao.EmailDestinatario = notComp.EmailDestinatario;
                notificacao.EmailComCopia = notComp.EmailCopia;
                notificacao.EmailComCopiaOculta = notComp.EmailCopiaOculta;
                notificacao.DataPrevistaEnvio = notComp.DataPrevistaEnvio;
                notificacao.Mensagem = notComp.Mensagem;

                notificacao.Arquivos = notComp.Anexos.ToList();
            }
            if (notificacao != null)
                return notificacao.Transform<ConsultaNotificacaoData>();
            return new ConsultaNotificacaoData();
        }
    }
}
