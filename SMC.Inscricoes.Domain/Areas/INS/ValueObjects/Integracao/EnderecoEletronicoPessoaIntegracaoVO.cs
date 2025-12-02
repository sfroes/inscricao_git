using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class EnderecoEletronicoPessoaIntegracaoVO : ISMCMappable
    {
        public TipoEnderecoEletronico TipoEnderecoEletronico { get; set; }
        
        public string Descricao { get; set; }
    }
}
