namespace Emprestimos.API.Models
{
    public class Emprestimo
    {
        public decimal ValorSolicitado { get; set; }
        public int Meses { get; set; }
        public decimal TaxaJuros { get; set; }
        public decimal TotalPagar { get; set; }
    }
}
