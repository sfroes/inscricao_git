using SMC.Framework;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.Framework.Security;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.GPI.Inscricao.Areas.INS.Models;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Framework.Mapper;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Localidades.ServiceContract.Areas.LOC.Interfaces;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Localidades.ServiceContract.Areas.LOC.Data;
using SMC.Localidades.UI.Mvc.Models;
using SMC.Localidades.Common.Areas.LOC.Enums;
using SMC.Framework.Extensions;

namespace SMC.GPI.Inscricao.Areas.INS.Services
{
    public class InscritoControllerService : SMCControllerServiceBase
    {
        #region Service

        private IInscritoService InscritoService
        {
            get { return this.Create<IInscritoService>(); }
        }

        private ILocalidadeService LocalidadeService
        {
            get { return this.Create<ILocalidadeService>(); }
        }

        #endregion

        /// <summary>
        /// Busca o sequencial de um inscrito do usuário logado
        /// </summary>
        /// <returns>Sequencial do inscrito do usuário do SAS logado, ou NULL caso não encontre.</returns>
        public long? BuscarSeqInscritoLogado()
        {
            // busca o sequencial o usuário logado
            long seqUsuario = BuscarSequencialUsuarioLogado();

            // Chama o serviço para encontrar o inscrito do usuário logado
            return InscritoService.BuscarSeqInscrito(seqUsuario);
        }

        /// <summary>
        /// Busca os dados do inscrito do usuário logado
        /// </summary>
        /// <returns>Dados do inscrito do usuário logado</returns>
        public InscritoViewModel BuscarInscrito()
        {
            // Busca o sequencial do inscrito do usuário logado
            long? seqInscrito = BuscarSeqInscritoLogado();

            InscritoViewModel model = new InscritoViewModel();

            if (seqInscrito.HasValue && seqInscrito > 0)
            {
                var dadosConsentimento = InscritoService.BuscarInscritoLGPD(seqInscrito.Value, null);
                model.ConsentimentoLGPD = dadosConsentimento.ConsentimentoLGPD.HasValue ? dadosConsentimento.ConsentimentoLGPD.Value : false;
            }

            // Se não encontrou o seq do inscrito
            if (!seqInscrito.HasValue)
            {
                // Monta o modelo com os dados do usuario logado
                short nacionalidade = 0;
                short.TryParse(SMCContext.User.SMCGetTipoNacionalidade(), out nacionalidade);
                model = new InscritoViewModel()
                {
                    SeqUsuarioSas = BuscarSequencialUsuarioLogado(),
                    Nome = SMCContext.User.SMCGetNome(),
                    Cpf = SMCContext.User.SMCGetCPF(),
                    NumeroPassaporte = SMCContext.User.SMCGetPassaporte(),
                    Email = SMCContext.User.SMCGetEmail(),
                    EmailConfirmacao = SMCContext.User.SMCGetEmail(),
                    NomeMae = SMCContext.User.SMCGetNomeMae(),
                    Nacionalidade = (TipoNacionalidade)nacionalidade,

                };
                if (SMCContext.User.SMCGetDataNascimento().HasValue)
                {
                    model.DataNascimento = SMCContext.User.SMCGetDataNascimento().Value;
                }
            }
            else // Se encontrou o sequencial do inscrito, busca seus dados
            {
                model = this.SearchBySeq<InscritoData, InscritoViewModel>(InscritoService.BuscarInscrito, seqInscrito.Value);
                model.EmailConfirmacao = model.Email;
                model.PossuiInscricao = true;
            }

            // Se não tem nenhum endereço inclui informando o tipo residencial e marcando para correspondencia
            if (model.Enderecos == null || (model.Enderecos != null && model.Enderecos.Count == 0))
            {
                var endereco = new InformacoesEnderecoViewModel()
                {
                    TipoEndereco = (short)TipoEndereco.Residencial,
                    Correspondencia = true
                };
                model.Enderecos = new AddressList();
                model.Enderecos.Add(endereco);
            }

            return model;
        }

        /// <summary>
        /// Busca os dados do inscrito do usuário logado para consulta
        /// </summary>
        /// <param name="seqInscrito">Sequencial do inscrito. Caso não seja informado, assume como o inscrito logado</param>
        /// <returns>Dados do inscrito do usuário logado</returns>
        public DadosInscritoViewModel BuscarDadosInscrito(long? seqInscrito = null)
        {
            // Busca o sequencial do inscrito do usuário logado
            seqInscrito = seqInscrito ?? BuscarSeqInscritoLogado();
            if (!seqInscrito.HasValue)
                throw new InscritoInvalidoException();

            // Busca os dados do inscrito
            InscritoData data = InscritoService.BuscarInscrito(seqInscrito.Value);

            // Cria o viewModel a partir do data
            DadosInscritoViewModel model = SMCMapperHelper.Create<DadosInscritoViewModel>(data);

            // Busca o pais da nacionalidade
            PaisData paisData = LocalidadeService.BuscarPais(data.CodigoPaisNacionalidade);
            model.PaisNacionalidade = paisData != null ? paisData.Nome : string.Empty;

            // Busca o nome da cidade da naturalidade
            if (data.CodigoCidadeNaturalidade.HasValue && !string.IsNullOrEmpty(data.UfNaturalidade))
            {
                CidadeData dataCidade = LocalidadeService.BuscarCidade(data.CodigoCidadeNaturalidade.Value, data.UfNaturalidade);
                model.CidadeNaturalidade = dataCidade != null ? dataCidade.Nome : string.Empty;
            }

            // Ajusta paises do endereço
            foreach (var endereco in model.Enderecos)
            {
                paisData = LocalidadeService.BuscarPais(endereco.SeqPais);
                endereco.DescPaisSelecionado = paisData.Nome;
            }

            return model;
        }


        /// <summary>
        /// Salvar Inscrito
        /// </summary>
        /// <param name="modelo">Modelo do inscrito a ser salvo</param>
        /// <returns>Sequencial do inscrito salvo</returns>
        public long SalvarInscrito(Models.InscritoViewModel modelo)
        {
            return InscritoService.SalvarInscrito( modelo.Transform<InscritoData>());
        }

        /// <summary>
        /// Valida o primeiro passo do cadastro.
        /// </summary>
        /// <param name="modelo">Modelo do inscrito a ser salvo</param>
        /// <returns>True se o modelo estiver válido. Caso contrário False.</returns>
        public bool ValidaInscritoPrimeiroPasso(InscritoViewModel modelo)
        {
            InscritoData data = SMCMapperHelper.Create<InscritoData>(modelo);
            return InscritoService.ValidaInscritoPrimeiroPasso(data);
        }

        /// <summary>
        /// Recupera o sequencial do usuário logado.
        /// </summary>
        /// <returns>Sequencial do usuário logado.</returns>
        private long BuscarSequencialUsuarioLogado()
        {
            // verifica o usuário autenticado
            var seq = SMCContext.User.SMCGetSequencialUsuario();
            if (!seq.HasValue)
                throw new UsuarioInvalidoException();
            return seq.Value;
        }
    }
}