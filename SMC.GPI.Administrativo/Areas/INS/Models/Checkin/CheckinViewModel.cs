using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Models;
using SMC.Localidades.UI.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class CheckinViewModel : SMCViewModelBase, ISMCMappable
    {
        public long SeqProcesso { get; set; }
        public string DescricaoProcesso { get; set; }        
        public bool Atendente { get; set; }
        public DateTime DataInicioAtividade { get; set; }
        public DateTime DataFimAtividade { get; set; }
        public List<string> Ofertas { get; set; }
    }
}