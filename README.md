# 📘 API de Empréstimos - Testes Automatizados

Este repositório contém testes automatizados para a **API de Empréstimos**, que realiza o cálculo de juros compostos sobre um valor solicitado por determinado número de meses.

## 📌 Visão Geral

A API calcula o valor total a ser pago com base em:
- **Valor solicitado**
- **Quantidade de meses**
- **Taxa de juros mensal fixa de 1%**

Exemplo de chamada:
```
GET /emprestimo?valor=1000&qtdMeses=6
```

Retorno:
```json
{
  "valorSolicitado": 1000.0,
  "meses": 6,
  "taxaJuros": 0.01,
  "totalPagar": 1061.52
}
```

---

## 🧪 Testes Automatizados

A solução contém duas classes de testes:

### ✅ `EmprestimosServiceTests`
Testes **unitários** da lógica interna de cálculo no serviço `EmprestimoService`.

#### 📋 Coberturas:
- Cálculo correto do valor final (`TotalPagar`)
- Verificações de arredondamento com margem de erro (`BeApproximately`)
- Validação de parâmetros inválidos (ex: zero ou negativos)
- Comparação completa com objeto esperado usando `BeEquivalentTo`
- Performance do cálculo (deve executar em menos de 100ms)
- Snapshot do resultado com o [Verify](https://github.com/VerifyTests/Verify)

#### 🛠 Tecnologias:
- xUnit
- FluentAssertions
- Verify (Snapshot testing)

---

### ✅ `EmprestimosApiIntegrationTests`
Testes **de integração**, garantindo o comportamento da API exposta via HTTP.

#### 📋 Coberturas:
- Resposta correta para chamadas válidas (200 OK com conteúdo JSON esperado)
- Validação de parâmetros inválidos com retorno `400 Bad Request`
- Verificação do conteúdo da resposta com `BeApproximately` e `Verify`

#### 🛠 Tecnologias:
- `WebApplicationFactory<T>` do ASP.NET Core para hospedagem em memória
- xUnit
- FluentAssertions
- Verify

---

## 🚀 Como Executar os Testes

### Pré-requisitos:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- IDE como Visual Studio, Rider ou VS Code
- Snapshots aprovados se for rodar com Verify (ou aceite manualmente após primeiro teste)

### Comando:
```bash
dotnet test
```

> Após rodar os testes com Verify pela primeira vez, é necessário aprovar os arquivos `.received.txt` gerados como referência (`.verified.txt`).

---

## 📂 Organização dos Testes

```
Emprestimos.Tests/
│
├── EmprestimosServiceTests.cs        # Testes unitários da lógica de negócio
├── EmprestimosApiIntegrationTests.cs # Testes de integração da API REST
├── __snapshots__/                    # (opcional) Pasta para arquivos de snapshot do Verify
```

---

## 📈 Boas Práticas Adotadas

- Separação entre testes unitários e de integração
- Cobertura de cenários positivos e negativos
- Verificação de performance mínima (usando `ExecutionTime`)
- Assertivas claras e expressivas com FluentAssertions
- Testes parametrizados com `[Theory]` e `[InlineData]`
- Uso de snapshot testing para detectar regressões

---


## 📃 Exemplo de Requisição via cURL

```bash
curl "https://localhost:5001/emprestimo?valor=1000&qtdMeses=6"
```

---

## ✍️ Autor
Desenvolvido por Thiago Darlei
Tests: `xUnit`, `FluentAssertions`, `Verify`  
API: `.NET 8`, `Minimal API`

---

## 🛡️ Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
```
