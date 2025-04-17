# API de Empréstimos - Exemplo em Português

API mínima desenvolvida em .NET 8 para simulação de empréstimos com juros compostos, escrita totalmente em português.

## Funcionalidades

- Simulação de empréstimos com taxa de juros mensal fixa (1%)
- Validação de parâmetros
- Testes com:
  - xUnit
  - FluentAssertions
  - NSubstitute
  - Verify
  - Testes de integração com WebApplicationFactory

## Como Executar

```bash
dotnet run --project ApiEmprestimos
```

Acesse via navegador: `http://localhost:5000/emprestimo?valor=1000&meses=12`

## Executar Testes

```bash
dotnet test
```

## Criação da Solução (instruções manuais)

```bash
dotnet new sln -n ApiEmprestimos
dotnet new web -n ApiEmprestimos
dotnet new xunit -n ApiEmprestimos.Tests

dotnet sln add ApiEmprestimos/ApiEmprestimos.csproj
dotnet sln add ApiEmprestimos.Tests/ApiEmprestimos.Tests.csproj

dotnet add ApiEmprestimos.Tests reference ApiEmprestimos

dotnet add ApiEmprestimos.Tests package FluentAssertions
dotnet add ApiEmprestimos.Tests package NSubstitute
dotnet add ApiEmprestimos.Tests package Verify.Xunit
dotnet add ApiEmprestimos.Tests package Microsoft.AspNetCore.Mvc.Testing
```