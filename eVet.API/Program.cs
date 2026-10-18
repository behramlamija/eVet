using DotNetEnv;
using eVet.API.Infrastructure;
using eVet.Model.Responses;
using eVet.Services;
using eVet.Services.Database;
using eVet.Services.Slike;
using eVet.Services.Termini;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;

Env.NoClobber().TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("eVetConnection")
    ?? throw new InvalidOperationException("Connection string 'eVetConnection' nije definisan u .env datoteci.");
builder.Services.AddDbContext<EVetDbContext>(options =>
    options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure())
        // Zapisi vezani za soft-obrisanog roditelja namjerno se ne prikazuju.
        .ConfigureWarnings(w => w.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning)));

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IKorisniciService, KorisniciService>();
builder.Services.AddScoped<IUslugeService, UslugeService>();
builder.Services.AddScoped<IVrsteUslugaService, VrsteUslugaService>();
builder.Services.AddScoped<IGradoviService, GradoviService>();
builder.Services.AddScoped<IVrsteLjubimacaService, VrsteLjubimacaService>();
builder.Services.AddScoped<IRaseService, RaseService>();
builder.Services.AddScoped<ISpecijalnostiService, SpecijalnostiService>();
builder.Services.AddScoped<INaciniPlacanjaService, NaciniPlacanjaService>();
builder.Services.AddScoped<IUlogeService, UlogeService>();
builder.Services.AddScoped<ILjubimciService, LjubimciService>();
builder.Services.AddScoped<ITerapijeService, TerapijeService>();
builder.Services.AddScoped<IObavezeService, ObavezeService>();
builder.Services.AddScoped<IPromocijeService, PromocijeService>();
builder.Services.AddScoped<ITerminiService, TerminiService>();
builder.Services.AddScoped<IRecenzijeService, RecenzijeService>();
builder.Services.AddScoped<IObavijestiService, ObavijestiService>();
builder.Services.AddScoped<IStatistikaService, StatistikaService>();
builder.Services.AddScoped<ISlikeService, SlikeService>();

builder.Services.AddOptions<TerminiSettings>()
    .BindConfiguration(TerminiSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

MapsterConfig.Register(TypeAdapterConfig.GlobalSettings);
builder.Services.AddMapster();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(new ErrorResponse
        {
            Poruka = "Uneseni podaci nisu ispravni.",
            Greske = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(x => x.Key, x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()),
            TraceId = context.HttpContext.TraceIdentifier
        });
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Unesite JWT token dobijen sa /Auth/login."
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EVetDbContext>();
    await context.Database.MigrateAsync();
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
