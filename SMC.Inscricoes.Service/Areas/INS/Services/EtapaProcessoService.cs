using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class EtapaProcessoService : SMCServiceBase, IEtapaProcessoService
    {
        #region DomainService

        private EtapaProcessoDomainService EtapaProcessoDomainService
        {
            get { return this.Create<EtapaProcessoDomainService>(); }
        }

        private IEtapaService EtapaService
        {
            get { return this.Create<IEtapaService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        #endregion

        /// <summary>
        /// Busca a lista de etapas de um processo filtrada
        /// </summary>        
        public SMCPagerData<EtapaProcessoListaData> BuscarEtapasProcesso(EtapaProcessoFiltroData filtro)
        {
            var etapas = this.EtapaProcessoDomainService.SearchBySpecification(
                            new EtapaProcessoFilterSpecification(filtro.SeqProcesso) { SeqEtapaSGF = filtro.SeqEtapaSGF });
            var seqEtapasSGF = etapas.Select(x => x.SeqEtapaSGF).ToArray();
            var etapasSGF = this.EtapaService.BuscarEtapasKeyValue(seqEtapasSGF);

            List<EtapaProcessoListaData> data = new List<EtapaProcessoListaData>();
            foreach (var item in etapasSGF)
            {
                var etapa = etapas.Where(f => f.SeqEtapaSGF == item.Seq).First();
                var etapadata = etapa.Transform<EtapaProcessoListaData>();
                etapadata.DescricaoEtapaSGF = item.Descricao;
                data.Add(etapadata);
            }
            return new SMCPagerData<EtapaProcessoListaData>(data, data.Count);
        }

        public EtapaProcessoData BuscarEtapaProcesso(long seqEtapaProcesso)
        {
            return this.EtapaProcessoDomainService.SearchByKey<EtapaProcesso, EtapaProcessoData>(seqEtapaProcesso);
        }

        public void ExcluirEtapaProcesso(long seqEtapaProcesso)
        {
            this.EtapaProcessoDomainService.ExcluirEtapaProcesso(seqEtapaProcesso);
        }

        public long SalvarEtapaProcesso(EtapaProcessoData etapaProcesso)
        {
            return this.EtapaProcessoDomainService.SalvarEtapaProcesso(etapaProcesso.Transform<EtapaProcesso>());
        }

        public SMCDatasourceItem[] BuscarEtapasSGFKeyValue(long seqProcesso)
        {
            var seqTemplateSGF = this.ProcessoDomainService.SearchProjectionByKey(
                new SMCSeqSpecification<Processo>(seqProcesso),
                x => x.SeqTemplateProcessoSGF);
            return this.EtapaService.BuscarEtapasPorTemplateKeyValue(seqTemplateSGF);
        }

        public List<SMCDatasourceItem> BuscarSituacoesPermitidas(long seqEtapaProcesso)
        {
            return this.EtapaProcessoDomainService.BuscarSituacoesPermitidas(seqEtapaProcesso);
        }

        public CabecalhoProcessoEtapaData BuscarCabecalhoProcessoEtapa(long seqEtapaProcesso)
        {
            var cabecalho = this.EtapaProcessoDomainService.SearchProjectionByKey(
                    new SMCSeqSpecification<EtapaProcesso>(seqEtapaProcesso),
                    x => new CabecalhoProcessoEtapaData
                    {
                        SeqEtapaProcesso = seqEtapaProcesso,
                        SeqProcesso = x.SeqProcesso,
                        SeqEtapaSGF = x.SeqEtapaSGF,
                        DescricaoProcesso = x.Processo.Descricao,
                        DescricaoTipoProcesso = x.Processo.TipoProcesso.Descricao
                    }
                );
            cabecalho.DescricaoEtapa = this.EtapaService.BuscarEtapasKeyValue(
                new long[] { cabecalho.SeqEtapaSGF })[0].Descricao;
            return cabecalho;
        }

        
        /// <summary>
        /// Verifica se a inclusão de etapas é permitida
        /// </summary>        
        public void VerificarPermissaoCadastrarEtapa(long seqProcesso) 
        {
            this.EtapaProcessoDomainService.VerificarPermissaoCadastrarEtapa(seqProcesso);
        }

        /// <summary>
        /// Busca a lista de configurações das ofertas selecionadas bem como as taxas
        /// existentes para estas ofertas (DISTINCT)
        /// </summary>        
        public List<ProrrogacaoConfiguracaoData> BuscarConfiguracoesProrrogacao(long seqEtapaProcesso
            , long[] seqOfertas)
        {
            return this.EtapaProcessoDomainService.BuscarConfiguracoesProrrogacao(seqEtapaProcesso,
                seqOfertas).TransformList<ProrrogacaoConfiguracaoData>();
        }

        /// <summary>
        /// Recupera o sumário de uma prorrogação de processo para ser exibido para o usuário
        /// </summary>
        public ProrrogacaoEtapaData SumarioProrrogacao(ProrrogacaoEtapaData etapaProrrogar)
        {
            return this.EtapaProcessoDomainService.SumarioProrrogacao(
                etapaProrrogar.Transform<ProrrogacaoEtapaVO>()).Transform<ProrrogacaoEtapaData>();
         }

        /// <summary>
        /// Prorroga a etapa informada com os dados passados no DTO
        /// </summary>
        /// <param name="etapaProrrogar"></param>
        public void ProrrogarEtapa(ProrrogacaoEtapaData etapaProrrogar)
        {
            this.EtapaProcessoDomainService.ProrrogarEtapa(
                etapaProrrogar.Transform<ProrrogacaoEtapaVO>());
        }

        /// <summary>
        /// Verifica se é possível prorrogar a etapa informada
        /// </summary>        
        public void VerificarPossibilidadeProrrogacao(long seqEtapaProcesso)
        {
            this.EtapaProcessoDomainService.VerificarPossibilidadeProrrogacao(seqEtapaProcesso);
        }

        public bool ExisteEtapa(long seqProcesso, string token)
        {
            var spec = new EtapaProcessoFilterSpecification(seqProcesso) { Token = token };
            return EtapaProcessoDomainService.Count(spec) > 0;
        }
    }
}
