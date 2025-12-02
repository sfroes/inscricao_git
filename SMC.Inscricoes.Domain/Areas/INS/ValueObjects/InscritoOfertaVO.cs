using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class InscritoOfertaVO : ISMCMappable
    {
        public long SeqInscricaoOferta { get; set; }

        public long SeqInscricao { get; set; }

        public string Nome { get; set; }        

        public string Situacao { get; set; }

        public string TokenSituacao { get; set; }

        public string TokenEtapa { get; set; }

        public bool? Exportado { get; set; }

        public bool IntegraSGALegado { get; set; }
    }
}
