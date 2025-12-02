using SMC.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Models
{
    public partial class ArquivoAnexado : ISMCFile
    {
        [NotMapped]
        public string Description
        {
            get
            {
                return string.IsNullOrEmpty(_descricaoArquivo)? this.Nome : _descricaoArquivo;
            }
            set
            {
                _descricaoArquivo = value;
            }
        }

        [NotMapped]
        public string Extension
        {
            get
            {
                return Path.GetExtension(this.Nome);
            }
            set {}
        }

        private string _descricaoArquivo;

        [NotMapped]
        public SMCUploadFileState State { get; set; }
    }
}
