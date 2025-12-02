using SMC.Framework.Mapper;
using System;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class InscritoLGPDData : ISMCMappable
    {
        public long? SeqInscrito { get; set; }

        public int Idade
        {
            get
            {
                int idade = 0;
                idade = DateTime.Now.Year - DataNascimento.Year;
                if (DataNascimento > DateTime.Now.AddYears(-idade))
                    idade--;

                return idade;
            }
        }

        public bool? ConsentimentoLGPD { get; set; }
        public DateTime? DataConsentimentoLGPD { get; set; }

        public DateTime DataNascimento { get; set; }

        public bool? ExibeTermoConsentimentoLGPD { get; set; }

        public string TermoLGPD { get; set; }
    }
}