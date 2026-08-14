using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Views;
using Microsoft.Extensions.Logging;

namespace MauiAppMinhasCompras
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "minhascompras.db3");
            builder.Services.AddSingleton<SQLiteDatabaseHelper>(s =>
                new SQLiteDatabaseHelper(dbPath));
            builder.Services.AddTransient<NovoProduto>();

            return builder.Build();
        }
    }
}
