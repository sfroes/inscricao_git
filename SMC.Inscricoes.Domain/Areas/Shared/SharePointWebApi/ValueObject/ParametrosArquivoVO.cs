using SMC.Inscricoes.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject
{
    public class ParametrosArquivoVO
    {
        /// <summary>
        /// Obrigatorio para todos os metodos do arquivo
        /// </summary>
        public long SeqInscricao { get; set; }
        /// <summary>
        /// Obrigatorio para criação de arquivo
        /// </summary>
        public bool DocumentoDeferidoPreviamente { get; set; }
        /// <summary>
        /// Obrigatorio para criação de arquivo
        /// </summary>
        public bool DocumentoDeferido { get; set; }
        /// <summary>
        /// Obrigatorio para criação arquivo
        /// </summary>
        public bool GeradoPeloSistema { get; set; }
        /// <summary>
        /// Obrigatorio para criação arquivo
        /// </summary>
        public List<long> SeqsDocumentos { get; set; }
        /// <summary>
        /// Obrigatorio para criação arquivo
        /// </summary>
        public List<string> SeqsGuidArquivo {get; set; }
        /// <summary>
        /// Origem que indica a origem da chamda, para consulta dos dados
        /// </summary>
        public bool OrigemInscricaoDocumento { get; set; }
        /// <summary>
        /// Origem indica a origem da chamda, para consulta dos dados
        /// </summary>
        public bool OrigemNovaEntrega { get; set; }
        /// <summary>
        /// Origem que indica a origem da chamda, para consulta dos dados
        /// </summary>
        public bool OrigemComprovanteInscricao { get; set; }
        /// <summary>
        /// Sistema que esta chamando
        /// </summary>
        public TipoSistema TipoSistema { get; set; }
    }
}
