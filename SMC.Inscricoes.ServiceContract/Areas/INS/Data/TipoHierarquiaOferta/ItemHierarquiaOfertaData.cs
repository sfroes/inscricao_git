using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class ItemHierarquiaOfertaData : ISMCMappable
    {
        public long Seq { get; set; }

        public long? SeqPai { get; set; }

        [SMCMapProperty("ItemHierarquiaOfertaPai.TipoItemHierarquiaOferta.Descricao")]
        public string DescricaoPai { get; set; }

        public long SeqTipoHierarquiaOferta { get; set; }

        public long SeqTipoItemHierarquiaOferta { get; set; }

        public bool HabilitaCadastroOferta { get; set; }

        public string Descricao { get; set; }
    }
}
