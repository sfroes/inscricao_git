namespace SMC.Inscricoes.Domain.Models
{
    public partial class Telefone
    {
        public string TelefoneFormatado
        {
            get
            {
                return string.Format("+{0} ({1}) {2}", this.CodigoPais, this.CodigoArea, this.Numero.Trim());
            }
        }
    }
}