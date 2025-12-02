using SMC.Framework.Mapper;
using SMC.Framework.Model;

namespace SMC.Inscricoes.ServiceContract.Areas.SEL.Data
{
    public class OfertaCabecalhoData : SMCPagerFilterData, ISMCMappable
    {
        /// <summary>
        /// Número de inscrição
        /// </summary>
        public string NumeroInscricao { get; set; }

        /// <summary>
        /// Candidato: nome do inscrito.
        /// </summary>
        public string Candidato { get; set; }

        /// <summary>
        /// Oferta original: caminho completo da oferta original do candidato.
        /// </summary>
        public string OfertaOriginal { get; set; }

        /// <summary>
        ///  Exibir o número da opção seguido do caracter "ª".
        /// </summary>
        public string Opcao { get; set; }

        /// <summary>
        /// Justificativa da alteração: justificativa da alteração da oferta.
        /// </summary>
        public string Situacao { get; set; }
    }
}