using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Extensions;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Inscricao.Areas.INS.Models;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;

namespace SMC.GPI.Inscricao.Areas.INS.Services
{
    public class ProcessoControllerService : SMCControllerServiceBase
    {
        
        #region Service

        private IProcessoService ProcessoService
        {
            get { return this.Create<IProcessoService>(); }
        }

        #endregion

        /// <summary>
        /// Buscar as inscrições que estão abertas
        /// </summary>
        /// <param name="idioma">Filtros para pesquisa</param>
        /// <returns>Lista de inscrições abertas</returns>
        public SMCPagerModel<ProcessoAbertoListaViewModel> BuscarProcessosComInscricoesEmAberto(ProcessoAbertoFiltroViewModel filtro)
        {
            EtapaProcessoAbertoFiltroData filtroData = SMCMapperHelper.Create<EtapaProcessoAbertoFiltroData>(filtro);
            SMCPagerData<EtapaProcessoAbertoListaData> datas = ProcessoService.BuscarProcessosComInscricoesEmAberto(filtroData);
            SMCPagerData<ProcessoAbertoListaViewModel> model = SMCMapperHelper.Create<SMCPagerData<ProcessoAbertoListaViewModel>>(datas);
            return new SMCPagerModel<ProcessoAbertoListaViewModel>(model, filtro.PageSettings, filtro);
        }

        /// <summary>
        /// Buscar os grupos de oferta com inscrição em aberto em um processo
        /// </summary>
        /// <param name="filtro">Filtros para pesquisa</param>
        /// <returns>Lista de grupos de oferta com inscrição em aberto em um processo</returns>
        public SMCPagerModel<GrupoOfertaProcessoListaVewModel> BuscarGrupoOfertaInscricaoAberta(ProcessoAbertoFiltroViewModel filtro)
        {
            EtapaProcessoAbertoFiltroData filtroData = SMCMapperHelper.Create<EtapaProcessoAbertoFiltroData>(filtro);
            SMCPagerData<EtapaProcessoAbertoListaData> datas = ProcessoService.BuscarProcessosComInscricoesEmAberto(filtroData);
            if (datas.Itens.Count == 1)
            {
                List<GrupoOfertaProcessoListaVewModel> lista = datas.Itens.First().Grupos.TransformList<GrupoOfertaProcessoListaVewModel>();
                return new SMCPagerModel<GrupoOfertaProcessoListaVewModel>(lista, filtro.PageSettings, filtro);
            }
            else
            {
                return new SMCPagerModel<GrupoOfertaProcessoListaVewModel>();
            }
        }

        /// <summary>
        /// Busca as informações de um processo com suas configurações de inscrições em aberto
        /// </summary>
        /// <param name="uidProcesso">Uid do processo</param>
        /// <param name="idioma">Idioma que as informações devem ser recuperadas</param>
        /// <returns>Informações do processo com suas configurações de inscrição em aberto</returns>
        public ProcessoHomeViewModel BuscarProcessoHome(Guid uidProcesso, SMCLanguage idioma, long? seqInscrito)
        {
            ProcessoHomeData data = ProcessoService.BuscarProcessoHome(uidProcesso, idioma, seqInscrito);
            return SMCMapperHelper.Create<ProcessoHomeViewModel>(data);
        }


        public string BuscarDescricaoOfertaProcesso(long seqConfiguracaoEtapa, SMCLanguage idioma) 
        {
            return ProcessoService.BuscarDescricaoOfertaProcesso(seqConfiguracaoEtapa, idioma);
        }

    }
}