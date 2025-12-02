using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Administrativo.Areas.NOT.Models;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System.Collections.Generic;
using System.Linq;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;
using SMC.Framework;
using SMC.Seguranca.ServiceContract.Areas.APL.Interfaces;

namespace SMC.GPI.Administrativo.Areas.NOT.Services
{
    public class ConsultaNotificacaoControllerService : SMCControllerServiceBase
    {
        #region Services
        private IConsultaNotificacaoService ConsultaNotificacaoService
        {
            get { return this.Create<IConsultaNotificacaoService>(); }
        }

        private IProcessoService ProcessoService
        {
            get { return this.Create<IProcessoService>(); }
        }

        private INotificacaoService NotificacaoService
        {
            get { return this.Create<INotificacaoService>(); }
        }

        private Inscricoes.ServiceContract.Areas.NOT.Interfaces.ITipoNotificacaoService TipoNotificacaoService
        {
            get { return this.Create<Inscricoes.ServiceContract.Areas.NOT.Interfaces.ITipoNotificacaoService>(); }
        }

        private IAplicacaoService AplicacaoService
        {
            get { return this.Create<IAplicacaoService>(); }
        }
        #endregion

        public SMCPagerModel<ConsultaNotificacaoListaViewModel> BuscarNotificacoes(ConsultaNotificacaoFiltroViewModel filtros)
        {
            var pagerData = this.ConsultaNotificacaoService.BuscarNotificacoes(filtros.Transform<ConsultaNotificacaoFiltroData>())
                .Transform<SMCPagerData<ConsultaNotificacaoListaViewModel>>();
            var pagerModel = new SMCPagerModel<ConsultaNotificacaoListaViewModel>(pagerData, filtros.PageSettings, filtros);
            return pagerModel;
        }

        public List<SMCDatasourceItem> BuscarTiposNotificacao(long? seqProcesso)
        {
            if (seqProcesso.HasValue)
            {
                var unidadeResponsavel = ProcessoService.BuscarUnidadeResponsavelNotificacao(seqProcesso.Value);
                var aplicacao = AplicacaoService.BuscarAplicacaoPelaSigla(SMCContext.ApplicationId);


                if (unidadeResponsavel.HasValue)
                    return NotificacaoService.BuscarTiposNotificacaoPorUnidadeResponsavel(unidadeResponsavel.Value, aplicacao.SeqGrupoAplicacao.Value).TransformList<SMCDatasourceItem>();
            }
            else
            {
                var tiposNotificacoes = TipoNotificacaoService.BuscarTipoNotificacoes(new TipoNotificacaoFiltroData());
                return tiposNotificacoes.Select(x => new SMCDatasourceItem
                    {
                        Seq = x.Seq,
                        Descricao = x.TipoNotificacao
                    }).ToList();
            }
            return new List<SMCDatasourceItem>();
        }

        public ConsultaNotificacaoViewModel BuscarNotificacao(long seqNotificacao)
        {
            return ConsultaNotificacaoService.BuscarNotificacao(seqNotificacao).Transform<ConsultaNotificacaoViewModel>();
        }

        public SMCUploadFile BuscarArquivo(long seq)
        {
            return NotificacaoService.BuscarArquivo(seq);
        }
    }
}