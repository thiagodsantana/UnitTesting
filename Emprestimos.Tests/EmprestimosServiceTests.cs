using Emprestimos.API.Models;
using Emprestimos.API.Services;
using FluentAssertions;

namespace Emprestimos.Tests
{
    // Testes unitários da lógica de cálculo de empréstimos
    public class EmprestimosServiceTests : VerifyBase
    {
        public EmprestimosServiceTests() : base() { }

        // Verifica se o valor total a pagar está correto com base no valor e prazo informados
        [Theory]
        [InlineData(1000, 12, 1126.83)]
        [InlineData(2000, 6, 2123.04)]
        [InlineData(800, 5, 840.81)]
        public void Deve_CalcularEmprestimo_ComValorEsperado(decimal valor, int meses, decimal totalEsperado)
        {
            var emprestimoService = new EmprestimoService();

            // Realiza o cálculo do empréstimo
            var resultado = emprestimoService.CalcularEmprestimo(valor, meses);

            // Verifica se o valor total a pagar está dentro da margem de erro aceitável
            resultado.TotalPagar.Should().BeApproximately(totalEsperado, 0.01m);
        }

        // Garante que parâmetros inválidos (zero ou negativos) disparam exceção
        [Theory]
        [InlineData(0, 12)]     // valor zero
        [InlineData(1000, 0)]   // meses zero
        public void Deve_LancarExcecao_SeParametrosInvalidos(decimal valor, int meses)
        {
            var emprestimoService = new EmprestimoService();

            // Ação que deve gerar uma exceção
            var acao = () => emprestimoService.CalcularEmprestimo(valor, meses);

            // Verifica se a exceção é lançada com a mensagem esperada
            acao.Should().Throw<ArgumentException>()
                .WithMessage("*maior que 0*");
        }

        // Garante que todas as propriedades do retorno foram corretamente preenchidas
        [Fact]
        public void Deve_Ter_Propriedades_PreenchidasCorretamente()
        {
            var emprestimoService = new EmprestimoService();

            // Realiza o cálculo do empréstimo
            var emprestimo = emprestimoService.CalcularEmprestimo(1500, 10);

            // Verifica se os campos estão consistentes
            emprestimo.ValorSolicitado.Should().Be(1500);
            emprestimo.Meses.Should().Be(10);
            emprestimo.TaxaJuros.Should().Be(0.01m);
            emprestimo.TotalPagar.Should().BeGreaterThan(1500);
        }

        // Compara um objeto de empréstimo gerado com um objeto esperado (considerando tolerância nos valores decimais)
        [Fact]
        public void Deve_Matchar_EmprestimoEsperado()
        {
            var emprestimoService = new EmprestimoService();
            var resultado = emprestimoService.CalcularEmprestimo(1000, 6);

            // Objeto esperado
            var esperado = new Emprestimo
            {
                ValorSolicitado = 1000,
                Meses = 6,
                TaxaJuros = 0.01m,
                TotalPagar = 1061.52m
            };

            // Verifica se todos os campos são equivalentes, com tolerância em campos decimais
            resultado.Should().BeEquivalentTo(esperado, cfg =>
                cfg.Using<decimal>(ctx =>
                    ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01m)
                ).WhenTypeIs<decimal>());
        }

        // Verifica se o tempo de execução do método é inferior a 100 milissegundos
        [Fact]
        public void Deve_CalcularRapidamente()
        {
            var emprestimoService = new EmprestimoService();

            // Mede tempo de execução da ação
            Action action = () => emprestimoService.CalcularEmprestimo(1500, 8);
            action.ExecutionTime().Should().BeLessThan(TimeSpan.FromMilliseconds(100));
        }

        // Gera snapshot da resposta para inspeção e validação com Verify
        [Fact]
        public Task Deve_GerarSnapshot_Verify()
        {
            var emprestimoService = new EmprestimoService();
            var resultado = emprestimoService.CalcularEmprestimo(1000, 6);

            // Gera snapshot com Verify (comparação de estrutura e conteúdo do retorno)
            return Verify(resultado);
        }

        // Valida que o método não excede 1 segundo de execução, garantindo performance mínima
        [Fact]
        public void Metodo_Deve_Executar_AbaixoDe1Segundo()
        {
            var emprestimoService = new EmprestimoService();

            Action action = () => emprestimoService.CalcularEmprestimo(1500, 8);

            // Garante que o método não ultrapassa 1 segundo
            action.ExecutionTime().Should().BeLessThan(TimeSpan.FromSeconds(1));
        }
    }
}
