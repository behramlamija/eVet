using eVet.Model.Responses;
using eVet.Services;
using eVet.Services.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace eVet.API.Infrastructure
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection(JwtSettings.SectionName);
            var jwtSettings = jwtSection.Get<JwtSettings>();

            if (string.IsNullOrWhiteSpace(jwtSettings?.Key) || Encoding.UTF8.GetByteCount(jwtSettings.Key) < 32)
            {
                throw new InvalidOperationException("Jwt__Key mora biti definisan u .env datoteci i imati najmanje 32 znaka.");
            }

            services.Configure<JwtSettings>(jwtSection);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                        NameClaimType = JwtClaimNames.KorisnickoIme,
                        RoleClaimType = JwtClaimNames.Uloga,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };

                    options.Events = new JwtBearerEvents
                    {
                        // Token je nevažeći nakon odjave, promjene lozinke od strane admina ili deaktivacije.
                        OnTokenValidated = async context =>
                        {
                            var principal = context.Principal!;
                            var stamp = principal.FindFirst(JwtClaimNames.SecurityStamp)?.Value;

                            if (!int.TryParse(principal.FindFirst(JwtClaimNames.KorisnikId)?.Value, out var korisnikId) || stamp == null)
                            {
                                context.Fail("Token ne sadrži potrebne podatke.");
                                return;
                            }

                            var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                            if (!await authService.IsSecurityStampValidAsync(korisnikId, stamp, context.HttpContext.RequestAborted))
                            {
                                context.Fail("Token više nije važeći.");
                            }
                        },
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsJsonAsync(new ErrorResponse
                            {
                                Poruka = "Niste prijavljeni ili je vaša sesija istekla. Prijavite se ponovo.",
                                TraceId = context.HttpContext.TraceIdentifier
                            });
                        },
                        OnForbidden = async context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            await context.Response.WriteAsJsonAsync(new ErrorResponse
                            {
                                Poruka = "Nemate pravo pristupa ovoj akciji.",
                                TraceId = context.HttpContext.TraceIdentifier
                            });
                        }
                    };
                });

            return services;
        }
    }
}
