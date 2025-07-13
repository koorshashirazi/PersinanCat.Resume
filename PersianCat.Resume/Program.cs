using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PersianCat.Resume;
using PersianCat.Resume.Localization;
using PersianCat.Resume.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<JavaScriptInterop>();

builder.Services.AddScoped<IGoogleAnalyticsService, GoogleAnalyticsService>();
builder.Services.AddScoped<TimeOnPageTracker>();
builder.Services.AddScoped<LanguageService>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddSingleton<ToastService>();
builder.Services.AddScoped<OfflineDataService>();
builder.Services.AddScoped<NetworkService>();
builder.Services.AddScoped<Initializer>();
builder.Services.AddScoped<UrlChangeMonitor>();

builder.Services.AddJsonLocalization();

builder.Services.AddBlazoredLocalStorage();

var host = builder.Build();

var initializer = host.Services.GetRequiredService<Initializer>();
await initializer.InitializeAsync();

await host.RunAsync();
