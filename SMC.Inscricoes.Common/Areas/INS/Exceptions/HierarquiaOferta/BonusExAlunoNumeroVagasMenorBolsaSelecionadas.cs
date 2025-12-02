using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class BonusExAlunoNumeroVagasMenorBolsaSelecionadas : SMCApplicationException
    {
        /// <summary>
        /// Construtor de BonusExAlunoNumeroVagasMenorBolsaSelecionadas
        /// </summary>
        public BonusExAlunoNumeroVagasMenorBolsaSelecionadas()
            : base(ExceptionsResource.BonusExAlunoNumeroVagasMenorBolsaSelecionadas)
        {
        }
    }
}
