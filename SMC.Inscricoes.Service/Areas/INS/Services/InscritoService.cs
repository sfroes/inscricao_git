using SMC.DadosMestres.Common;
using SMC.DadosMestres.ServiceContract.Areas.PES.Data;
using SMC.DadosMestres.ServiceContract.Areas.PES.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Seguranca.ServiceContract.Areas.USU.Data;
using SMC.Seguranca.ServiceContract.Areas.USU.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    /// <summary>
    /// Serviço que chama o DomainService de Inscrito
    /// </summary>
    public class InscritoService : SMCServiceBase, IInscritoService
    {
        #region DomainService
        private ProcessoDomainService ProcessoDomainService => Create<ProcessoDomainService>();
        private InscritoDomainService InscritoDomainService
        {
            get { return this.Create<InscritoDomainService>(); }
        }

        private IUsuarioService UsuarioService
        {
            get { return this.Create<IUsuarioService>(); }
        }

        public IIntegracaoDadoMestreService IntegracaoDadoMestreService
        {
            get { return this.Create<IIntegracaoDadoMestreService>(); }
        }

        #endregion DomainService

        /// <summary>
        /// Busca o sequencial do inscrito do usuário logado
        /// </summary>
        /// <param name="seqUsuarioSas">Sequencial do usuário SAS</param>
        /// <returns>Sequencial do inscrito do usuário do SAS logado, ou NULL caso não encontre.</returns>
        public long? BuscarSeqInscrito(long seqUsuarioSas)
        {
            return InscritoDomainService.BuscarSeqInscrito(seqUsuarioSas);
        }

        /// <summary>
        /// Busca os dados de um inscrito
        /// </summary>
        /// <param name="seqInscrito">Sequencial do inscrito</param>
        /// <returns>Dados do inscrito</returns>
        public InscritoData BuscarInscrito(long seqInscrito)
        {
            //return this.InscritoDomainService.SearchByKey<Inscrito, InscritoData>(seqInscrito, IncludesInscrito.Enderecos |
            //                                                                                   IncludesInscrito.Telefones |
            //                                                                                   IncludesInscrito.EnderecosEletronicos);

            return InscritoDomainService.BuscarInscrito(seqInscrito).Transform<InscritoData>();
        }

        /// <summary>
        /// Salva os dados de um inscrito
        /// </summary>
        /// <param name="inscrito">Inscrito a ser salvo</param>
        /// <returns>Sequencial do inscrito salvo</returns>
        public long SalvarInscrito(InscritoData inscrito)
        {
            return this.InscritoDomainService.SalvarInscrito(SMCMapperHelper.Create<InscritoVO>(inscrito));
        }

        /// <summary>
        /// Valida o primeiro passo do cadastro.
        /// </summary>
        /// <param name="modelo">Inscrito a ser salvo</param>
        /// <returns>True se o modelo estiver válido. Caso contrário False.</returns>
        public bool ValidaInscritoPrimeiroPasso(InscritoData inscrito)
        {
            return this.InscritoDomainService.ValidaInscritoPrimeiroPasso(SMCMapperHelper.Create<Inscrito>(inscrito), inscrito.UidProcesso);
        }

        /// <summary>
        /// Altera o nome social de um inscrito.
        /// </summary>
        /// <param name="seqInscrito">Sequencial do inscrito.</param>
        /// <param name="nomeSocial">Nome social.</param>
        public void AlterarNomeSocial(long seqInscrito, string nomeSocial)
        {
            var spec = new SMCSeqSpecification<Inscrito>(seqInscrito);
            var inscrito = this.InscritoDomainService.SearchByKey(spec);

            //Formata o nome social conforme as regras:
            // - Retirar os espaços em branco antes e depois do nome.
            // - Colocar a primeira letra de cada palavra em maiúsculo e as restantes em minúsculo.
            // - Substituir acento agudo sozinho, por apóstrofo.
            // - Colocar a primeira letra após "-" ou apóstrofo em maiúsculo.
            // - Manter palavras como "I", "II" e "III" em maiúsculo.
            // - Manter palavras como "de", "da", "do", "das", "dos", "em" e "e" em minúsculo.
            nomeSocial = nomeSocial.SMCToPascalCaseName();

            inscrito.NomeSocial = nomeSocial;

            if (inscrito.SeqUsuarioSas.HasValue)
            {
                var usuSas = this.UsuarioService.BuscarUsuario(inscrito.SeqUsuarioSas.Value);
                if (usuSas == null)
                    throw new UsuarioSASNaoEncontradoException();
                usuSas.NomeSocial = nomeSocial;
                this.UsuarioService.SalvarUsuario(usuSas);
            }

            this.InscritoDomainService.SaveEntity(inscrito);
        }

        /// <summary>
        /// Altera dados de um inscrito.
        /// </summary>
        /// <param name="seqInscrito">Sequencial do inscrito.</param>
        /// <param name="nomeSocial">Nome social.</param>
        public void AlterarInscrito(InscritoData inscrito, bool sincronizarGDM = false)
        { 
            this.InscritoDomainService.AlterarInscrito(SMCMapperHelper.Create<Inscrito>(inscrito),sincronizarGDM, inscrito.UidProcesso);
        }

        public SMCPagerData<InscritoLookupListaData> BuscarInscritosLookup(InscritoLookupFiltroData filtro)
        {
            int total;
            var lista = InscritoDomainService.SearchProjectionBySpecification(filtro.Transform<InscritoFilterSpecification>(),
                            f => new InscritoLookupListaData
                            {
                                SeqInscrito = f.Seq,
                                Inscrito = string.IsNullOrEmpty(f.NomeSocial) ? f.Nome : f.NomeSocial + " (" + f.Nome + ")",
                                DataNascimento = f.DataNascimento,
                                Cpf = f.Cpf,
                                Passaporte = f.NumeroPassaporte
                            }, out total).ToList();
            return new SMCPagerData<InscritoLookupListaData>(lista, total);
        }

        public List<LookupInscritoData> BuscarInscritoLookup(long[] seqInscrito)
        {
            return InscritoDomainService.SearchProjectionBySpecification(new SMCContainsSpecification<Inscrito, long>(x => x.Seq, seqInscrito),
                                    f => new LookupInscritoData
                                    {
                                        SeqInscrito = f.Seq,
                                        Nome = string.IsNullOrEmpty(f.NomeSocial) ? f.Nome : f.NomeSocial + " (" + f.Nome + ")",
                                        CPF = f.Cpf,
                                        DataNascimento = f.DataNascimento,
                                        NumeroPassaporte = f.NumeroPassaporte
                                    }).ToList();
        }

        public InscritoLGPDData BuscarInscritoLGPD(long seqInscrito, long? seqProcesso)
        {
            bool? exibeTermoConsentimentoLGPD = null;
            var descTermoLGPD = "";
            if (seqProcesso.HasValue)
            {
                descTermoLGPD = ProcessoDomainService.SearchProjectionByKey(seqProcesso.Value, x => x.TipoProcesso.TermoConsentimentoLGPD);
                exibeTermoConsentimentoLGPD = !string.IsNullOrEmpty(descTermoLGPD);
            }
            if (seqInscrito > 0)
            {
                var inscrito = InscritoDomainService.SearchProjectionByKey(seqInscrito, x => new InscritoLGPDData
                {
                    SeqInscrito = seqInscrito,  
                    ConsentimentoLGPD = x.ConsentimentoLGPD,
                    DataConsentimentoLGPD = x.DataConsentimentoLGPD,
                    DataNascimento = x.DataNascimento,
                    ExibeTermoConsentimentoLGPD = exibeTermoConsentimentoLGPD,
                    TermoLGPD = descTermoLGPD
                });
                if (inscrito != null)
                {
                    return inscrito;
                }
            }
            DateTime dataNascimento = new DateTime();
            return new InscritoLGPDData()
            {
                SeqInscrito = seqInscrito,
                ConsentimentoLGPD = seqProcesso.HasValue?true:false,
                DataConsentimentoLGPD = null,
                DataNascimento = dataNascimento,
                ExibeTermoConsentimentoLGPD = exibeTermoConsentimentoLGPD,
                TermoLGPD = descTermoLGPD
            };
        }

        /// <summary>
        /// Valida se os dados necessários para o processo especificado estão preenchidos para o inscrito indicado.
        /// </summary>
        /// <param name="seqInscrito">O sequência do inscrito.</param>
        /// <param name="uidProcesso">O identificador exclusivo do processo.</param>
        /// <returns>Verdadeiro se os dados forem válidos</returns>
        public bool ValidarDadosInscritoPreenchidosParaProcesso(long seqInscrito, Guid uidProcesso)
        {
          return InscritoDomainService.ValidarDadosInscritoPreenchidosParaProcesso(seqInscrito, uidProcesso);
        }
    }
}