using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.SEL.Data
{
    public class AcompanhamentoSelecaoFiltroData : SMCPagerFilterData, ISMCMappable
    {
        public long? SeqUnidadeResponsavel { get; set; }

        public long? SeqTipoProcesso { get; set; }

        public long? SeqProcesso { get; set; }

        public string DescricaoProcesso { get; set; }

        public int? SemestreReferencia { get; set; }

        public int? AnoReferencia { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }
    }
}
