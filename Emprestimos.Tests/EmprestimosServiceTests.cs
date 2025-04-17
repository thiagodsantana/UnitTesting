using Emprestimos.API.Models;
using Emprestimos.API.Services;
using FluentAssertions;
using NSubstitute;

namespace Emprestimos.Tests
{
    public class EmprestimosServiceTests : VerifyBase
    {
        public EmprestimosServiceTests() : base() { }

        [Fact]
        public void Deve_CalcularEmprestimo_Corretamente()
        {
            var servico = new EmprestimoService();

            var resultado = servico.CalcularEmprestimo(1000, 12);

            resultado.TotalPagar.Should().BeApproximately(1126.83m, 0.01m);            
        }

        [Fact]
        public void Deve_LancarExcecao_QuandoParametrosInvalidos()
        {
            var servico = new EmprestimoService();

            var acao1 = () => servico.CalcularEmprestimo(0, 12);
            var acao2 = () => servico.CalcularEmprestimo(1000, 0);

            acao1.Should().Throw<ArgumentException>()
                .WithMessage("*maior que 0*");

            acao2.Should().Throw<ArgumentException>()
                .WithMessage("*maior que 0*");
        }        

        [Fact]
        public void Deve_Ter_PropriedadesCorretas()
        {
            var servico = new EmprestimoService();
            var emprestimo = servico.CalcularEmprestimo(2000, 10);

            emprestimo.Should().BeOfType<Emprestimo>();
            emprestimo.ValorSolicitado.Should().Be(2000);
            emprestimo.Meses.Should().Be(10);
            emprestimo.TotalPagar.Should().BeGreaterThan(2000);
        }

        [Fact]
        public void Deve_SerEquivalenteAObjetoEsperado()
        {
            var servico = new EmprestimoService();

            var resultado = servico.CalcularEmprestimo(1000, 6);

            var esperado = new Emprestimo
            {
                ValorSolicitado = 1000,
                Meses = 6,
                TotalPagar = 1061.52m
            };

            resultado.Should().BeEquivalentTo(esperado, op =>
                op.Using<decimal>(ctx =>
                    ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01m)
                ).WhenTypeIs<decimal>());
        }

        [Fact]
        public void Deve_FinalizarRapidamente()
        {
            var servico = new EmprestimoService();

            Action acao = () => servico.CalcularEmprestimo(1500, 8);

            acao.ExecutionTime().Should().BeLessThan(TimeSpan.FromMilliseconds(100));
        }

        [Fact]
        public void Deve_RetornarEmprestimoValido()
        {
            var servico = new EmprestimoService();

            var resultado = servico.CalcularEmprestimo(800, 5);

            resultado.Should().Match<Emprestimo>(e =>
                e.ValorSolicitado == 800 &&
                e.Meses == 5 &&
                e.TotalPagar > 800);
        }
    }
}
