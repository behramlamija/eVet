namespace eVet.Messaging
{
    /// <summary>
    /// Poruka koju API šalje na RabbitMQ, a Worker je obrađuje slanjem e-maila.
    /// </summary>
    public record EmailPoruka(string Primalac, string ImePrimaoca, string Naslov, string Tekst);
}
