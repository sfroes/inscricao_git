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
    public class PosicaoConsolidadaProcessoVO : PosicaoConsolidadaVO
    {
        public int OfertasNaoSelecionadas { get; set; }

    }
}
