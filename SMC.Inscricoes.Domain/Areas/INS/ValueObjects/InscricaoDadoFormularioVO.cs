using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    public class InscricaoDadoFormularioVO : ISMCMappable
    {

        public long Seq { get; set; }

        public long SeqInscricao { get; set; }

        public long SeqFormulario { get; set; }

        public long SeqVisao { get; set; }

        public bool Editable { get; set; }

        public string TituloFormulario { get; set; }

        public int? Ordem { get; set; }

        public long? SeqVisaoConfigucacao { get; set; }

        public long? SeqVisaoGestaoConfiguracao { get; set; }
    }
}
