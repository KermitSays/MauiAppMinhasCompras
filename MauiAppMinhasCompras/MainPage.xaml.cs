using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;
using MauiAppMinhasCompras.Views;

namespace MauiAppMinhasCompras;

public partial class MainPage : ContentPage
{
    private readonly SQLiteDatabaseHelper _database;

    public MainPage(SQLiteDatabaseHelper database)
    {
        InitializeComponent();

        _database = database;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await CarregarProdutos();
    }

    private async Task CarregarProdutos()
    {
        BarraPesquisa.Text = string.Empty;

        var produtos = await _database.GetAll();

        ListaProdutos.ItemsSource = produtos;

        double total = produtos.Sum(p => p.Total);

        LabelTotal.Text = $"Total: R$ {total:F2}";
    }

    private async void OnAdicionarProdutoClicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(NovoProduto));
    }

    private async void OnEditarClicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not Produto produto)
        {
            return;
        }

        await Shell.Current.GoToAsync(
            nameof(NovoProduto),
            new Dictionary<string, object>
            {
                ["Produto"] = produto
            });
    }

    private async void OnExcluirClicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not Produto produto)
        {
            return;
        }

        bool confirmar = await DisplayAlertAsync(
            "Excluir produto",
            $"Deseja realmente excluir '{produto.Descricao}'?",
            "Sim",
            "Não");

        if (!confirmar)
        {
            return;
        }

        await _database.Delete(produto.Id);

        await CarregarProdutos();
    }

    private async void OnPesquisaTextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        string texto = e.NewTextValue ?? string.Empty;

        if (string.IsNullOrWhiteSpace(texto))
        {
            await CarregarProdutos();
            return;
        }

        var produtos = await _database.Search(texto);

        ListaProdutos.ItemsSource = produtos;

        double total = produtos.Sum(p => p.Total);

        LabelTotal.Text = $"Total: R$ {total:F2}";
    }
}