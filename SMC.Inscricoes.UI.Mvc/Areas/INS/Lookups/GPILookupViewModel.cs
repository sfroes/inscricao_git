using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class GPILookupViewModel : SMCViewModelBase, ISMCLookupData
	{
		[SMCDescription]
		[SMCOrder(1)]
		public string Descricao { get; set; }
		[SMCKey]
		[SMCOrder(0)]
		public long? Seq { get; set; }


	}
}
