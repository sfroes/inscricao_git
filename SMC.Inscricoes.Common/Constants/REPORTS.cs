using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Constants
{
    public class REPORTS
    {

        /// <summary>
        /// Path desse relatório no projeto SMC.Inscricoes.ReportHost
        /// </summary>        
        public const string RDLC_INSCRITO_ATIVIDADE_RELATORIO = @"Areas\INS\Reports\InscritoAtividadeRelatorio.rdlc";
        
        /// <summary>
        /// Nome do DataSet do Relatório de Listagem dos Inscritos na Atividade
        /// </summary>        
        public const string DS_INSCRITO_ATIVIDADE = "DSInscritoAtividadeRelatorio";
    }
}
