using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Linq;
using System.Collections.Generic;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Framework.Specification;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class GrupoTaxaService : SMCServiceBase, IGrupoTaxaService
    {
        #region DomainService

        private GrupoTaxaDomainService GrupoTaxaDomainService
        {
            get { return this.Create<GrupoTaxaDomainService>(); }
        }

        private GrupoTaxaItemDomainService GrupoTaxaItemDomainService
        {
            get { return this.Create<GrupoTaxaItemDomainService>(); }
        }

        private TipoTaxaDomainService TipoTaxaDomainService
        {
            get { return this.Create<TipoTaxaDomainService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        #endregion

        /// <summary>
        /// Retorna o par de chave/valor para os grupos de Taxa do processo informado
        /// </summary>        

        public IEnumerable<SMCDatasourceItem> BuscaGrupoTaxa(long SeqProcesso)
        {
            return GrupoTaxaDomainService.BuscarGrupoTaxa(new GrupoTaxaFilterSpecification() { SeqProcesso = SeqProcesso });
        }

        /// <summary>
        /// Retorna a lista de grupos de taxa
        /// </summary>
        public List<GrupoTaxaListaData> BuscarGruposTaxa(GrupoTaxaFiltroData filtros)
        {
            var lista = this.GrupoTaxaDomainService.SearchProjectionBySpecification(
                filtros.Transform<GrupoTaxaFilterSpecification>(),
                grpt => new GrupoTaxaListaData
                {
                    Seq = grpt.Seq,                    
                    SeqProcesso = grpt.SeqProcesso,
                    Descricao = grpt.Descricao,
                    NumeroMinimoItens = grpt.NumeroMinimoItens,
                    NumeroMaximoItens = grpt.NumeroMaximoItens

                }).ToList();

            lista.ForEach(item =>
            {
                var itensGrupoTaxa = ListaGrupoTaxaItemPorGrupoTaxa(item.Seq);
                if (itensGrupoTaxa != null)
                {
                    item.Itens = new List<string>(itensGrupoTaxa);
                }
            });

            return lista;
        }

        private List<string> ListaGrupoTaxaItemPorGrupoTaxa(long seqGrupoTaxa)
        {
            var lista = GrupoTaxaItemDomainService.BuscaGrupoTaxaItem(seqGrupoTaxa).Select(g => g.DescTipoTaxa).ToList();

            return lista;
        }

        public IEnumerable<SMCDatasourceItem> BuscarGrupoTaxaItemPorGrupoTaxaSelect(long? seqGrupoTaxa)
        {
            return GrupoTaxaItemDomainService.BuscaGrupoTaxaItemSelect(seqGrupoTaxa.Value);
        }

        public List<SMCDatasourceItem> BuscarTipoTaxaSelect(long seqProcesso)
        {
            var itens = ProcessoDomainService.SearchProjectionByKey(
                            new SMCSeqSpecification<Processo>(seqProcesso),
                            x => x.Taxas
                            .Where(t => t.TipoCobranca != Common.Areas.INS.Enums.TipoCobranca.PorQuantidadeOfertas)
                            .Select(t => new SMCDatasourceItem
                            {
                                Seq = t.Seq,
                                Descricao = t.TipoTaxa.Descricao
                            }).OrderBy(o => o.Descricao));

            return itens.OrderBy(i => i.Descricao).ToList();
        }

        public List<SMCDatasourceItem> BuscarTipoTaxaSelect(long? seqGrupoTaxa)
        {
            var itens = TipoTaxaDomainService.SearchAll().Select(x => new SMCDatasourceItem()
            {
                Seq = x.Seq,
                Descricao = x.Descricao
            });
            
            return itens.ToList();            
        }

        public GrupoTaxaData BuscarGrupoTaxa(long seqGrupoTaxa)
        {
            var grupoTaxa = this.GrupoTaxaDomainService.SearchByKey<GrupoTaxa, GrupoTaxaData>(seqGrupoTaxa);

            if (grupoTaxa.Itens == null)
            {
                grupoTaxa.Itens = new List<GrupoTaxaItemData>();
            }

            var listaItens = GrupoTaxaItemDomainService
                .SearchBySpecification(new GrupoTaxaItemFilterSpecification { SeqGrupoTaxa = seqGrupoTaxa }).ToList();


            listaItens.ForEach(item =>
            {
                if (item != null)
                {
                    grupoTaxa.Itens.Add(new GrupoTaxaItemData
                    {
                        Seq = item.Seq,
                        SeqGrupoTaxa = item.SeqGrupoTaxa,
                        SeqTaxa = item.SeqTaxa
                    });
                }
            });

            return grupoTaxa;
        }

        public long SalvarGrupoTaxa(GrupoTaxaData grupoTaxaData)
        {
            return this.GrupoTaxaDomainService.SalvarGrupoTaxa(grupoTaxaData.Transform<GrupoTaxa>());
        }

        public void ExcluirGrupoTaxa(long seqGrupoTaxa)
        {
            this.GrupoTaxaDomainService.ExcluirGrupoTaxa(seqGrupoTaxa);
        }

        public List<GrupoTaxaData> BuscarGruposTaxaPorSeqProcesso(long seqProcesso)
        {
            return this.GrupoTaxaDomainService.BuscarGruposTaxaPorSeqProcesso(seqProcesso).TransformList<GrupoTaxaData>();
        }
    }
}
