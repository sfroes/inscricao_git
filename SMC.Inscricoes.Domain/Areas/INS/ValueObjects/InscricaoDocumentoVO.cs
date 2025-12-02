using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Model;
using SMC.Inscricoes.Domain.Models;
using System;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    public class InscricaoDocumentoVO : DocumentoSimplificadoVO
    {

        public long SeqInscricao { get; set; }

        public SituacaoEntregaDocumento SituacaoEntregaDocumento { get; set; }

        public long? SeqArquivoAnexado { get; set; }

        public SMCUploadFile ArquivoAnexado { get; set; }

        public string DescricaoArquivoAnexado { get; set; }

        public DateTime? DataEntrega { get; set; }

        public FormaEntregaDocumento? FormaEntregaDocumento { get; set; }

        public VersaoDocumento? VersaoDocumento { get; set; }

        public VersaoDocumento VersaoDocumentoExigido { get; set; }

        public string Observacao { get; set; }

        public bool ExibirExibirObservacaoParaInscrito { get; set; }

        public bool ExibirInformacaoExibirObservacaoParaInscrito { get; set; }

        public bool ExibirObservacaoParaInscrito { get; set; }

        public bool TipoDocumentoPermiteVariosArquivos { get; set; }

        public bool UploadObrigatorio { get; set; }

        public bool Obrigatorio { get; set; }

        public string SituacaoInscricao { get; set; }

        public bool DocumentacaoEntregue { get; set; }

        public Sexo? Sexo { get; set; }

        public bool PermiteVarios { get; set; }

        public bool PermiteEntregaPosterior { get; set; }

        public bool ValidacaoOutroSetor { get; set; }

        public bool BloquearTodosOsCampos { get; set; }

        public DateTime? DataPrazoEntrega { get; set; }

        public long? SeqConfiguracaoEtapa { get; set; }

        public bool PermiteUploadArquivo { get; set; }

        public SituacaoEntregaDocumento SituacaoEntregaDocumentoInicial { get; set; }

        public bool ExibeTermoResponsabilidadeEntrega { get; set; }

        public DateTime? DataLimiteEntrega { get; set; }

        public bool EntregaPosterior { get; set; }

        public bool ConvertidoParaPDF { get; set; }
    }
}
