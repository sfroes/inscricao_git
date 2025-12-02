using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Seguranca.ServiceContract.Areas.APL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class TipoProcessoControllerService : SMCControllerServiceBase
    {

        #region Services

        private SMC.DadosMestres.ServiceContract.Areas.GED.Interfaces.IContextoBibliotecaService ContextoBibliotecaService
        {
            get
            {
                return this.Create<SMC.DadosMestres.ServiceContract.Areas.GED.Interfaces.IContextoBibliotecaService>();
            }
        }
   
        private IAplicacaoService AplicacaoService
        {
            get { return this.Create<IAplicacaoService>(); }
        }

        private ITipoProcessoService TipoProcessoService
        {
            get { return this.Create<ITipoProcessoService>(); }
        }

        private ITipoTemplateProcessoService TipoTemplateProcessoService
        {
            get { return this.Create<ITipoTemplateProcessoService>(); }
        }

        #endregion Services

        /// <summary>
        /// Retorna a lista de situações de destino a partir da situação de origem
        /// </summary>
        public List<SMCDatasourceItem> BuscarTipoProcessoSitucaoDestinoSelect(long seqTipoProcessoSituacaoOrigem, long? seqProcesso = null)
        {
            return TipoProcessoService.BuscarTipoProcessoSitucaoDestinoKeyValue(seqTipoProcessoSituacaoOrigem, seqProcesso)
                .TransformList<SMCDatasourceItem>();
        }

        public SMCPagerModel<TipoProcessoListaViewModel> BuscarTiposProcesso(TipoProcessoFiltroViewModel filtros)
        {
            TipoProcessoFiltroData filtroData = SMCMapperHelper.Create<TipoProcessoFiltroData>(filtros);
            var datas = TipoProcessoService.BuscarTiposProcesso(filtroData);
            SMCPagerData<TipoProcessoListaViewModel> model =
                SMCMapperHelper.Create<SMCPagerData<TipoProcessoListaViewModel>>(datas);
            return new SMCPagerModel<TipoProcessoListaViewModel>(model, filtros.PageSettings, filtros);
        }

        public TipoProcessoViewModel BuscarTipoProcesso(long seqTipoProcesso)
        {
            var retorno = SMCMapperHelper.Create<TipoProcessoViewModel>(
                        TipoProcessoService.BuscarTipoProcesso(seqTipoProcesso));
            return retorno;
        }

        public long SalvarTipoProcesso(TipoProcessoViewModel modelo)
        {
            return TipoProcessoService.SalvarTipoProcesso(SMCMapperHelper.Create<TipoProcessoData>(modelo));
        }

        public void ExcluirTipoProcesso(long seqTipoProcesso)
        {
            TipoProcessoService.ExcluirTipoProcesso(seqTipoProcesso);
        }

        public List<SMCSelectListItem> BuscarTiposProcessosSelect(long? seqUnidadeResponsavel = null)
        {
            return this.TipoProcessoService.BuscarTiposProcessoKeyValue(seqUnidadeResponsavel).TransformList<SMCSelectListItem>();
        }

        public List<SMCDatasourceItem> BuscarTiposTemplateProcessoSelect()
        {
            return TipoTemplateProcessoService.BuscarTiposTemplateProcessoKeyValuePorGrupoAplicacaoSAS(SIGLA_APLICACAO.GRUPO_APLICACAO)
                    .OrderBy(x => x.Descricao)
                    .TransformList<SMCDatasourceItem>();
        }

        public List<SMCSelectListItem> BuscarTemplatesTiposProcessoSelect(long seqTipoTemplateProcesso)
        {
            var itens = TipoTemplateProcessoService.BuscarTemplatesProcessoPorTipoTemplate(seqTipoTemplateProcesso)
                        .OrderBy(x => x.Descricao)
                        .TransformList<SMCSelectListItem>();

            foreach (var item in itens)
            {
                if (item.IsDisabled)
                    item.Text += Views.TipoProcesso.App_LocalResources.UIResource._EditarTipoProcesso_Template_Inativo;
            }

            return itens;
        }

        public SMCMasterDetailList<TipoProcessoSituacaoViewModel> BuscarSituacoesTiposProcesso(long seqTipoTemplateProcesso)
        {
            var situacoes = TipoTemplateProcessoService.BuscarSituacaoPorTipoTemplate(seqTipoTemplateProcesso);

            var retorno = new SMCMasterDetailList<TipoProcessoSituacaoViewModel>();
            foreach (var situacao in situacoes)
            {
                var item = new TipoProcessoSituacaoViewModel();
                item.DescricaoSGF = situacao.Descricao;
                item.Descricao = situacao.Descricao;
                item.Token = situacao.Token;
                item.SeqTipoProcesso = situacao.SeqTipoTemplateProcesso;
                item.SeqSituacao = situacao.Seq;
                retorno.Add(item);
            }

            return retorno;
        }

        public List<SMCDatasourceItem> BuscarTemplatesAssociados(long seqTipoProcesso)
        {
            return this.TipoProcessoService.BuscarTemplatesProcessoAssociados(seqTipoProcesso).TransformList<SMCDatasourceItem>();
        }

        public List<SMCDatasourceItem> BuscarTiposTaxaAssociados(long seqTipoProcesso)
        {
            return this.TipoProcessoService.BuscarTiposTaxaAssociados(seqTipoProcesso).TransformList<SMCDatasourceItem>();
        }

        public bool VerificaPossuiConsistencia(long seqProcesso, TipoConsistencia tipoConsistencia)
        {
            return this.TipoProcessoService.VerificaPossuiConsistencia(new TipoProcessoConsistenciaData() { SeqProcesso = seqProcesso, TipoConsistencia = tipoConsistencia });
        }

        public TipoProcessoData BuscarTipoProcessoPorProcesso(long seqProcesso)
        {
            return TipoProcessoService.BuscarTipoProcessoPorProcesso(seqProcesso);
        }

        public List<SMCDatasourceItem> BuscarTiposDocumentoSelect(long seqTipoProcesso)
        {
            return this.TipoProcessoService.BuscarTiposDocumentoSelect(seqTipoProcesso);
        }

        public bool ConferirHabilitaDatasEvento(long seqTipoProcesso)
        {
            return this.TipoProcessoService.ConferirHabilitaDatasEvento(seqTipoProcesso);
        }

        public List<SMCSelectListItem> BuscarContextosBibliotecas()
        {
            return ContextoBibliotecaService.BuscarContextosBibliotecas().TransformList<SMCSelectListItem>();
        }
        public long BuscarSeqContexto(long seqContextoBiblioteca)
        {
            return ContextoBibliotecaService.BuscarSeqContexto(seqContextoBiblioteca);
        }
    }
}