using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupOfertaFolhaTransformer : ISMCTreeTransformer<LookupOfertaFiltroViewModel>
    {
        public List<SMCTreeViewNode<object>> Transform(IEnumerable<object> source, LookupOfertaFiltroViewModel filter)
        {
            source = source.OrderBy(s => (s as LookupOfertaViewModel).Descricao);
            List<SMCTreeViewNode<object>> treeView = SMCTreeView.For(source).AllowCheck(x => (x as LookupOfertaViewModel).IsLeaf);
            return treeView;
        }
    }
}