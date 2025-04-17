using Emprestimos.API.Models;

namespace Emprestimos.API.Services
{
    public class EmprestimoService : IEmprestimoService
    {
        private const decimal TaxaJurosMensal = 0.01m;

        public Emprestimo CalcularEmprestimo(decimal valor, int meses)
        {
            if (valor <= 0) throw new ArgumentException("Valor deve ser maior que 0.");
            if (meses <= 0) throw new ArgumentException("Meses deve ser maior que 0.");

            var total = valor * (decimal)Math.Pow((double)(1 + TaxaJurosMensal), meses);
            return new Emprestimo
            {
                ValorSolicitado = valor,
                Meses = meses,
                TaxaJuros = TaxaJurosMensal,
                TotalPagar = Math.Round(total, 2)
            };
        }
    }
}
