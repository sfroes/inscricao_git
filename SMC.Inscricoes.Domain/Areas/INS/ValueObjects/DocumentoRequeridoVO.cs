using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{

    public class DocumentoRequeridoVO : ISMCMappable
    {
        public long Seq { get; set; }

        public long SeqConfiguracaoEtapa { get; set; }

        public long SeqTipoDocumento { get; set; }

        public bool PermiteUploadArquivo { get; set; }

        public VersaoDocumento VersaoDocumento { get; set; }

        public Sexo? Sexo { get; set; }

        public bool Obrigatorio { get; set; }

        public bool UploadObrigatorio { get; set; }

        public bool PermiteEntregaPosterior { get; set; }

        public bool ValidacaoOutroSetor { get; set; }

        public bool PermiteVarios { get; set; }

        public string DescricaoTipoDocumento { get; set; }

        public long SeqInscricao { get; set; }

        public VersaoDocumento VersaoDocumentoExigido { get; set; }

        public string Observacao { get; set; }

        

        public bool EntregaPosterior { get; set; }

        public List<SMCDatasourceItem<string>> SolicitacoesEntregaDocumento { get; set; }

        public List<InscricaoDocumentoVO> InscricaoDocumentos { get; set; }
    }
}
