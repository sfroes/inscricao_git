using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class ProcessoIdiomaDomainService : InscricaoContextDomain<ProcessoIdioma>
    {
        #region Domain Services
        private ConfiguracaoEtapaDomainService ConfiguracaoEtapaDomainService => Create<ConfiguracaoEtapaDomainService>();
        #endregion

        /// <summary>
        /// Busca a descrição complementar do processo no idioma informado na specification
        /// Caso o idioma não seja informado retorna a descrição complementar no idioma padrão do processo
        /// </summary>
        public string BuscarDescricaoComplementarProcesso(ProcessoIdiomaFilterSpecification filtro, out SMCLanguage idioma)
        {
            var descricoes = this.SearchProjectionBySpecification(filtro,
                x => new { Descricao = x.DescricaoComplementar, Padrao = x.Padrao, Idioma = x.Idioma });
            if (descricoes.Count() == 1)
            {
                idioma = descricoes.First().Idioma;
                return descricoes.First().Descricao;
            }
            else
            {
                var padrao = descricoes.Where(x => !x.Padrao).FirstOrDefault();
                idioma = padrao.Idioma;
                return padrao.Descricao;
            }
        }


        /// <summary>
        /// Buscar a lista de idiomas disponíveis para um processo
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <returns>Lista de idiomas disponíveis</returns>
        public List<SMCLanguage> BuscarIdiomasDisponiveis(long seqProcesso)
        {
            ProcessoIdiomaFilterSpecification spec = new ProcessoIdiomaFilterSpecification(seqProcesso);
            return this.SearchProjectionBySpecification(spec, p => p.Idioma).ToList();
        }

        public string BuscarDescricaoOfertaProcesso(long seqConfiguracaoEtapa, SMCLanguage idioma)
        {
            var specConfig = new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa);
            long seqProcesso = ConfiguracaoEtapaDomainService.SearchProjectionByKey(specConfig, x => x.EtapaProcesso.SeqProcesso);
            ProcessoIdiomaFilterSpecification spec = new ProcessoIdiomaFilterSpecification(seqProcesso, idioma);
            return SearchProjectionByKey(spec, x => x.LabelOferta);
        }
    }

}
