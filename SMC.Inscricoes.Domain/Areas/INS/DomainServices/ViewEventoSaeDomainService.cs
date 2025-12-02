using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class ViewEventoSaeDomainService : InscricaoContextDomain<ViewEventoSae>
    {


        public List<SMCDatasourceItem> BuscarEventosSaeSelect(ViewEventoSaeFilterSpecification spec)
        {
            spec.SetOrderBy(s => s.Nome);

            var retorno = this.SearchProjectionBySpecification(spec, s => new SMCDatasourceItem
            {
                Seq = s.Codigo,
                Descricao = s.Nome
            }).ToList();

            return retorno;
        }
    }
}
