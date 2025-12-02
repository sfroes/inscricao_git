using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class TelefonePessoaIntegracaoVO : ISMCMappable
    {
        public TipoTelefone TipoTelefone { get; set; }
        
        public int CodigoPais { get; set; }
        
        public int CodigoArea { get; set; }
        
        public string Numero { get; set; }
    }
}
