using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System.Collections.Generic;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class NovaEntregaDocumentacaoCabecalhoViewModel : SMCViewModelBase, ISMCMappable
    {      
        public string Usuario { get; set; }
        public string Processo { get; set; }
        public string GrupoOferta { get; set; }
        public List<string> Oferta { get; set; }
    }
}