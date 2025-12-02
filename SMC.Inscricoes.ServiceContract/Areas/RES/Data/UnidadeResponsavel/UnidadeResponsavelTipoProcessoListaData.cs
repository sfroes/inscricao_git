using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.RES.Data
{
    public class UnidadeResponsavelTipoProcessoListaData : SMCPagerFilterData, ISMCMappable
    {
        public long Seq { get; set; }

        public long SeqUnidadeResponsavel { get; set; }

        public string Descricao { get; set; }

        public bool Ativo { get; set; }
    }
}
