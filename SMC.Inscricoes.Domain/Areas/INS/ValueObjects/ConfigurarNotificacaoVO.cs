using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{    
    public class ConfigurarNotificacaoVO : ISMCMappable
    {        
        public long Seq { get; set; }
        
        public long SeqProcesso { get; set; }
        
        public long SeqTipoNotificacao { get; set; }

        public string DescricaoTipoNotificacao { get; set; }
        
        public bool EnvioAutomatico { get; set; }
       
        public List<ConfiguracaoNotificacaoIdiomaVO> ConfiguracoesEmail { get; set; }

        public bool ValidaTags { get; set; }
    }
}
