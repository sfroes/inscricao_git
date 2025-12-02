using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using System;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ListarCheckinLoteViewModel : SMCViewModelBase, ISMCMappable
    {
        public long SeqInscricao { get; set; }

        public string NomeInscrito { get; set; }

        public bool PossuiCheckin { get; set; }

        public DateTime? DataHoraCheckin { get; set; }

        public string DataHoraFormatado
        {
            get => string.Format("{0:dd/MM/yyyy HH:mm}", this.DataHoraCheckin);
        }

        public string Responsavel { get; set; }
    }
}