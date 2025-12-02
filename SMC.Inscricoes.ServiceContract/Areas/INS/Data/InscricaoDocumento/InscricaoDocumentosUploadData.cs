using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System.Collections.Generic;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{

    /// <summary>
    /// VO usado para upload de documentos no processo de inscrição
    /// </summary>
    public class InscricaoDocumentosUploadData : ISMCMappable
    {
        /// <summary>
        /// Lista que pertencem a um grupo de documentos obrigatório
        /// </summary>
        public List<InscricaoDocumentoGrupoUploadData> DocumentosEmGruposObrigatorios { get; set; }

        /// <summary>
        /// Lista de documentos cujo o upload está configurado como obrigatório
        /// </summary>
        public List<InscricaoDocumentoData> DocumentosObrigatorios { get; set; }

        /// <summary>
        /// Lista de documentos opicionais cujo upload já foi realizado
        /// </summary>
        public List<InscricaoDocumentoData> DocumentosOpcionais { get; set; }

        /// <summary>
        /// Lista de tipos de documentos que podem ser adicionados
        /// </summary>
        public List<SMCDatasourceItem> TiposDocumentosOpcionais { get; set; }

        /// <summary>
        /// Lista de documentos adicionais
        /// </summary>
        public List<InscricaoDocumentoData> DocumentosAdicionais { get; set; }

        /// <summary>
        /// Lista de tipos de documentos Adicionais, que podem ser adicionados
        /// </summary>
        public List<SMCDatasourceItem> TiposDocumentosAdicionais { get; set; }

        ///<summary>
        ///Html com a mensagem do Termo de Entrega Posterior
        ///</summary>
        public string DescricaoTermoEntregaDocumentacao { get; set; }

        public bool ExibirMensagemInformativaConversaoPDF { get; set; }

    }
}
