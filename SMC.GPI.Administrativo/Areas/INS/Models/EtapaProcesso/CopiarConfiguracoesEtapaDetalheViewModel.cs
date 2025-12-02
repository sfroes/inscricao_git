using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class CopiarConfiguracoesEtapaDetalheViewModel : SMCViewModelBase, ISMCMappable
    {


        [SMCRequired]
        [SMCSelect("ConfiguracaoesEtapaOrigem")]
        [SMCSize(SMCSize.Grid7_24)]
        public long SeqConfiguracaoEtapa { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid11_24)]
        [SMCDependency("SeqConfiguracaoEtapa","BuscarDescricaoConfiguracao","ConfiguracaoEtapa",false)]
        public string Descricao { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        public string NumeroEdital { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid7_24)]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime? DataInicio { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid7_24)]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        [SMCMinDate("DataInicio")]
        public DateTime? DataFim { get; set; }

        [SMCSize(SMCSize.Grid7_24)]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]        
        public DateTime? DataLimiteDocumentacao { get; set; }
        
        
                        

    }
}