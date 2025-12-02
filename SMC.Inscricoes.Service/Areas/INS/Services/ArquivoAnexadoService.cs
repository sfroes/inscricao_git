using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Models;
using SMC.DadosMestres.ServiceContract.Areas.SHA.Data;
using SMC.Framework.Domain;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class ArquivoAnexadoService : SMCServiceBase, IArquivoAnexadoService
    {
        private ArquivoAnexadoDomainService ArquivoAnexadoDomainService
        {
            get { return this.Create<ArquivoAnexadoDomainService>(); }
        }

        public SMCUploadFileGED BuscarArquivoAnexado(long seq)
        {
            var result = this.ArquivoAnexadoDomainService.SearchByKey(seq);
            var retorno = result.Transform<SMCUploadFileGED>();
            retorno.UidArquivoGed = result.UidArquivoGed;
            retorno.UrlDownloadGed = result.UrlDownloadGed;
            retorno.UrlPrivadaGed = result.UrlPrivadaGed;
            retorno.UrlPublicaGed = result.UrlPublicaGed;

            return retorno;
        }
    }
}
