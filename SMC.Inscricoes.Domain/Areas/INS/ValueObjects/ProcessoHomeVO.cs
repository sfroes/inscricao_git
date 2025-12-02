using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ProcessoHomeVO : ISMCMappable
    {
        public long SeqProcesso { get; set; }

        public string DescricaoProcesso { get; set; }

        public string DescricaoComplementarProcesso { get; set; }

        public List<SMCLanguage> IdiomasDisponiveis { get; set; }

        public string UrlInformacaoComplementar { get; set; }

        public SMCLanguage IdiomaAtual { get; set; }

        public bool ProcessoCancelado { get; set; }

        public SituacaoEtapa SituacaoEtapaInscricao { get; set; }

        public long SeqTipoProcesso { get; set; }

        public string TituloInscricoes { get; set; }

        public string TokenResource { get; set; }

        public string UrlCss { get; set; }

        public bool TodasOfertasProcessoInativas { get; set; }

        public bool ProcessoEncerrado { get; set; }

        public string TokenCssAlternativoSas { get; set; }

        public string OrientacaoCadastroInscrito { get; set; }

        public FormularioImpactoVO FormularioImpacto { get; set; }
    }
}
