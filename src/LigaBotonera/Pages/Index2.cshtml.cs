using LigaBotonera.Pages.Shared.Lookup2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LigaBotonera.Pages;

public class Index2Model : PageModel
{
    public Index2Model()
    {
        //ClienteLookup.SetInitialValue("1", "Cliente 1");
    }

    public LookupViewModel ProdutoLookup { get; set; } = new()
    {
        Label = "Selecione o Produto",
        QueryHandlerName = "?handler=BuscarProdutos",
        Grid = new()
        {
            IdSelector = p => ((ProdutoViewModel)p).Id,
            NameSelector = p => ((ProdutoViewModel)p).Nome,
            Columns =
            [
                new()
                {
                    Header = "Cód",
                    ValueSelector = p => ((ProdutoViewModel)p).Codigo,
                    CssClass = "w-16"
                },
                new()
                {
                    Header = "Nome do Produto",
                    ValueSelector = p => ((ProdutoViewModel)p).Nome
                },
                new()
                {
                    Header = "Preço Unitário",
                    ValueSelector = p => ((ProdutoViewModel)p).Preco.ToString("C"),
                    CssClass = "text-right font-semibold text-emerald-600 dark:text-emerald-400"
                }
            ]
        }
    };

    public LookupViewModel ClienteLookup { get; set; } = new()
    {
        Label = "Cliente",
        QueryHandlerName = "?handler=BuscarClientes",
        Grid = new()
        {
            IdSelector = p => ((ClienteViewModel)p).Id,
            NameSelector = p => ((ClienteViewModel)p).Nome,
            Columns =
            [
                new()
                {
                    Header = "Nome do Cliente",
                    ValueSelector = p => ((ClienteViewModel)p).Nome
                },
                new()
                {
                    Header = "idade",
                    ValueSelector = p => ((ClienteViewModel)p).Idade.ToString(),
                }
            ]
        }
    };

    public IActionResult OnGetBuscarProdutos(string query)
    {
        Console.WriteLine($"CALLING BACKEND with [{query}] {DateTime.Now:HH:mm:ss}");

        if (string.IsNullOrEmpty(query)) return Content("");

        List<ProdutoViewModel> lista = [
            new ProdutoViewModel() { Id = 1, Codigo = "001", Nome = "Produto 1", Preco = 100, CategoriaId = 1, CategoriaNome = "Categoria 1" },
            new ProdutoViewModel() { Id = 2, Codigo = "002", Nome = "Produto 2", Preco = 200, CategoriaId = 2, CategoriaNome = "Categoria 2" },
            new ProdutoViewModel() { Id = 3, Codigo = "003", Nome = "Produto 3", Preco = 300, CategoriaId = 3, CategoriaNome = "Categoria 3" }
            ];

        var listaFiltrada = lista
            .Where(p => p.Nome.Contains(query, StringComparison.CurrentCultureIgnoreCase))
            .Take(10)
            .ToList();

        ProdutoLookup.Grid.Items = listaFiltrada.Cast<dynamic>().ToList();
        return Partial("Lookup2/_LookupGrid", ProdutoLookup.Grid);
    }

    public IActionResult OnGetBuscarClientes(string query)
    {
        Console.WriteLine($"CALLING BACKEND with [{query}] {DateTime.Now:HH:mm:ss}");

        if (string.IsNullOrEmpty(query)) return Content("");

        List<ClienteViewModel> lista = [
            new ClienteViewModel() { Id = 1, Nome = "Cliente 1", Idade = 51 },
            new ClienteViewModel() { Id = 2, Nome = "Cliente 2", Idade = 52 },
            new ClienteViewModel() { Id = 3, Nome = "Cliente 3", Idade = 53 }
            ];

        var listaFiltrada = lista
            .Where(p => p.Nome.Contains(query, StringComparison.CurrentCultureIgnoreCase))
            .Take(10)
            .ToList();

        ClienteLookup.Grid.Items = listaFiltrada.Cast<dynamic>().ToList();
        return Partial("Lookup2/_LookupGrid", ClienteLookup.Grid);
    }
}
