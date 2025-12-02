using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class AcompanhamentoInscritoCheckinListaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCKey]
        public long Seq { get; set; }
        public long SeqProcesso { get; set; }
        public string DescricaoOferta { get; set; }
        public int NumeroInscrito { get; set; }
        public int NumeroChecinRealizado { get; set; }
        public int RestanteVagas { get; set; }
        public long NumeroVagasOferta { get; set; }
    }
}