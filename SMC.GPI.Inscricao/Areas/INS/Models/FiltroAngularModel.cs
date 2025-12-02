using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class FiltroAngularModel 
    {

        public long SeqConfiguracaoEtapaPagina { get; set; }

        public long SeqConfiguracaoEtapa { get; set; }

        public long SeqGrupoOferta { get; set; }

        public long SeqInscricao { get; set; }

        public SMCLanguage idioma { get; set; }

        public Guid uidProcesso { get; set; }

        public string tokenAngular { get; set; }
    }
}