using MC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Financeiro.ServiceContract.BLT.Data;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.SEL.DomainServices;
using SMC.Inscricoes.Domain.Areas.SEL.Models;
using SMC.Inscricoes.Domain.Areas.SEL.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.AcompanhamentoCheckin;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.Inscricao;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.InscricaoOferta;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Seguranca.ServiceContract.Areas.USU.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class InscricaoOfertaService : SMCServiceBase, IInscricaoOfertaService
    {
        #region DomainService

        private InscricaoOfertaDomainService InscricaoOfertaDomainService => this.Create<InscricaoOfertaDomainService>();

        #endregion DomainService

        public RespostaCheckinData EfetuarCheckin(CheckinData dados)
        {
            return this.InscricaoOfertaDomainService.EfetuarCheckin(dados.Transform<CheckinVO>()).Transform<RespostaCheckinData>();
        }

        public RespostaCheckinData EfetuarCheckinManual(CheckinData dados)
        {
            return this.InscricaoOfertaDomainService.EfetuarCheckinManual(dados.Transform<CheckinVO>()).Transform<RespostaCheckinData>();
        }

        public RespostaCheckinData PesquisaNomeCheckinManual(FiltroCheckinData dados)
        {
            return this.InscricaoOfertaDomainService.PesquisaNomeCheckinManual(dados.Transform<FiltroCheckinVO>()).Transform<RespostaCheckinData>();
        }

        public RespostaCheckinData EfetuarCheckinLote(CheckinData dados)
        {
            return this.InscricaoOfertaDomainService.EfetuarCheckinLote(dados.Transform<CheckinVO>()).Transform<RespostaCheckinData>();
        }
        public RespostaCheckinData DesfazerCheckinLote(CheckinData dados)
        {
            return this.InscricaoOfertaDomainService.DesfazerCheckinLote(dados.Transform<CheckinVO>()).Transform<RespostaCheckinData>();
        }

        public List<RespostaPesquiaOfertaCheckinData> PesquisaOfertaCheckinManual(FiltroCheckinData dados)
        {
            return this.InscricaoOfertaDomainService.PesquisaOfertaCheckinManual(dados.Transform<FiltroCheckinVO>()).TransformList<RespostaPesquiaOfertaCheckinData>();
        }

        /// <summary>
        /// Efeturar o checkout do inscrito
        /// </summary>
        /// <param name="guid">Guid inscrição oferta</param>
        /// <returns></returns>
        public RespostaCheckinData EfetuarCheckout(string guid)
        {
            return this.InscricaoOfertaDomainService.EfetuarCheckout(guid).Transform<RespostaCheckinData>();
        }

        public bool VerificaPossuiCkeckin(long seqInscricao)
        {
            return this.InscricaoOfertaDomainService.VerificaPossuiCkeckin(seqInscricao);
        }

        public CabecalhoCheckinLoteData BuscarCabecalhoCheckinLote(long seqOferta, long seqProcesso)
        {
            return this.InscricaoOfertaDomainService.BuscarCabecalhoCheckinLote(seqOferta, seqProcesso).Transform<CabecalhoCheckinLoteData>();
        }

        public SMCPagerData<ListarCheckinLoteData> BuscarInscritosCheckinLote(CheckinLoteFiltroData filtro)
        {
            var retorno = this.InscricaoOfertaDomainService.BuscarInscritosCheckinLote(filtro).Transform<SMCPagerData<ListarCheckinLoteData>>();
            return retorno;
        }

        /// <summary>
        /// Buscar dados da inscrição oferta por guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>Dados da inscricao oferta</returns>
        public InscricaoOfertaData BuscarInscricaoOfertaPorGuid(Guid guid)
        {
            var retorno = this.InscricaoOfertaDomainService.BuscarInscricaoOfertaPorGuid(guid).Transform<InscricaoOfertaData>();
            return retorno;
        }
    }
}