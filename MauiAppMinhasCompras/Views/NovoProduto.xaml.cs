using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;
using System.Globalization;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage, IQueryAttributable
{
    private readonly SQLiteDatabaseHelper _database;

    private Produto? _produtoEmEdicao;

    public NovoProduto(SQLiteDatabaseHelper database)
    {
        InitializeComponent();

        _database = database;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Produto", out object? valor) &&
            valor is Produto produto)
        {
            _produtoEmEdicao = produto;

            txtDescricao.Text = produto.Descricao;
            txtQuantidade.Text = produto.Quantidade.ToString(
                CultureInfo.InvariantCulture);
            txtPreco.Text = produto.Preco.ToString(
                CultureInfo.InvariantCulture);

            Title = "Editar Produto";
        }
        else
        {
            _produtoEmEdicao = null;

            Title = "Novo Produto";
        }
    }

    private async void OnSalvarClicked(
        object sender,
        EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtDescricao.Text))
        {
            await DisplayAlertAsync(
                "Atenção",
                "Informe a descrição do produto.",
                "OK");

            return;
        }

        string quantidadeTexto =
            txtQuantidade.Text?.Replace(",", ".") ?? "";

        if (!double.TryParse(
                quantidadeTexto,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out double quantidade) ||
            quantidade <= 0)
        {
            await DisplayAlertAsync(
                "Atenção",
                "Informe uma quantidade maior que zero.",
                "OK");

            return;
        }

        string precoTexto =
            txtPreco.Text?.Replace(",", ".") ?? "";

        if (!double.TryParse(
                precoTexto,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out double preco) ||
            preco <= 0)
        {
            await DisplayAlertAsync(
                "Atenção",
                "Informe um preço maior que zero.",
                "OK");

            return;
        }

        Produto produto;

        if (_produtoEmEdicao is not null)
        {
            produto = _produtoEmEdicao;

            produto.Descricao = txtDescricao.Text.Trim();
            produto.Quantidade = quantidade;
            produto.Preco = preco;
        }
        else
        {
            produto = new Produto
            {
                Descricao = txtDescricao.Text.Trim(),
                Quantidade = quantidade,
                Preco = preco
            };
        }

        await _database.SaveProdutoAsync(produto);

        await DisplayAlertAsync(
            "Sucesso",
            _produtoEmEdicao is null
                ? "Produto cadastrado com sucesso!"
                : "Produto atualizado com sucesso!",
            "OK");

        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancelarClicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}