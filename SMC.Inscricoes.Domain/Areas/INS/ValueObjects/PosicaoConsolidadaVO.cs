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
    /// Value Objec para representar a posição consolidade de um processo
    /// </summary>
    public class PosicaoConsolidadaVO : ISMCMappable
    {
        public int Total { get; set; }
        public int Deferidos { get; set; }
        public int Indeferidos { get; set; }
        public int Finalizadas { get; set; }
        public int Iniciadas { get; set; }
        public int Confirmadas { get; set; }
        public int NaoConfirmadas { get; set; }
        public int DocumentacoesEntregues { get; set; }
        public int Pagas { get; set; }
        public int Canceladas { get; set; }
        public string Descricao { get; set; }
        public long Seq { get; set; }
        public int OfertasNaoSelecionadas { get; set; }

    }
}
