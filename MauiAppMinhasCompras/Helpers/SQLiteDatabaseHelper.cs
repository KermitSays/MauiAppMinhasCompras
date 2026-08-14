using SQLite;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public SQLiteDatabaseHelper(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task<List<Produto>> GetProdutosAsync()
        {
            await Init();

            return await _database.Table<Produto>().ToListAsync();
        }

        public async Task<Produto> GetProdutoAsync(int id)
        {
            await Init();

            return await _database.Table<Produto>()
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> SaveProdutoAsync(Produto produto)
        {
            await Init();

            if (produto.Id != 0)
                return await _database.UpdateAsync(produto);

            return await _database.InsertAsync(produto);
        }

        public async Task<int> DeleteProdutoAsync(Produto produto)
        {
            await Init();

            return await _database.DeleteAsync(produto);
        }

        private async Task Init()
        {
            if (_database == null)
                return;

            await _database.CreateTableAsync<Produto>();
        }

        public async Task<List<Produto>> SearchAsync(string texto)
        {
            await Init();

            if (string.IsNullOrWhiteSpace(texto))
            {
                return await _database
                    .Table<Produto>()
                    .ToListAsync();
            }

            string textoPesquisa = texto.ToLower();

            var produtos = await _database
                .Table<Produto>()
                .ToListAsync();

            return produtos
                .Where(p => p.Descricao
                    .ToLower()
                    .Contains(textoPesquisa))
                .ToList();
        }
    }
}