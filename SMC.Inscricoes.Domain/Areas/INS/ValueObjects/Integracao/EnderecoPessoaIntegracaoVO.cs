using SMC.Framework.Mapper;
using SMC.Localidades.Common.Areas.LOC.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class EnderecoPessoaIntegracaoVO : ISMCMappable
    {
        public TipoEndereco TipoEndereco { get; set; }
        
        public int CodigoPais { get; set; }
        
        public string Cep { get; set; }
        
        public string Logradouro { get; set; }
        
        public string Numero { get; set; }
        
        public string Complemento { get; set; }
        
        public string Bairro { get; set; }
        
        public int? CodigoCidade { get; set; }
        
        public string NomeCidade { get; set; }
        
        public string SiglaUf { get; set; }
        
        public bool? Correspondencia { get; set; }
    }
}
