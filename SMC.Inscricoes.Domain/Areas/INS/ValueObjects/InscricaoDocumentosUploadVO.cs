using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{

    /// <summary>
    /// VO usado para upload de documentos no processo de inscrição
    /// </summary>
    public class InscricaoDocumentosUploadVO : ISMCMappable
    {
        /// <summary>
        /// Lista que pertencem a um grupo de documentos obrigatório
        /// </summary>
        public List<InscricaoDocumentoGruposVO> DocumentosEmGruposObrigatorios { get; set; }

        /// <summary>
        /// Lista de documentos cujo o upload está configurado como obrigatório
        /// </summary>
        public List<InscricaoDocumentoVO> DocumentosObrigatorios { get; set; }

        /// <summary>
        /// Lista de documentos opicionais cujo upload já foi realizado
        /// </summary>
        public List<InscricaoDocumentoVO> DocumentosOpcionais { get; set; }

        /// <summary>
        /// Lista de tipos de documentos que podem ser adicionados
        /// </summary>
        public List<SMCDatasourceItem> TiposDocumentosOpcionais { get; set; }

        /// <summary>
        /// Lista de documentos adicionais
        /// </summary>
        public List<InscricaoDocumentoVO> DocumentosAdicionais { get; set; }

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
