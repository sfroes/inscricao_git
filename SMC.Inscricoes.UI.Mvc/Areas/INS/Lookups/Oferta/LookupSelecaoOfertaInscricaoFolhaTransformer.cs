using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{

    public class LookupSelecaoOfertaInscricaoFolhaTransformer : ISMCTreeTransformer<LookupOfertaFiltroViewModel>
    {
        public List<SMCTreeViewNode<object>> Transform(IEnumerable<object> source, LookupOfertaFiltroViewModel filter)
        {
            source = source.OrderBy(s => (s as LookupSelecaoOfertaInscricaoViewModel).Descricao);
            List<SMCTreeViewNode<object>> treeView = SMCTreeView.For(source).AllowCheck(x => (x as LookupSelecaoOfertaInscricaoViewModel).IsLeaf);
            return treeView;
        }
    }
}
