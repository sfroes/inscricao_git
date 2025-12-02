using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class InscricaoDadoFormularioListaViewModel : SMCViewModelBase, ISMCMappable
    {
        public long SeqDadoFormulario { get; set; }

        public long SeqInscricao { get; set; }

        public string TituloFormulario { get; set; }

        public long SeqFormularioSGF { get; set; }

        public long SeqVisaoSGF { get; set; }
    }
}