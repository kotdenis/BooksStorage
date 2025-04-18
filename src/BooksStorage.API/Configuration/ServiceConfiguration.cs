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

        public static void AddServices(this IServiceCollection services)
        {

            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IPublisherService, PublisherService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddFluentValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<BookDto>, BookDtoValidator>();
            services.AddScoped<IValidator<UpdateBookDto>, UpdateBookDtoValidator>();
            services.AddScoped<IValidator<SupplierDto>, SupplierDtoValidator>();
            services.AddScoped<IValidator<PublisherDto>, PublisherValidator>();
        }
    }
}
