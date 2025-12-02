using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class DocumentosPendentesIndeferidosVO : ISMCMappable
    {
        public long SeqInscricao { get; set; }
        public string DocumentosPendentes { get; set; }
        public string DocumentosIndeferidos { get; set; }
    }
}
