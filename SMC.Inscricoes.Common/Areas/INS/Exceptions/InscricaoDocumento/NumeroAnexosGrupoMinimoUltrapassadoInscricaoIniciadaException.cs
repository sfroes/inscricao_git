using SMC.Framework.Exceptions;
using System;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.InscricaoDocumento
{
    public class NumeroAnexosGrupoMinimoUltrapassadoInscricaoIniciadaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoJaExistenteException
        /// </summary>
        public NumeroAnexosGrupoMinimoUltrapassadoInscricaoIniciadaException(long numeroMinimo, string descricaoGrupo)
            : base(String.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "NumeroAnexosGrupoMinimoUltrapassadoInscricaoIniciadaException", System.Threading.Thread.CurrentThread.CurrentCulture), numeroMinimo, descricaoGrupo))
        {
        }
    }
}