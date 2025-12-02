using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class IngressoOfertaViewModel : SMCViewModelBase
    {
        public long SeqInscricaoOferta { get; set; }
        public string DescricaoOferta { get; set; }
        public bool HabilitaCheckin { get; set; }
        public Guid? UidInscricaoOferta { get; set; }
        public string DescicaoParte1 { get; set; }
        public string DescicaoParte2 { get; set; }
        public string DescicaoParte3 { get; set; }
        public string QrCodeOferta { get; set; }
        public long SeqOferta { get; set; }
        public string TagAtividade { get; set; }
        public string CSSTagAtividade { get; set; }

        public string Titulo { get; set; }
    }
}