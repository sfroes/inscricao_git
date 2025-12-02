using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.SEL.Data
{
    public class CabecalhoSelecaoData : ISMCMappable
    {
        public long? SeqProcesso { get; set; }

        public string TipoProcesso { get; set; }

        public string Descricao { get; set; }

        public string GrupoOferta { get; set; }

        [SMCMapProperty("DescricaoOferta")]
        public string Oferta { get; set; }

        public long? SeqInscricao { get; set; }

        public string Candidato { get; set; }

        public string Opcao { get; set; }

        public long SeqOferta { get; set; }
    }
}
