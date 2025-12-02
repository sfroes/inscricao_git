using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class InscricaoDocumentoService : SMCServiceBase, IInscricaoDocumentoService
    {
        #region DomainServices

        private InscricaoDocumentoDomainService InscricaoDocumentoDomainService => Create<InscricaoDocumentoDomainService>();

        #endregion DomainServices

        public List<SMCDatasourceItem<string>> BuscarSituacoesEntregaDocumento(long seqInscricao, long seqDocumentoRequerido)
        {
            // recupera o documento
            var documento = InscricaoDocumentoDomainService.BuscarInscricaoDocumentos(new Domain.Areas.INS.Specifications.InscricaoDocumentoFilterSpecification
            {
                SeqInscricao = seqInscricao,
                SeqDocumentoRequerido = seqDocumentoRequerido
            }).FirstOrDefault();

            var documentoRequerido = InscricaoDocumentoDomainService.TransFormInscricaoDocumentoToDocumentoRequerido(documento);

            return InscricaoDocumentoDomainService.BuscarSituacoesEntregaDocumento(documentoRequerido, seqInscricao);
        }
    }
}