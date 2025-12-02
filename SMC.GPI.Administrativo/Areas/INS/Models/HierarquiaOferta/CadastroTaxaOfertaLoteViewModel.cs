using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class CadastroTaxaOfertaLoteViewModel : SMCViewModelBase, ISMCMappable
    {
        public CadastroTaxaOfertaLoteViewModel()
        {
            ItensAdicionar = new SMCPagerModel<SMCDatasourceItem>();
            ItensExcluir = new SMCPagerModel<SMCDatasourceItem>();
        }

        public SMCPagerModel<SMCDatasourceItem> ItensAdicionar { get; set; }

        public SMCPagerModel<SMCDatasourceItem> ItensExcluir { get; set; }
    }
}
