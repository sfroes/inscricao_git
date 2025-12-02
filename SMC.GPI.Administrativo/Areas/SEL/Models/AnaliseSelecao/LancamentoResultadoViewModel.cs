using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class LancamentoResultadoViewModel : SMCViewModelBase
    {
        #region Cabeçalho

        public string TipoProcesso { get; set; }

        public string Descricao { get; set; }

        public string GrupoOferta { get; set; }

        public string DescricaoOferta { get; set; }

        #endregion Cabeçalho

        #region Filtro

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long? SeqOferta { get; set; }

        // Se a tela vier da analise em lote, ajusta o post do lookup de oferta.
        public SMCDatasourceItem Oferta
        {
            get
            {
                if (SeqOferta.HasValue)
                    return new SMCDatasourceItem() { Seq = SeqOferta.Value };
                return null;
            }
            set
            {
                SeqOferta = value.Seq;
            }
        }

        [SMCHidden]
        public List<long> InscricoesOfertas { get; set; }

        // Se a tela enviar apenas uma inscrição, gera uma lista para ficar padronizado a seleção
        public SMCEncryptedLong SeqInscricaoOferta
        {
            get
            {
                if (InscricoesOfertas == null || InscricoesOfertas.Count > 1)
                    return null;
                return new SMCEncryptedLong(InscricoesOfertas.FirstOrDefault());
            }
            set
            {
                if (value != 0)
                    InscricoesOfertas = new List<long>() { value };
            }
        }

        #endregion Filtro

        #region Edição

        public List<LancamentoResultadoItemViewModel> Lancamentos { get; set; }

        [SMCHidden]
        public bool EdicaoLote { get; set; }

        #endregion Edição

        #region Datasources

        [SMCDataSource(SMCStorageType.TempData)]
        public List<SMCDatasourceItem> Situacoes { get; set; }

        #endregion Datasources
        public string BackUrl { get; set; }
    }
}