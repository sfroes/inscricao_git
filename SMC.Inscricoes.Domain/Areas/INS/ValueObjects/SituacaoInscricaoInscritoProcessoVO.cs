using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Domain.Models;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    public class SituacaoInscricaoInscritoProcessoVO : ISMCMappable
    {

        public SituacaoInscricaoInscritoProcessoVO()
        {
            OpcoesOferta = new List<OpcaoOfertaVO>();
        }

        public long Seq { get; set; }

        public long SeqProcesso { get; set; }

        public long SeqInscrito { get; set; }

        public long SeqSituacao { get; set; }

        public string NomeInscrito { get; set; }

        public string EmailInscrito { get; set; }

        public List<Telefone> Telefones { get; set; }

        public string DescricaoSituacaoAtual { get; set; }

        public long? SeqMotivo { get; set; }

        public string MotivoSituacaoAtual { get; set; }

        public string JustificativaSituacaoAtual { get; set; }

        public string DescricaoGrupoOferta { get; set; }

        public List<OpcaoOfertaVO> OpcoesOferta { get; set; }

        public DateTime DataInscricao { get; set; }

        public bool TaxaInscricaoPaga { get; set; }

        public bool DocumentacaoEntregue { get; set; }
        
        public DateTime DataNascimento { get; set; }
        
        public string Cpf { get; set; }

        public SituacaoDocumentacao SituacaoDocumentacao { get; set; }

        public string DocumentosPendentes { get; set; }
        public string DocumentosIndeferidos { get; set; }

        public decimal ValorTitulo { get; set; }
    }
}
