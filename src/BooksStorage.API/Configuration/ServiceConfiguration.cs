namespace BooksStorage.API.Configuration
{
    public static class ServiceConfiguration
    {
        public static void AddNpgsqlDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("NpgConnection");
            NpgsqlDataSourceBuilder npgsqlData = new NpgsqlDataSourceBuilder(connectionString);
            npgsqlData.EnableDynamicJson();
            npgsqlData.UseJsonNet();
            var dataSource = npgsqlData.Build();
            services.AddDbContext<BooksStorageDbContext>(options =>
                options.UseNpgsql(dataSource, npgsqlOptionsAction => npgsqlOptionsAction.MigrationsAssembly("BooksStorage.Infrastructure")));
        }
    }
}
