using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class InscricaoDocumentoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        public long SeqDocumentoRequerido { get; set; }

        [DataMember]
        public long SeqTipoDocumento { get; set; }

        [DataMember]
        public string DescricaoTipoDocumento { get; set; }

        [DataMember]
        public SituacaoEntregaDocumento SituacaoEntregaDocumento { get; set; }

        [DataMember]
        public long? SeqArquivoAnexado { get; set; }

        [DataMember]
        public SMCUploadFile ArquivoAnexado { get; set; }

        [DataMember]
        public string DescricaoArquivoAnexado { get; set; }

        [DataMember]
        public DateTime? DataEntrega { get; set; }

        [DataMember]
        public FormaEntregaDocumento FormaEntregaDocumento { get; set; }

        [DataMember]
        public VersaoDocumento VersaoDocumento { get; set; }

        [DataMember]
        public VersaoDocumento VersaoDocumentoExigido { get; set; }

        [DataMember]
        public string Observacao { get; set; }

        [DataMember]
        public bool ExibirExibirObservacaoParaInscrito { get; set; }

        [DataMember]
        public bool ExibirInformacaoExibirObservacaoParaInscrito { get; set; }

        [DataMember]
        public bool ExibirObservacaoParaInscrito { get; set; }

        [DataMember]
        public bool TipoDocumentoPermiteVariosArquivos { get; set; }

        [DataMember]
        public string SituacaoInscricao { get; set; }

        [DataMember]
        public bool DocumentacaoEntregue { get; set; }

        [DataMember]
        public bool PermiteVarios { get; set; }

        public List<SMCDatasourceItem<string>> SolicitacoesEntregaDocumento { get; set; }

        public bool PermiteEntregaPosterior { get; set; }

        public bool ValidacaoOutroSetor { get; set; }

        public bool BloquearTodosOsCampos { get; set; }

        [DataMember]
        public DateTime? DataPrazoEntrega { get; set; }

        public long? SeqConfiguracaoEtapa { get; set; }

        [DataMember]
        public SituacaoEntregaDocumento SituacaoEntregaDocumentoInicial { get; set; }

        [DataMember]
        public bool Obrigatorio { get; set; }

        [DataMember]
        public bool ExibeTermoResponsabilidadeEntrega { get; set; }

        [DataMember]
        public DateTime? DataLimiteEntrega { get; set; }

        [DataMember]
        public bool EntregaPosterior { get; set; }

        [DataMember]
        public bool ExibirTermoEOrientacaoPDF { get; set; }
        
        [DataMember]
        public bool ConvertidoParaPDF { get; set; }

    }
}
