using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common.Enums;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class OfertaTaxaSelecionadaViewModel : SMCPagerViewModel, ISMCMappable
    {
        public long SeqProcesso { get; set; }

        public long SeqTipoTaxa { get; set; }

        public long? SeqGrupoOferta { get; set; }

        public string Periodo { get; set; }

        public List<long> GridIncluir { get; set; }

        public List<long> GridExcluir { get; set; }
    }
}