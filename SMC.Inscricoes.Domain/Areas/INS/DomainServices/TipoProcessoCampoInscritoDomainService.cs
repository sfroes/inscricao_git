using SMC.DadosMestres.ServiceContract.Areas.SHA.Data;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Exceptions.TipoProcesso;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{

    public class TipoProcessoCampoInscritoDomainService : InscricaoContextDomain<TipoProcessoCampoInscrito>
    {

        /// <summary>
        /// Busca os tipos de processo campos inscrito por tipo de processo.
        /// </summary>
        /// <param name="seqTipoProcesso">O sequencial do tipo de processo.</param>
        /// <returns>Uma lista de tipos de processo campos inscrito.</returns>
        public List<TipoProcessoCampoInscrito> BuscarTiposProcessoCamposInscritoPorTipoProcesso(long seqTipoProcesso)
        {
            var spec = new TipoProcessoCampoInscritoFilterSpecification() { SeqTipoProcesso = seqTipoProcesso };
            var retorno = this.SearchBySpecification(spec).ToList();
            return retorno;
        }
    }
}