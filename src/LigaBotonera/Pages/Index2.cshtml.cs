using LigaBotonera.Pages.Shared;
using LigaBotonera.Pages.Shared.Lookup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LigaBotonera.Pages;

public class Index2Model : PageModel
{
    [BindProperty]
    public ViewModel Data { get; set; } = new();

    public Index2Model()
    {
        Data.IdCidade = 1;
        Data.NomeCidade = "Cidade 1";
        Data.IdUF = 1;
        Data.SiglaUF = "UF 1";

        CidadeLookup.SetInitialValue(Data.IdCidade.ToString(), Data.NomeCidade);
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

    public LookupViewModel CidadeLookup { get; set; } = new()
    {
        Label = "Cidade",
        QueryHandlerName = "?handler=BuscarCidades",
        Grid = new()
        {
            IdSelector = p => ((CidadeViewModel)p).Id,
            NameSelector = p => ((CidadeViewModel)p).Nome,
            Columns =
            [
                new()
                {
                    Header = "Cidade",
                    ValueSelector = p => ((CidadeViewModel)p).Nome,
                },
                new()
                {
                    Header = "UF",
                    ValueSelector = p => ((CidadeViewModel)p).SiglaUF
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
            .ToList();

        ProdutoLookup.Grid.Items = listaFiltrada.Cast<dynamic>().ToList();
        return Partial(PartialViewId.Lookup_LookupGrid, ProdutoLookup.Grid);
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
            .ToList();

        ClienteLookup.Grid.Items = listaFiltrada.Cast<dynamic>().ToList();
        return Partial(PartialViewId.Lookup_LookupGrid, ClienteLookup.Grid);
    }

    public IActionResult OnGetBuscarCidades(string query)
    {
        Console.WriteLine($"CALLING BACKEND with [{query}] {DateTime.Now:HH:mm:ss}");

        if (string.IsNullOrEmpty(query)) return Content("");

        List<CidadeViewModel> lista = [
            new CidadeViewModel() { Id = 1, Nome = "Cidade 1", IdUF = 1, SiglaUF = "UF 1" },
            new CidadeViewModel() { Id = 2, Nome = "Cidade 2", IdUF = 2, SiglaUF = "UF 2" },
            new CidadeViewModel() { Id = 3, Nome = "Cidade 3", IdUF = 3, SiglaUF = "UF3 3" }
            ];

        var listaFiltrada = lista
            .Where(p => p.Nome.Contains(query, StringComparison.CurrentCultureIgnoreCase))
            .ToList();

        CidadeLookup.Grid.Items = listaFiltrada.Cast<dynamic>().ToList();
        return Partial(PartialViewId.Lookup_LookupGrid, CidadeLookup.Grid);
    }

    public class ViewModel
    {
        public int IdCidade { get; set; }
        public string NomeCidade { get; set; } = string.Empty;
        public int IdUF { get; set; }
        public string SiglaUF { get; set; } = string.Empty;
    }
}
