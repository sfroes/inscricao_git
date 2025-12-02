using SMC.Framework;
using SMC.Framework.Mapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ProcessoHierarquiaQRcodeVO : ISMCMappable, IEquatable<ProcessoHierarquiaQRcodeVO>
    {
        public long SeqProcesso { get; set; }
        public long SeqGrupoOferta { get; set; }
        public long SeqHierarquia { get; set; }
        public string DescricaoHierarquia { get; set; }

        public bool Equals(ProcessoHierarquiaQRcodeVO other)
        {
            return this.SeqProcesso == other.SeqProcesso &&
                   this.SeqGrupoOferta == other.SeqGrupoOferta &&
                   this.SeqHierarquia == other.SeqHierarquia &&
                   this.DescricaoHierarquia == other.DescricaoHierarquia;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
