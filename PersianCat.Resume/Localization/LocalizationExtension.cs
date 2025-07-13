using Microsoft.Extensions.Localization;

namespace PersianCat.Resume.Localization;

public static class LocalizationExtension
{
    public static IServiceCollection AddJsonLocalization(this IServiceCollection services)
    {
        services.AddScoped<JsonStringLocalizer>();
        services.AddScoped<IStringLocalizerFactory>(sp => new JsonStringLocalizerFactory(sp.GetRequiredService<JsonStringLocalizer>()));
        services.AddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
        return services;
    }
}
