using SMC.Framework.Model;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Framework.Extensions;
using SMC.Inscricoes.Common.Areas.INS;
using System.Collections;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class GrupoTaxaItemDomainService : InscricaoContextDomain<GrupoTaxaItem>
    {
        public List<GrupoTaxaItemVO> BuscaGrupoTaxaItem(long seqGrupoTaxa)
        {
            var results = this.SearchProjectionBySpecification(
                new GrupoTaxaItemFilterSpecification { SeqGrupoTaxa = seqGrupoTaxa }, 
                grpTI => new GrupoTaxaItemVO
                {
                    Seq = grpTI.Seq,
                    SeqTipoTaxa = grpTI.Taxa.TipoTaxa.Seq,
                    SeqTaxa = grpTI.Taxa.Seq,
                    SeqGrupoTaxa = grpTI.SeqGrupoTaxa,
                    DescTipoTaxa = grpTI.Taxa.TipoTaxa.Descricao
                }).ToList();                       

            return results;
        }

        public List<SMCDatasourceItem> BuscaGrupoTaxaItemSelect(long seqGrupoTaxa) 
        {
            GrupoTaxaItemFilterSpecification spec = new GrupoTaxaItemFilterSpecification { SeqGrupoTaxa = seqGrupoTaxa };
            spec.SetOrderBy(gti => gti.Taxa.TipoTaxa.Descricao);

            var itens = this.SearchProjectionBySpecification(spec, x => new SMCDatasourceItem { 
            
                Seq = x.Seq,
                Descricao = x.Taxa.TipoTaxa.Descricao

            }).OrderBy(x => x.Descricao).ToList();
            

            return itens;
        }

        public List<SMCDatasourceItem> BuscaGrupoTaxaItemSelected(long seqGrupoTaxa)
        {
            GrupoTaxaItemFilterSpecification spec = new GrupoTaxaItemFilterSpecification { SeqGrupoTaxa = seqGrupoTaxa };
            spec.SetOrderBy(gti => gti.Taxa.TipoTaxa.Descricao);

            var itens = this.SearchProjectionBySpecification(spec, x => new SMCDatasourceItem
            {

                Seq = x.Seq,
                Descricao = x.Taxa.TipoTaxa.Descricao,
                Selected = true

            }).ToList();


            return itens;
        }

    }
}