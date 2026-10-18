using eVet.Model.Exceptions;
using eVet.Model.Responses;
using eVet.Services.Security;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace eVet.API.Infrastructure
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, poruka) = exception switch
            {
                BusinessException => (StatusCodes.Status400BadRequest, exception.Message),
                NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                UnauthorizedException => (StatusCodes.Status401Unauthorized, exception.Message),
                ForbiddenException => (StatusCodes.Status403Forbidden, exception.Message),
                DbUpdateException => (StatusCodes.Status409Conflict, "Promjene nije moguće sačuvati jer su podaci povezani s drugim zapisima ili su u međuvremenu izmijenjeni."),
                _ => (StatusCodes.Status500InternalServerError, "Došlo je do neočekivane greške. Pokušajte ponovo kasnije.")
            };

            var korisnikId = httpContext.User.FindFirst(JwtClaimNames.KorisnikId)?.Value ?? "anonimni";

            if (statusCode >= StatusCodes.Status500InternalServerError || exception is DbUpdateException)
            {
                _logger.LogError(exception,
                    "Greška pri obradi zahtjeva {Method} {Path}{Query} (korisnik: {KorisnikId}, trace: {TraceId})",
                    httpContext.Request.Method, httpContext.Request.Path, httpContext.Request.QueryString, korisnikId, httpContext.TraceIdentifier);
            }
            else
            {
                _logger.LogWarning("{ExceptionType} pri obradi {Method} {Path}{Query} (korisnik: {KorisnikId}): {Poruka}",
                    exception.GetType().Name, httpContext.Request.Method, httpContext.Request.Path, httpContext.Request.QueryString, korisnikId, exception.Message);
            }

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(new ErrorResponse
            {
                Poruka = poruka,
                TraceId = httpContext.TraceIdentifier
            }, cancellationToken);

            return true;
        }
    }
}
