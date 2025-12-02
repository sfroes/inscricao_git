using SMC.Framework.Mapper;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// VO contendo os dados de uma ofertaTaxa
    /// </summary>
    public class OfertaPeriodoTaxaVO : ISMCMappable
    {        
        public long Seq { get; set; }
        
        public long SeqTaxa { get; set; }
        
        public long SeqOferta { get; set; }
        
        public System.DateTime DataInicio { get; set; }
        
        public System.DateTime DataFim { get; set; }
        public Nullable<short> NumeroMinimo { get; set; }
        public Nullable<short> NumeroMaximo { get; set; }
        
        public int SeqEventoTaxa { get; set; }
        
        public int SeqParametroCrei { get; set; }
        
        public System.DateTime DataInclusao { get; set; }
        
        public string UsuarioInclusao { get; set; }
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        
        public string UsuarioAlteracao { get; set; }

        public Nullable<long> SeqPermissaoInscricaoForaPrazo { get; set; }

        public Oferta Oferta { get; set; }

        public Taxa Taxa { get; set; }


        



        

    }
}
