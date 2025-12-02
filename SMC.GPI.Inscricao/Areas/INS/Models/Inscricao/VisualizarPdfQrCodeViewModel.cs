using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using System.Drawing;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class VisualizarPdfQrCodeViewModel : SMCViewModelBase, ISMCMappable
    {

        public string QrCode { get; set; }
    }
}