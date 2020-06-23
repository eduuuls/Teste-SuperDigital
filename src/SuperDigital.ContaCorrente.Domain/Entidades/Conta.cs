namespace SuperDigital.ContaCorrente.Domain.Entidades
{
    public sealed class Conta: Base.Entidade<Conta>
    {
        public Conta()
        {
        }

        public int? CodigoAgencia { get; set; }
        public int NumeroConta { get; set; }
        public decimal Saldo { get; set; }

    }
}
