namespace SuperDigital.ContaCorrente.API.ViewModels
{
    public class MovimentacaoViewModel
    {
        public ContaCorrenteViewModel ContaOrigem { get; set; }
        public ContaCorrenteViewModel ContaDestino { get; set; }
        public decimal Valor { get; set; }

    }
}
