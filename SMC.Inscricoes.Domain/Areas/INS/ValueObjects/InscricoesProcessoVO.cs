using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Object usado para listagem de inscrições num determinado processo  na tela inicial do GPI
    /// </summary>
    public class InscricoesProcessoVO : ISMCMappable
    {

        public InscricoesProcessoVO()
        {
            Inscricoes = new List<InscricaoVO>();
        }

        public List<InscricaoVO> Inscricoes { get; set; }        

        public long SeqProcesso {get;set;}

        public string DescricaoProcesso { get; set; }

        public Guid UidProcesso { get; set; }

        public int QuantidadeOfertas { get; set; }

        public string TokenResource { get; set; }

        public string SituacaoProcesso { get; set; }
        
        public bool GestaoEventos { get; set; }
    }


    public class InscricoesProcessoVOComparer : IEqualityComparer<InscricoesProcessoVO>
    {
        public bool Equals(InscricoesProcessoVO x, InscricoesProcessoVO y)
        {
            return x.SeqProcesso == y.SeqProcesso;
        }

        public int GetHashCode(InscricoesProcessoVO obj)
        {
            return obj.SeqProcesso.GetHashCode();
        }
    }
}
