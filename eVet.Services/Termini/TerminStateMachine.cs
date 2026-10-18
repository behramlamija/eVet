using eVet.Model.Enums;
using eVet.Model.Exceptions;

namespace eVet.Services.Termini
{
    // Jedino mjesto koje definiše dozvoljene prelaze statusa termina.
    public static class TerminStateMachine
    {
        public static readonly StatusTermina[] AktivniStatusi = { StatusTermina.NaCekanju, StatusTermina.Potvrdjen };

        private static readonly Dictionary<StatusTermina, StatusTermina[]> DozvoljeniPrelazi = new()
        {
            [StatusTermina.NaCekanju] = new[] { StatusTermina.Potvrdjen, StatusTermina.Odbijen, StatusTermina.Otkazan },
            [StatusTermina.Potvrdjen] = new[] { StatusTermina.Zavrsen, StatusTermina.Otkazan },
            [StatusTermina.Zavrsen] = Array.Empty<StatusTermina>(),
            [StatusTermina.Otkazan] = Array.Empty<StatusTermina>(),
            [StatusTermina.Odbijen] = Array.Empty<StatusTermina>()
        };

        public static bool JeDozvoljen(StatusTermina trenutni, StatusTermina novi)
        {
            return DozvoljeniPrelazi[trenutni].Contains(novi);
        }

        public static void ProvjeriPrelaz(StatusTermina trenutni, StatusTermina novi)
        {
            if (!JeDozvoljen(trenutni, novi))
            {
                throw new BusinessException($"Termin u statusu '{Naziv(trenutni)}' nije moguće prebaciti u status '{Naziv(novi)}'.");
            }
        }

        public static string Naziv(StatusTermina status) => status switch
        {
            StatusTermina.NaCekanju => "Na čekanju",
            StatusTermina.Potvrdjen => "Potvrđen",
            StatusTermina.Zavrsen => "Završen",
            StatusTermina.Otkazan => "Otkazan",
            StatusTermina.Odbijen => "Odbijen",
            _ => status.ToString()
        };
    }
}
