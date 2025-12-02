using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.RES;
using System;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
	public class DocumentoInscricaoViewModel : SMCViewModelBase, ISMCMappable
	{
		[SMCHidden]
		[SMCKey]
		public long Seq { get; set; }

		[SMCSize(SMCSize.Grid8_24)]
		[SMCSelect] 
		[SMCMaxLength(100)]        
        public string DescricaoTipoDocumento { get; set; }

		public DateTime? DataEntrega { get; set; }

        [SMCMapProperty("FormaEntregaDocumento")]
		[SMCMaxLength(100)]
		public FormaEntregaDocumento FormaEntrega { get; set; }

		[SMCMaxLength(100)]
		public VersaoDocumento VersaoDocumento { get; set; }

        [SMCHidden]
        public long? SeqArquivoAnexado { get; set; }

        [SMCHidden]
        public SMCEncryptedLong seqArquivo
        {
            get
            {
                return SeqArquivoAnexado.HasValue
            ? new SMCEncryptedLong(SeqArquivoAnexado.Value) : new SMCEncryptedLong(0) ; } }

        [SMCSize(SMCSize.Grid6_24)]
        public string DescricaoArquivoAnexado { get; set; }

        public SituacaoEntregaDocumento SituacaoEntregaDocumento { get; set; }

        [SMCHidden]
        public SMCUploadFile ArquivoAnexado { get; set; }
                
        [SMCHidden]
        public string Arquivo { get { return ArquivoAnexado != null ? ArquivoAnexado.Name : ""; } }

        [SMCHidden]
        public bool ExibeTermoResponsabilidadeEntrega { get; set; }

        [SMCHidden]
        public DateTime? DataLimiteEntrega { get; set; }
                
        [SMCHidden]
        public bool EntregaPosterior { get; set; }
    }
}
 