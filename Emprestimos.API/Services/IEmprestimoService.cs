using Emprestimos.API.Models;

namespace Emprestimos.API.Services
{
    public interface IEmprestimoService
    {
        Emprestimo CalcularEmprestimo(decimal valor, int meses);
    }
}
