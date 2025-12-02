using SMC.Framework;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.RES.Data.UnidadeResponsavel
{
    public class UnidadeResponsavelTipoProcessoIdentidadeVisualData : ISMCMappable
    {
        public long Seq { get; set; }

        public long SeqUnidadeResponsavelTipoProcesso { get; set; }

        public string Descricao { get; set; }

        public string CssAplicacao { get; set; }

        public string TokenCssAlternativoSas { get; set; }

        public long SeqLayoutMensagemEmail { get; set; }

        public bool Ativo { get; set; }
    }
}
