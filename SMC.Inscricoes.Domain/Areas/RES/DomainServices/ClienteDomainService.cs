using SMC.Framework.Domain;
using SMC.Framework.Model;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.RES.DomainServices
{
    public class ClienteDomainService : InscricaoContextDomain<Cliente>
    {
        /// <summary>
        /// Retorna todos os tipos de processo com apenas seq  e descricao preenchidos
        /// </summary>        
        public IEnumerable<SMCDatasourceItem> BuscarClientesKeyValue() 
        {
            return this.SearchProjectionAll(x => new SMCDatasourceItem { Seq = x.Seq, Descricao = x.Nome }
                , x => x.Nome).ToList();
        }
    }
     
}
