using Blazor_WASM_MovieApp.Areas.Identity;
using Blazor_WASM_MovieApp.Data;
using Blazor_WASM_MovieApp.Models;
using Blazor_WASM_MovieApp.Repositories;
using Blazor_WASM_MovieApp.Services;
using Blazor_WASM_MovieApp.Validators;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});
builder.Services.AddDbContext<BlazorMovieContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MDB")));

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BlazorMovieContext>();

builder.Services.AddTransient<MovieService>();
builder.Services.AddTransient<GenreService>();
builder.Services.AddTransient<CreditService>();
builder.Services.AddTransient<PersonService>();
builder.Services.AddTransient<AuthenticationService>();
builder.Services.AddTransient<MovieValidator>();
builder.Services.AddTransient<GenreValidator>();
builder.Services.AddTransient<PersonValidator>();
builder.Services.AddTransient<AuthInputValidator>();
builder.Services.AddTransient<MovieRepository>();
builder.Services.AddTransient<GenreRepository>();
builder.Services.AddTransient<CreditRepository>();
builder.Services.AddTransient<PersonRepository>();
builder.Services.AddTransient<AuthenticationRepository>();
builder.Services.AddTransient<UserManager<IdentityUser>>();

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    IWebHostEnvironment webHostEnvironment = services.GetRequiredService<IWebHostEnvironment>();


    SeedData.Initialize(services, webHostEnvironment);


}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
