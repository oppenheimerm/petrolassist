using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PA.Core.Models;
using PA.Datastore.EFCore;
using PA.Datastore.EFCore.Interfaces;
using PA.Datastore.EFCore.Repositories;
using PA.UseCases.CountriesUseCase;
using PA.UseCases.Interfaces;
using PA.UseCases.PetrolStationUseCase;
using PA.UseCases.VendorsUseCase;
using PA.Web.Admin.TmpData;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<ApplicationManagmentDbContext>(options =>
    options.UseSqlServer(connectionString, x => x.UseNetTopologySuite()));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    //Add Role services to Identity
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationManagmentDbContext>();
builder.Services.AddRazorPages();

//  Require authenticated users
/*builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
*/
// Authorization handlers.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 7;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

//  Fix error: System.Text.Json.JsonException: A possible object cycle was detected. This can either be due
//  to a cycle or if the object depth is larger than the maximum allowed depth of 32. Consider
//  using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles.
//
//  nuget package: Microsoft.AspNetCore.Mvc.NewtonsoftJson
builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

//  Repositories
builder.Services.AddScoped<IPetrolStationRepository, PetrolStationRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

//  Development only
//builder.Services.AddTransient<IInitRepository , InitRepository>();


// UseCases
builder.Services.AddTransient<IGetAllCountriesUseCase, GetAllCountriesUseCase>();
builder.Services.AddTransient<IGetAllVendorsUseCase, GetAllVendorsUseCase>();
builder.Services.AddTransient<IGetAllStationsUseCase, GetAllStationsUseCase>();
//builder.Services.AddTransient<IGetAllPetrolStationsFlatUseCase, GetAllPetrolStationsFlatUseCase>();
builder.Services.AddTransient<IGetCountryByIdUseCase, GetCountryByIdUseCase>();
builder.Services.AddTransient<IGetPetrolStationByIdUseCase, GetPetrolStationByIdUseCase>();
builder.Services.AddTransient<IAddPetrolStationUseCase, AddPetrolStationUseCase>();

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

app.MapRazorPages();

app.Run();
