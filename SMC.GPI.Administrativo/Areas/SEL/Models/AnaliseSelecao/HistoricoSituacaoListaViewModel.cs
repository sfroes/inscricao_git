using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class HistoricoSituacaoListaViewModel : SMCViewModelBase
    {
        public long SeqProcesso { get; set; }
        
        public string TipoProcesso { get; set; }
        
        public string Descricao { get; set; }
                
        public long SeqInscricao { get; set; }
        
        public string Candidato { get; set; }
        
        public string GrupoOferta { get; set; }
        
        public string Opcao { get; set; }
        
        public string Oferta { get; set; }

        public List<HistoricoSituacaoItemViewModel> Historicos { get; set; }

        public string BackURL { get; set; }
    }
}