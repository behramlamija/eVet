# eVet (PetCity)

Seminarski rad iz predmeta Razvoj softvera II na Fakultetu informacijskih tehnologija u Mostaru.

Sistem za veterinarsku stanicu: desktop aplikacija za vlasnika i veterinare, mobilna aplikacija za klijente i REST API.

## Struktura

| Folder | Sadržaj |
|---|---|
| `eVet.API` | ASP.NET Core Web API (kontroleri, autentifikacija, obrada grešaka) |
| `eVet.Services` | Poslovna logika, EF Core `DbContext`, migracije i seed podaci |
| `eVet.Model` | DTO, request i search objekti, enumi i konstante |
| `eVet.Messaging` | Zajednički ugovori za RabbitMQ (poruke, redovi, publisher) |
| `eVet.Worker` | Pomoćni servis koji preuzima poruke iz RabbitMQ-a i šalje e-mailove |
| `evet_shared` | Zajednički Flutter paket: API klijent, modeli, provideri, tema |
| `evet_desktop` | Flutter Windows aplikacija (vlasnik i veterinari) |
| `evet_mobile` | Flutter Android aplikacija (klijenti) |

## Pokretanje

### Backend

1. Raspakovati `.env` datoteku u root folder repozitorija (konfiguracija se ne nalazi u kodu niti u `appsettings.json`).
2. Pokrenuti API:

   ```bash
   cd eVet.API
   dotnet run --launch-profile http
   ```

   API je dostupan na `http://localhost:5103`, a Swagger na `http://localhost:5103/swagger`.
   Pri pokretanju se automatski kreira baza `210210`, primjenjuju migracije i upisuju testni podaci.

### Docker (API, baza, RabbitMQ, Worker, Mailpit)

```bash
docker compose up -d --build
```

| Servis | Adresa |
|---|---|
| API i Swagger | `http://localhost:5103/swagger` |
| RabbitMQ management | `http://localhost:15672` (podaci iz `.env`) |
| Mailpit (pregled poslanih e-mailova) | `http://localhost:8025` |

API događaje (registracija, promjene termina, recenzije, zaboravljena lozinka) šalje u RabbitMQ red `evet.emailovi`.
Worker iz tog reda šalje e-mailove preko SMTP-a; neuspjelo slanje ponavlja 4 puta (1, 2, 4, 8 s), a zatim poruku prebacuje u red `evet.emailovi.neuspjeli`.
U razvoju se kao SMTP server koristi Mailpit, pa se svi poslani e-mailovi (npr. kod za promjenu lozinke) vide na `http://localhost:8025`.

### Desktop aplikacija

```bash
cd evet_desktop
flutter run -d windows --dart-define=API_BASE_URL=http://localhost:5103
```

### Mobilna aplikacija (Android emulator)

```bash
cd evet_mobile
flutter run --dart-define=API_BASE_URL=http://10.0.2.2:5103
```

## Pristupni podaci

Lozinka za sve testne korisnike je `test`.

| Kontekst | Korisničko ime | Lozinka |
|---|---|---|
| Desktop verzija (vlasnik) | `desktop` | `test` |
| Desktop verzija (veterinar) | `veterinar` | `test` |
| Mobilna verzija (klijent) | `mobile` | `test` |
