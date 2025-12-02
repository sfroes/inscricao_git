using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class ProcessoCampoInscritoDomainService : InscricaoContextDomain<ProcessoCampoInscrito>
    {
        /// <summary>
        /// Buscar campos inscritos por processo.
        /// </summary>
        /// <param name="seqProcesso">O sequencial do processo.</param>
        /// <returns>Uma lista de campos inscritos.</returns>
        public List<ProcessoCampoInscrito> BuscarCamposIncritosPorProcesso(long seqProcesso)
        {
            var spec = new ProcessoCampoInscritoFilterSpecification() { SeqProcesso = seqProcesso };

            List<ProcessoCampoInscrito>  retorno = SearchBySpecification(spec).ToList();

            return retorno;
        }

        /// <summary>
        /// Buscar campos inscritos por UIID do processo.
        /// </summary>
        /// <param name="guid">O UIID do processo.</param>
        /// <returns>Uma lista de campos inscritos.</returns>
        public List<ProcessoCampoInscrito> BuscarCamposInscritosPorUIIDProcesso(Guid guid)
        {
            var spec = new ProcessoCampoInscritoFilterSpecification() { GuidProcesso = guid };

            List<ProcessoCampoInscrito> retorno = SearchBySpecification(spec).ToList();

            return retorno;

        }
    }
}