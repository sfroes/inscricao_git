using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{

    public class LookupOfertaPeriodoInscricaoFolhaTransformer : ISMCTreeTransformer<LookupOfertaPeriodoInscricaoFiltroViewModel>
    {
        public List<SMCTreeViewNode<object>> Transform(IEnumerable<object> source, LookupOfertaPeriodoInscricaoFiltroViewModel filter)
        {
            return SMCTreeView.For(source).AllowCheck(x=> (x as LookupOfertaPeriodoInscricaoViewModel).EOferta);
        }
    }
}
