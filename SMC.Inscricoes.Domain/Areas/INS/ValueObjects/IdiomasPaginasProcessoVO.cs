using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class IdiomasPaginasProcessoVO : ISMCMappable
    {

        public long SeqConfiguracaoEtapa { get; set; }

        /// <summary>
        /// Idioma padrão do processo (não pode ser removido)
        /// </summary>
        public SMCLanguage IdiomaPadrao { get; set; }

        /// <summary>
        /// Lista de idiomas configurados para o processo (menos o idioma padrão)
        /// </summary>
        public List<SMCLanguage> IdiomasDisponiveis { get; set; }

        /// <summary>
        /// Idiomas que estão sendo usados para as páginas
        /// </summary>
        public List<SMCLanguage> IdiomasEmUso { get; set; }
    }
}
