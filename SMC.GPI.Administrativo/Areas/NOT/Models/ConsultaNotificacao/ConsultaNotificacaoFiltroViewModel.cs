using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.NOT.Models
{
    public class ConsultaNotificacaoFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        [LookupProcesso]
        [SMCSize(SMCSize.Grid10_24)]
        [SMCFilter]
        public GPILookupViewModel Processo { get; set; }

        [SMCFilter(false,false)]
        public long? SeqProcesso
        {
            get
            {
                if (Processo != null)
                    return Processo.Seq;
                return null;
            }
            set
            {
                Processo = new GPILookupViewModel() { Seq = value };
            }
        }

        [SMCSelect("GruposOferta")]
        [SMCDependency("Processo", "BuscarGrupoOferta", "ConsultaNotificacao", false)]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCFilter]
        public long? SeqGrupoOferta { get; set; }
        public List<SMCDatasourceItem> GruposOferta { get; set; }

        [SMCDependency("SeqGrupoOferta", true)]
        [SMCDependency("SeqProcesso")]
        [LookupSelecaoOferta]
        [SMCSize(SMCSize.Grid10_24)]
        [SMCFilter]
        public GPILookupViewModel Oferta { get; set; }

        [SMCFilter]
        public long? SeqOferta
        {
            get
            {
                if (Oferta != null)
                    return Oferta.Seq;
                return null;
            }
            set
            {
                Oferta = new GPILookupViewModel() { Seq = value };
            }
        }

        [SMCFilter]
        [SMCSelect("TiposNotificacao")]
        [SMCDependency("Processo", "BuscarTipoNotificacao", "ConsultaNotificacao", false)]
        [SMCSize(SMCSize.Grid5_24)]
        public long? SeqTipoNotificacao { get; set; }
        public List<SMCDatasourceItem> TiposNotificacao { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid5_24)]
        public string Inscrito { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid5_24)]
        public string Assunto { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCConditionalRequired("DataFim", SMCConditionalOperation.NotEqual, "")]
        public DateTime? DataInicio { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCMinDate("DataInicio")]
        [SMCConditionalRequired("DataInicio", SMCConditionalOperation.NotEqual, "")]
        public DateTime? DataFim { get; set; }

        public long? SeqInscrito { get; set; }

        public List<long> SeqUnidadeResponsavel { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid3_24)]
        public long? SeqInscricao { get; set; }

        [SMCHidden]
        public string BackURL { get; set; }

        [SMCHidden]
        public bool AutoPesquisa
        {
            get
            {
                var retorno = true;
                if (SeqInscrito != null ||
                    Inscrito != null ||
                    SeqGrupoOferta != null ||
                    SeqInscricao != null ||
                    SeqProcesso != null ||
                    SeqTipoNotificacao != null ||
                    SeqOferta != null ||
                    DataFim != null ||
                    DataInicio != null)
                {
                    retorno = false;
                }

                return retorno;
            }
        }
    }
}