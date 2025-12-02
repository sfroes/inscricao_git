using SMC.DadosMestres.Common.Areas.GED.Enums;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.DadosMestres.Common.Areas.SHA.Enums;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class IngressoOfertaData : ISMCMappable
    {
        public long SeqInscricaoOferta { get; set; }
        public string DescricaoOferta { get; set; }
        public bool HabilitaCheckin { get; set; }
        public Guid? UidInscricaoOferta { get; set; }
        public string DescicaoParte1 { get; set; }
        public string DescicaoParte2 { get; set; }
        public string DescicaoParte3 { get; set; }
        public long SeqOferta { get; set; }
        public string QrCodeOferta { get; set; }
        public string TagAtividade { get; set; }
        public string CSSTagAtividade { get; set; }
        public string Titulo { get; set; }
    }
}
