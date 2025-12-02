using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class ConvocacaoFiltroViewModel : SMCViewModelBase
    {
        public long SeqProcesso { get; set; }

        public long SeqOferta { get; set; }
        
        public List<long> InscricoesOfertas { get; set; }
    }
}