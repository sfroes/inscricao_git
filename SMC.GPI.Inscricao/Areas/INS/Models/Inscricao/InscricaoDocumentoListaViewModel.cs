using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common.Areas.RES;
using SMC.Framework.DataAnnotations;
using System;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class InscricaoDocumentoListaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long Seq { get; set; }
        
        [SMCHidden]
        public long SeqArquivoAnexado { get; set; }

        [SMCHidden]
        public SMCEncryptedLong Id { get { return new SMCEncryptedLong(SeqArquivoAnexado); } }

        [SMCSize(SMCSize.Grid8_24)]
        public string DescricaoTipoDocumento { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        public string DescricaoArquivoAnexado { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCOrder(3)]
        [SMCLink("DownloadDocumento", "Inscricao", SMCLinkTarget.NewWindow, "Id")]
        [SMCMapProperty("ArquivoAnexado.Name")]        
        [SMCHidden]
        public string NomeArquivoAnexado { get; set; }

        [SMCHidden]
        public bool ExibeTermoResponsabilidadeEntrega { get; set; }

        [SMCHidden]
        public DateTime? DataLimiteEntrega { get; set; }
                
        [SMCHidden]
        public bool EntregaPosterior { get; set; }

        [SMCHidden]
        public bool ConvertidoParaPDF { get; set; }
    }
}