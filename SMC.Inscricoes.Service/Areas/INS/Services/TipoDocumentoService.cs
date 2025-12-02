using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using SMC.Framework.Specification;
using System;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    /// <summary>
    /// Inteface para o serviço que chama o DomainService de Inscrito
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public class TipoDocumentoService : SMCServiceBase, ITipoDocumentoService
    {

        #region DomainServices

        private TipoDocumentoDomainService TipoDocumentoDomainService
        {
            get
            {
                return this.Create<TipoDocumentoDomainService>();
            }
        }

        private ViewTipoDocumentoDomainService ViewTipoDocumentoDomainService
        {
            get
            {
                return this.Create<ViewTipoDocumentoDomainService>();
            }
        }

        #endregion

        #region Services

        private DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService TipoDocumentoDadosMestresService
        {
            get
            {
                return this.Create<DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService>();
            }
        }

        #endregion

        /// <summary>
        /// Buscar os tipos de documentos configurados
        /// </summary>        
        public SMCPagerData<TipoDocumentoData> BuscarTiposDocumento(TipoDocumentoFiltroData filtro)
        {
            var datas = this.ViewTipoDocumentoDomainService.SearchBySpecification<TipoDocumentoFilterSpecification, TipoDocumentoFiltroData,
                TipoDocumentoData, ViewTipoDocumento>(filtro);
            return datas;
        }

        public List<SMCDatasourceItem> BuscarTiposDocumentoKeyValue()
        {
            return this.ViewTipoDocumentoDomainService.SearchAll(x => x.Descricao).TransformList<SMCDatasourceItem>(); ;
        }

        /// <summary>
        /// Busca a configuração pra um tipo de documento pelo sequencial
        /// </summary>        
        public TipoDocumentoData BuscarTipoDocumento(long seqTipoDocumento)
        {
            return this.TipoDocumentoDomainService.SearchByKey<TipoDocumento, TipoDocumentoData>(seqTipoDocumento);
        }

        /// <summary>
        /// Salva a configuração de um tipo de documento
        /// </summary>        
        public long SalvarTipoDocumento(TipoDocumentoData tipoDocumento)
        {
            // Verifica se o tipo de documento já existe no banco para inserir ou atualizar. O framework não consegue resolver isso com SaveEntity pois
            // a chave da entitade não é auto-generated.
            if (TipoDocumentoDomainService.Count(new SMCSeqSpecification<TipoDocumento>(tipoDocumento.Seq)) > 0)
            {
                return TipoDocumentoDomainService.UpdateEntity(tipoDocumento.Transform<TipoDocumento>()).Seq;
            }
            else
            {
                return TipoDocumentoDomainService.InsertEntity(tipoDocumento.Transform<TipoDocumento>()).Seq;
            }
        }

        /// <summary>
        /// Exclui a configuração para um tipo de documento
        /// </summary>        
        public void ExcluirTipoDocumento(long seqTipoDocumento)
        {
            this.TipoDocumentoDomainService.DeleteEntity(seqTipoDocumento);
        }
    }
}
