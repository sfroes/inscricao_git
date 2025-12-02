using SMC.Framework;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMC.Framework.DataAnnotations;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupEtapaFiltroViewModel : SMCLookupFilterViewModel
    {
        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        public long? Seq { get; set; }

        [SMCFilter]
        [SMCSelect("UnidadesResponsaveis")]
        [SMCSize(SMCSize.Grid6_24)]
        public long? SeqUnidadeResponsavelSelecionado { get; set; }

        public List<SMCDatasourceItem> UnidadesResponsaveis { get; set; }

        [SMCFilter]
        [SMCSelect("TiposProcesso")]
        [SMCSize(SMCSize.Grid6_24)]
        public long? SeqTipoProcessoSelecionado { get; set; }

        public List<SMCDatasourceItem> TiposProcesso { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid8_24)]
        public string Processo { get; set; }

        [SMCFilter]
        [SMCMask("0000")]
        [SMCSize(SMCSize.Grid3_24)]
        public int? Ano { get; set; }

        [SMCFilter]
        [SMCMask("0")]
        [SMCMinValue(1)]
        [SMCMaxValue(2)]
        [SMCSize(SMCSize.Grid3_24)]
        public int? Semestre { get; set; }

        [SMCFilter]
        [SMCSelect("Etapas")]
        [SMCSize(SMCSize.Grid4_24)]
        public long? SeqEtapaSelecionado { get; set; }

        public List<SMCDatasourceItem> Etapas { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid6_24)]
        public string Configuracao { get; set; }
    }
}
