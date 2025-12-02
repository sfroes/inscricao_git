using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Rest.Data
{
    public class DocumentFileData
    {
        public string Type { get; set; }

        public byte[] FileData { get; set; }

        public string Name { get; set; }
    }
}
