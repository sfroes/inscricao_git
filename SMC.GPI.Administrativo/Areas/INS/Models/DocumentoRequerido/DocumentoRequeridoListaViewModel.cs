using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class DocumentoRequeridoListaViewModel : SMCViewModelBase, ISMCMappable
    {        
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqEtapaProcesso { get; set; }

        [SMCHidden]
        public long SeqConfiguracaoEtapa { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }

        public string DescricaoTipoDocumento { get; set; }

        public bool Obrigatorio { get; set; }

        public Sexo? Sexo { get; set; }

        public bool PermiteUploadArquivo { get; set; }

        public bool UploadObrigatorio { get; set; }

        public VersaoDocumento VersaoDocumento { get; set; }
            
    }
}