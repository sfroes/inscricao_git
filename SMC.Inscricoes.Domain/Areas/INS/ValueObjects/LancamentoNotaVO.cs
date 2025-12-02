using SMC.Framework;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec uma opção de oferta de um processo
    /// </summary>
    public class LancamentoNotaVO : ISMCMappable
    {   
        public long SeqInscricao { get; set; }

        public decimal? NotaGeral { get; set; }

        public int? NumeroClassificacao { get; set; }

        public string NomeInscrito { get; set; }
    }
}
