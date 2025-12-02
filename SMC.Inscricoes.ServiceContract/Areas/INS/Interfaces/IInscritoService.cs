using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    /// <summary>
    /// Inteface para o serviço que chama o DomainService de Inscrito
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IInscritoService : ISMCService
    {
        /// <summary>
        /// Busca o sequencial do inscrito do usuário logado
        /// </summary>
        /// <param name="seqUsuarioSas">Sequencial do usuário SAS</param>
        /// <returns>Sequencial do inscrito do usuário do SAS logado, ou NULL caso não encontre.</returns>
        long? BuscarSeqInscrito(long seqUsuarioSas);

        /// <summary>
        /// Busca os dados de um inscrito
        /// </summary>
        /// <param name="seqInscrito">Sequencial do inscrito</param>
        /// <returns>Dados do inscrito</returns>
        InscritoData BuscarInscrito(long seqInscrito);

        /// <summary>
        /// Salva os dados de um inscrito
        /// </summary>
        /// <param name="inscrito">Inscrito a ser salvo</param>
        /// <returns>Sequencial do inscrito salvo</returns>
        long SalvarInscrito(InscritoData inscrito);

        /// <summary>
        /// Valida o primeiro passo do cadastro.
        /// </summary>
        /// <param name="modelo">Inscrito a ser salvo</param>
        /// <returns>True se o modelo estiver válido. Caso contrário False.</returns>
        bool ValidaInscritoPrimeiroPasso(InscritoData inscrito);

        /// <summary>
        /// Altera o nome social de um inscrito.
        /// </summary>
        /// <param name="seqInscrito">Sequencial do inscrito.</param>
        /// <param name="nomeSocial">Nome social.</param>
        void AlterarNomeSocial(long seqInscrito, string nomeSocial);

        /// <summary>
        /// Altera os dados de um inscrito.
        /// </summary>
        void AlterarInscrito(InscritoData inscrito, bool sincronizarGDM = false);

        SMCPagerData<InscritoLookupListaData> BuscarInscritosLookup(InscritoLookupFiltroData filtro);

        List<LookupInscritoData> BuscarInscritoLookup(long[] seqInscrito);

        InscritoLGPDData BuscarInscritoLGPD(long seqInscrito, long? seqProcesso);

        /// <summary>
        /// Valida se os dados necessários para o processo especificado estão preenchidos para o inscrito indicado.
        /// </summary>
        /// <param name="seqInscrito">O sequência do inscrito.</param>
        /// <param name="uidProcesso">O identificador exclusivo do processo.</param>
        /// <returns>Verdadeiro se os dados forem válidos</returns>
        bool ValidarDadosInscritoPreenchidosParaProcesso(long seqInscrito, Guid uidProcesso);

    }
}
