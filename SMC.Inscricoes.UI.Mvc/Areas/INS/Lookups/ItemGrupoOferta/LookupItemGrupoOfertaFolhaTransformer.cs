using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{

    public class LookupItemGrupoOfertaFolhaTransformer : ISMCTreeTransformer<LookupItemGrupoOfertaFiltroViewModel>
    {
        public List<SMCTreeViewNode<object>> Transform(IEnumerable<object> source, LookupItemGrupoOfertaFiltroViewModel filter)
        {
            return SMCTreeView.For(source).AllowCheck(x => (x as LookupItemGrupoOfertaViewModel).EOferta);
        }
    }
}
