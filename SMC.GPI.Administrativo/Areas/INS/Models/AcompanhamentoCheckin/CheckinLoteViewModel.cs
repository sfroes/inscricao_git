using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class CheckinLoteViewModel : CheckinLoteFiltroViewModel, ISMCMappable
    {

        #region[Cabecalho]

        public string DescricaoProcesso { get; set; }

        public string DescricaoOferta { get; set; }

        //public long TotalInscritos { get; set; }
        //public long TotalChekinsRealizados { get; set; }

        //public long TotalRestantes { get; set; }

        public long SeqProcesso { get; set; }

        public long SeqOferta { get; set; }


        #endregion
    }
}