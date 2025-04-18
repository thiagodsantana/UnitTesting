using Emprestimos.API.Models;
using Emprestimos.API.Services;
using FluentAssertions;

namespace Emprestimos.Tests
{
    // Testes unitários da lógica de cálculo de empréstimos
    public class EmprestimosServiceTests : VerifyBase
    {
        public EmprestimosServiceTests() : base() { }

        // Valida o cálculo do total a pagar para diferentes valores e prazos
        [Theory]
        [InlineData(1000, 12, 1126.83)]
        [InlineData(2000, 6, 2123.04)]
        [InlineData(800, 5, 840.81)]
        public void Deve_CalcularEmprestimo_ComValorEsperado(decimal valor, int meses, decimal totalEsperado)
        {
            var emprestimoService = new EmprestimoService();
            var resultado = emprestimoService.CalcularEmprestimo(valor, meses);
            resultado.TotalPagar.Should().BeApproximately(totalEsperado, 0.01m);
        }

        // Garante que valores inválidos lançam exceções
        [Theory]
        [InlineData(0, 12)]
        [InlineData(1000, 0)]
        public void Deve_LancarExcecao_SeParametrosInvalidos(decimal valor, int meses)
        {
            var emprestimoService = new EmprestimoService();
            var acao = () => emprestimoService.CalcularEmprestimo(valor, meses);
            acao.Should().Throw<ArgumentException>().WithMessage("*maior que 0*");
        }

        // Verifica se todas as propriedades do retorno estão corretamente preenchidas
        [Fact]
        public void Deve_Ter_Propriedades_PreenchidasCorretamente()
        {
            var emprestimoService = new EmprestimoService();
            var emprestimo = emprestimoService.CalcularEmprestimo(1500, 10);

            emprestimo.ValorSolicitado.Should().Be(1500);
            emprestimo.Meses.Should().Be(10);
            emprestimo.TaxaJuros.Should().Be(0.01m);
            emprestimo.TotalPagar.Should().BeGreaterThan(1500);
        }

        // Compara o resultado com um objeto esperado, considerando tolerância decimal
        [Fact]
        public void Deve_Matchar_EmprestimoEsperado()
        {
            var emprestimoService = new EmprestimoService();
            var resultado = emprestimoService.CalcularEmprestimo(1000, 6);

            var esperado = new Emprestimo
            {
                ValorSolicitado = 1000,
                Meses = 6,
                TaxaJuros = 0.01m,
                TotalPagar = 1061.52m
            };

            resultado.Should().BeEquivalentTo(esperado, cfg =>
                cfg.Using<decimal>(ctx =>
                    ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01m)
                ).WhenTypeIs<decimal>());
        }

        // Garante performance do cálculo abaixo de 100ms
        [Fact]
        public void Deve_CalcularRapidamente()
        {
            var emprestimoService = new EmprestimoService();
            Action action = () => emprestimoService.CalcularEmprestimo(1500, 8);
            action.ExecutionTime().Should().BeLessThan(TimeSpan.FromMilliseconds(100));
        }

        // Gera snapshot do resultado com Verify
        [Fact]
        public Task Deve_GerarSnapshot_Verify()
        {
            var emprestimoService = new EmprestimoService();
            var resultado = emprestimoService.CalcularEmprestimo(1000, 6);
            return Verify(resultado);
        }

        [Fact]
        public void Metodo_Deve_Executar_AbaixoDe1Segundo()
        {
            var emprestimoService = new EmprestimoService();
            Action action = () => emprestimoService.CalcularEmprestimo(1500, 8);

            // Usa FluentAssertions para verificar tempo de execução
            action.ExecutionTime().Should().BeLessThan(TimeSpan.FromSeconds(1));
        }
    }
}
