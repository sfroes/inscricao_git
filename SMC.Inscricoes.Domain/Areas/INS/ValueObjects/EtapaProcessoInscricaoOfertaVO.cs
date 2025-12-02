using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class EtapaProcessoInscricaoOfertaVO : ISMCMappable
    {
        public long? SeqInscricaoOferta { get; set; }


        public DateTime? DataInicioEtapa { get; set; }

        public DateTime? DataFimEtapa { get; set; }

        public string EtapaProcessoToken { get; set; }

        public SituacaoEtapa SituacaoEtapa { get; set; }

    }
}
