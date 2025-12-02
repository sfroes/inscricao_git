using SMC.Financeiro.BLT.Common;
using SMC.Financeiro.Service.FIN;
using SMC.Financeiro.ServiceContract.BLT;
using SMC.Financeiro.ServiceContract.BLT.Data;
using SMC.Financeiro.ServiceContract.TXA.Data;
using SMC.Framework;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Exceptions.InscricaoBoletoTitulo;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscricaoBoletoTituloDomainService : InscricaoContextDomain<InscricaoBoletoTitulo>
    {
        #region DomainServices

        private InscricaoBoletoDomainService InscricaoBoletoDomainService
        {
            get { return this.Create<InscricaoBoletoDomainService>(); }
        }

        private OfertaPeriodoTaxaDomainService OfertaPeriodoTaxaDomainService
        {
            get { return this.Create<OfertaPeriodoTaxaDomainService>(); }
        }

        private InscricaoBoletoTaxaDomainService InscricaoBoletoTaxaDomainService
        {
            get { return this.Create<InscricaoBoletoTaxaDomainService>(); }
        }

        private InscritoDomainService InscritoDomainService
        {
            get { return this.Create<InscritoDomainService>(); }
        }

        private InscricaoDomainService InscricaoDomainService
        {
            get { return Create<InscricaoDomainService>(); }
        }

        #endregion DomainServices

        #region Services

        private IIntegracaoFinanceiroService IntegracaoFinanceiroService
        {
            get { return this.Create<IIntegracaoFinanceiroService>(); }
        }

        private IFinanceiroService FinanceiroService => Create<IFinanceiroService>();

        #endregion Services

        public void VerificaPermissaoGerarBoletoInscricao(long seqInscricao)
        {
            // Busca a inscrição para saber se está ou não cancelada
            var inscricaoDados = InscricaoDomainService.SearchProjectionByKey(
                                        new SMCSeqSpecification<Inscricao>(seqInscricao),
                                        x => new
                                        {
                                            ProcessoCancelado = (x.Processo.DataCancelamento.HasValue && x.Processo.DataCancelamento.Value <= DateTime.Now),
                                            InscricaoCancelada = x.HistoricosSituacao.Any(y => y.Atual && y.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA),
                                            SituacaoAtual = x.HistoricosSituacao.FirstOrDefault(y => y.Atual).TipoProcessoSituacao.Descricao,
                                            SeqOferta = x.Ofertas.FirstOrDefault(o => o.NumeroOpcao == 1).SeqOferta,
                                            Boletos = x.Boletos.Select(s => new
                                            {
                                                Titulos = s.Titulos,
                                                Taxas = s.Taxas
                                            })
                                        });

            if (inscricaoDados == null)
                return;

            // Não permite geração do boleto caso o processo de inscrição esteja cancelado
            if (inscricaoDados.ProcessoCancelado)
                throw new BoletoInscricaoCanceladaException("Processo cancelado.");
            if (inscricaoDados.InscricaoCancelada)
                throw new BoletoInscricaoCanceladaException(inscricaoDados.SituacaoAtual);

            foreach (var boleto in inscricaoDados.Boletos)
            {
                if (!VerificaExistenciaDeTaxasVigentes(inscricaoDados.SeqOferta, boleto.Taxas.Select(f => f.SeqTaxa)))
                {

                    var hoje = DateTime.Today;                    
                    // Dispara a exceção apenas se o boleto vencido não foi pago.
                    if (boleto.Titulos == null || boleto.Titulos.Where(f => f.DataCancelamento == null).Any(f => f.DataPagamento == null && f.DataVencimento < hoje ))
                    {
                        // Não existem taxas atuais vigentes
                        throw new BoletoInscricaoOfertaPeriodoTaxaFechadaException();
                    }
                }
            }
        }

        private bool VerificaExistenciaDeTaxasVigentes(long seqOferta, IEnumerable<long> seqTaxas)
        {
            foreach (var seqTaxa in seqTaxas)
            {
                var existeTaxaPeriodo = OfertaPeriodoTaxaDomainService.Count(
                   new OfertaPeriodoTaxaFilterSpecification
                   {
                       SeqOferta = seqOferta,
                       SeqTaxa = seqTaxa
                   } & new OfertaPeriodoTaxaVigenteSpecification());

                if (existeTaxaPeriodo == 0)
                {
                    // Não existem taxas atuais vigentes
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Implementa a RN_INS37 Montagem de boleto
        /// </summary>
        public BoletoData GerarOuRecuperarTitulo(long seqInscricaoBoleto)
        {
            int seqTituloRetorno;

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    // Busca a inscrição para saber se está ou não cancelada
                    var inscricaoDados = InscricaoBoletoDomainService.SearchProjectionByKey(
                                                new SMCSeqSpecification<InscricaoBoleto>(seqInscricaoBoleto),
                                                x => new
                                                {
                                                    ProcessoCancelado = (x.Inscricao.Processo.DataCancelamento.HasValue && x.Inscricao.Processo.DataCancelamento.Value <= DateTime.Now),
                                                    InscricaoCancelada = x.Inscricao.HistoricosSituacao.Any(y => y.Atual && y.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA),
                                                    SituacaoAtual = x.Inscricao.HistoricosSituacao.FirstOrDefault(y => y.Atual).TipoProcessoSituacao.Descricao,
                                                    SeqOferta = x.Inscricao.Ofertas.FirstOrDefault(o => o.NumeroOpcao == 1).SeqOferta,
                                                    SeqTaxasBoleto = x.Taxas.Select(t => t.SeqTaxa)
                                                });

                    // Não permite geração do boleto caso o processo de inscrição esteja cancelado
                    if (inscricaoDados.ProcessoCancelado)
                        throw new BoletoInscricaoCanceladaException("Processo cancelado.");
                    if (inscricaoDados.InscricaoCancelada)
                        throw new BoletoInscricaoCanceladaException(inscricaoDados.SituacaoAtual);

                    // Busca os títulos da inscrição
                    var spec = new InscricaoBoletoTituloFilterSpecification { SeqInscricaoBoleto = seqInscricaoBoleto };
                    var titulos = this.SearchBySpecification(spec).ToList();
                    if (titulos == null || titulos.Count == 0)
                    {
                        // Se não existe regristro de título emitir um novo
                        seqTituloRetorno = GerarNovoTitulo(seqInscricaoBoleto);
                    }
                    else
                    {
                        var titulo = titulos.FirstOrDefault(x => (!x.Cancelado && !x.Vencido) || (!x.Cancelado && x.Vencido && x.Pago));
                        if (titulo != null)
                        {
                            // Titulo já foi criado no financeiro e ainda é válido (pago e vencido, ou não vencido e não cancelado)
                            seqTituloRetorno = titulo.SeqTitulo;

                            // Se o titulo ainda estiver válido, verifica se o mesmo está registrado no banco ou registra novamente caso não esteja.
                            FinanceiroService.RegistrarBoletoOnline(titulo.Seq, "GRA");
                        }
                        else
                        {
                            //Verificar oferta período taxa
                            if (!VerificaExistenciaDeTaxasVigentes(inscricaoDados.SeqOferta, inscricaoDados.SeqTaxasBoleto))
                            {
                                // Não existem taxas atuais vigentes
                                throw new OfertaPeriodoTaxaFechadaException();
                            }

                            // Existe título não pago e vencido, ou cancelado
                            titulo = titulos.FirstOrDefault(x => x.Vencido && !x.Pago && !x.DataCancelamento.HasValue);
                            CancelarTitulo(titulo);
                            seqTituloRetorno = GerarNovoTitulo(seqInscricaoBoleto);
                        }
                    }
                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw e;
                }
            }

            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["ApiBoleto"]))
            {
                return IntegracaoFinanceiroService.BuscarBoletoCrei(new BoletoFiltroData()
                {
                    SeqTitulo = seqTituloRetorno,
                    Crei = true,
                    Sistema = SistemaBoleto.GPI
                });
            }

            return new BoletoData() { Titulo = new TituloBoletoData() { NumeroDocumento = seqTituloRetorno } };
        }

        /// <summary>
        /// Cancela um título existente
        /// </summary>
        /// <param name="titulo">Título a ser cancelado</param>
        private void CancelarTitulo(InscricaoBoletoTitulo titulo)
        {
            // Atualiza a data de cancelamento do título
            titulo.DataCancelamento = DateTime.Now;
            this.UpdateEntity(titulo);

            // Cancela o título no GRA
            this.IntegracaoFinanceiroService.CancelarTitulo(new CancelarTituloData
            {
                SeqTitulo = titulo.SeqTitulo,
                Descricao = "Cancelamento por data vencimento",
                UsuarioOperacao = "GPI"
            });
        }

        /// <summary>
        /// Realiza as buscas necessárias para gerar um novo boleto no Financeiro
        /// Gera o boleto e retorna o BoletoData para renderização
        /// </summary>
        /// <param name="seqInscricaoBoleto">Sequencial da Inscrição Boleto para geração do Titulo</param>
        /// <returns>Sequencial do Título Gerado</returns>
        public int GerarNovoTitulo(long seqInscricaoBoleto)
        {
            // Recupera os dados da Inscrição Boleto
            var infoInscricaoBoleto = this.InscricaoBoletoDomainService.
                SearchProjectionByKey(new SMCSeqSpecification<InscricaoBoleto>(seqInscricaoBoleto),
                    x => new
                    {
                        x.Inscricao.SeqInscrito,
                        x.Inscricao.Processo.SeqEvento,
                        x.SeqInscricao,
                        x.Inscricao.Ofertas
                    });

            // Recupera os dados do inscrito
            Inscrito inscrito = InscritoDomainService.BuscarInscrito(infoInscricaoBoleto.SeqInscrito);

            if (!inscrito.Enderecos.Any())
            {
                throw new EnderecoObrigatorioParaBoletoException();
            }

            // Recupera as taxas da Inscrição Boleto
            var taxasBoleto = InscricaoBoletoDomainService.
                SearchByKey(new SMCSeqSpecification<InscricaoBoleto>(seqInscricaoBoleto), IncludesInscricaoBoleto.Taxas | IncludesInscricaoBoleto.Taxas_Taxa);

            // Busca o parametro CREI e Seq-Evento-Taxas
            var taxasCrei = new List<Financeiro.ServiceContract.BLT.Data.TituloCreiTaxaData>();
            ParametroCREIData parametroCrei = null;


            foreach (var taxa in taxasBoleto.Taxas.Where(x => x.NumeroItens > 0))
            {
                long? seqOferta = infoInscricaoBoleto.Ofertas.FirstOrDefault(o => o.NumeroOpcao == 1).SeqOferta;

                if (taxa.Taxa.TipoCobranca == SMC.Inscricoes.Common.Areas.INS.Enums.TipoCobranca.PorOferta)
                {
                    seqOferta = infoInscricaoBoleto.Ofertas.FirstOrDefault(f => f.SeqOferta == taxa.SeqOferta).SeqOferta;

                }


                var ofertaPeriodoTaxa = OfertaPeriodoTaxaDomainService.SearchBySpecification(
                    new OfertaPeriodoTaxaFilterSpecification
                    {
                        SeqOferta = seqOferta,
                        SeqTaxa = taxa.SeqTaxa
                    } & new OfertaPeriodoTaxaVigenteSpecification()).First();
                var taxaData = new Financeiro.ServiceContract.BLT.Data.TituloCreiTaxaData
                {
                    Quantidade = taxa.NumeroItens,
                    SeqEventoTaxa = ofertaPeriodoTaxa.SeqEventoTaxa,
                };
                taxasCrei.Add(taxaData);
                var parametroTemp = IntegracaoFinanceiroService.BuscarParametrosCREI(new ParametroCREIFiltroData
                {
                    SeqParametroCREI = ofertaPeriodoTaxa.SeqParametroCrei
                }).First();
                if (parametroCrei == null || parametroCrei.DataVencimentoTitulo < parametroTemp.DataVencimentoTitulo)
                {
                    parametroCrei = parametroTemp;
                }
            }



            // Busca o endereço de correspondencia do inscrito
            Endereco inscritoEnd = inscrito.Enderecos.Where(e => e.Correspondencia.HasValue && e.Correspondencia.Value).FirstOrDefault();
            // Se não houver endereço marcado como correspondência o banco está inconsistente. Para evitar objeto nulo, busca o primeiro valor.
            if (inscritoEnd == null)
                inscritoEnd = inscrito.Enderecos.First();

            // Cria o titulo CREI
            var param = new Financeiro.ServiceContract.BLT.Data.TituloCreiParametroData
            {
                TipoGeracao = 'A',
                TipoCorrecaoTitulo = 3,
                NomeReceptor = inscrito.Nome,
                NomeUsuarioOperacao = SMCContext.User.Identity.Name,
                GeraRecibo = false,
                SeqEvento = infoInscricaoBoleto.SeqEvento,
                SeqParametroCrei = parametroCrei.SeqParametroCREI,
                Taxas = taxasCrei,
                TipoDocumentoIdentidade = 1, // CPF
                NumeroDocumento = inscrito.Cpf,
                LogradouroEndereco = inscritoEnd.Logradouro,
                NumeroEndereco = inscritoEnd.Numero,
                ComplementoEndereco = inscritoEnd.Complemento,
                BairroEndereco = inscritoEnd.Bairro,
                CidadeEndereco = inscritoEnd.NomeCidade,
                CepEndereco = inscritoEnd.Cep,
                UfEndereco = inscritoEnd.Uf,
                NomePagador = inscrito.Nome
            };

            // Quando um aluno é estrangeiro e não possui CPF e endereço aqui no Brasil,
            // ficou combinado com a equipe do GRA que o registro do boleto deverá ser feito com os dados da PUC Minas
            if (string.IsNullOrWhiteSpace(param.NumeroDocumento))
            {
                param.TipoDocumentoIdentidade = 2; // CNPJ
                param.NumeroDocumento = "17.178.195/0014-81";
            }
            if (inscritoEnd.CodigoPais != CONSTANTS.CODIGO_PAIS_BRASIL)
            {
                param.LogradouroEndereco = "Dom Jose Gaspar";
                param.NumeroEndereco = "500";
                param.ComplementoEndereco = "Predio 18";
                param.BairroEndereco = "Coração Eucarístico";
                param.CidadeEndereco = "Belo Horizonte";
                param.CepEndereco = "30.535-901";
                param.UfEndereco = "MG";
            }

            var retornoData = IntegracaoFinanceiroService.GerarTituloCrei(param);

            // Cria o Titulo para o boleto
            InscricaoBoletoTitulo novoTitulo = new InscricaoBoletoTitulo
            {
                SeqInscricaoBoleto = seqInscricaoBoleto,
                DataGeracao = DateTime.Now,
                SeqTitulo = retornoData.SeqTitulo,
                DataVencimento = retornoData.DataVencimento.Value,
                ValorTitulo = retornoData.ValorTitulo ?? 0
            };
            this.InsertEntity(novoTitulo);

            // Retorna o sequencial do título gerado
            return retornoData.SeqTitulo;
        }

        /// <summary>
        /// Busca os títulos existentes para uma inscrição
        /// </summary>
        public List<TituloInscricaoVO> BuscarTitulosInscricao(long seqInscricao)
        {
            var spec = new InscricaoBoletoFilterSpecification { SeqInscricao = seqInscricao };
            var seqsInscricaoBoleto = InscricaoBoletoDomainService.SearchProjectionBySpecification(spec, x => x.Seq);
            var containsSpec = new SMCContainsSpecification<InscricaoBoletoTitulo, long>(x => x.SeqInscricaoBoleto, seqsInscricaoBoleto.ToArray());
            containsSpec.SetOrderByDescending(x => x.DataGeracao);
            var titulos = this.SearchProjectionBySpecification(containsSpec, x =>
                new TituloInscricaoVO
                {
                    SeqInscricaoBoletoTitulo = x.Seq,
                    SeqInscricaoBoleto = x.SeqInscricaoBoleto,
                    DataVencimento = x.DataVencimento,
                    DataCancelamento = x.DataCancelamento,
                    DataPagamento = x.DataPagamento,
                    TipoBoleto = x.InscricaoBoleto.TipoBoleto,
                    SeqTitulo = x.SeqTitulo,
                    DataGeracao = x.DataGeracao,
                    Valor = x.ValorTitulo
                }).ToList();
            foreach (var titulo in titulos)
            {
                titulo.Taxas = new List<TaxaTituloInscricaoVO>();
                var taxaSpec = new InscricaoBoletoTaxaFilterSpecification { SeqInscricaoBoleto = titulo.SeqInscricaoBoleto };
                var taxas = InscricaoBoletoTaxaDomainService.SearchProjectionBySpecification(taxaSpec,
                    x => new TaxaTituloInscricaoVO
                    {
                        NumeroItens = x.NumeroItens,
                        Descricao = x.Taxa.TipoTaxa.Descricao
                    });
                titulo.Taxas.AddRange(taxas);
            }
            return titulos;
        }

        public InscricaoBoletoTitulo BuscarTitulo(long seqInscricaoBoleto, long seqTitulo)
        {
            var spec = new InscricaoBoletoTituloFilterSpecification { SeqTitulo = seqTitulo, SeqInscricaoBoleto = seqInscricaoBoleto };
            return this.SearchBySpecification(spec).FirstOrDefault();
        }
    }
}