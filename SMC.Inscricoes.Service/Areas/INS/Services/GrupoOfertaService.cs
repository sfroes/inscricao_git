using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Linq;
using System.Collections.Generic;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Common.Areas.INS;
using System;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class GrupoOfertaService : SMCServiceBase, IGrupoOfertaService
    {
        #region DomainService

        private OfertaDomainService OfertaDomainService
        {
            get { return this.Create<OfertaDomainService>(); }
        }

        private GrupoOfertaDomainService GrupoOfertaDomainService
        {
            get { return this.Create<GrupoOfertaDomainService>(); }
        }

        #endregion

        public SMCPagerData<PosicaoConsolidadaGrupoOfertaData> BuscarPosicaoConsolidadaGruposOferta(PosicaoConsolidadaGrupoOfertaFiltroData filtro)
        {
            int total;
            var grupos = this.OfertaDomainService.BuscarPosicaoConsolidadaProcesso(
                SMCMapperHelper.Create<PosicaoConsolidadaGrupoOfertaFilterSpecification>(filtro), out total);
            return new SMCPagerData<PosicaoConsolidadaGrupoOfertaData>(
                grupos.TransformList<PosicaoConsolidadaGrupoOfertaData>(), total);            
        }


        public IEnumerable<SMCDatasourceItem> BuscaGruposOfertaKeyValue(long seqProcesso)
        {
            return GrupoOfertaDomainService.BuscarGruposOfertaKeyValue(new
                GrupoOfertaFilterSpecification { SeqProcesso = seqProcesso});
        }



        public SMCPagerData<GrupoOfertaListaData> BuscarGruposOferta(GrupoOfertaFiltroData filtroData)
        {
            var spec = SMCMapperHelper.Create<GrupoOfertaFilterSpecification>(filtroData);
            int total = 0;
            var itens = this.GrupoOfertaDomainService.SearchProjectionBySpecification(spec,
                        x => new GrupoOfertaListaData
                        {
                            Seq = x.Seq,
                            Nome = x.Nome,
                            SeqProcesso = x.SeqProcesso
                        }, out total);
            return new SMCPagerData<GrupoOfertaListaData>(itens, total);
        }

        public GrupoOfertaData BuscarGrupoOferta(long seqGrupoOferta)
        {
            var ret = this.GrupoOfertaDomainService.SearchByKey<GrupoOferta, GrupoOfertaData>(seqGrupoOferta, IncludesGrupoOferta.Ofertas);
            var spec = new OfertaFilterSpecification { SeqGrupoOferta = seqGrupoOferta };
                spec.SetOrderBy(x => x.Nome);
            var ofertas = this.OfertaDomainService.SearchByDepth(spec,10,x=>x.HierarquiaOfertaPai, x => x.Processo);
            foreach(var of in ret.Ofertas)
            {
                var oferta = ofertas.FirstOrDefault(x => x.Seq == of.Seq);

                OfertaDomainService.AdicionarDescricaoCompleta(oferta, oferta.Processo.ExibirPeriodoAtividadeOferta);
                
                of.DescricaoCompleta = oferta.DescricaoCompleta;
                of.NomeGrupoOferta = ret.Nome;
            }

            return ret;

        }

        public long SalvarGrupoOferta(GrupoOfertaData grupoOfertaData)
        {
            return this.GrupoOfertaDomainService.Salvar(grupoOfertaData.Transform<GrupoOferta>());
        }

        public void ExcluirGrupoOferta(long seqGrupoOferta)
        {
            this.GrupoOfertaDomainService.DeleteEntity(seqGrupoOferta);
        }

        public Guid BuscarSeqProcessoPorSeqGrupoOferta(long seqGrupoOferta)
        {
           return  this.GrupoOfertaDomainService.BuscarSeqProcessoPorSeqGrupoOferta(seqGrupoOferta);
        }
    }
}
