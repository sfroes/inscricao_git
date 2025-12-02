using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class NovaEntregaDocumentacaoFiltroViewModel : SMCViewModelBase, ISMCMappable
    {
        public long SeqInscricao { get; set; }

        public long SeqConfiguracaoEtapa { get; set; }
    }
}