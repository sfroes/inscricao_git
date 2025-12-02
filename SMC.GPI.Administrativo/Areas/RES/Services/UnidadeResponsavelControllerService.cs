using SMC.Formularios.ServiceContract.Areas.FRM.Data;
using SMC.Formularios.ServiceContract.Areas.FRM.Interfaces;
using SMC.Formularios.ServiceContract.FRM.Data;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.RES.Models;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.RES.DomainServices;
using SMC.Inscricoes.Domain.Areas.RES.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.RES.Data;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace SMC.GPI.Administrativo.Areas.RES.Services
{
    public class UnidadeResponsavelControllerService : SMCControllerServiceBase
    {
        #region Services

        private SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces.IUnidadeResponsavelService UnidadeResponsavelService
        {
            get { return this.Create<SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces.IUnidadeResponsavelService>(); }
        }

        private SMC.Formularios.ServiceContract.Areas.FRM.Interfaces.IUnidadeResponsavelService UnidadeResponsavelServiceSGF
        {
            get { return this.Create<SMC.Formularios.ServiceContract.Areas.FRM.Interfaces.IUnidadeResponsavelService>(); }
        }

        private ITipoProcessoService TipoProcessoService
        {
            get { return this.Create<ITipoProcessoService>(); }
        }

        private ITipoHierarquiaOfertaService TipoHierarquiaOfertaService
        {
            get { return this.Create<ITipoHierarquiaOfertaService>(); }
        }

        private IFormularioService FormularioService
        {
            get { return this.Create<IFormularioService>(); }
        }

        private INotificacaoService NotificacaoService
        {
            get { return this.Create<INotificacaoService>(); }
        }

        #endregion Services

        #region Unidade Responsável

        public SMCPagerModel<UnidadeResponsavelListaViewModel> BuscarUnidadesResponsaveis(UnidadeResponsavelFiltroViewModel filtros)
        {
            UnidadeResponsavelFiltroData filtroData = SMCMapperHelper.Create<UnidadeResponsavelFiltroData>(filtros);
            var datas = UnidadeResponsavelService.BuscarUnidadesResponsaveis(filtroData);
            SMCPagerData<UnidadeResponsavelListaViewModel> model =
                SMCMapperHelper.Create<SMCPagerData<UnidadeResponsavelListaViewModel>>(datas);
            return new SMCPagerModel<UnidadeResponsavelListaViewModel>(model, filtros.PageSettings, filtros);
        }

        public UnidadeResponsavelViewModel BuscarUnidadeResponsavel(long seqUnidadeResponsavel)
        {
            return SMCMapperHelper.Create<UnidadeResponsavelViewModel>(
                UnidadeResponsavelService.BuscarUnidadeResponsavel(seqUnidadeResponsavel));
        }

        public UnidadeResponsavelCabecalhoViewModel BuscarUnidadeResponsavelCabecalho(long seqUnidadeResponsavel)
        {
            return SMCMapperHelper.Create<UnidadeResponsavelCabecalhoViewModel>(
                UnidadeResponsavelService.BuscarUnidadeResponsavelSimplificado(seqUnidadeResponsavel));
        }

        public List<SMCSelectListItem> BuscarUnidadesResponsaveisSelect()
        {
            return UnidadeResponsavelService.BuscarUnidadesResponsaveisKeyValue().TransformList<SMCSelectListItem>();
        }

        public long SalvarUnidadeResponsavel(UnidadeResponsavelViewModel modelo)
        {
            return UnidadeResponsavelService.SalvarUnidadeResponsavel(
                SMCMapperHelper.Create<SMC.Inscricoes.ServiceContract.Areas.RES.Data.UnidadeResponsavelData>(modelo));
        }

        public void ExcluirUnidadeResponsavel(long seqUnidadeResponsavel)
        {
            UnidadeResponsavelService.ExcluirUnidadeResponsavel(seqUnidadeResponsavel);
        }

        public List<SMCDatasourceItem> BuscarUnidadesReponsaveisNotificacaoSelect()
        {
            return this.NotificacaoService.BuscarUnidadesResponsaveisKeyValue().TransformList<SMCDatasourceItem>();
        }

        #endregion Unidade Responsável

        #region Unidade Responsável x Tipos Formulario

        public SMCPagerModel<UnidadeResponsavelTipoFormularioListaViewModel> BuscarUnidadeResponsavelTiposFormularios(long seqParametro)
        {
            var datas = UnidadeResponsavelService.BuscarUnidadeResponsavelTiposFormularios(seqParametro);
            SMCPagerData<UnidadeResponsavelTipoFormularioListaViewModel> model =
                SMCMapperHelper.Create<SMCPagerData<UnidadeResponsavelTipoFormularioListaViewModel>>(datas);
            return new SMCPagerModel<UnidadeResponsavelTipoFormularioListaViewModel>(model);
        }

        public UnidadeResponsavelTipoFormularioViewModel BuscarUnidadeResponsavelTipoFormulario(long seqFormulario)
        {
            return SMCMapperHelper.Create<UnidadeResponsavelTipoFormularioViewModel>(
                        UnidadeResponsavelService.BuscarUnidadeResponsavelTipoFormulario(seqFormulario));
        }

        public long SalvarUnidadeResponsavelTipoFormulario(UnidadeResponsavelTipoFormularioViewModel modelo)
        {
            return UnidadeResponsavelService.SalvarTipoFormularioUnidadeResponsavel(
                    SMCMapperHelper.Create<UnidadeResponsavelTipoFormularioData>(modelo));
        }

        public void ExcluirUnidadeResponsavelTipoFormulario(long seqConfiguracaoTipoFormulario)
        {
            UnidadeResponsavelService.ExcluirUnidadeResponsavelTipoFormulario(seqConfiguracaoTipoFormulario);
        }

        /// <summary>
        /// Busca os tipos de formulário disponíveis para a unidade responsável e grupo de aplicação GPI
        /// </summary>
        /// <param name="seqUnidadeResponsavel">Sequencial da unidade responsável</param>
        /// <returns>Lista de tipos de formulário</returns>
        public List<SMCDatasourceItem> BuscarTiposFormulariosSelect(long seqUnidadeResponsavel)
        {
            // Busca a unidade responsável do SGF que está vinculada a unidade recebida como parâmetro
            var seqUnidadeResponsavelSgf = UnidadeResponsavelService.BuscarSeqUnidadeResponsavelSgf(seqUnidadeResponsavel);
            if (!seqUnidadeResponsavelSgf.HasValue)
                return new List<SMCDatasourceItem>();

            // Chama o serviço do SGF para buscar os tipos de formulário.
            var filtro = new TipoFormularioKeyValueFiltroData() 
            { 
                SeqUnidadeResponsavel = seqUnidadeResponsavelSgf.Value,
                SiglaGrupoAplicacaoSAS = SIGLA_APLICACAO.GRUPO_APLICACAO
            };
            return FormularioService.BuscarTipoFormularioKeyValue(filtro).TransformList<SMCDatasourceItem>();
        }

        public List<SMCDatasourceItem> BuscarTiposFormularioAssociadosSelect(long seqUnidadeResponsavel)
        {
            return this.UnidadeResponsavelService.BuscarTiposFormularioKeyValue(seqUnidadeResponsavel)
                .TransformList<SMCDatasourceItem>();
        }

        public List<SMCDatasourceItem> BuscarFormulariosSelect(long SeqTipoFormulario)
        {
            return this.FormularioService.BuscarFormulariosKeyValue(
                new FormularioFiltroData { SeqTipoFormulario = SeqTipoFormulario })
                .TransformList<SMCDatasourceItem>();
        }

        public List<SMCDatasourceItem> BuscarVisoesSelect(long SeqTipoFormulario)
        {
            return this.FormularioService.BuscarVisoesKeyValue(
                new VisaoFiltroData { SeqTipoFormulario = SeqTipoFormulario })
                .TransformList<SMCDatasourceItem>();
        }

        public List<SMCDatasourceItem> BuscarUnidadesResponsaveisSelectSGF()
        {
            return this.UnidadeResponsavelServiceSGF.BuscarUnidadesResponsaveisSelect();
        }

        #endregion Unidade Responsável x Tipos Formulario

        #region Unidade Responsável x Tipo Processo

        public SMCPagerModel<UnidadeResponsavelTipoProcessoListaViewModel> BuscarUnidadeResponsavelTiposProcessos(long seqParametro)
        {
            var datas = UnidadeResponsavelService.BuscarUnidadeResponsavelTiposProcessos(seqParametro);
            SMCPagerData<UnidadeResponsavelTipoProcessoListaViewModel> model =
                SMCMapperHelper.Create<SMCPagerData<UnidadeResponsavelTipoProcessoListaViewModel>>(datas);
            return new SMCPagerModel<UnidadeResponsavelTipoProcessoListaViewModel>(model);
        }

        public UnidadeResponsavelTipoProcessoViewModel BuscarUnidadeResponsavelTipoProcesso(long seqParametro)
        {
            return SMCMapperHelper.Create<UnidadeResponsavelTipoProcessoViewModel>(
                        UnidadeResponsavelService.BuscarUnidadeResponsavelTipoProcesso(seqParametro));
        }

        public long SalvarUnidadeResponsavelTipoProcessoTipoHierarquiaOferta(UnidadeResponsavelTipoProcessoViewModel modelo)
        {
            return UnidadeResponsavelService.SalvarUnidadeResponsavelTipoProcessoTipoHierarquiaOferta(
                        SMCMapperHelper.Create<UnidadeResponsavelTipoProcessoData>(modelo));
        }

        public void ExcluirUnidadeResponsavelTipoProcessoTipoHierarquiaOferta(long seqConfiguracaoTipoProcessoTipoHierarquiaOferta)
        {
            UnidadeResponsavelService.ExcluirUnidadeResponsavelTipoProcessoTipoHierarquiaOferta(seqConfiguracaoTipoProcessoTipoHierarquiaOferta);
        }

        public List<SMCDatasourceItem> BuscarTiposProcessoAssociados(long? seqUnidadeResponsavel)
        {
            return this.UnidadeResponsavelService.BuscarTiposProcessoAssociados(seqUnidadeResponsavel)
                .TransformList<SMCDatasourceItem>();
        }

        public List<SMCDatasourceItem> BuscarTiposHierarquiaOfertaAssociados(long seqUnidadeResponsavel, long seqTipoProcesso)
        {
            return this.UnidadeResponsavelService.BuscarTiposHieraquiaOfertaAssociados(seqUnidadeResponsavel, seqTipoProcesso)
               .TransformList<SMCDatasourceItem>();
        }

        #endregion Unidade Responsável x Tipo Processo

        public List<SMCDatasourceItem<string>> BuscarSistemaOrigemGADSelect(string sigla)
        {
            return UnidadeResponsavelService.BuscarSistemaOrigemGADSelect(sigla);
        }
    }
}