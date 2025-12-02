using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject
{
    public class CriarProcessoVO
    {
        [Required]
        public long SeqInscricao { get; set; }
        [Required]
        public string IdGedPortfolio { get; set; }
        [Required]
        public string GuidBiblioteca { get; set; }
    }
}
