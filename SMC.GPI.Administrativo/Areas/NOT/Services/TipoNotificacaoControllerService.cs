using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Administrativo.Areas.NOT.Models;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Data;
using System.Collections.Generic;
using SMC.Framework;
using SMC.Framework.Util;
using SMC.Seguranca.ServiceContract.Areas.APL.Interfaces;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;

namespace SMC.GPI.Administrativo.Areas.NOT.Services
{
    public class TipoNotificacaoControllerService : SMCControllerServiceBase
    {
        #region Serviços
        private Inscricoes.ServiceContract.Areas.NOT.Interfaces.ITipoNotificacaoService TipoNotificacaoService
        {
            get { return this.Create<Inscricoes.ServiceContract.Areas.NOT.Interfaces.ITipoNotificacaoService>(); }
        }

        private IAplicacaoService AplicacaoService
        {
            get { return this.Create<IAplicacaoService>(); }
        }

        private INotificacaoService NotificacaoService
        {
            get { return this.Create<INotificacaoService>(); }
        }
        #endregion

        public SMCPagerModel<TipoNotificacaoListaViewModel> BuscarTipoNotificacoes(TipoNotificacaoFiltroViewModel filtros)
        {
            var pagerData = this.TipoNotificacaoService.BuscarTipoNotificacoes(filtros.Transform<TipoNotificacaoFiltroData>())
                .Transform<SMCPagerData<TipoNotificacaoListaViewModel>>();
            var pagerModel = new SMCPagerModel<TipoNotificacaoListaViewModel>(pagerData, filtros.PageSettings, filtros);
            return pagerModel;
        }

        public List<SMCDatasourceItem> BuscarTipoNotificacao()
        {
            //FIX: Remover o serviço de busca da aplicação quando o Seq do Grupo Aplicação for colocado no SMCContext.
            var aplicacao = AplicacaoService.BuscarAplicacaoPelaSigla(SMCContext.ApplicationId);

            return NotificacaoService.BuscarTiposNoficicacaoPorGrupoAplicacao(aplicacao.SeqGrupoAplicacao.Value).TransformList<SMCDatasourceItem>();
        }

        public TipoNotificacaoViewModel BuscarTipoNotificacao(long seqTipoNotificacao)
        {
            return TipoNotificacaoService.BuscarTipoNotificacao(seqTipoNotificacao).Transform<TipoNotificacaoViewModel>();
        }

        public long SalvarTipoNotificacao(TipoNotificacaoViewModel modelo)
        {
            return TipoNotificacaoService.SalvarTipoNotificacao(modelo.Transform<TipoNotificacaoData>());
        }

        public void Excluir(long seqTipoNotificacao)
        {
            TipoNotificacaoService.Excluir(seqTipoNotificacao);
        }

        public List<SMCDatasourceItem> BuscarAtributosAgendamentoTipo(long seqTipoNotificacao)
        {
            var tipo = TipoNotificacaoService.BuscarTipoNotificacao(seqTipoNotificacao);
            List<SMCDatasourceItem> ret = new List<SMCDatasourceItem>();
            foreach (var atributo in tipo.Atributos) 
            {
                ret.Add(new SMCDatasourceItem
                {
                    Seq = (short)atributo.Atributo,
                    Descricao = SMCEnumHelper.GetDescription(atributo.Atributo)
                });
            }
            return ret;
        }
    }
}