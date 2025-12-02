using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class DadosFormularioSeminarioData : ISMCMappable
    {
        public string AreaConhecimento { get; set; }
        public string NomeOrientador { get; set; }
        public string EmailOrientador { get; set; }
        public List<string> Alunos { get; set; }
    }
}
