using SMC.DadosMestres.Common.Constants;
using SMC.Framework.Mapper;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class DadosFormularioSeminarioVO : ISMCMappable
    {
        public string AreaConhecimento { get; set; }
        public string NomeOrientador { get; set; }
        public string EmailOrientador { get; set; }
        public List<string> Alunos { get; set; }
        public int CodigoPessoa { get; set; }
    }
}