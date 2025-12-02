using SMC.DadosMestres.Common;
using SMC.DadosMestres.ServiceContract.Areas.PES.Data;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Const;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Models;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject;
using System;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Portfolio
{
    public class BibliotecaApiDoaminService : InscricaoContextDomain<SharepointApi>
    {
        public RetornoBibliotecaSharepointVO BuscarGuidBiblioteca(long seqContextoBiblioteca)
        {

            var dados = new
            {
                SeqContextoBiblioteca = seqContextoBiblioteca
            };

            var retorno = RequisicoesSharePoint.EnviarPostApiGed<RetornoBibliotecaSharepointVO>(dados, ACOES_WEBAPI_SHAREPOINT.BUSCAR_GUID_BIBLIOTECA);

            return retorno;
        }
    }
}
