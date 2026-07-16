using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LigaBotonera.Pages;

public class Index1Model : PageModel
{
    public IActionResult OnGetBuscarItens(string query)
    {
        Console.WriteLine($"CALLING BACKEND with [{query}] {DateTime.Now:HH:mm:ss}");

        if (string.IsNullOrEmpty(query)) return Content("");

        List<ProdutoViewModel> lista = [
            new ProdutoViewModel() { Id = 1, Codigo = "001", Nome = "Produto 1", Preco = 100 },
            new ProdutoViewModel() { Id = 2, Codigo = "002", Nome = "Produto 2", Preco = 200 },
            new ProdutoViewModel() { Id = 3, Codigo = "003", Nome = "Produto 3", Preco = 300 }
            ];

        var listaFiltrada = lista
            .Where(p => p.Nome.Contains(query))
            .Take(10)
            .ToList();

        return Partial("_GridResultadosPartial", listaFiltrada);
    }
}
