using SMC.Framework.Mapper;
using SMC.Inscricoes.Service.Data;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.ServiceContract.Areas.SEL.Data
{
    public class ConsultaCandidatosProcessoListaData : ISMCMappable
    {
        public long SeqInscricaoOferta { get; set; }

        public long SeqInscricao { get; set; }

        public long SeqInscrito { get; set; }

        public long SeqProcesso { get; set; }

        public long SeqGrupoOferta { get; set; }

        public long SeqOferta { get; set; }

        public long? SeqOfertaOriginal { get; set; }

        public long? SeqMotivoSituacao { get; set; }

        public string Candidato { get; set; }

        public string NumeroIdentidade { get; set; }

        public string Cpf { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Opcao { get; set; }

        public string Oferta { get; set; }

        public bool PossuiJustificativa { get; set; }

        public long? SeqInscricaoHistoricoSituacao { get; set; }

        public string Situacao { get; set; }

        public string Motivo { get; set; }

        public decimal? Nota { get; set; }

        public decimal? SegundaNota { get; set; }

        public int? Classificacao { get; set; }

        public string HierarquiaCompleta { get; set; }

        public string Email { get; set; }

        public List<TelefoneData> Telefones { get; set; }

        public List<EnderecoData> Enderecos { get; set; }

        public DateTime DataInscricao { get; set; }
    }
}