using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Emprestimos.API.Models;

namespace Emprestimos.Tests
{
    public class EmprestimosApiIntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task Should_ReturnOk_WithLoan()
        {
            var response = await _client.GetAsync("/emprestimo?valor=1000&qtdMeses=6");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loan = await response.Content.ReadFromJsonAsync<Emprestimo>();
            loan.Should().NotBeNull();
            loan!.TotalPagar.Should().BeGreaterThan(1000);
        }

        [Fact]
        public async Task Should_ReturnBadRequest_ForInvalidParams()
        {
            var response = await _client.GetAsync("/emprestimo?valor=-100&qtdMeses=6");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}