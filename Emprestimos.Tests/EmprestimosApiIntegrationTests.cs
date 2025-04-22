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
        // Cria cliente HTTP a partir da aplicação real (em memória)
        private readonly HttpClient _client = factory.CreateClient();

        // Testa casos válidos de simulação de empréstimo, comparando com o valor total esperado
        [Theory]
        [InlineData(1000, 6, 1061.52)]
        [InlineData(500, 12, 563.41)]
        public async Task Deve_RetornarEmprestimo_ComSucesso(decimal valor, int meses, decimal esperado)
        {
            // Chama o endpoint GET com os parâmetros fornecidos
            var response = await _client.GetAsync($"/emprestimo?valor={valor}&qtdMeses={meses}");

            // Verifica se a resposta HTTP foi 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Desserializa o conteúdo da resposta em um objeto Emprestimo
            var emprestimo = await response.Content.ReadFromJsonAsync<Emprestimo>();
            emprestimo.Should().NotBeNull();

            // Verifica se os dados retornados batem com os parâmetros enviados
            emprestimo!.ValorSolicitado.Should().Be(valor);
            emprestimo.Meses.Should().Be(meses);

            // Verifica o total a pagar com margem de erro de 0.01
            emprestimo.TotalPagar.Should().BeApproximately(esperado, 0.01m);

            // Verifica se a taxa de juros padrão está correta (1%)
            emprestimo.TaxaJuros.Should().Be(0.01m);

            // Snapshot test com Verify para garantir consistência da resposta
            await Verify(emprestimo).UseParameters(valor, meses);
        }

        // Testa se a API retorna BadRequest quando parâmetros inválidos são passados
        [Theory]
        [InlineData(-100, 6)]  // valor negativo
        [InlineData(1000, 0)]  // meses igual a zero
        public async Task Deve_RetornarBadRequest_ParaParametrosInvalidos(decimal valor, int meses)
        {
            // Faz a requisição com parâmetros inválidos
            var response = await _client.GetAsync($"/emprestimo?valor={valor}&qtdMeses={meses}");

            // Deve retornar 400 Bad Request
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // A mensagem de erro deve conter uma dica do problema
            var msg = await response.Content.ReadAsStringAsync();
            msg.Should().Contain("maior que 0");
        }

        // Testa se a API retorna NotFound para uma rota inexistente
        [Fact]
        public async Task Deve_RetornarNotFound_ParaRotaInvalida()
        {
            var response = await _client.GetAsync("/rota-inexistente");

            // Verifica se retorna 404 Not Found
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        // Testa o tempo de resposta do endpoint, garantindo que seja rápido
        [Fact(Timeout = 1000)] // Tempo máximo permitido: 1 segundo
        public async Task Endpoint_Deve_Responder_Rapidamente()
        {
            var response = await _client.GetAsync("/emprestimo?valor=1000&qtdMeses=6");

            // Garante que a resposta foi bem-sucedida dentro do tempo
            response.EnsureSuccessStatusCode();
        }
    }
}
