using SMC.DadosMestres.Common.Areas.GED.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject
{
    public class DadosProcessoSharepointVO
    {
        public Guid GuidBiblioteca { get; set; }
        public string IdGEDPortfolio { get; set; }
        public long SeqHierarquiaClassificacao { get; set; }
        public string EstruturaPasta { get; set; }
        public string NomePastaProcesso { get; set; }
        public string NumeroProtocoloProcesso { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Interessado { get; set; }
        public NivelAcesso NivelAcesso { get; set; }
        public PrevisaoDesclassificacao PrevisaoDesclassificacao { get; set; }
        public string IdGEDProcesso { get; set; }
    }
}
