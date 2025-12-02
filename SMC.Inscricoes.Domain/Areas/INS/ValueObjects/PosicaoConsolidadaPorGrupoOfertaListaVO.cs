using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class PosicaoConsolidadaPorGrupoOfertaListaVO : ISMCMappable
    {
        public long Seq { get; set; }

        public long SeqProcesso { get; set; }

        public long? SeqGrupoOferta { get; set; }

        public string Descricao { get; set; }

        public int CandidatosConfirmados { get; set; }

        public int CandidatosDesistentes { get; set; }

        public int CandidatosReprovados { get; set; }

        public int CandidatosSelecionados { get; set; }

        public int CandidatosExcedentes { get; set; }

        public int Convocados { get; set; }

        public int ConvocadosDesistentes { get; set; }

        public int ConvocadosConfirmados { get; set; }

        public List<PosicaoConsolidadaPorOfertaListaVO> PosicoesConsolidadasOfertas { get; set; }
    }
}
