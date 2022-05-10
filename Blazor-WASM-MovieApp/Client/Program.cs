using Blazor_WASM_MovieApp.Client;
using Blazor_WASM_MovieApp.Client.AuthProviders;
using Blazor_WASM_MovieApp.Client.Repositories;
using Blazor_WASM_MovieApp.Client.Services;
using Blazored.Modal;
using Blazorise;
using Blazorise.Bootstrap;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredModal();
builder.Services.AddTransient<WASM_MovieRepository>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<WASM_IMovieService, WASM_MovieService>();
builder.Services.AddScoped<WASM_ICreditService, WASM_CreditService>();
builder.Services.AddScoped<WASM_IGenreService, WASM_GenreService>();
builder.Services.AddScoped<WASM_IPersonService, WASM_PersonService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();


await builder.Build().RunAsync();