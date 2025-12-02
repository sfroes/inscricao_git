using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ConfiguracaoPaginaVO : ISMCMappable
    {

        public long SeqConfiguracaoEtapaPagina { get; set; }

        public long SeqPaginaEtapaSGF { get; set; }

        public string DescricaoPagina { get; set; }

        public string PaginaToken { get; set; }

        public bool? ExibirConfirmacao { get; set; }        

        public bool? ExibirComprovante { get; set; }       

        public bool? ExibeDadosPessoais { get; set; }       
    }
}
