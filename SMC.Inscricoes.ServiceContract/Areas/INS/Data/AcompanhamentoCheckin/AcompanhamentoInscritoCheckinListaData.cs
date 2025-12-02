using SMC.DadosMestres.Common.Constants;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{

    public class AcompanhamentoInscritoCheckinListaData : ISMCMappable
    {
        public long Seq { get; set; }
        public long SeqProcesso { get; set; }
        public string DescricaoOferta { get; set; }
        public int NumeroInscrito { get; set; }
        public int NumeroChecinRealizado { get; set; }
        public int RestanteVagas { get; set; }
        public long NumeroVagasOferta { get; set; }
    }
}
