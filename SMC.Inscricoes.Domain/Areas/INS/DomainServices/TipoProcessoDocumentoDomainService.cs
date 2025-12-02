using SMC.AssinaturaDigital.ServiceContract.Areas.CAD.Interfaces;
using SMC.DadosMestres.ServiceContract.Areas.GED.Interfaces;
using SMC.Framework.Model;
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
    public class TipoProcessoDocumentoDomainService : InscricaoContextDomain<TipoProcessoDocumento>
    {
        #region Services

        private SMC.DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService TipoDocumentoService => Create<SMC.DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService>();

        #endregion

        /// <summary>
        /// Busca a descrição dos tipos de documentos configurados no tipo de processo do processo
        /// </summary> 
        public List<SMCDatasourceItem> BuscarTiposDocumentoSelect(long seqTipoProcesso)
        {
            var spec = new TipoProcessoDocumentoFilterSpecification() { SeqTipoProcesso = seqTipoProcesso };
            var seqsTipoDocumento = this.SearchProjectionBySpecification(spec, x => x.SeqTipoDocumento).ToList();
            if (seqsTipoDocumento.Count == 0 )
            {
                List<SMCDatasourceItem> nulo = null;
                return nulo;
            }
            var retorno = TipoDocumentoService.BuscarTiposDocumentosSelect(seqsTipoDocumento);
            return retorno;
        }
    }
}
