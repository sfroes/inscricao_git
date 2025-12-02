using SMC.Framework.UI.Mvc;
using SMC.Localidades.UI.Mvc.Models;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class ConsultaCandidatosProcessoListaExcelViewModel : SMCViewModelBase
    {
        public long SeqInscricao { get; set; }

        public string Candidato { get; set; }

        public string NumeroIdentidade { get; set; }

        public string Cpf { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Opcao { get; set; }

        public string Oferta { get; set; }

        public string Situacao { get; set; }

        public string Motivo { get; set; }

        public decimal? Nota { get; set; }

        public decimal? SegundaNota { get; set; }

        public int? Classificacao { get; set; }

        public string Email { get; set; }

        public List<TelefoneViewModel> Telefones { get; set; }

        public List<InformacoesEnderecoViewModel> Enderecos { get; set; }

        public DateTime DataInscricao { get; set; }
    }
}