using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PA.Web.Admin.Areas.Identity;
using PA.Web.Admin.Data;
using PA.Datastore.EFCore;
using PA.Datastore.EFCore.Interfaces;
using PA.Datastore.EFCore.Repositories;
using PA.UseCases.CountriesUseCase;
using PA.UseCases.Interfaces;
using PA.UseCases.VendorsUseCase;
using PA.UseCases.PetrolStationUseCase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationManagmentDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationManagmentDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

//  21/01/2024
builder.Services.AddControllers();

//  A server-side app doesn't include an HttpClient service by default. Provide an HttpClient to the app using
//  the HttpClient factory infrastructure.
builder.Services.AddHttpClient();

//  Fix error: System.Text.Json.JsonException: A possible object cycle was detected. This can either be due
//  to a cycle or if the object depth is larger than the maximum allowed depth of 32. Consider
//  using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles.
//
//  nuget package: Microsoft.AspNetCore.Mvc.NewtonsoftJson
builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddSingleton<WeatherForecastService>();


//  Repositories
builder.Services.AddScoped<IPetrolStationRepository, PetrolStationRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

// UseCases
builder.Services.AddTransient<IGetAllCountriesUseCase, GetAllCountriesUseCase>();
builder.Services.AddTransient<IGetAllVendorsUseCase, GetAllVendorsUseCase>();
builder.Services.AddTransient<IGetAllStationsUseCase, GetAllStationsUseCase>();
builder.Services.AddTransient<IGetAllPetrolStationsFlatUseCase, GetAllPetrolStationsFlatUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");



//  21/01/2024
app.MapControllers();

app.Run();
