using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Rest.Models.File
{
    public class DocumentFile
    {
#pragma warning disable
        /// <summary>
        /// Tipo do documento
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// conteudo do documento
        /// </summary>
        public byte[] FileData { get; set; }

        /// <summary>
        /// Nome do documento
        /// </summary>
        public string Name { get; set; }
    }
}
