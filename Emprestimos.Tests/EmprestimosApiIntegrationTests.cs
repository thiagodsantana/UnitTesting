using System.Net;
using System.Net.Http.Json;
using Emprestimos.API.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Emprestimos.Tests
{
    // Testes de integração da API de empréstimos
    public class EmprestimosApiIntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Theory]
        [InlineData(1000, 6, 1061.52)]
        [InlineData(500, 12, 563.41)]
        public async Task Deve_RetornarEmprestimo_ComSucesso(decimal valor, int meses, decimal esperado)
        {
            var response = await _client.GetAsync($"/emprestimo?valor={valor}&qtdMeses={meses}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var emprestimo = await response.Content.ReadFromJsonAsync<Emprestimo>();
            emprestimo.Should().NotBeNull();

            emprestimo!.ValorSolicitado.Should().Be(valor);
            emprestimo.Meses.Should().Be(meses);
            emprestimo.TotalPagar.Should().BeApproximately(esperado, 0.01m);
            emprestimo.TaxaJuros.Should().Be(0.01m);

            // Resolve conflito de prefixos nos testes parametrizados
            await Verify(emprestimo).UseParameters(valor, meses);
        }


        // Garante tratamento adequado de requisições inválidas
        [Theory]
        [InlineData(-100, 6)]
        [InlineData(1000, 0)]
        public async Task Deve_RetornarBadRequest_ParaParametrosInvalidos(decimal valor, int meses)
        {
            var response = await _client.GetAsync($"/emprestimo?valor={valor}&qtdMeses={meses}");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var msg = await response.Content.ReadAsStringAsync();
            msg.Should().Contain("maior que 0");
        }

        // Garante que rotas inválidas retornam 404
        [Fact]
        public async Task Deve_RetornarNotFound_ParaRotaInvalida()
        {
            var response = await _client.GetAsync("/rota-inexistente");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }        

        // Garante que o endpoint responde dentro do tempo esperado
        [Fact(Timeout = 1000)] // Tempo máximo de 1 segundo
        public async Task Endpoint_Deve_Responder_Rapidamente()
        {
            var response = await _client.GetAsync("/emprestimo?valor=1000&qtdMeses=6");
            response.EnsureSuccessStatusCode();
        }
    }
}
