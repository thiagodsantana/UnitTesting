using Emprestimos.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmprestimoService, EmprestimoService>();

var app = builder.Build();

app.MapGet("/emprestimo", (decimal valor, int qtdMeses, IEmprestimoService service) =>
{
    if (valor <= 0) return Results.BadRequest("Valor deve ser maior que 0.");
    if (qtdMeses <= 0) return Results.BadRequest("Meses deve ser maior que 0.");

    var loan = service.CalcularEmprestimo(valor, qtdMeses);
    return Results.Ok(loan);
});

app.Run();

public partial class Program { } // Para testes de integração
