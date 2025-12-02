using SMC.Framework.Mapper;
using System;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects.AcompanhamentoCheckin
{
    public class ListarCheckinLoteVO : ISMCMappable
    {
        public long SeqInscricao { get; set; }

        public string NomeInscrito { get; set; }

        public bool PossuiCheckin { get; set; }

        public DateTime? DataHoraCheckin { get; set; }

        public string Responsavel { get; set; }
    }
}
