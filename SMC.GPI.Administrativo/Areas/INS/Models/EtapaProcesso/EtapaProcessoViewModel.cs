using Newtonsoft.Json;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class EtapaProcessoViewModel : SMCViewModelBase, ISMCMappable
    {
        public CabecalhoProcessoEtapaViewModel Cabecalho { get; set; }

        [SMCKey]
        [SMCHidden]        
        public long Seq { get; set; }

        [SMCHidden]        
        public long SeqProcesso { get; set; }

        [SMCRequired]
        [SMCSelect("Etapas")]
        [SMCSize(SMCSize.Grid6_24)]
        public long SeqEtapaSGF { get; set; }

        public List<SMCDatasourceItem> Etapas { get; set; }

        [SMCRequired]
        [SMCSelect("Situacoes")]
        [SMCSize(SMCSize.Grid6_24)]
        public short SituacaoEtapa { get; set; }

        public List<SMCDatasourceItem> Situacoes { get; set; }

        [SMCRequired]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        [SMCSize(SMCSize.Grid6_24)]
        public DateTime DataInicioEtapa { get; set; }

        [SMCRequired]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCMinDate("DataInicioEtapa")]
        public DateTime DataFimEtapa { get; set; }
    }
}