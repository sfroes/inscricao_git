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
    public class EtapaProcessoVO : ISMCMappable
    {
        public long SeqProcesso { get; set; }

        public string DescricaoProcesso { get; set; }

        public string DescricaoComplementarProcesso { get; set; }
        
        public List<SMCLanguage> IdiomasDisponiveis { get; set; }

        public string UrlInformacaoComplementar { get; set; }

        public DateTime DataInicioEtapa { get; set; }

        public DateTime DataFimEtapa { get; set; }

        public List<GrupoOfertaConfiguracaoEtapaVO> Grupos { get; set; }

        public int QuantidadeGrupos { get; set; }

        public SMCLanguage IdiomaAtual { get; set; }

        public Guid UidProcesso { get; set; }

        public string TokenResource { get; set; }
    }

    public class EtapaProcessoVOComparer : IEqualityComparer<EtapaProcessoVO>
    {
        public bool Equals(EtapaProcessoVO x, EtapaProcessoVO y)
        {
            return x.SeqProcesso == y.SeqProcesso;
        }

        public int GetHashCode(EtapaProcessoVO obj)
        {
            return obj.SeqProcesso.GetHashCode();
        }
    }
}
