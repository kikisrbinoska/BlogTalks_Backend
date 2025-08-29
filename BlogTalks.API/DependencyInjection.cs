namespace BlogTalks.API
{
    public static class DependencyInjection 
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddMediatR(config => 
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });
            return services; 
        }
    }
}
