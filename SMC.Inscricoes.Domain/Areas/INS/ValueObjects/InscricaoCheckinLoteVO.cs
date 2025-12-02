using System;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class InscricaoCheckinLoteVO
    {
        public long SeqInscricao { get; set; }

        public string NomeInscrito { get; set; }

        public bool PossuiCheckin { get; set; }

        public DateTime? DataHoraCheckin { get; set; }

        public string Responsavel { get; set; }
    }
}
