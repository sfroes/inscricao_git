using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class ArquivoSecaoPaginaDomainService : InscricaoContextDomain<ArquivoSecaoPagina>
    {

        private ConfiguracaoEtapaPaginaIdiomaDomainService ConfiguracaoEtapaPaginaIdiomaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaPaginaIdiomaDomainService>(); }
        }

        private ArquivoAnexadoDomainService ArquivoAnexadoDomainService
        {
            get { return this.Create<ArquivoAnexadoDomainService>(); }
        }

        private IPaginaService PaginaService
        {
            get { return this.Create<IPaginaService>(); }
        }

        /// <summary>
        /// Salvar os arquivos de uma seção de uma página em um idioma
        /// </summary>        
        public void SalvarArquivosSecaoPagina(long seqConfiguracaoEtapaIdioma, long seqSecaoSGF, List<ArquivoSecaoPagina> secaoArquivos)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    if (secaoArquivos != null)
                    {
                        var situacao = this.ConfiguracaoEtapaPaginaIdiomaDomainService.
                            SearchProjectionByKey(
                            new SMCSeqSpecification<ConfiguracaoEtapaPaginaIdioma>(seqConfiguracaoEtapaIdioma),
                            x => x.ConfiguracaoEtapaPagina.ConfiguracaoEtapa.EtapaProcesso.SituacaoEtapa);

                        if (situacao == SituacaoEtapa.Liberada)
                        {
                            throw new AlteracaoPaginaEtapaLiberadaException();
                        }

                        // Verifica se todos possuiem uma ordem diferente
                        for (int i = 0; i < secaoArquivos.Count; i++)
                        {
                            for (int x = 0; x < secaoArquivos.Count; x++)
                            {
                                if (x != i)
                                {
                                    if (secaoArquivos[i].Ordem == secaoArquivos[x].Ordem)
                                        throw new ArquivosSecaoOrdemRepetidaException();
                                }
                            }
                        }

                        var token = PaginaService.BuscarSecaoPagina(seqSecaoSGF).Token;
                        foreach (var arq in secaoArquivos)
                        {
                            EnsureFileIntegrity<ArquivoSecaoPagina, ArquivoAnexado>(arq, x => x.SeqArquivo, x => x.Arquivo);
                            arq.Token = token;
                        }

                        var configuracaoEtapaPagIdioma = ConfiguracaoEtapaPaginaIdiomaDomainService
                                    .SearchByKey(new SMCSeqSpecification<ConfiguracaoEtapaPaginaIdioma>(seqConfiguracaoEtapaIdioma));
                        configuracaoEtapaPagIdioma.Arquivos = secaoArquivos;
                        ConfiguracaoEtapaPaginaIdiomaDomainService.SaveEntity(configuracaoEtapaPagIdioma);
                    }
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Retorna true se houver alteração nos arquivos
        /// </summary>
        /// <param name="arquivosNovos"></param>
        /// <param name="arquivosBanco"></param>
        /// <returns></returns>
        public bool VerificarMudancaArquivos(List<ArquivoSecaoPagina> arquivosNovos, ArquivoSecaoPagina[] arquivosBanco)
        {
            foreach (var arquivo in arquivosNovos)
            {
                var arquivoOld = arquivosBanco.FirstOrDefault(x => x.Seq == arquivo.Seq);
                if (arquivoOld == null
                    || arquivo.Descricao != arquivoOld.Descricao
                    || arquivo.NomeLink != arquivoOld.NomeLink
                    || arquivo.Ordem != arquivoOld.Ordem
                    || arquivo.Arquivo.State == SMCUploadFileState.Changed)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
