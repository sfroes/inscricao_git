using SMC.Framework.Domain;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscricaoHistoricoPaginaDomainService : InscricaoContextDomain<InscricaoHistoricoPagina>
    {
        /// <summary>
        /// Busca as informações da ultima página acessada por uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Informações da ultima página acessada por uma inscrição</returns>
        public InscricaoHistoricoPagina BuscarUltimaPaginaInscricao(long seqInscricao)
        {
            // Busca as informações da ultima página acessada na inscrição
            IncludesInscricaoHistoricoPagina includes = IncludesInscricaoHistoricoPagina.Inscricao |
                                                        IncludesInscricaoHistoricoPagina.ConfiguracaoEtapaPagina;
            InscricaoHistoricoPaginaFilterSpecification spec = new InscricaoHistoricoPaginaFilterSpecification()
            {
                SeqInscricao = seqInscricao
            };
            spec.SetOrderByDescending(h => h.DataAcesso);
            InscricaoHistoricoPagina historico = SearchBySpecification(spec, includes).FirstOrDefault();

            return historico;
        }
    }
}
