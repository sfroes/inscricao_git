using SMC.DadosMestres.Common.Areas.GED.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject
{
    public class DadosArquivoSharepointVO
    {
        public string GuidBiblioteca { get; set; }
        public string IdGEDPortfolio { get; set; }
        public string IdGEDProcesso { get; set; }
        public string SiglaOrigem { get; set; }
        public string EstruturaPasta { get; set; }
        public string NomePastaProcesso { get; set; }
        public string NomeArquivo { get; set; }
        public string SeqHierarquiaClassificacao { get; set; }
        public string SeqTipoDocumento { get; set; }
        public string IdentificadorArquivo { get; set; }
        public byte[] Conteudo { get; set; }
        public string NumeroDocumento { get; set; }
        public string NumeroProtocolo { get; set; }
        public string TipoMeio { get; set; }
        public string StatusDocumento { get; set; }
        public string Autor { get; set; }
        public string Destinatario { get; set; }
        public string Originador { get; set; }
        public string IdentificadorComponenteDigital { get; set; }
        public string IndicacaoAnexos { get; set; }
        public string FasePrazoGuarda { get; set; }
        public string DataInicioFase { get; set; }
        public string EventoValidacao { get; set; }
        public string NivelAcesso { get; set; }
        public string PrevisaoDesclassificacao { get; set; }
        public string EventoAutenticacao { get; set; }
        public string TipoOperacao { get; set; }
        public string IdGEDArquivo { get; set; }
        public string AtualizarEventoValidacao { get; set; }
    }
}
