using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class IngressoOfertaVO : ISMCMappable
    {
        public long SeqInscricaoOferta { get; set; }
        public string DescricaoOferta { get; set; }
        public bool HabilitaCheckin { get; set; }
        public Guid? UidInscricaoOferta { get; set;}
        public string DescicaoParte1 { get; set; }
        public string DescicaoParte2 { get; set; }
        public string DescicaoParte3 { get; set; }
        public long SeqOferta { get; set; }
        public string QrCodeOferta { get; set; }
        public DateTime? DataInicioAtividade { get; set; }
        public DateTime? DataFimAtividade { get; set; }
        public string TagAtividade { get; set; }
        public string CSSTagAtividade { get; set; }

        public string Titulo
        {
            get
            {
                string retorno = string.Empty;

                if (DataFimAtividade.HasValue && DataInicioAtividade.HasValue)
                {
                    if(DataInicioAtividade.Value.Day == DataFimAtividade.Value.Day)
                    {                        
                        retorno = DataInicioAtividade.Value.ToString("dd/MM/yyyy - HH:mm") + " às " + DataFimAtividade.Value.ToString("HH:mm");
                    }
                    else
                    {                        
                        retorno = DataInicioAtividade.Value.ToString("dd/MM/yyyy - HH:mm") + " a " + DataFimAtividade.Value.ToString("dd/MM/yyyy - HH:mm");
                    }
                }
                return retorno;
            }
        }
    }
}
