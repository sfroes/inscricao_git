using System;
using System.Collections.Generic;
using SMC.Framework;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Framework.Model;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Localidades.UI.Mvc.Models;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class InscricaoOfertaListaViewModel : SMCViewModelBase, ISMCMappable
    {
        public short NumeroOpcao { get; set; }

        public string DescricaoCompleta { get; set; }

        public string JustificativaInscricao { get; set; }

        public bool ExibeDestacado 
        { 
            get
            {
                return (NumeroOpcao == 1);
            }
        }
    }

}



